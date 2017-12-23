using System;
using System.Collections.Generic;
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
        //[TestMethod]
        //public void Winners()
        //{
        //    var lResult = new GameResult {Sets = new List<Set>
        //    {
        //        new Set {SideA = 21, SideB = 15},
        //        new Set {SideA = 15, SideB = 21},
        //        new Set {SideA = 21, SideB = 15},
        //    } };

        //    Assert.IsTrue( lResult.AWinner);
        //    Assert.IsFalse(lResult.BWinner);
        //    lResult.Sets.Add(new Set { SideA = 15, SideB = 21 });
        //    lResult.Sets.Add(new Set { SideA = 15, SideB = 21 });

        //    Assert.IsTrue(lResult.BWinner);
        //    Assert.IsFalse(lResult.AWinner);

        //    lResult.Sets.Add(new Set { SideB = 15, SideA = 21 });

        //    Assert.IsFalse(lResult.BWinner);
        //    Assert.IsFalse(lResult.AWinner);
        //}

        [TestMethod]
        public void CreatingGamesRand()
        {
            TestGenerating(150,1);
            TestGenerating(150,2);
            TestGenerating(150,3);
            TestGenerating(150,4);
            TestGenerating(150,5);
        }

        [TestMethod]
        public void CreatingGamesShort()
        {
            TestGenerating(150,3);
        }

        [TestMethod]
        public void CreatingGameLong()
        {
           TestGenerating(150*200,3);
        }

        private void TestGenerating(int aCycles, int aCourts)
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
                    new User {ID = 10, Name = "K"}
                }
            };
            var lMax = 0;
            for (int i = 0; i < aCycles; i++)
            {
                var lRound = RoundFactory.CreateRound(new List<uint> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }, lClass, DateTime.Now, aCourts, "", "");

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
                lMax = lGrouping.Select(a => a.Count).Max();
            }
            Console.WriteLine($"c {aCourts} - g {lMax}");
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
