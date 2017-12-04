using System.Reflection;
using Dragablz;
using log4net;

namespace Turgo.ViewModel
{
    public abstract class BaseViewModel : HeaderedItemViewModel
    {
        protected static ILog mLog = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    }
}