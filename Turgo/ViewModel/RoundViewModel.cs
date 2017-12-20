using System.Collections.ObjectModel;
using Turgo.Common.Model;

namespace Turgo.ViewModel
{
    public class RoundViewModel : StandartViewModel
    {
        public Round Round { get; }

        private ObservableCollection<GameViewModel> mGameViewModelList;
        public ObservableCollection<GameViewModel> GameViewModelList
        {
            get { return mGameViewModelList; }
            set
            {
                if (mGameViewModelList == value) return;
                mGameViewModelList = value;
                OnPropertyChanged();
            }
        }

        public RoundViewModel(Round aRound)
        {
            Round = aRound;
            GameViewModelList = new ObservableCollection<GameViewModel>(new GameViewModel[] {null, null, null});
        }


    }
}