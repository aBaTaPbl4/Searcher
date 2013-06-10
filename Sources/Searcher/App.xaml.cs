using System;
using System.Windows;
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