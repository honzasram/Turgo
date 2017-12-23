using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Sramek.FX.WPF;
using Sramek.FX.WPF.ViewModel;
using Turgo.Common.Model;
using Turgo.Controller;

namespace Turgo.ViewModel
{
    public class BaseResultViewModel : StandartViewModel
    {
        protected readonly Dictionary<uint, uint> mResultDict = new Dictionary<uint, uint>();

        private ObservableCollection<Tuple<User, string>> mRoundResultList;
        public ObservableCollection<Tuple<User, string>> RoundResultList
        {
            get { return mRoundResultList; }
            set
            {
                if (mRoundResultList == value) return;
                mRoundResultList = value;
                OnPropertyChanged();
            }
        }

        public ICommand ExportCommand => new RelayCommand(() =>
        {
            string lFileName;
            if (StandardMetroViewService.I.SaveFileDialog(out lFileName, ".csv", "CSV|*.csv"))
            {
                var lBuilder = new StringBuilder();
                var lBaseUserList = TurgoController.I.GetUserBase();

                foreach (var iItem in mResultDict.Keys)
                {
                    lBuilder.AppendLine($"{GameNameFactory.UserName(lBaseUserList.First(a => a.ID == iItem))}; {mResultDict[iItem]}");
                }

                using (var lWriter = new StreamWriter(lFileName, false))
                {
                    lWriter.Write(lBuilder.ToString());
                }
            }
        });

        protected static void AssignPoint(IEnumerable<User> aUsers, Dictionary<uint, uint> aDict)
        {
            foreach (var iUser in aUsers.Select(a=>a.ID))
            {
                if (aDict.ContainsKey(iUser))
                {
                    aDict[iUser] += 1;
                }
                else
                {
                    aDict.Add(iUser, 1);
                }
            }
        }

        protected void ProcessRound(Round aRound)
        {
            foreach (var iGame in aRound.Games)
            {
                if (iGame.Result.ASideWon)
                {
                    AssignPoint(iGame.SideA, mResultDict);
                }
                else if (iGame.Result.BSideWon)
                {
                    AssignPoint(iGame.SideB, mResultDict);
                }
            }
        }

        protected void CreateResultList()
        {
            var lList = new List<Tuple<User, uint>>();
            var lBaseUserList = TurgoController.I.GetUserBase();
            foreach (var iUser in mResultDict.Keys)
            {
                var lCount = mResultDict[iUser];
                lList.Add(new Tuple<User, uint>(lBaseUserList.First(a=>a.ID == iUser), lCount));
            }
            RoundResultList = new ObservableCollection<Tuple<User, string>>(
                lList.OrderByDescending(a => a.Item2)
                    .Select(a => new Tuple<User, string>(a.Item1, $"{a.Item1.Name} {a.Item1.Surname} - {a.Item2} výher"))
                    .ToList());
        }
    }

    public class ResultViewModel : BaseResultViewModel
    {
        private Round Round { get; }

        public ICommand ShowTotalResultsCommnad => new RelayCommand(() =>
        {
            BaseWindowViewModel.I.ShowTab("TotalResult");
        });

        public ResultViewModel(Round aRound)
        {
            Round = aRound;
            ProcessRound(aRound);
            CreateResultList();
        }
    }

    public class TotalResultsViewModel : BaseResultViewModel
    {
        public TotalResultsViewModel()
        {
            var lRoundList = TurgoController.I.GetAllRounds();

            foreach (var iRound in lRoundList)
            {
                ProcessRound(iRound);
            }
            CreateResultList();
        }
    }
}
