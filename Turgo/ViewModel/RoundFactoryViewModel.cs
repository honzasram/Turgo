using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Sramek.FX.WPF;
using Turgo.Common;
using Turgo.Common.Model;

namespace Turgo.ViewModel
{
    public class RoundFactoryViewModel : BaseViewModel
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

        public ObservableCollection<User> SelectedUsers => new ObservableCollection<User>(TurgoSettings.I.BaseClassConfiguration.UserBaseList.Where(a=>a.IsSelected).ToList());

        public ICommand PrintPDFCommand => new RelayCommand(() =>
            {
                mLog.Info("Printing PDF...");
            }, 
            () =>
            {
                CheckGeneratingCondition();
                return CanGenerate;
            });

        public RoundFactoryViewModel()
        {
            PlayersCount = SelectedUsers.Count;
            CheckGeneratingCondition();
            if (!CanGenerate)
            {
                throw new Exception(ErrorMessage);
            }
        }

        private void CheckGeneratingCondition()
        {
            if (PlayersCount < 8)
            {
                CanGenerate = false;
                ErrorMessage = "Not enough players";
                return;
            }
        }
    }
}