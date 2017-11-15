using System.Windows.Input;
using Sramek.FX.WPF;
using Sramek.FX.WPF.ViewModel;
using Turgo.View;

namespace Turgo.ViewModel
{
    public class MainWindowViewModel : BaseWindowViewModel
    {
        public string Cosi { get; set; }

        public ICommand ShowPlayersCommand { get; }

        public MainWindowViewModel()
        {
            ShowPlayersCommand = new RelayCommand(() => { ShowTab("Players"); });
            mFactoryFuncDictionary.Add("Players", () => new TabContent(TurgoLoc.I.Players, new PlayersView()));
        }
    }
}