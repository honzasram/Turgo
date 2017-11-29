﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using log4net.Util;
using Turgo.Common;
using Turgo.Common.Model;

namespace Turgo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            ConsoleSetup(false);
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

        
    }

}
