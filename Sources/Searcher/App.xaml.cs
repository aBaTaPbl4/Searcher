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
            StartupUri = new Uri("WndMain.xaml", UriKind.Relative);
            base.OnStartup(e);
        }


    }
}
