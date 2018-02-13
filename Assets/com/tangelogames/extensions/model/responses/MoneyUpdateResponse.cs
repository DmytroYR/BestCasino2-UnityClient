using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sfs2X.Protocol.Serialization;

namespace com.tangelogames.extensions.model.responses
{

    class MoneyUpdateResponse: BaseResponseModel, SerializableSFSType
    {
        public long avlMoney;

        override public string getCommandName()
        {
            return "RES_MONEY_UPDATE";
        }

        public MoneyUpdateResponse()
        {

        }
    }
}
