using System;
using System.Windows.Input;
using Sramek.FX.WPF;
using Turgo.Common.Model;

namespace Turgo.ViewModel
{
    public class GameViewModel : BaseViewModel
    {
        public Game Game { get; }

        private int mSideAValue;
        public int SideAValue
        {
            get { return mSideAValue; }
            set
            {
                if (mSideAValue == value) return;
                mSideAValue = value;
                OnPropertyChanged();
            }
        }

        private int mSideBValue;
        public int SideBValue
        {
            get { return mSideBValue; }
            set
            {
                if (mSideBValue == value) return;
                mSideBValue = value;
                OnPropertyChanged();
            }
        }

        public ICommand NewSetCommand => new RelayCommand(() =>
        {
            var lSet = new Set {SideA = SideAValue, SideB = SideBValue};
            Game.Result.Sets.Add(lSet);

            SideAValue = 0;
            SideBValue = 0;
        });

        public GameViewModel(Game aGame)
        {
            if(aGame == null) throw new ArgumentNullException(nameof(aGame));
            Game = aGame;
        }
    }
}