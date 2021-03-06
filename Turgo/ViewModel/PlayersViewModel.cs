﻿using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Sramek.FX;
using Sramek.FX.WPF;
using Turgo.Common.Model;
using Turgo.Controller;

namespace Turgo.ViewModel
{
    public class PlayersViewModel : StandartViewModel
    {
        private FullyObservableCollection<User> mPlayers;
        public FullyObservableCollection<User> Players
        {
            get { return mPlayers; }
            set
            {
                if (mPlayers == value) return;
                mPlayers = value;
                OnPropertyChanged();
                UpdateSelected();
            }
        }

        private bool mUserBaseSelected;
        public bool UserBaseSelected
        {
            get { return mUserBaseSelected; }
            set
            {
                if (mUserBaseSelected == value) return;
                mUserBaseSelected = value;
                TurgoController.I.SelectedPlayers.Clear();
                LoadPlayers();
                OnPropertyChanged();
            }
        }

        public ICommand LoadPlayersCommand => new RelayCommand(LoadPlayers);

        public ICommand SavePlayersCommand => new RelayCommand(() =>
        {
            if (UserBaseSelected)
            {
                TurgoController.I.UpdateStandardUserBase(Players.ToList());
            }
            else
                TurgoController.I.UpdateUserBase(Players.ToList());
            TurgoController.I.SelectedPlayers = null;
            LoadPlayers();
        });
        
        public ICommand UserBaseCommand => new RelayCommand(LoadPlayers);

        public PlayersViewModel()
        {
            LoadPlayers();
        }

        private void LoadPlayers()
        {
            User.No = 1;
            if (UserBaseSelected)
            {
                Players = new FullyObservableCollection<User>(TurgoController.I.GetStandardUserBase());
            }
            else
                Players = new FullyObservableCollection<User>(TurgoController.I.GetUserBase());
            Players.ItemPropertyChanged += (a, b) => UpdateSelected();
        }

        private void UpdateSelected()
        {
            if (TurgoController.I.SelectedPlayers == null)
            {
                TurgoController.I.SelectedPlayers =
                    new ObservableCollection<User>(Players.Where(a => a.IsSelected));
            }
            else
            {
                foreach (var iUser in Players.Where(a=>a.IsSelected))
                {
                    if (!TurgoController.I.SelectedPlayers.Contains(iUser))
                    {
                        TurgoController.I.SelectedPlayers.Add(iUser);
                    }
                }

                foreach (var iDeletableItem in TurgoController.I.SelectedPlayers.Where(a => !a.IsSelected).ToList())
                {
                    TurgoController.I.SelectedPlayers.Remove(iDeletableItem);
                }
            }
        }
    }
}
