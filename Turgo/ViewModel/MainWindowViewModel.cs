using System.Windows.Input;
using Sramek.FX.WPF;
using Sramek.FX.WPF.ViewModel;
using Turgo.View;

namespace Turgo.ViewModel
{
    public class MainWindowViewModel : BaseWindowViewModel
    {
        public ICommand ShowPlayersCommand => new RelayCommand(() => { ShowTab("Players"); });
        public ICommand NewRoundCommand => new RelayCommand(() => { ShowTab("Round"); });

        public MainWindowViewModel()
        {
            mFactoryFuncDictionary.Add("Players", () => new TabContent(TurgoLoc.I.Players, new PlayersView()));
            mFactoryFuncDictionary.Add("Round", () => new TabContent(TurgoLoc.I.Round, new RoundFactoryView()));
            if(mInstanceCount == 0) ShowTab("Players");
            mInstanceCount++;
        }
    }
}