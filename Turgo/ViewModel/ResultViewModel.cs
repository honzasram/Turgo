using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing.Design;
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
        protected readonly Dictionary<uint, DetailedUserResult> mResultDict = new Dictionary<uint, DetailedUserResult>();

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

                lBuilder.AppendLine($"Jméno; Výherních setů; Bodů získal; Bodů dostal");
                foreach (var iItem in mResultDict.Values
                    .OrderByDescending(a=>a.SetsWon)
                    .ThenByDescending(a=>a.PointsWon)
                    .ThenBy(a=>a.PointsLost))
                {
                    lBuilder.AppendLine($"{GameNameFactory.UserName(lBaseUserList.First(a => a.ID == iItem.UserId))}; " +
                                        $"{iItem.SetsWon}; " +
                                        $"{iItem.PointsWon}; " +
                                        $"{iItem.PointsLost};");
                }

                using (var lWriter = new StreamWriter(lFileName, false, Encoding.UTF8))
                {
                    lWriter.Write(lBuilder.ToString());
                }
            }
        });

        protected static void AssignSetPoint(IEnumerable<User> aUsers, Dictionary<uint, DetailedUserResult> aDict)
        {
            foreach (var iUser in aUsers.Select(a=>a.ID))
            {
                if (aDict.ContainsKey(iUser))
                {
                    aDict[iUser].SetsWon += 1;
                }
                else
                {
                    aDict.Add(iUser, new DetailedUserResult { UserId = iUser, SetsWon = 1});
                }
            }
        }

        protected static void AssignWonPoints(uint aPoints, IEnumerable<User> aUsers, Dictionary<uint, DetailedUserResult> aDict)
        {
            foreach (var iUser in aUsers.Select(a=>a.ID))
            {
                if (aDict.ContainsKey(iUser))
                {
                    aDict[iUser].PointsWon += aPoints;
                }
                else
                {
                    aDict.Add(iUser, new DetailedUserResult { UserId = iUser, PointsWon = aPoints});
                }
            }
        }

        protected static void AssignLostPoints(uint aPoints, IEnumerable<User> aUsers, Dictionary<uint, DetailedUserResult> aDict)
        {
            foreach (var iUser in aUsers.Select(a=>a.ID))
            {
                if (aDict.ContainsKey(iUser))
                {
                    aDict[iUser].PointsLost += aPoints;
                }
                else
                {
                    aDict.Add(iUser, new DetailedUserResult {UserId = iUser, PointsLost = aPoints});
                }
            }
        }
        
        protected static void AssignPoints(Game aGame, Dictionary<uint, DetailedUserResult> aDict)
        {
            var lSets = aGame.Result.Sets;
            var lSideA = aGame.SideA;
            var lSideB = aGame.SideB;
            foreach (var iSet in lSets)
            {
                AssignWonPoints((uint) iSet.SideA, lSideA, aDict);
                AssignWonPoints((uint) iSet.SideB, lSideB, aDict);
                AssignLostPoints((uint) iSet.SideB, lSideA, aDict);
                AssignLostPoints((uint) iSet.SideA, lSideB, aDict);
                if (iSet.SideA > iSet.SideB)
                {
                    AssignSetPoint(lSideA, aDict);
                }
                else if (iSet.SideA < iSet.SideB)
                {
                    AssignSetPoint(lSideB, aDict);
                }
            }
        }

        protected void ProcessRound(Round aRound)
        {
            foreach (var iGame in aRound.Games)
            {
                AssignPoints(iGame, mResultDict);
            }
        }

        protected void CreateResultList()
        {
            var lList = new List<Tuple<User, uint, uint, uint>>();
            var lBaseUserList = TurgoController.I.GetUserBase();
            foreach (var iUser in mResultDict.Keys)
            {
                var lCount = mResultDict[iUser];
                lList.Add(new Tuple<User, uint, uint, uint>(lBaseUserList.First(a => a.ID == iUser), lCount.SetsWon, lCount.PointsWon, lCount.PointsLost));
            }
            RoundResultList = new ObservableCollection<Tuple<User, string>>(
                lList.OrderByDescending(a => a.Item2)
                    .ThenByDescending(a=>a.Item3)
                    .ThenBy(a=>a.Item4)
                    .Select(a => new Tuple<User, string>(a.Item1, $"{a.Item1.Name} {a.Item1.Surname} - {a.Item2} výher - {a.Item3}:{a.Item4} bodů"))
                    .ToList());
        }
    }

    public class DetailedUserResult
    {
        public uint UserId { get; set; }
        public uint SetsWon { get; set; }
        public uint PointsWon { get; set; }
        public uint PointsLost { get; set; }
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
