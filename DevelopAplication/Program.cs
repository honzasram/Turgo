using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using log4net.Util;
using Turgo.Common;
using Turgo.Common.Model;

namespace DevelopAplication
{
    class Program
    {
        private static ILog mLog = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static Random mRandom = new Random((int) (DateTime.Now.Ticks /5));

        static void Main(string[] args)
        {
            var lCount = 15;
            var lRoundCount = 50;
            
            GenerujMore(lCount, lRoundCount);

            /**
            lSelectedPlayers = lArrList.GetListExceptCountFromList(lSitting, lDict.GetMaxOrEqualRandom(lSitting));

            // vygeneruj nove kombinace po tuto partu lidi. 
            lCombinations = GetKCombs(lSelectedPlayers, 2).ToList();

            //for (int i = 0; i < 80; i++)
            do
            {
                var lGameList = new List<IEnumerable<int>>();
                lCombinations = GetKCombs(lSelectedPlayers, 2).ToList();
                do
                {
                    mLog.Info("Continue by key....\n\n\n");
                    //Console.ReadKey();
                    List<int> lUnique = new List<int>();
                    /**
                    if (lDict.Values.Any())
                    {
                        mLog.Info("DictValues");
                        if (lDict.Values.Min() != lDict.Values.Max())
                        {
                            mLog.Info("Values diff");
                            var lMax = lDict.Values.Max();
                            var lInts = new List<int>();
                            foreach (var iKey in lDict.Keys)
                            {
                                if (lDict[iKey] == lMax)
                                {
                                    lInts.Add(iKey);
                                }
                            }
                            
                            lUnique = lRoundList.GetLIstOfItemsUnique();
                            lUnique.AddRange(lInts);
                            lUnique.AddRange(lGameList.GetLIstOfItemsUnique());
                        }
                        else
                        {
                            mLog.Info("Values consist");
                            lUnique = lRoundList.GetLIstOfItemsUnique();
                            lUnique.AddRange(lGameList.GetLIstOfItemsUnique());
                        }
                    }
                    else
                    {//
                    mLog.Info("Default selection");
                    lUnique = lRoundList.GetLIstOfItemsUnique();
                    lUnique.AddRange(lGameList.GetLIstOfItemsUnique());
                    //}
                    
                    lUnique = lUnique.OrderBy(a => a).Distinct().ToList();
                    var lFreeCombinations = lCombinations.RemoveItemsWithSubitemList(lUnique);

                    if (lFreeCombinations.Count == 0)
                    {
                        
                        mLog.Info("Clearing due 0 free conbinations");
                        lRoundList.Clear();
                        lGameList.Clear();
                        lDict.Clear();
                        continue;
                    }

                    var lGamePair = lFreeCombinations.GetRandomItem();
                    if (lGamePair.Any(a => lGameList.ItemContainsTypeItem(a)) ||
                        lGamePair.Any(a => lRoundList.ItemContainsTypeItem(a)))
                    {
                        mLog.Error($"Selected pair {lGamePair.Select(a => $"{a}").Aggregate((a, b) => $"{a}|{b}")}");
                    }
                    else
                    {
                        mLog.Info("Adding");
                        lGameList.Add(lGamePair);
                    }
                    
                } while (lGameList.Count < 2);

                lGameList.ForEach(a => lRoundList.Add(a));
                lGameList.ForEach(a => lGlobalRoundList.Add(a));


                lDict.Clear();
                lGameList.Clear();
                lArrList.ForEach(a=> lDict.Add(a,0));
                lGlobalRoundList.ToList().ForEach(a=>a.ToList().ForEach(b => lDict[b]++));
                //dodej lsit lidi kteri si maji jit sednout;
                
                lSelectedPlayers = lArrList.GetListExceptCountFromList(lSitting, lDict.GetMaxOrEqualRandom(lSitting));

                //// vygeneruj nove kombinace po tuto partu lidi. 
                //lCombinations = GetKCombs(lSelectedPlayers, 2).ToList();
            } while (lDict.Values.Min() != lDict.Values.Max() || lDict.Values.Min() < lRoundCount);
            
            //ChopAndSuey(lGlobalRoundList);

            lDict.Clear();
            lArrList.ForEach(a=> lDict.Add(a,0));
            lGlobalRoundList.ToList().ForEach(a=>a.ToList().ForEach(b => lDict[b]++));
            mLog.Info("\n" + lDict.Keys.ToList().Select(a=> $"{a}: {lDict[a]}").Aggregate((a,b)=> $"{a}\n{b}"));//*/

            Console.ReadKey();
            return;
            
            #region Bordel
            //var lArr = lArrList.ToArray(); //new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            //while (lList.Count < Combinatorics.Combinations(lCount, 2))
            //{
            //    var lPair = lArr.SelectCombination(2, lRandom).ToArray();

            //    if (lPair[0] > lPair[1])
            //    {
            //        var lPom = lPair[0];
            //        lPair[0] = lPair[1];
            //        lPair[1] = lPom;
            //    }

            //    if (!lList.Any(a => a.Item1 == lPair[0] && a.Item2 == lPair[1]))
            //    {
            //        lList.Add(new Tuple<int, int>(lPair[0], lPair[1]));
            //    }
            //}

            //mLog.Info(lList.OrderBy(a => a.Item1).ThenBy(a => a.Item2).Select(a => $"{a.Item1}|{a.Item2}").Aggregate((a, b) => $"{a}\n{b}"));

            while (true)
            {
                var lClass = new Class
                {
                    UserBase = new List<User>
                {
                    new User {ID = 0,  Name = "A"},
                    new User {ID = 1,  Name = "B"},
                    new User {ID = 2,  Name = "C"},
                    new User {ID = 3,  Name = "D"},
                    new User {ID = 4,  Name = "E"},
                    new User {ID = 5,  Name = "F"},
                    new User {ID = 6,  Name = "G"},
                    new User {ID = 7,  Name = "H"},
                    new User {ID = 8,  Name = "I"},
                    new User {ID = 9,  Name = "J"},
                    new User {ID = 10, Name = "K"},
                    new User {ID = 11, Name = "L"},
                    new User {ID = 12, Name = "M"},
                    new User {ID = 13, Name = "N"},
                    new User {ID = 14, Name = "O"},
                    new User {ID = 15, Name = "P"},
                    new User {ID = 16, Name = "Q"},
                    new User {ID = 17, Name = "R"},
                    new User {ID = 18, Name = "S"},
                    new User {ID = 19, Name = "T"},
                    new User {ID = 20, Name = "U"},
                }
                };
                var lPla = 16;
                var lRound = RoundFactory.CreateRound2(lClass.UserBase.Take(lPla).Select(a => a.ID).ToList(), lClass, DateTime.Now, CourtCountCalc(lPla), "", "", 5);


                var lSelected = lRound.Games.Select(a => a.SideA.Select(b => b.ID.ToString()).Aggregate((c, b) => c + ":" + b)).ToList();
                lSelected.AddRange(lRound.Games.Select(a => a.SideB.Select(b => b.ID.ToString()).Aggregate((c, b) => c + ":" + b)));

                var lDictionary = new Dictionary<string, int>();
                var lGrouped = lSelected.GroupBy(a => a);
                foreach (var iGroup in lGrouped)
                {
                    if (lDictionary.ContainsKey(iGroup.Key))
                    {
                        lDictionary[iGroup.Key] += iGroup.Count();
                    }
                    else
                    {
                        lDictionary.Add(iGroup.Key, iGroup.Count());
                    }
                }

                foreach (var iKey in lDictionary.Keys.OrderBy(a => a))
                {
                    if (lDictionary[iKey] > 1) Console.ForegroundColor = ConsoleColor.Red;
                    else Console.ResetColor();
                    Console.WriteLine($"{iKey} - {lDictionary[iKey]}");
                }

                Console.ReadKey();
                Console.Clear(); 
                #endregion
            }
        }

