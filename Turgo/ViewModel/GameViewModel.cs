using System;
using System.Windows.Input;
using Sramek.FX;
using Sramek.FX.WPF;
using Turgo.Common.Model;

namespace Turgo.ViewModel
{
    public class GameViewModel : BaseViewModel
    {
        public Game Game { get; }
        public Action<GameViewModel> FinishingAction { get; set; }

        private string mA1;
        public string A1
        {
            get { return mA1; }
            set
            {
                if (mA1 == value) return;
                mA1 = value;
                OnPropertyChanged();
            }
        }

        private string mA2;
        public string A2
        {
            get { return mA2; }
            set
            {
                if (mA2 == value) return;
                mA2 = value;
                OnPropertyChanged();
            }
        }

        private string mB1;
        public string B1
        {
            get { return mB1; }
            set
            {
                if (mB1 == value) return;
                mB1 = value;
                OnPropertyChanged();
            }
        }

        private string mB2;
        public string B2
        {
            get { return mB2; }
            set
            {
                if (mB2 == value) return;
                mB2 = value;
                OnPropertyChanged();
            }
        }
        
        //private int mSideAValue = 0;
        //public int SideAValue
        //{
        //    get { return mSideAValue; }
        //    set
        //    {
        //        if (mSideAValue == value) return;
        //        mSideAValue = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private int mSideBValue = 0;
        //public int SideBValue
        //{
        //    get { return mSideBValue; }
        //    set
        //    {
        //        if (mSideBValue == value) return;
        //        mSideBValue = value;
        //        OnPropertyChanged();
        //    }
        //}

        public ICommand WonCommand => new RelayCommand<string>(a =>
        {
            if (a == "A")
            {
                Game.Result.ASideWon = true;
                Game.Result.BSideWon = false;
            }
            else if(a == "B")
            {
                Game.Result.ASideWon = false;
                Game.Result.BSideWon = true;
            }
        });

        //public ICommand NewSetCommand => new RelayCommand(() =>
        //{
        //    var lSet = new Set {SideA = SideAValue, SideB = SideBValue};
        //    Game.Result.Sets.Add(lSet);

        //    SideAValue = 0;
        //    SideBValue = 0;
        //});

        public ICommand EndGameCommand => new RelayCommand(() =>
        {
            Game.Finished = true;
            FinishingAction?.Invoke(this);
        });

        public GameViewModel(Game aGame)
        {
            if(aGame == null) throw new ArgumentNullException(nameof(aGame));
            Game = aGame;

            try
            {
                A1 = $"{Game.SideA[0].Name} {Game.SideA[0].Surname}";
                A2 = $"{Game.SideA[1].Name} {Game.SideA[1].Surname}";
                B1 = $"{Game.SideA[0].Name} {Game.SideA[0].Surname}";
                B2 = $"{Game.SideB[1].Name} {Game.SideB[1].Surname}";
            }
            catch (Exception e)
            {
                mLog.Error(Messages.BuildErrorMessage(e));
            }
        }

        public static GameViewModel GetEmpty()
        {
            return new GameViewModel(new Game() {Finished = true});
        }
    }
}