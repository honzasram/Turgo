using System.Reflection;
using Dragablz;
using log4net;
using Sramek.FX.WPF.ViewModel;

namespace Turgo.ViewModel
{
    public abstract class BaseViewModel : HeaderedItemViewModel
    {
        protected static ILog mLog = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public void Close(object aToClose)
        { 
            BaseWindowViewModel.I.CloseTab(this);
        }
    }
}