        private static void GenerujMore(int lCount, int lRoundCount)
        {
            var lCourts = lCount / 4;
            var lSitting = Convert.ToInt32((lCount / (double) lCourts - 4) * lCourts);

            var lRandom = new Random();
            var lArrList = new List<int>();
            for (var i = 1; i < lCount + 1; i++) lArrList.Add(i);

            var lDict = new Dictionary<int, int>();
            lArrList.ForEach(a => lDict.Add(a, 0));
            var lRoundList = new List<IEnumerable<int>>();
            var lGlobalRoundList = new List<IEnumerable<int>>();
            var lFirstRun = true;
            do
            {
                var lAdepti = lArrList.Where(a => !lDict.GetKeysIfMinValue().Contains(a)).ToList();
                if (lAdepti.Count == 0) lAdepti = lArrList.ToList();
                var lSelectedPlayers = lArrList.ToList();
                lAdepti.ForEach(a => lSelectedPlayers.Remove(a));

                if (lSelectedPlayers.Count < 2 && lFirstRun)
                {
                    lSelectedPlayers = lArrList.ToList();
                    for (var i = 0; i < lSitting; i++)
                    {
                        var lAdept = lAdepti[lRandom.Next(lAdepti.Count)];
                        lSelectedPlayers.Remove(lAdept);
                        lAdepti.Remove(lAdept);
                    }
                }

                if (lSelectedPlayers.Count < 4)
                {
                    var l = lSelectedPlayers.Count;
                    for (var i = 0; i < 4 - l; i++)
                    {
                        var lAdept = lAdepti[lRandom.Next(lAdepti.Count)];
                        lSelectedPlayers.Add(lAdept);
                        lAdepti.Remove(lAdept);
                    }
                }

                var lCombinations = GetKCombs(lSelectedPlayers, 2).ToList();

                var lGameList = new List<IEnumerable<int>>();
                do
                {
                    var lUnique = lRoundList.GetLIstOfItemsUnique();
                    lUnique.AddRange(lGameList.GetLIstOfItemsUnique());

                    lUnique = lUnique.OrderBy(a => a).Distinct().ToList();
                    var lFreeCombinations = lCombinations.RemoveItemsWithSubitemList(lUnique);

                    var lGamePair = lFreeCombinations.GetRandomItem().ToList();

                    if (lGamePair.Any(a => lGameList.ItemContainsTypeItem(a)) ||
                        lGamePair.Any(a => lRoundList.ItemContainsTypeItem(a)))
                    {
                        mLog.Error($"Selected pair {lGamePair.Select(a => $"{a}").Aggregate((a, b) => $"{a}|{b}")}");
                    }
                    else
                    {
                        var lFirst = lGamePair.First();
                        var lLast = lGamePair.Last();
                        if (lDict[lLast] > lDict.Values.Min() + 1 ||lDict[lFirst] > lDict.Values.Min() + 1 )
                        {
                            mLog.Warn("checkni to");
                        }
                        lGameList.Add(lGamePair);
                    }
                } while (lGameList.Count < 2);

                lGameList.ForEach(a => lGlobalRoundList.Add(a));

                lDict.Clear();
                lArrList.ForEach(a => lDict.Add(a, 0));
                lGlobalRoundList.ToList().ForEach(a => a.ToList().ForEach(b => lDict[b]++));
                lFirstRun = false;
            } while (lDict.Values.Min() != lDict.Values.Max() || lDict.Values.Min() < lRoundCount);

            mLog.Info("\n" + lDict.Keys.ToList().Select(a => $"{a}: {lDict[a]}").Aggregate((a, b) => $"{a}\n{b}"));

            var lDict2 = CheckDuplicity(lGlobalRoundList);
            mLog.Info("\n" + lDict2.Keys
                          .OrderBy(a=>a.Item1)
                          .ThenBy(a=>a.Item2)
                          .ToList()
                          .Select(a => $"{a.Item1},{a.Item2}: {lDict2[a]}").Aggregate((a, b) => $"{a}\n{b}"));

        }

