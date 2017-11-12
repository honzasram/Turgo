using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using log4net.Util;
using Sramek.FX;
using Turgo.Common;

namespace DevelopAplication
{
    class Program
    {
        private static ILog mLog = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private const int cNo = 8;
        private const int cCourts = 2;
        private static Random mRandom = new Random((int) (DateTime.Now.Ticks /5));

        static void Main(string[] args)
        {
            ConsoleSetup(false);
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
            for (int i = 0; i < 150; i++)
            {
                var lRound = RoundFactory.CreateRound(new List<uint> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }, lClass, DateTime.Now, 3, "", "");

                var lGames = lRound.Games;

                foreach (var iGame in lGames)
                {
                    var lIDs = iGame.SideA.Select(a => a.ID).ToList();
                    lIDs.AddRange(iGame.SideB.Select(a => a.ID));
                }
            }
            //while (true)
            //{
            //    var lNumbers = new List<int>();
            //    for (int i = 1; i < cNo + 1; i++)
            //        lNumbers.Add(i);
            //    var lPairs = new List<Tuple<int, int>>();
            //    var lAttendDict = new Dictionary<int, int>();
            //    lNumbers.ForEach(a => lAttendDict.Add(a, 0));

            //    for (int i = 0; i < (cNo * cCourts); i++)
            //    {
            //        var lA = RandomItemFromMin(lAttendDict, mRandom);
            //        lAttendDict[lA] += 1;
            //        var lB = RandomItemFromMin(lAttendDict, mRandom);
            //        while (lB == lA) lB = RandomItemFromMin(lAttendDict, mRandom);
            //        lAttendDict[lB] += 1;
            //        if(lA < lB)
            //            lPairs.Add(new Tuple<int, int>(lA,lB));
            //        else
            //            lPairs.Add(new Tuple<int, int>(lB,lA));
            //    }

            //    PrintPairs(lPairs);
            //    CreateGames(lPairs);

            //    Console.ReadKey(); 
            //}
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
}
