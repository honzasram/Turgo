using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using log4net.Util;

namespace Sramek.FX
{
    public static class Extensions
    {
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            return source.Shuffle(new Random((int)DateTime.Now.Ticks / 5));
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source, Random rng)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (rng == null) throw new ArgumentNullException("rng");

            return source.ShuffleIterator(rng);
        }

        private static IEnumerable<T> ShuffleIterator<T>(this IEnumerable<T> source, Random rng)
        {
            var buffer = source.ToList();
            for (int i = 0; i < buffer.Count; i++)
            {
                int j = rng.Next(i, buffer.Count);
                yield return buffer[j];

                buffer[j] = buffer[i];
            }
        }
    }

    public static class ILogExtrension
    {
        public static ILog Get()
        {
            return null;
        }

        public static void ConsoleSetup(this ILog aLogger, bool aFile)
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
    }
}