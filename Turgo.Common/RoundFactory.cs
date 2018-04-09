using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using log4net;
using log4net.Util;
using MathNet.Numerics;
using Sramek.FX;
using Turgo.Common.Model;

namespace Turgo.Common
{
    public static class RoundFactory
    {
        private static ILog mLog = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static Random mRandom = new Random((int)(DateTime.Now.Ticks / 25));

        public static Round CreateRound(List<uint> aUsers, Class aClass, DateTime aDate, int aCourts, string aDescribtion, string aPlace, bool aIgnoreNotEvenDistribution = false)
        {
            var lAttendingPlayers = aClass.UserBase.Where(a => aUsers.Contains(a.ID)).ToList();
            var lPairs = new List<Tuple<uint, uint>>();
            var lAttendDict = new Dictionary<uint, int>();
            lAttendingPlayers.ForEach(a=> lAttendDict.Add(a.ID, 0));

            for (int i = 0; i < (lAttendDict.Keys.Count * aCourts); i++)
            {
                var lA = RandomItemFromMin(lAttendDict, mRandom);
                lAttendDict[lA] += 1;
                var lB = RandomItemFromMin(lAttendDict, mRandom);
                while (lB == lA) lB = RandomItemFromMin(lAttendDict, mRandom);
                lAttendDict[lB] += 1;
                if (lA < lB)
                    lPairs.Add(new Tuple<uint, uint>(lA, lB));
                else
                    lPairs.Add(new Tuple<uint, uint>(lB, lA));
            }
            var lGames = CreateGames(lPairs, aIgnoreNotEvenDistribution);
            var lRound = new Round
            {
                AttentedPlayers = lAttendingPlayers,
                CourtCount = aCourts,
                DateTime = aDate,
                Description = aDescribtion,
                Games = new List<Game>(),
                Place = aPlace
            };

            foreach (var iGame in lGames)
            {
                var lPairA = new List<User>();
                lPairA.Add(lAttendingPlayers.Find(a=>a.ID == iGame.Item1.Item1));
                lPairA.Add(lAttendingPlayers.Find(a=>a.ID == iGame.Item1.Item2));
                var lPairB = new List<User>();
                lPairB.Add(lAttendingPlayers.Find(a=>a.ID == iGame.Item2.Item1));
                lPairB.Add(lAttendingPlayers.Find(a=>a.ID == iGame.Item2.Item2));

                var lGame = new Game {SideA = lPairA, SideB = lPairB, Result = new GameResult(), Parent = lRound };
                lRound.Games.Add(lGame);
            }

            return lRound;
        }

        private static List<Tuple<Tuple<uint, uint>, Tuple<uint, uint>>> CreateGames(List<Tuple<uint, uint>> aPairs, bool aIgnoreNotEvenDistribution)
        {
            var lGames = new List<Tuple<Tuple<uint, uint>, Tuple<uint, uint>>>();
            int aLimit = 0;
            var lPairs = aPairs.ToList();
            while (lPairs.Any())
            {
                if (lPairs.Count == 1)
                {
                    if (aIgnoreNotEvenDistribution) return lGames;
                    throw new NowEvenCountException();
                }
                var lPair = lPairs[0];
                var lPair2 = lPairs[1];
                var lTries = 0;
                while (PlayerDuplication(lPair, lPair2))
                {
                    aLimit++;
                    lTries++;
                    lPair2 = lPairs[lTries+1];
                    if (aLimit > 50)
                    {
                        return CreateGames(aPairs,aIgnoreNotEvenDistribution);
                    }
                }

                lGames.Add(new Tuple<Tuple<uint, uint>, Tuple<uint, uint>>(lPair, lPair2));
                lPairs.Remove(lPair);
                lPairs.Remove(lPair2);
            }
            return lGames;
        }

