using Sramek.FX.WPF.ViewModel;

namespace Turgo.ViewModel
{
    public abstract class StandartViewModel : BaseViewModel
    {
        public void Close(object aToClose)
        {
            BaseWindowViewModel.I.CloseTab(this);
        }
    }
}