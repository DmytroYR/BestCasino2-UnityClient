using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour {

    public SymbolManager symbolManager;
    public SpinManager playerSpinManager;
    public SpinManager[] socialSpinManagers;
    public smartfoxproxy foxProxy;

    public bool connectswitch = false;


	// Use this for initialization
	void Start () {
       
       foxProxy.ConnectedEvent += OnFoxConnected;

       foxProxy.Connect();

    }
	
    void OnFoxConnected(object sender, System.EventArgs e)
    {
        Debug.Log("LOGIN");
        foxProxy.Login();
    }

    void SpinResultHandler()
    {
        //playerSpinManager.setResultData();

        /*
        SpinResultVO sr = new SpinResultVO();
        sr.lines = new List<LineWinResult>();
        sr.lines.Add(new LineResult());
        */
    }


}
