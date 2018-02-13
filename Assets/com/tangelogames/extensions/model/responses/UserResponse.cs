using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sfs2X.Protocol.Serialization;

namespace com.tangelogames.extensions.model.responses
{
    public class UserResponse : BaseResponseModel
    {
        public int userId;
        public int seatNum;
        public long moneyOnSeat;
        public long availableMoney;

        public string smallPicUrl;
        public string largePicUrl;

        public long levelMoney;
        public long experience;

        public string displayedItems;

        public long wonAmount;
        public string fullName;
        public string name;

        override public string getCommandName()
        {
            return null;
        }


        public UserResponse()
        {

        }

    }
}
