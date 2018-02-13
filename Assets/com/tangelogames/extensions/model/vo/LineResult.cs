using Sfs2X.Protocol.Serialization;
using System.Collections.Generic;
using Sfs2X.Entities.Data;


namespace com.tangelogames.extensions.model.vo
{
    public class LineResult : LineWinResult , SerializableSFSType
    {
        public bool hasWild;
        public int wilds;
        public bool isJackpot;

    }
}