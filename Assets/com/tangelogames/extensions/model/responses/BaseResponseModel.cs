using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sfs2X.Protocol.Serialization;

public abstract class BaseResponseModel : SerializableSFSType
{
   public abstract string getCommandName();
    
   public BaseResponseModel()
   {

    }
}

