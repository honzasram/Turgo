﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Turgo.Common;
using Sramek.FX;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Turgo.Common.Model;

namespace Turgo.Test
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void CreatingGamesRand()
        {
            TestGenerating(150,1,8);
            TestGenerating(150,2,8);
            TestGenerating(150,3,8);
            TestGenerating(150,4,8);
            TestGenerating(150,5,8);
        }

        [TestMethod]
        public void CreatingGamesShort()
        {
            for (int j = 8; j < 21; j++)
            {
                var i = CourtCountCalc(j, 4);
                Console.Write($"c {i} - p {j}");
                TestGenerating(100, i, j);
            }
        }

        [TestMethod]
        public void CreatingGameLong()
        {
           TestGenerating(150*200,2,8);
        }

        private void TestGenerating(int aCycles, int aCourts, int aPlayers)
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
            var lMax = 0.0;
            bool lEven = true;
            for (int i = 0; i < aCycles; i++)
            {
                var lRound = RoundFactory.CreateRound(lClass.UserBase.Take(aPlayers).Select(a => a.ID).ToList(), lClass, DateTime.Now, aCourts, "", "", true);

                var lGames = lRound.Games;

                foreach (var iGame in lGames)
                {
                    var lIDs = iGame.SideA.Select(a => a.ID).ToList();
                    lIDs.AddRange(iGame.SideB.Select(a => a.ID));
                    Assert.IsFalse(lIDs.GroupBy(a => a).Any(a => a.Count() > 1));
                }

                var lIds = lGames.SelectMany(a => a.SideA.Select(b => b.ID)).ToList();
                lIds.AddRange(lGames.SelectMany(a => a.SideB.Select(b => b.ID)).ToList());
                var lGrouping = lIds.GroupBy(a => a).Select(a => new { a.Key, Count = a.Count() }).ToList();
                //Assert.IsTrue(lGrouping.Select(a => a.Count).Max() == lGrouping.Select(a => a.Count).Min());
                if (lEven) lEven = lGrouping.Select(a => a.Count).Max() == lGrouping.Select(a => a.Count).Min();
                if (lMax == 0) lMax = lGrouping.Select(a => a.Count).Max();
                else lMax = lGrouping.Select(a => a.Count).Max() * 0.01 + lMax * 0.99;
            }
            Console.Write( lEven ? "" : " nerovne ");
            Console.WriteLine($" - g {lMax}");
        }

        [TestMethod]
        public void TestEdgeCase()
        {
            var lClass = GetClass;

            for (int i = 4; i < 25; i++)
            {
                for (int j = 8; j < 20; j++)
                {
                    Console.WriteLine($"Generating for {j} Users. Min {i} games for one player.");
                    var lRound = RoundFactory.CreateRound2(
                        lClass.UserBase.Take(j).Select(a => a.ID).ToList(),
                        lClass, DateTime.Now, j/4, "", "" , 
                        i);
                    Console.WriteLine($"Generated! {lRound.Games.Count} games. ");
                }
            }
        }


        [TestMethod]
        public void CreatingGamesShort2()
        {
            for (int j = 8; j < 21; j++)
            {
                var i = CourtCountCalc(j, 4);
                Console.Write($"c {i} - p {j}");
                TestGenerating2(1, i, j);
            }
        }

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
            }
        };

        private static int CourtCountCalc(int aPlayers, int aPlayersPerGame)
        {
            //int lCnt = 1;
            //while (((aPlayers / (double)lCnt) - aPlayersPerGame) * lCnt >= aPlayersPerGame)
            //{
            //    lCnt++;
            //}
            return Convert.ToInt32(aPlayers/(double) aPlayersPerGame);
        }


        [TestMethod]
        public void CustomGame()
        {
            TestGenerating2(100, 3, 15);
        }

        private void TestGenerating2(int aCycles, int aCourts, int aPlayers)
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
            var lMax = 0.0;
            for (int i = 0; i < aCycles; i++)
            {
                var lRound = RoundFactory.CreateRound2(lClass.UserBase.Take(aPlayers).Select(a=>a.ID).ToList(), lClass, DateTime.Now, aCourts, "", "", 1);

                var lGames = lRound.Games;

                foreach (var iGame in lGames)
                {
                    var lIDs = iGame.SideA.Select(a => a.ID).ToList();
                    lIDs.AddRange(iGame.SideB.Select(a => a.ID));
                    Assert.IsFalse(lIDs.GroupBy(a => a).Any(a => a.Count() > 1));
                }

                var lIds = lGames.SelectMany(a => a.SideA.Select(b => b.ID)).ToList();
                lIds.AddRange(lGames.SelectMany(a => a.SideB.Select(b => b.ID)).ToList());
                var lGrouping = lIds.GroupBy(a => a).Select(a => new { a.Key, Count = a.Count() }).ToList();
                Assert.IsTrue(lGrouping.Select(a => a.Count).Max() == lGrouping.Select(a => a.Count).Min());
                if (lMax == 0) lMax = lGrouping.Select(a => a.Count).Max();
                else lMax = lGrouping.Select(a => a.Count).Max()*0.01 + lMax * 0.99;
            }
            Console.WriteLine($" - g {lMax}");
        }

    }

    [TestClass]
    public class ConfigUnitTest
    {
        [TestMethod]
        public void CreateAndLoadConfiguration()
        {
            //var lModel = new TurgoSettings {BaseClassConfiguration = new ClassConfiguration {
            //    UserBaseList = new List<User>
            //        {
            //            new User() {ID = 0, Name = "A", Surname = "a"}, 
            //            new User() {ID = 1, Name = "B", Surname = "b"}, 
            //            new User() {ID = 2, Name = "C", Surname = "c"}, 
            //        } 
            //    },
            //    Model = new TurgoModel()
            //};
            //lModel.Model.ClassList = new List<Class>();
            //lModel.Model.ClassList.Add(new Class()
            //{
            //    Name = "CLASS", UserBase = lModel.BaseClassConfiguration.UserBaseList, Year = 2017
            //});

            //TurgoSettings.Save(lModel);

            TurgoSettings.Load();
            if (TurgoSettings.I.Model.ClassList == null)
            {
                var lClassList = new List<Class>();
                TurgoSettings.I.Model.ClassList = lClassList;
            }
            if (!TurgoSettings.I.Model.ClassList.Any())
            {
                var lClass = new Class { Name = "Default", Year = DateTime.Now.Year };
                TurgoSettings.I.Model.ClassList.Add(lClass);
                TurgoSettings.Save(TurgoSettings.I);
            }

            Console.Write(TurgoSettings.I.Model.ClassList[0].Name);
        }
    }
}
