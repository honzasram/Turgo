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
using Sramek.FX.WPF;
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
            ConsoleSetup(false);

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
            }
        }

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

    public class TLoc : Loc<TLoc>
    {
        [Default("name")]
        public string Name { get; set; }

        public TLoc()
        {
            Load();
        }
    }

    public class Base
    {

        public List<PropertyInfo> GetInfo()
        {
            var lType = GetType();
            return lType.GetProperties().ToList();
        }
    }

    public class Target : Base
    {
        public bool Cosi { get; set; }
        public bool Cosi2 { get; set; }
    }
}