        static Dictionary<Tuple<int, int>, int> CheckDuplicity(List<IEnumerable<int>> aListOfGames)
        {
            var lDict = new Dictionary<Tuple<int, int>, int>();

            foreach (var iGame in aListOfGames)
            {
                var lTuple = new Tuple<int,int>(iGame.ToList()[0],iGame.ToList()[1]);
                Tuple<int, int> lKey;
                if(lDict.Keys.Any(a=>a.Item1 == lTuple.Item1 && a.Item2 == lTuple.Item2))
                {
                    lKey = lDict.Keys.First(a => a.Item1 == lTuple.Item1 && a.Item2 == lTuple.Item2);
                }
                else
                {
                    lKey = lTuple;
                    lDict.Add(lKey,0);
                }
                lDict[lKey]++;
            }

            return lDict;
        }

        static void ChopAndSuey(List<IEnumerable<int>> aListOfGames)
        {
            string lStr = "";
            for (int i = 0; i < aListOfGames.Count/2; i++)
            {
                var lTaken = aListOfGames.Skip(i * 2).Take(2).Select(a=>a.ToList()).ToList();
                lStr += $"\n{lTaken[0][0]}|{lTaken[0][1]}:{lTaken[1][0]}|{lTaken[1][1]}";
            }
            mLog.Info(lStr);
        }
        
