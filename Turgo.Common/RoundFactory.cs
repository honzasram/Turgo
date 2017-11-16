using System;
using System.Collections.Generic;
using System.Linq;
using Turgo.Common.Model;

namespace Turgo.Common
{
    public static class RoundFactory
    {
        private static readonly Random mRandom = new Random((int)(DateTime.Now.Ticks / 5));

        public static Round CreateRound(List<uint> aUsers, Class aClass, DateTime aDate, int aCourts, string aDescribtion, string aPlace)
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
            var lGames = CreateGames(lPairs);
            var lRound = new Round
            {
                AttentedPlayers = lAttendingPlayers,
                CourtCount = aCourts,
                DateTime = aDate,
                Describtion = aDescribtion,
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

                var lGame = new Game {SideA = lPairA, SideB = lPairB, Result = new GameResult()};
                lRound.Games.Add(lGame);
            }

            return lRound;
        }

        private static List<Tuple<Tuple<uint, uint>, Tuple<uint, uint>>> CreateGames(List<Tuple<uint, uint>> aPairs)
        {
            var lGames = new List<Tuple<Tuple<uint, uint>, Tuple<uint, uint>>>();
            int aLimit = 0;
            var lPairs = aPairs.ToList();
            while (lPairs.Any())
            {
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
                        return CreateGames(aPairs);
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
    }
}