        private static List<Tuple<Tuple<uint, uint>, Tuple<uint, uint>>> CreateGames2(List<Tuple<uint, uint>> aPairs, bool aIgnoreNotEvenDistribution)
        {
            var lGames = new List<Tuple<Tuple<uint, uint>, Tuple<uint, uint>>>();
            int aLimit = 0;
            var lPairs = aPairs.ToList();

            var lCombinations = GetKCombs(lPairs, 2).ToList();

            foreach (var iComb in lCombinations.ToList())
            {
                var lComb = iComb.ToList();

                if (CheckCombination(lComb[0], lComb[1]))
                {
                    lCombinations.Remove(iComb);
                }
            }

            if (lCombinations.Count < aPairs.Count / 2)
            {

            }
            for (int i = 0; i < aPairs.Count / 2; i++)
            {
                var lEnumerableComb = lCombinations[mRandom.Next(lCombinations.Count)];
                var lCombination = lEnumerableComb.ToList();
                lGames.Add(new Tuple<Tuple<uint, uint>, Tuple<uint, uint>>(lCombination[0], lCombination[1]));
                lCombinations.Remove(lEnumerableComb);
            }
            
            return lGames;
        }

        private static Func<Tuple<uint, uint>, Tuple<uint, uint>, bool> CheckCombination =>
            (a, b) => a.Item1 == b.Item1 || a.Item2 == b.Item2;

        private static bool PlayerDuplication(Tuple<uint, uint> aA, Tuple<uint, uint> aB)
        {
            var lList = new List<uint> { aA.Item1, aA.Item2, aB.Item1, aB.Item2 };
            return lList.GroupBy(a => a).Any(a => a.Count() > 1);
        }

        private static uint RandomItemFromMin(Dictionary<uint, int> aDict, Random aRandom)
        {
            try
            {
                var lLast = aDict.Where(a => a.Value == aDict.Min(b => b.Value)).Select(a => a.Key).ToArray();
                var lRand = lLast[aRandom.Next(lLast.Length)];
                return lRand;
            }
            catch (Exception e)
            {
                return 0;
            }
            finally
            {

            }
        }
        
        public static Round CreateRound2(List<uint> aUsers, Class aClass, DateTime aDate, int aCourts,
            string aDescribtion, string aPlace, int aMultiplicator)
        {
            var lSittingPlaces = Convert.ToInt32(((aUsers.Count / (double) aCourts)-4) * aCourts);
            Debug.Assert(lSittingPlaces>=0);
            mRandom = new Random((int)(DateTime.Now.Ticks / 25));
            
            var lAttendingPlayers = aClass.UserBase.Where(a => aUsers.Contains(a.ID)).ToList();
            var lRound = new Round
            {
                AttentedPlayers = lAttendingPlayers,
                CourtCount = aCourts,
                DateTime = aDate,
                Description = aDescribtion,
                Games = new List<Game>(),
                Place = aPlace
            };

            var lPairs = new List<Tuple<uint, uint>>();
            var lSittingDict = new Dictionary<uint, int>();
            var lPlayedGamesDict = new Dictionary<uint, int>();
            lAttendingPlayers.ForEach(a => lSittingDict.Add(a.ID, 0));
            lAttendingPlayers.ForEach(a => lPlayedGamesDict.Add(a.ID, 0));

            int lWhilectn = 0;
            do
            {
                var lSitting = new int[lSittingPlaces];
                for (int i = 0; i < lSittingPlaces; i++) lSitting[i] = -1;
                for (int i = 0; i < lSittingPlaces; i++)
                {
                    
                    int aLimit = 0;
                    var lSelected = RandomItemFromMin(lSittingDict, mRandom);
                    while (lSitting.Any(a=>a==lSelected))
                    {
                        lSelected = RandomItemFromMin(lSittingDict, mRandom);
                        if(aLimit == 50) throw new Exception("Cycled...");
                        aLimit++;
                    }
                    lSitting[i] = (int) lSelected;
                    lSittingDict[(uint) lSitting[i]] += 1;
                }

                var lPlayingRest = lSittingDict.Keys.Where(a => !lSitting.Contains((int)a)).ToList();
                lPlayingRest.ForEach(a => lPlayedGamesDict[a] += 1);
                var lRoundPairs = CreatePairs2(lPlayingRest, ref lPairs, aUsers.Count);
                lPairs.AddRange(lRoundPairs);
                if (lRoundPairs.Count % 2 != 0) throw new Exception("Not event count of pairs");

                var lGames = CreateGames(lRoundPairs, false);
                foreach (var iGame in lGames)
                {
                    var lPairA = new List<User>();
                    lPairA.Add(lAttendingPlayers.Find(a => a.ID == iGame.Item1.Item1));
                    lPairA.Add(lAttendingPlayers.Find(a => a.ID == iGame.Item1.Item2));
                    var lPairB = new List<User>();
                    lPairB.Add(lAttendingPlayers.Find(a => a.ID == iGame.Item2.Item1));
                    lPairB.Add(lAttendingPlayers.Find(a => a.ID == iGame.Item2.Item2));

                    var lGame = new Game {SideA = lPairA, SideB = lPairB, Result = new GameResult(), Parent = lRound};
                    lRound.Games.Add(lGame);
                }

                if (lWhilectn == 10000)
                {
                    throw new Exception("Too many rounds?");
                }

                lWhilectn++;
                if (lSittingPlaces == 0)
                {
                    foreach (var iKey in lSittingDict.Keys.ToList())
                    {
                        lSittingDict[iKey] += 1;
                    }
                }

                if (lPlayedGamesDict.Values.Min() == lPlayedGamesDict.Values.Max() &&
                    lPlayedGamesDict.Values.Min() >= aMultiplicator) break;
            } while (lSittingDict.Values.Min() != lSittingDict.Values.Max() || lSittingDict.Values.Min() == 0 || lPlayedGamesDict.Values.Min() < aMultiplicator);

            return lRound;
        }