        static IEnumerable<IEnumerable<T>> 
            GetKCombs<T>(IEnumerable<T> list, int length) where T : IComparable
        {
            if (length == 1) return list.Select(t => new T[] { t });
            return GetKCombs(list, length - 1)
                .SelectMany(t => list.Where(o => o.CompareTo(t.Last()) > 0), 
                    (t1, t2) => t1.Concat(new T[] { t2 }));
        }

        private static void GetAllCombinations()
        {

        }

        public static void TestEdgeCase()
        {
            var lClass = GetClass;

            for (int i = 5; i < 25; i++)
            {
                for (int j = 9; j < 30; j++)
                {
                    Console.Write($"Generating for {j} Users. Min {i} games for one player... ");
                    var lRound = RoundFactory.CreateRound4(
                        lClass.UserBase.Take(j).Select(a => a.ID).ToList(),
                        lClass, DateTime.Now, j/4, "", "" , 
                        i);
                    Console.WriteLine($"Generated {lRound.Games.Count} games! ");
                    AnalyzeRound(lRound);
                }
            }
        }

        private static void AnalyzeRound(Round aRound)
        {
            var lGames = aRound.Games;
            var lUsers = aRound.AttentedPlayers;
            
            var lUserDict = new Dictionary<uint, int>();
            lUsers.ForEach(a=> lUserDict.Add(a.ID, 0));

            foreach (var iGame in lGames)
            {
                iGame.SideA.ForEach(a => lUserDict[a.ID]++);
                iGame.SideB.ForEach(a => lUserDict[a.ID]++);
            }

            Console.WriteLine(lUserDict.Keys.Select(a=> $"{lUsers.First(b=> b.ID == a).Name}: {lUserDict[a]} Games" ).Aggregate((a,b)=>$"{a}\n{b}"));

        }
        #region MyRegion

        private static Class GetClass => new Class
        {
            UserBase = new List<User>
            {
                new User {ID = 0,  Name = "A"},
                new User {ID = 1,  Name = "B"},
                new User {ID = 2,  Name = "C"},
                new User {ID = 3,  Name = "D"},
                new User {ID = 4,  Name = "E"},
                new User {ID = 5,  Name = "F"},
                new User {ID = 6,  Name = "G"},
                new User {ID = 7,  Name = "H"},
                new User {ID = 8,  Name = "I"},
                new User {ID = 9,  Name = "J"},
                new User {ID = 10, Name = "K"},
                new User {ID = 11, Name = "L"},
                new User {ID = 12, Name = "M"},
                new User {ID = 13, Name = "N"},
                new User {ID = 14, Name = "O"},
                new User {ID = 15, Name = "P"},
                new User {ID = 16, Name = "Q"},
                new User {ID = 17, Name = "R"},
                new User {ID = 18, Name = "S"},
                new User {ID = 19, Name = "T"},
                new User {ID = 20, Name = "U"},
                new User {ID = 21, Name = "V"},
                new User {ID = 22, Name = "W"},
                new User {ID = 23, Name = "Q"},
                new User {ID = 24, Name = "Z"},
                new User {ID = 25, Name = "AA"},
                new User {ID = 26, Name = "AB"},
                new User {ID = 27, Name = "AC"},
                new User {ID = 28, Name = "AD"},
                new User {ID = 29, Name = "AE"},
                new User {ID = 30, Name = "AF"},
            }
        };


        #endregion
        private static int CourtCountCalc(int aPlayers, int aPlayersPerGame = 4)
        {
            int lCnt = 1;
            while (((aPlayers / (double)lCnt) - aPlayersPerGame) * lCnt >= aPlayersPerGame)
            {
                lCnt++;
            }

            return lCnt;
        }

