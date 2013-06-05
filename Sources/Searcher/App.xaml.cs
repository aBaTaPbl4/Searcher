using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using Common;
using Common.Interfaces;
using Searcher.VM;
using ServiceImpls;
using log4net.Config;

namespace Searcher
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            XmlConfigurator.Configure();
            RegisterServices();
            var pm = AppContext.PluginManager as PluginManager;
            pm.PrivateDomain = AppDomain.CurrentDomain;
            pm.ScanPluginsFolder();
            StartupUri = new Uri("wndMain.xaml", UriKind.Relative);
            base.OnStartup(e);
        }

        protected void RegisterServices()
        {
            AppContext.RegisterService(new ScanOptionVM(),typeof(ISearchSettings));
            AppContext.RegisterService(new FileSystem(), typeof(IFileSystem));
            AppContext.RegisterService(new PluginManager(), typeof(IPluginManager));
        }
    }
}
