using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using Turgo.Common.Model;

namespace Turgo.Common
{
    public static class RoundFactory
    {
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

        private static bool PlayerDuplication(Tuple<uint, uint> aA, Tuple<uint, uint> aB)
        {
            var lList = new List<uint> { aA.Item1, aA.Item2, aB.Item1, aB.Item2 };
            return lList.GroupBy(a => a).Any(a => a.Count() > 1);
        }

        private static uint RandomItemFromMin(Dictionary<uint, int> aDict, Random aRandom)
        {
            var lLast = aDict.Where(a => a.Value == aDict.Min(b => b.Value)).Select(a => a.Key).ToArray();
            var lRand = lLast[aRandom.Next(lLast.Length)];
            return lRand;
        }
        
        public static Round CreateRound2(List<uint> aUsers, Class aClass, DateTime aDate, int aCourts,
            string aDescribtion, string aPlace, int aMultiplicator)
        {
            var lSittingPlaces = Convert.ToInt32(((aUsers.Count / (double) aCourts) - 4) * aCourts);
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
                var lRoundPairs = CreatePairs(lPlayingRest, ref lPairs);
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

                if (aPreviousPairs.Count(a => a.Item1 == lA && a.Item2 == lB) > 0)
                {
                    if (lPlayers.Count > 0)
                    {
                        var lB2 = RandomFrom(lPlayers, mRandom);
                        Debug.Assert(lB != lB2);
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

        private static uint RandomFrom(List<uint> aPlayers, Random mRandom)
        {
            var lSelected = mRandom.Next(aPlayers.Count);
            var lNumber = aPlayers[lSelected];
            aPlayers.RemoveAt(lSelected);
            return lNumber;
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
}