        public static Round CreateRound3(List<uint> aUsers, Class aClass, DateTime aDate, int aCourts,
            string aDescribtion, string aPlace, int aMultiplicator)
        {
            var lSittingPlaces = Convert.ToInt32(((aUsers.Count / (double) aCourts)-4) * aCourts);
            Debug.Assert(lSittingPlaces>=0);
            mRandom = new Random((int)(DateTime.Now.Ticks / 25));
            
            var lAttendingPlayers = aClass.UserBase.Where(a => aUsers.Contains(a.ID)).ToList();
            var lRound = new Round
            {
                AttentedPlayers = lAttendingPlayers,
                CourtCount = aCourts,
                DateTime = aDate,
                Description = aDescribtion,
                Games = new List<Game>(),
                Place = aPlace
            };

            var lPairs = new List<Tuple<uint, uint>>();
            var lSittingDict = new Dictionary<uint, int>();
            var lPlayedGamesDict = new Dictionary<uint, int>();
            lAttendingPlayers.ForEach(a => lSittingDict.Add(a.ID, 0));
            lAttendingPlayers.ForEach(a => lPlayedGamesDict.Add(a.ID, 0));

            var lPlyerCombinations = GenerateAllCombinations(lAttendingPlayers.Select(a => a.ID));

            int lWhilectn = 0;
            do
            {
                var lSitting = new int[lSittingPlaces];
                for (int i = 0; i < lSittingPlaces; i++) lSitting[i] = -1;
                for (int i = 0; i < lSittingPlaces; i++)
                {
                    
                    int aLimit = 0;
                    var lSelected = RandomItemFromMin(lSittingDict, mRandom);
                    while (lSitting.Any(a=>a==lSelected))
                    {
                        lSelected = RandomItemFromMin(lSittingDict, mRandom);
                        if(aLimit == 50) throw new Exception("Cycled...");
                        aLimit++;
                    }
                    lSitting[i] = (int) lSelected;
                    lSittingDict[(uint) lSitting[i]] += 1;
                }

                var lPlayingRest = lSittingDict.Keys.Where(a => !lSitting.Contains((int)a)).ToList();
                lPlayingRest.ForEach(a => lPlayedGamesDict[a] += 1);
                
                
                if (lPlyerCombinations.Count < lPlayingRest.Count/2)
                {
                    lPlyerCombinations = GenerateAllCombinations(lAttendingPlayers.Select(a => a.ID));
                }

                var lRoundPairs = CreatePairs3(lPlayingRest, ref lPlyerCombinations);
                

                lPairs.AddRange(lRoundPairs);
                if (lRoundPairs.Count % 2 != 0) throw new Exception("Not event count of pairs");

                var lGames = CreateGames2(lRoundPairs, false);
                foreach (var iGame in lGames)
                {
                    var lPairA = new List<User>();
                    lPairA.Add(lAttendingPlayers.Find(a => a.ID == iGame.Item1.Item1));
                    lPairA.Add(lAttendingPlayers.Find(a => a.ID == iGame.Item1.Item2));
                    var lPairB = new List<User>();
                    lPairB.Add(lAttendingPlayers.Find(a => a.ID == iGame.Item2.Item1));
                    lPairB.Add(lAttendingPlayers.Find(a => a.ID == iGame.Item2.Item2));

                    var lGame = new Game {SideA = lPairA, SideB = lPairB, Result = new GameResult(), Parent = lRound};
                    lRound.Games.Add(lGame);
                }

                if (lWhilectn == 10000)
                {
                    throw new Exception("Too many rounds?");
                }

                lWhilectn++;
                if (lSittingPlaces == 0)
                {
                    foreach (var iKey in lSittingDict.Keys.ToList())
                    {
                        lSittingDict[iKey] += 1;
                    }
                }

                if (lPlayedGamesDict.Values.Min() == lPlayedGamesDict.Values.Max() &&
                    lPlayedGamesDict.Values.Min() >= aMultiplicator) break;
            } while (lSittingDict.Values.Min() != lSittingDict.Values.Max() || lSittingDict.Values.Min() == 0 || lPlayedGamesDict.Values.Min() < aMultiplicator);

            return lRound;
        }

