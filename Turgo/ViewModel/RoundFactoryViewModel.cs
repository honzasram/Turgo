using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Sramek.FX;
using Sramek.FX.WPF;
using Sramek.FX.WPF.ViewModel;
using Turgo.Common;
using Turgo.Common.Model;
using Turgo.Controller;

namespace Turgo.ViewModel
{
    public class RoundFactoryViewModel : StandartViewModel
    {
        #region NotifiedProperties
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

        private DateTime mRoundDate;
        public DateTime RoundDate
        {
            get { return mRoundDate; }
            set
            {
                if (mRoundDate == value) return;
                mRoundDate = value;
                OnPropertyChanged();
            }
        }

        private int mRoundNo;
        public int RoundNo
        {
            get { return mRoundNo; }
            set
            {
                if (mRoundNo == value) return;
                mRoundNo = value;
                OnPropertyChanged();
            }
        } 
        #endregion

        public ObservableCollection<User> SelectedUsers => TurgoController.I.SelectedPlayers;

        public ICommand OpenRoundCommand => new RelayCommand(() =>
        {
            Round lRound = null;
            try
            {
                lRound = RoundFactory.CreateRound2(
                    SelectedUsers.Select(a => a.ID).ToList(),
                    TurgoSettings.I.Model.ClassList[TurgoSettings.I.SelectedClassIndex],
                    DateTime.Now, CourtCount,
                    "",
                    "");
            }
            //catch (NowEvenCountException)
            //{
            //    var lUneven = StandardMetroViewService.I.OkCancelQuestion("Pozor", "Tento počet kurtů a hráčů nelze kombinovat. Jeden hráč bude v nevýhodě a bude mít o kolo méně. Checte přesto pokračovat?");
            //    if (lUneven)
            //    {
            //        lRound = RoundFactory.CreateRound(
            //            SelectedUsers.Select(a => a.ID).ToList(),
            //            TurgoSettings.I.Model.ClassList[TurgoSettings.I.SelectedClassIndex],
            //            DateTime.Now, CourtCount,
            //            "",
            //            "", true);
            //    }
            //}
            catch (Exception e)
            {
                mLog.Error(Messages.BuildErrorMessage(e));
                return;
            }
            if(lRound == null) throw new Exception("Generated null. What the....");

            lRound.DateTime = RoundDate;
            lRound.Number = RoundNo;

            BaseWindowViewModel.I.ShowTab("Round", new RoundViewModel(lRound));
            Close(this);
        }, CheckGeneratingCondition);

        public RoundFactoryViewModel()
        {
            PlayersCount = SelectedUsers.Count;
            RoundDate = DateTime.Now;
            if (PlayersCount < 8)
            {
                StandardMetroViewService.I.Message("Málo hráčů", "Vyberte alespoň osm hráču.");
                throw new Exception("Under 8 players! need more...");
            }

            CourtCount = CourtCountCalc(PlayersCount, 4);
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

        private static int CourtCountCalc(int aPlayers, int aPlayersPerGame)
        {
            int lCnt = 1;
            while (((aPlayers/(double)lCnt)-aPlayersPerGame)*lCnt >= aPlayersPerGame)
            {
                lCnt++;
            }

            return lCnt;
        }
    }
}