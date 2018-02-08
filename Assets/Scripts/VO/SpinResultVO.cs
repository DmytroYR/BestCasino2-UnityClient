using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.VO
{
    public class SpinResultVO
    {
        public long totalWon = 0;
        public string[] symbolsList;
        public int freeSpinMultiplier = 0;
        public long scatter; //  count of scatter images
        public List<LineWinResult> lines;
        public string specialSym;
        public long totalBonusLvl;
    }
}
