using Sramek.FX.WPF.ViewModel;
using Turgo.View;

namespace Turgo.ViewModel
{
    public class MainWindowViewModel : BaseWindowViewModel
    {
        public MainWindowViewModel()
        {
            mFactoryFuncDictionary.Add("Players", a => new TabContent(TurgoLoc.I.Players, new PlayersView()));
            mFactoryFuncDictionary.Add("RoundFactory", a => new TabContent(TurgoLoc.I.Round, new RoundFactoryView()));
            mFactoryFuncDictionary.Add("Round", a => new TabContent(TurgoLoc.I.Round, new RoundView((RoundViewModel)a)));
            mFactoryFuncDictionary.Add("Result", a => new TabContent(TurgoLoc.I.Result, new ResultsView((ResultViewModel)a)));
            mFactoryFuncDictionary.Add("TotalResult", a => new TabContent(TurgoLoc.I.TotalResults, new TotalResultsView()));
            if(mInstanceCount == 0) ShowTab("Players");
            mInstanceCount++;
        }
    }
}