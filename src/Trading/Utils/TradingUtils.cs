using System;

namespace Trading.Utils
{
    public static class TradingUtils
    {
        public static int GetAffinity(int sourceProposeMatch, int targetSearchMatch)
        {
            var matchingCount = Math.Min(sourceProposeMatch, targetSearchMatch);

            var removedMatchPartSource = sourceProposeMatch - matchingCount;
            var removedMatchPartTarget = targetSearchMatch - matchingCount;

            return matchingCount * 2 + removedMatchPartSource + removedMatchPartTarget;
        }
    }
}
