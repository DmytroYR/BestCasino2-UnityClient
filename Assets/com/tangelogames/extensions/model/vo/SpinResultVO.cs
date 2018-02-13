
using Sfs2X.Protocol.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sfs2X.Entities.Data;

namespace com.tangelogames.extensions.model.vo
{
    public class SpinResultVO : SerializableSFSType
    {

        public SpinResultVO()
        {

        }

        public long totalWon = 0;
        public int[] symbolsList;
        public int freeSpinMultiplier = 0;
        public long scatter; //  count of scatter images
        public List<LineWinResult> lines = new List<LineWinResult>();
        public string specialSym;
        public long totalBonusLvl;
    }
}

