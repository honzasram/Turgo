using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Sramek.FX;
using Sramek.FX.WPF;
using Sramek.FX.WPF.ViewModel;
using Turgo.Common.Model;
using Turgo.Controller;

namespace Turgo.ViewModel
{
    public class RoundViewModel : StandartViewModel
    {
        public Round Round { get; }

        private FullyObservableCollection<GameViewModel> mGameViewModelList;
        public FullyObservableCollection<GameViewModel> GameViewModelList
        {
            get { return mGameViewModelList; }
            set
            {
                if (mGameViewModelList == value) return;
                mGameViewModelList = value;
                OnPropertyChanged();
            }
        }

        private FullyObservableCollection<Game> mGames;
        public FullyObservableCollection<Game> Games
        {
            get { return mGames; }
            set
            {
                if (mGames == value) return;
                mGames = value;
                OnPropertyChanged();
            }
        }

        private Game mSelectedGame;
        public Game SelectedGame
        {
            get { return mSelectedGame; }
            set
            {
                if (mSelectedGame == value) return;
                mSelectedGame = value;
                OnPropertyChanged();
            }
        }

        public ICommand SelectGameCommand => new RelayCommand(() =>
        {
            InsertSelectedGame(SelectedGame);
        });

        public ICommand EndRoundCommand => new RelayCommand(() =>
        {
            if (StandardMetroViewService.I.OkCancelQuestion("Pozor", "Chete opravdu ukončit celé kolo?"))
            {
                TurgoController.I.SaveRound(Round);
                BaseWindowViewModel.I.ShowTab("Result", new ResultViewModel(Round));
                Close(this);
            }
        });

        public RoundViewModel(Round aRound)
        {
            Round = aRound;
            var GameList = Enumerable.Repeat(GameViewModel.GetEmpty(), Round.CourtCount).ToList();
            GameViewModelList = new FullyObservableCollection<GameViewModel>(GameList);
            Round.Games.ForEach(a=>
            {
                a.Result.Sets = new List<Set>();
                for (int i = 0; i < Round.SetCountPerGame; i++)
                    a.Result.Sets.Add(new Set());
            });

            Games = new FullyObservableCollection<Game>(Round.Games);
        }
        
        private void InsertSelectedGame(Game aGame)
        {
            var lVM = new GameViewModel(aGame) { FinishingAction = ClearFinishedGame };
            
            for (int i = 0; i < GameViewModelList.Count; i++)
            {
                if (GameViewModelList[i] == null)
                {
                    GameViewModelList[i] = lVM;
                    return;
                }
                if (GameViewModelList[i].Game.Finished)
                {
                    GameViewModelList[i] = lVM;
                    return;
                }
            }
            StandardMetroViewService.I.Message("Pozor", "Není volný kurt pro další hru. Ukončete předešlé a zkuste to znova.");
        }

        private void ClearFinishedGame(GameViewModel aVm)
        {
            var lIndex = GameViewModelList.IndexOf(aVm);
            GameViewModelList[lIndex] = GameViewModel.GetEmpty();
        }
    }
}