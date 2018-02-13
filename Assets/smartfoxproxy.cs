using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using Sfs2X;
using Sfs2X.Logging;
using Sfs2X.Util;
using Sfs2X.Core;
using Sfs2X.Entities;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;
using com.tangelogames.extensions.model.vo;
using com.tangelogames.extensions.model.responses;

public class smartfoxproxy : MonoBehaviour
    {

        public string IpAddress = "";
        public string Port = "";
        public bool DebugMode = true;
        public string loginName = "1:1";
        public bool connected = false;


        private SmartFox sfs;
        private bool firstJoin = true;
        private List<Room> roomlist;

        public event EventHandler ConnectedEvent;
        protected virtual void OnConnectedEvent(EventArgs e)
        {
            if (ConnectedEvent != null)
                ConnectedEvent(this, e);
        }


        void Update()
        {
            // As Unity is not thread safe, we process the queued up callbacks on every frame
            if (sfs != null)
                sfs.ProcessEvents();
        }

        void OnApplicationQuit()
        {
            // Always disconnect before quitting
            if (sfs != null && sfs.IsConnected)
                sfs.Disconnect();
        }

        // Disconnect from the socket when ordered by the main Panel scene
        public void Disconnect()
        {
            OnApplicationQuit();
        }


        public void Connect()
        {
            if (sfs == null || !sfs.IsConnected)
            {

                // CONNECT

                #if UNITY_WEBPLAYER
				                // Socket policy prefetch can be done if the client-server communication is not encrypted only (read link provided in the note above)
				                if (!Security.PrefetchSocketPolicy(hostInput.text, Convert.ToInt32(portInput.text), 500)) {
					                Debug.LogError("Security Exception. Policy file loading failed!");
				                }
                #endif

                    Debug.Log("Now connecting...");

                    // Initialize SFS2X client and add listeners
                    // WebGL build uses a different constructor
                #if !UNITY_WEBGL
				                sfs = new SmartFox();
                #else
                    sfs = new SmartFox(UseWebSocket.WS_BIN);
                #endif

                // Set ThreadSafeMode explicitly, or Windows Store builds will get a wrong default value (false)
                sfs.ThreadSafeMode = true;

                sfs.AddEventListener(SFSEvent.CONNECTION, OnConnection);
                sfs.AddEventListener(SFSEvent.CONNECTION_LOST, OnConnectionLost);

                sfs.AddLogListener(LogLevel.INFO, OnInfoMessage);
                sfs.AddLogListener(LogLevel.WARN, OnWarnMessage);
                sfs.AddLogListener(LogLevel.ERROR, OnErrorMessage);

                sfs.AddEventListener(SFSEvent.LOGIN, OnLogin);
                sfs.AddEventListener(SFSEvent.LOGIN_ERROR, OnLoginError);
                sfs.AddEventListener(SFSEvent.ROOM_JOIN, OnRoomJoin);
                sfs.AddEventListener(SFSEvent.ROOM_JOIN_ERROR, OnRoomJoinError);
                sfs.AddEventListener(SFSEvent.PUBLIC_MESSAGE, OnPublicMessage);
                sfs.AddEventListener(SFSEvent.USER_ENTER_ROOM, OnUserEnterRoom);
                sfs.AddEventListener(SFSEvent.USER_EXIT_ROOM, OnUserExitRoom);
                sfs.AddEventListener(SFSEvent.ROOM_ADD, OnRoomAdd);

                sfs.AddEventListener(SFSEvent.EXTENSION_RESPONSE, OnExtensionResponse);



            // Set connection parameters
            ConfigData cfg = new ConfigData();
                cfg.Host = IpAddress;
                cfg.Port = Convert.ToInt32(Port);
                cfg.Zone = "slotZ";
                cfg.Debug = DebugMode;

                // Connect to SFS2X
                sfs.Connect(cfg);
            }

        }

        public void disconnect()
        {
            if (sfs != null && sfs.IsConnected)
                sfs.Disconnect();
        }

        public void Login()
        {
            ISFSObject param = SFSObject.NewInstance();
            param.PutUtfString("version", "flash.vs.4.1.2");
            sfs.Send(new Sfs2X.Requests.LoginRequest(loginName, Md5Sum("1"), "slotZ", param));
        }

        //----------------------------------------------------------
        // Private helper methods
        //----------------------------------------------------------

        
        private void reset()
        {
            // Remove SFS2X listeners
            sfs.RemoveEventListener(SFSEvent.CONNECTION, OnConnection);
            sfs.RemoveEventListener(SFSEvent.CONNECTION_LOST, OnConnectionLost);

            sfs.RemoveLogListener(LogLevel.INFO, OnInfoMessage);
            sfs.RemoveLogListener(LogLevel.WARN, OnWarnMessage);
            sfs.RemoveLogListener(LogLevel.ERROR, OnErrorMessage);

            sfs = null;

            // Enable interface
            connected = false;
            //enableInterface(true);
        }

        //----------------------------------------------------------
        // SmartFoxServer event listeners
        //----------------------------------------------------------

        private void OnConnection(BaseEvent evt)
        {
            if ((bool)evt.Params["success"])
            {
                Debug.Log("Connection established successfully");
                Debug.Log("SFS2X API version: " + sfs.Version);
                Debug.Log("Connection mode is: " + sfs.ConnectionMode);

                connected = true;
                // Enable disconnect button
                //button.interactable = true;
                //buttonLabel.text = "DISCONNECT";
                  OnConnectedEvent(new EventArgs());

            }
            else
            {
                Debug.Log("Connection failed; is the server running at all?");

                // Remove SFS2X listeners and re-enable interface
                reset();
            }
        }

        private void OnConnectionLost(BaseEvent evt)
        {
            Debug.Log("Connection was lost; reason is: " + (string)evt.Params["reason"]);

            // Remove SFS2X listeners and re-enable interface
            reset();
        }

        //----------------------------------------------------------
        // SmartFoxServer log event listeners
        //----------------------------------------------------------

        public void OnInfoMessage(BaseEvent evt)
        {
            string message = (string)evt.Params["message"];
            ShowLogMessage("INFO", message);
        }

        public void OnWarnMessage(BaseEvent evt)
        {
            string message = (string)evt.Params["message"];
            ShowLogMessage("WARN", message);
        }

        public void OnErrorMessage(BaseEvent evt)
        {
            string message = (string)evt.Params["message"];
            ShowLogMessage("ERROR", message);
        }

        private void ShowLogMessage(string level, string message)
        {
            message = "[SFS > " + level + "] " + message;
            Debug.Log(message);

        }


    private void OnLogin(BaseEvent evt)
    {
        User user = (User)evt.Params["user"];

        // Rotate camera to main panel
        //cameraAnimator.SetBool("loggedIn", true);

        // Set "Hello" text
        //helloText.text = "Hello " + user.Name;

        // Clear chat panel, user list
        //chatText.text = "";
        //userListText.text = "";

        // Show system message
        string msg = "Connection established successfully\n";
        msg += "SFS2X API version: " + sfs.Version + "\n";
        msg += "Connection mode is: " + sfs.ConnectionMode + "\n";
        msg += "Logged in as " + user.Name + "\n";

        Debug.Log(msg);


        // Populate Room list
        populateRoomList(sfs.RoomList);

        // Join first Room in Zone
        if (sfs.RoomList.Count > 0)
        {
            //sfs.Send(new Sfs2X.Requests.JoinRoomRequest(sfs.RoomList[0].Name));
            sfs.Send(new Sfs2X.Requests.JoinRoomRequest(3521));
        }
    }

    bool sitting = false;
    private void OnExtensionResponse(BaseEvent evt)
    {
        Debug.Log("EXTENSION CALL RECIVED");


        if (sitting)
        {
            ISFSObject parameters = SFSObject.NewInstance();
            parameters.PutLong("0", 100); // bet amount
            parameters.PutInt("1", 10); // number of lines
            parameters.PutInt("2", 0); // seat number 
            parameters.PutLong("3", 1000); // user money 
            parameters.PutLong("4", 100); // user experiance

            sfs.Send(new ExtensionRequest("sss", parameters, sfs.LastJoinedRoom));
        }
        else
        {
            string cmd = (string)evt.Params["cmd"];
            SFSObject dataObject = (SFSObject)evt.Params["params"];

            Debug.Log(evt);
        }

    }

    private void clearRoomList()
    {
        roomlist = new List<Room>();
        
    }
    private void populateRoomList(List<Room> rooms)
    {
        // Clear current Room list
        clearRoomList();

        roomlist = rooms;
        // For the roomlist we use a scrollable area containing a separate prefab button for each Room
        // Buttons are clickable to join Rooms
        /*foreach (Room room in rooms)
        {
            int roomId = room.Id;

            GameObject newListItem = Instantiate(roomListItem) as GameObject;
            RoomItem roomItem = newListItem.GetComponent<RoomItem>();
            roomItem.nameLabel.text = room.Name;
            roomItem.maxUsersLabel.text = "[max " + room.MaxUsers + " users]";
            roomItem.roomId = roomId;

            roomItem.button.onClick.AddListener(() => OnRoomItemClick(roomId));

            newListItem.transform.SetParent(roomListContent, false);
        }*/
    }

    private void OnLoginError(BaseEvent evt)
    {
        // Disconnect
        sfs.Disconnect();

        // Remove SFS2X listeners and re-enable interface
        reset();

        // Show error message
        Debug.Log("Login failed: " + (string)evt.Params["errorMessage"]) ;
    }

    private void OnRoomJoin(BaseEvent evt)
    {
        Room room = (Room)evt.Params["room"];

        // Clear chat (uless this is the first time a Room is joined - or the initial system message would be deleted)
        //if (!firstJoin)
            //chatText.text = "";

        firstJoin = false;

        // Show system message
        Debug.Log("\nYou joined room '" + room.Name + "'\n");

        // Enable chat controls
        //chatControls.interactable = true;

        // Populate users list
        //populateUserList(room.UserList);

      
        ISFSObject parameters = SFSObject.NewInstance();
        parameters.PutInt("0", 1); // sit id
        parameters.PutLong("1", 1000000); // money on sit

        sfs.Send(new ExtensionRequest("sc", parameters));

        sitting = true;

        Debug.Log("SENDING Sit");

        ISFSObject parameters2 = SFSObject.NewInstance();
        parameters.PutLong("0", 100); // bet amount
        parameters.PutInt("1", 10); // number of lines
        parameters.PutInt("2", 0); // seat number 
        parameters.PutLong("3", 1000); // user money 
        parameters.PutLong("4", 100); // user experiance

        sfs.Send(new ExtensionRequest("sss", parameters2));

    }

    private void OnRoomJoinError(BaseEvent evt)
    {
        // Show error message
        Debug.Log("Room join failed: " + (string)evt.Params["errorMessage"]);
    }

    private void OnPublicMessage(BaseEvent evt)
    {
        User sender = (User)evt.Params["sender"];
        string message = (string)evt.Params["message"];

        Debug.Log(sender.ToString() +  message);
    }

    private void OnUserEnterRoom(BaseEvent evt)
    {
        User user = (User)evt.Params["user"];
        Room room = (Room)evt.Params["room"];

        // Show system message
        Debug.Log("User " + user.Name + " entered the room");

        // Populate users list
        //populateUserList(room.UserList);
    }

    private void OnUserExitRoom(BaseEvent evt)
    {
        User user = (User)evt.Params["user"];

        if (user != sfs.MySelf)
        {
            Room room = (Room)evt.Params["room"];

            // Show system message
            Debug.Log("User " + user.Name + " left the room");

            // Populate users list
            //populateUserList(room.UserList);
        }
    }

    private void populateUserList(List<User> users)
    {
        // For the userlist we use a simple text area, with a user name in each row
        // No interaction is possible in this example

        // Get user names
        List<string> userNames = new List<string>();

        foreach (User user in users)
        {

            string name = user.Name;

            if (user == sfs.MySelf)
                name += " <color=#808080ff>(you)</color>";

            userNames.Add(name);
        }

        // Sort list
        userNames.Sort();

        // Display list
        //userListText.text = "";
        //userListText.text = String.Join("\n", userNames.ToArray());
    }

    public string Md5Sum(string strToEncrypt)
    {
        System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
        byte[] bytes = ue.GetBytes(strToEncrypt);

        // encrypt bytes
        System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
        byte[] hashBytes = md5.ComputeHash(bytes);

        // Convert the encrypted bytes back to a string (base 16)
        string hashString = "";

        for (int i = 0; i < hashBytes.Length; i++)
        {
            hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
        }

        return hashString.PadLeft(32, '0');
    }


    private void OnRoomAdd(BaseEvent evt)
    {
        // Re-populate Room list
        populateRoomList(sfs.RoomList);
    }

}