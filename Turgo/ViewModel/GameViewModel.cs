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

        private Round Round => Game.Parent as Round;

        #region NotifiedProperties
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

        public string Slash { get; } = " / "; 
        #endregion

        public ICommand EndGameCommand => new RelayCommand(() =>
        {
            Game.Finished = true;
            FinishingAction?.Invoke(this);
        });

        public ICommand PointCommand => new RelayCommand<char>(a =>
        {
            switch (a)
            {
                case 'A':
                    Game.Result.Sets.ForEach(b=>b.SideA = Round.MaximumPointsPerSet);
                    Game.Result.Sets.ForEach(b=>b.SideB = 0);
                    break;
                case 'R':
                    Game.Result.Sets[0].SideA = Round.MaximumPointsPerSet;
                    Game.Result.Sets[1].SideB = Round.MaximumPointsPerSet;
                    Game.Result.Sets[1].SideA = 0;
                    Game.Result.Sets[0].SideB = 0;
                    break;
                case 'B':
                    Game.Result.Sets.ForEach(b => b.SideA = 0);
                    Game.Result.Sets.ForEach(b => b.SideB = Round.MaximumPointsPerSet);
                    break;
                default:
                    throw new Exception("Unrecognized parameter");
            }
            OnPropertyChanged("Game");
        });

        public GameViewModel(Game aGame)
        {
            if(aGame == null) throw new ArgumentNullException(nameof(aGame));
            Game = aGame;

            try
            {
                A1 = $"{Game.SideA[0].Name} {Game.SideA[0].Surname}";
                A2 = $"{Game.SideA[1].Name} {Game.SideA[1].Surname}";
                B1 = $"{Game.SideB[0].Name} {Game.SideB[0].Surname}";
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