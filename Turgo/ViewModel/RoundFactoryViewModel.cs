using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Sramek.FX.WPF;
using Sramek.FX.WPF.ViewModel;
using Turgo.Common;
using Turgo.Common.Model;
using Turgo.Controller;

namespace Turgo.ViewModel
{
    public class RoundFactoryViewModel : StandartViewModel
    {
        private int mPlayersCount;
        public int PlayersCount
        {
            get { return mPlayersCount; }
            set
            {
                if (mPlayersCount == value) return;
                mPlayersCount = value;
                OnPropertyChanged();
            }
        }

        private int mCourtCount = 2;
        public int CourtCount
        {
            get { return mCourtCount; }
            set
            {
                if (mCourtCount == value) return;
                mCourtCount = value;
                OnPropertyChanged();
            }
        }

        private bool mCanGenerate;
        public bool CanGenerate
        {
            get { return mCanGenerate; }
            set
            {
                if (mCanGenerate == value) return;
                mCanGenerate = value;
                OnPropertyChanged();
            }
        }

        private string mErrorMessage;
        public string ErrorMessage
        {
            get { return mErrorMessage; }
            set
            {
                if (mErrorMessage == value) return;
                mErrorMessage = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<User> SelectedUsers => TurgoController.I.SelectedPlayers;

        public ICommand OpenRoundCommand => new RelayCommand(() =>
            {
                var lRound = RoundFactory.CreateRound(SelectedUsers.Select(a => a.ID).ToList(), TurgoSettings.I.Model.ClassList[0],
                    DateTime.Now, CourtCount, "", "");
                BaseWindowViewModel.I.ShowTab("Round", new RoundViewModel(lRound));
                Close(this);
            }, CheckGeneratingCondition);

        public RoundFactoryViewModel()
        {
            PlayersCount = SelectedUsers.Count;
            if (PlayersCount < 8)
            {
                StandardMetroViewService.I.Message("Málo hráčů", "Vyberte alespoň osm hráču.");
                throw new Exception("Under 8 players! need more...");
            }

            CheckGeneratingCondition();
            SelectedUsers.CollectionChanged += (a, b) => CheckGeneratingCondition();
        }

        private bool CheckGeneratingCondition()
        {
            if (PlayersCount < 8)
            {
                ErrorMessage = "Not enough players";
                return false;
            }
            return true;
        }
    }
}