using System.Collections.Generic;

namespace Assets.Scripts.VO
{
    public class LineWinResult
    {
        public int lineNumber;
        public long wonAmount;
        public int streak;
        public List<symbolStreak> symbols;
        public string symbolIndexes;
        public enum symbolStreak { };

    }
}