        private static List<Tuple<Tuple<uint, uint>, Tuple<uint, uint>>> CreateGames3 (List<IEnumerable<uint>> aPairs)
        {
            var lGames = new List<Tuple<Tuple<uint, uint>, Tuple<uint, uint>>>();

            var lQueue = new Queue<IEnumerable<uint>>(aPairs);

            while (lQueue.Count >= 2)
            {
                var lFirst = lQueue.Dequeue().ToList();
                var lSecond = lQueue.Dequeue().ToList();

                lGames.Add(new Tuple<Tuple<uint, uint>, Tuple<uint, uint>>(new Tuple<uint, uint>(lFirst[0], lFirst[1]), new Tuple<uint, uint>(lSecond[0], lSecond[1])));

            }

            return lGames;
        }

        public static Round CreateRound4(List<uint> aUsers, Class aClass, DateTime aDate, int aCourts, string aDescribtion, string aPlace, int aMultiplicator)
        {
            var lPairs = GetListOfPairs(aUsers, aMultiplicator);
            var lGames = CreateGames3(lPairs);
            var lAttendingPlayers = aClass.UserBase.Where(a => aUsers.Contains(a.ID)).ToList();
            var lRound = new Round
            {
                AttentedPlayers = lAttendingPlayers,
                CourtCount = aCourts,
                DateTime = aDate,
                Description = aDescribtion,
                Games = new List<Game>(),
                Place = aPlace
            };
            foreach (var iGame in lGames)
            {
                var lPairA = new List<User>();
                lPairA.Add(lAttendingPlayers.Find(a => a.ID == iGame.Item1.Item1));
                lPairA.Add(lAttendingPlayers.Find(a => a.ID == iGame.Item1.Item2));
                var lPairB = new List<User>();
                lPairB.Add(lAttendingPlayers.Find(a => a.ID == iGame.Item2.Item1));
                lPairB.Add(lAttendingPlayers.Find(a => a.ID == iGame.Item2.Item2));

                var lGame = new Game { SideA = lPairA, SideB = lPairB, Result = new GameResult(), Parent = lRound};
                lRound.Games.Add(lGame);
            }
            return lRound;
        }

        private static List<IEnumerable<uint>> GetListOfPairs(List<uint> aUsers, int aRounds)
        {
            var lCourts = aUsers.Count / 4;
            var lSitting = Convert.ToInt32((aUsers.Count / (double) lCourts - 4) * lCourts);

            var lRandom = new Random();
            var lArrList = aUsers.ToList();
            
            var lDict = new Dictionary<uint, int>();
            lArrList.ForEach(a => lDict.Add(a, 0));
            var lRoundList = new List<IEnumerable<uint>>();
            var lGlobalRoundList = new List<IEnumerable<uint>>();
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

                var lGameList = new List<IEnumerable<uint>>();
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
            } while (lDict.Values.Min() != lDict.Values.Max() || lDict.Values.Min() < aRounds);

            return lGlobalRoundList;
        }
        