        private static void CreateGames(List<Tuple<int, int>> aPairs)
        {
            var lGames = new List<Tuple<Tuple<int, int>, Tuple<int, int>>>();
            int aLimit = 0;
            var lPairs = aPairs.ToList();
            while (lPairs.Any())
            {
                var lPair = GetRandomTuple(lPairs, mRandom);
                var lPair2 = GetRandomTuple(lPairs, mRandom);

                while (PlayerDuplication(lPair, lPair2))
                {
                    lPair2 = GetRandomTuple(lPairs, mRandom);
                    aLimit++;
                    if (aLimit < 50)
                    {
                        CreateGames(aPairs);
                        return;
                    }
                }
                    

                lGames.Add(new Tuple<Tuple<int, int>, Tuple<int, int>>(lPair, lPair2));
                lPairs.Remove(lPair);
                lPairs.Remove(lPair2);
            }

            mLog.Info(lGames.Select(a => $"{a.Item1.Item1}-{a.Item1.Item2} : {a.Item2.Item1}-{a.Item2.Item2}").Aggregate("\n", (a, b) => a + "\n" + b));
        }

        private static void ConsoleSetup(bool aFile)
        {
            Hierarchy lHier = (Hierarchy)LogManager.GetRepository();

            var patternLayout = new PatternLayout();
            patternLayout.ConversionPattern = "%date; [%thread] %-5level; %logger; %message%newline";
            patternLayout.AddConverter("%date; [%thread] %-5level; %logger; %message%newline", typeof(PatternConverter));
            patternLayout.ActivateOptions();

            var lConsole = new ColoredConsoleAppender();
            lConsole.AddMapping(new ColoredConsoleAppender.LevelColors
                { Level = Level.Info, ForeColor = ColoredConsoleAppender.Colors.HighIntensity & ColoredConsoleAppender.Colors.White });
            lConsole.AddMapping(new ColoredConsoleAppender.LevelColors
                { Level = Level.Warn, ForeColor = ColoredConsoleAppender.Colors.Red & ColoredConsoleAppender.Colors.HighIntensity });
            lConsole.Layout = patternLayout;
            lConsole.ActivateOptions();
            lHier.Root.AddAppender(lConsole);

            if (aFile)
            {
                var lFile = new RollingFileAppender
                {
                    AppendToFile = false,
                    MaxFileSize = 10 * 1024 * 1024 * 8,
                    LockingModel = new FileAppender.MinimalLock(),
                    RollingStyle = RollingFileAppender.RollingMode.Size,
                    MaxSizeRollBackups = 5,
                    StaticLogFileName = true,
                    File = @"Log.txt",
                    Layout = patternLayout
                };
                lFile.ActivateOptions();
                lHier.Root.AddAppender(lFile); 
            }

            lHier.Root.Level = Level.Info;
            lHier.Configured = true;
            BasicConfigurator.Configure(lHier);
        }

        private static void PrintDict<T, Q>(Dictionary<T, Q> aDictionary)
        {
            mLog.Info(aDictionary.OrderBy(a=>a.Key).Select(a=> $"{a.Key}: {a.Value}").Aggregate("\r\n",(a,b) => a + "\r\n" + b));
        }

        private static void PrintPairs<T, Q>(List<Tuple<T, Q>> aList)
        {
            mLog.Info(aList.Select(a=> $"{a.Item1}-{a.Item2}").Aggregate("\r\n",(a,b)=> a + "\r\n" + b));
        }
        
        private static bool PlayerDuplication(Tuple<int,int> aA,Tuple<int,int> aB)
        {
            var lList = new List<int> {aA.Item1,aA.Item2,aB.Item1,aB.Item2};
            return lList.GroupBy(a=>a).Any(a=>a.Count() > 1);
        }

        private static Tuple<T, Q> GetRandomTuple<T, Q>(List<Tuple<T, Q>> aList, Random aRandom)
        {
            var lSelected = aList[aRandom.Next(aList.Count)];
            return lSelected;
        }

