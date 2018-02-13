using Sfs2X.Protocol.Serialization;
using System.Collections.Generic;
using Sfs2X.Entities.Data;


namespace com.tangelogames.extensions.model.vo
{
    public class LineWinResult : SerializableSFSType 
    {
        public int lineNumber;
        public long wonAmount;
        public int streak;
        public List<symbolStreak> symbols;
        public string symbolIndexes;
        public enum symbolStreak { };

    }
}