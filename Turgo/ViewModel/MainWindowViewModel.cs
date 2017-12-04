using Sramek.FX.WPF.ViewModel;
using Turgo.View;

namespace Turgo.ViewModel
{
    public class MainWindowViewModel : BaseWindowViewModel
    {
        public MainWindowViewModel()
        {
            mFactoryFuncDictionary.Add("Players", () => new TabContent(TurgoLoc.I.Players, new PlayersView()));
            mFactoryFuncDictionary.Add("RoundFactory", () => new TabContent(TurgoLoc.I.Round, new RoundFactoryView()));
            mFactoryFuncDictionary.Add("Round", () => new TabContent(TurgoLoc.I.Round, new RoundFactoryView()));
            if(mInstanceCount == 0) ShowTab("Players");
            mInstanceCount++;
        }
    }
}