        private static int RandomItemFromMin(Dictionary<int, int> aDict, Random aRandom)
        {
            var lLast = aDict.Where(a => a.Value == aDict.Min(b => b.Value)).Select(a => a.Key).ToArray();
            var lRand = lLast[aRandom.Next(lLast.Length)];
            return lRand;
        }
    }

    public static class EnumerableExtension
    {
        private static Random mRandom = new Random((int) (DateTime.Now.Ticks / 5));
        public static IEnumerable<T> GetRandomItem<T>(this List<IEnumerable<T>> aList)
        {
            return GetRandomItem(aList, mRandom);
        }

        public static IEnumerable<T> GetRandomItem<T>(this List<IEnumerable<T>> aList, Random aRandom)
        {
            if (!aList.Any()) throw new Exception("List is empty");
            var lSelected = aList[aRandom.Next(aList.Count)];
            return lSelected;
        }

        public static bool ItemContainsTypeItem(this List<IEnumerable<int>> aList, int aMember)
        {
            foreach (var iItem in aList)
            {
                if (iItem.Contains(aMember)) return true;
            }
            return false;
        }

        public static List<IEnumerable<int>> RemoveItemsWithSubitem(this List<IEnumerable<int>> aList, int aSubItem)
        {
            var lEnumerable = aList.ToList();

            foreach (var iEnum in lEnumerable.ToList())
            {
                if (iEnum.Contains(aSubItem))
                {
                    lEnumerable.Remove(iEnum);
                }
            }

            return lEnumerable;
        }

        public static List<IEnumerable<int>> RemoveItemsWithSubitemList(this List<IEnumerable<int>> aList, List<int> aSubItemList)
        {
            var lEnumerable = aList.ToList();

            foreach (var iSubItem in aSubItemList)
            {
                lEnumerable = lEnumerable.RemoveItemsWithSubitem(iSubItem);
            }

            return lEnumerable;
        }

        public static List<int> GetLIstOfItemsUnique(this List<IEnumerable<int>> aList)
        {
            var lEnumerable = new List<int>();

            foreach (var iItem in aList)
            {
                foreach (var iSubItem in iItem)
                {
                    if (!lEnumerable.Contains(iSubItem))
                    {
                        lEnumerable.Add(iSubItem);
                    }
                }
            }

            return lEnumerable;
        }

        public static List<int> GetListExceptCountFromList(this List<int> aList, int aCount, List<int> aExclude)
        {
            if (aCount <= 0) return aList.ToList();
            if(aCount > aExclude.Count) throw new ArgumentOutOfRangeException(nameof(aCount));

            var lList = aList.ToList();

            for (int i = 0; i < aCount; i++)
            {
                var lSelected = aExclude[mRandom.Next(aExclude.Count)];
                if (aList.Contains(lSelected))
                {
                    lList.Remove(lSelected);
                    aExclude.Remove(lSelected);
                }
                else
                {
                    throw new ArgumentException($"{nameof(aExclude)} contain item which is not in {nameof(aList)}");
                }
            }

            return lList;
        }

        public static List<int> GetMaxOrEqualRandom(this Dictionary<int, int> aDict, int aCount)
        {
            if(aCount <= 0) throw new ArgumentOutOfRangeException(nameof(aCount));
            if(aDict.Keys.Count == 0 || aDict.Keys.Count < aCount) throw new ArgumentOutOfRangeException(nameof(aDict)+ " and/or " + nameof(aCount));
            var lExcludeList = new List<int>();
            var lMaxValue = aDict.Values.Max();
            do
            {
                var lMaxGroup = aDict.Where(a => a.Value == lMaxValue).Select(a => a.Key).ToList();
                for (int i = 0; i < lMaxGroup.Count; i++)
                {
                    if (lExcludeList.Count >= aCount) break;
                    var lValueToExcluede = lMaxGroup[mRandom.Next(lMaxGroup.Count)];
                    lExcludeList.Add(lValueToExcluede);
                    lMaxGroup.Remove(lValueToExcluede);
                }

                lMaxValue--;
            } while (lExcludeList.Count < aCount);

            return lExcludeList;
        }

        public static List<int> GetKeysIfMinValue(this Dictionary<int,int> aDict)
        {
            return aDict.Keys.Where(a => aDict[a] == aDict.Values.Min()).ToList();
        }
    }
}
