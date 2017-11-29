
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Sramek.FX.WPF;
using Turgo.Common.Model;
using Turgo.Controller;

namespace Turgo.ViewModel
{
    public class PlayersViewModel : BaseViewModel
    {
        private ObservableCollection<User> mPlayers;
        public ObservableCollection<User> Players
        {
            get { return mPlayers; }
            set
            {
                if (mPlayers == value) return;
                mPlayers = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoadPlayersCommand => new RelayCommand(LoadPlayers);

        public ICommand SavePlayersCommand => new RelayCommand(() => { TurgoController.I.UpdateUserBase(Players.ToList());});
        
        public PlayersViewModel()
        {
            LoadPlayers();
        }

        private void LoadPlayers()
        {
            Players = new ObservableCollection<User>(TurgoController.I.GetUserBase());
        }
    }
}
