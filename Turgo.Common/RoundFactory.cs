using System.Reflection;
using log4net;

namespace Turgo.Common
{
    public static class RoundFactory
    {
        private static readonly ILog mLog = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static Round CreateRound()
        {
            return null;
        }
    }
}