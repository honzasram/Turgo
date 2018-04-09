using System;
using System.Reflection;

[assembly: log4net.Config.XmlConfigurator(ConfigFile = "Config\\log4net.xml", Watch = true)]