        private static List<Tuple<uint, uint>> CreatePairs(List<uint> aPlayers, ref List<Tuple<uint, uint>> aPreviousPairs)
        {
            var lPlayers = aPlayers.ToList();
            var lPairs = new List<Tuple<uint,uint>>();
            var lCnt = lPlayers.Count;
            for (int i = 0; i < lCnt / 2 ; i++)
            {
                var lA = RandomFrom(lPlayers, mRandom);
                var lB = RandomFrom(lPlayers, mRandom);
                
                if (lA > lB)
                {
                    var lPom = lA;
                    lA = lB;
                    lB = lPom;
                }

                if (PairDuplicated(aPreviousPairs, lA, lB))
                {
                    if (lPlayers.Count > 0)
                    {
                        var lB2 = RandomFrom(lPlayers, mRandom);
                        Debug.Assert(lB != lB2);
                        if (lA > lB2)
                        {
                            var lPom = lA;
                            lA = lB2;
                            lB2 = lPom;
                        }
                        if (PairDuplicated(aPreviousPairs, lA, lB2))
                        {
                            lPlayers.Add(lA);
                            lPlayers.Add(lB);
                            lPlayers.Add(lB2);
                            continue;
                        }
                        lPlayers.Add(lB);
                        lB = lB2;
                    }
                    else
                    {
                        lPlayers = aPlayers.ToList();
                        lPairs.Clear();
                        i = -1;
                        continue;
                    }
                }

                lPairs.Add(new Tuple<uint, uint>(lA, lB));
            }

            foreach (var iPair in lPairs)
            {
                if (aPreviousPairs.Count(a => a.Item1 == iPair.Item1 && a.Item2 == iPair.Item2) > 0)
                {
                    return CreatePairs(aPlayers, ref aPreviousPairs);
                }
            }

            if(lPlayers.Any()) throw new Exception("Leaved players");
            return lPairs;
        }

        private static bool PairDuplicated(List<Tuple<uint, uint>> aPreviousPairs, uint aA, uint aB)
        {
            return aPreviousPairs.Count(a => a.Item1 == aA && a.Item2 == aB) > 0;
        }

        private static uint RandomFrom(List<uint> aPlayers, Random mRandom)
        {
            var lSelected = mRandom.Next(aPlayers.Count);
            var lNumber = aPlayers[lSelected];
            aPlayers.RemoveAt(lSelected);
            return lNumber;
        }

        private static List<Tuple<uint, uint>> GenerateAllCombinations(IEnumerable<uint> aAllPlayers)
        {
            var lCombinations = GetKCombs(aAllPlayers, 2);
            var lReturn = new List<Tuple<uint, uint>>();

            foreach (var iCombination in lCombinations)
            {
                var lArr = iCombination.ToArray();
                Debug.Assert(lArr.Length==2);
                lReturn.Add(new Tuple<uint, uint>(lArr[0], lArr[1]));
            }

            return lReturn;
        }

        private static List<Tuple<uint, uint>> CreatePairs3(List<uint> aPlayers,
            ref List<Tuple<uint, uint>> aAvailiablePairs)
        {
            var lReturn = new List<Tuple<uint,uint>>();

            for (int i = 0; i < aPlayers.Count/2; i++)
            {
                Tuple<uint, uint> lTuple;
                int q = 0;
                do
                {
                    lTuple = GetRandomFromAvaliable(aAvailiablePairs, aPlayers);
                    q++;
                    if (q > 1000)
                    {
                        
                    
                    }
                } while (IsAlreadySelected(lReturn, lTuple.Item1) || IsAlreadySelected(lReturn, lTuple.Item2));
                
                lReturn.Add(lTuple);
            }

            return lReturn;
        }

        private static Func<List<Tuple<uint, uint>>, uint, bool> IsAlreadySelected => (l, u) => l.Any(a => a.Item1 == u) || l.Any(a => a.Item2 == u);

        private static Tuple<uint, uint> GetRandomFromAvaliable(List<Tuple<uint, uint>> aAvailiablePairs, List<uint> aPlayers)
        {
            Tuple<uint, uint> lSelected = null;
            try
            {
                var lIndex = mRandom.Next(aAvailiablePairs.Count);
                lSelected = aAvailiablePairs[lIndex];

                if (CheckSelectedTuple(aPlayers, lSelected.Item1) && CheckSelectedTuple(aPlayers, lSelected.Item2))
                {
                    if (aAvailiablePairs.Count > 1)
                    {
                        if (lIndex + 1 >= aAvailiablePairs.Count)
                            lSelected = aAvailiablePairs[lIndex - 1];
                        else
                            lSelected = aAvailiablePairs[lIndex + 1];
                    }
                    else
                    {
                        throw new Exception("limit situation.... cannot choose from one availiable if there is no suitable record.");
                    }
                }

                return lSelected;
            }
            catch (Exception e)
            {
                mLog.Error(Messages.BuildErrorMessage(e));
                throw e;
            }
        }

        private static Func<List<uint>, uint, bool> CheckSelectedTuple => (a, b) => a.Any(v => v == b);


        private static List<Tuple<uint, uint>> CreatePairs2(List<uint> aPlayers, ref List<Tuple<uint, uint>> aPreviousPairs, int aTotalPlayers)
        {
            if(aPlayers.Count % 2 > 0) throw new Exception("Not even count");

            var lPlayers = aPlayers.ToList();
            var lReturn = new List<Tuple<uint, uint>>();

            var lAssertCounter = 0;
            for (int i = 0; i < aPlayers.Count/2; i++)
            {
                uint[] lGenerated;
                do
                {
                    lGenerated = lPlayers.SelectCombination(2, mRandom).ToArray();
                    SwapIfNeeded(lGenerated);
                    lAssertCounter++;
                    if (lAssertCounter > 100000)
                    {
                        return CreatePairs2(aPlayers, ref aPreviousPairs, aTotalPlayers);
                    }
                } while (PairDuplicated(aPreviousPairs, lGenerated[0], lGenerated[1]) 
                     && Combinatorics.Combinations(aTotalPlayers, 2) > aPreviousPairs.Count*2);

                lPlayers.Remove(lGenerated[0]);
                lPlayers.Remove(lGenerated[1]);

                lReturn.Add(new Tuple<uint, uint>(lGenerated[0], lGenerated[1]));
            }
            return lReturn;
        }

        static IEnumerable<IEnumerable<T>> GetKCombs<T>(IEnumerable<T> list, int length) 
            where T : IComparable
        {
            if (length == 1) return list.Select(t => new T[] { t });
            return GetKCombs(list, length - 1)
                .SelectMany(t => list.Where(o => o.CompareTo(t.Last()) > 0), 
                    (t1, t2) => t1.Concat(new T[] { t2 }));
        }

        private static void SwapIfNeeded(uint[] aArray)
        {
            if (aArray[0] > aArray[1])
            {
                var lPom = aArray[0];
                aArray[0] = aArray[1];
                aArray[1] = lPom;
            }
        }
    }


    public class NowEvenCountException : Exception
    {
        public NowEvenCountException()
        {
        }

        public NowEvenCountException(string message) : base(message)
        {
        }

        public NowEvenCountException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NowEvenCountException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
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

        public static bool ItemContainsTypeItem(this List<IEnumerable<uint>> aList, uint aMember)
        {
            foreach (var iItem in aList)
            {
                if (iItem.Contains(aMember)) return true;
            }
            return false;
        }

        public static List<IEnumerable<uint>> RemoveItemsWithSubitem(this List<IEnumerable<uint>> aList, uint aSubItem)
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

        public static List<IEnumerable<uint>> RemoveItemsWithSubitemList(this List<IEnumerable<uint>> aList, List<uint> aSubItemList)
        {
            var lEnumerable = aList.ToList();

            foreach (var iSubItem in aSubItemList)
            {
                lEnumerable = lEnumerable.RemoveItemsWithSubitem(iSubItem);
            }

            return lEnumerable;
        }

        public static List<uint> GetLIstOfItemsUnique(this List<IEnumerable<uint>> aList)
        {
            var lEnumerable = new List<uint>();

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

        public static List<uint> GetListExceptCountFromList(this List<uint> aList, int aCount, List<uint> aExclude)
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

        public static List<uint> GetMaxOrEqualRandom(this Dictionary<uint, int> aDict, int aCount)
        {
            if(aCount <= 0) throw new ArgumentOutOfRangeException(nameof(aCount));
            if(aDict.Keys.Count == 0 || aDict.Keys.Count < aCount) throw new ArgumentOutOfRangeException(nameof(aDict)+ " and/or " + nameof(aCount));
            var lExcludeList = new List<uint>();
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

        public static List<uint> GetKeysIfMinValue(this Dictionary<uint,int> aDict)
        {
            return aDict.Keys.Where(a => aDict[a] == aDict.Values.Min()).ToList();
        }
    }
}