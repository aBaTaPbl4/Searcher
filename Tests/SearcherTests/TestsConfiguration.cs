using System;
using System.Diagnostics;
using System.IO;
using Common;
using Common.Interfaces;
using NUnit.Framework;
using Searcher.VM;
using SearcherTests;
using SearcherTests.ObjectsFactory;
using SearcherTests.ServiceImpls;
using ServiceImpls;
using log4net.Config;


[SetUpFixture]
public class TestsConfiguration
{

    public static IObjectsFactory ObjectsFactory
    {
        get 
        { 
            return AppContext.GetServiceInstance(
                    typeof (IObjectsFactory)) as IObjectsFactory; 
        }
    }

    [SetUp]
    public static void RunBeforeAnyTests()
    {
        try
        {
            XmlConfigurator.Configure();
            RegisterServices();
            RestoreTestData();
            CopyFolder(
                Path.GetFullPath(@"..\..\..\..\Bin\Plugins"), 
                Path.Combine(Environment.CurrentDirectory, "Plugins"));
            var pm = AppContext.PluginManager as PluginManager;
            pm.ScanPluginsFolder();
            var settings = AppContext.SearchSettings as SearchSettings;
            settings.ActivePlugins = pm.AllPlugins;
        }
        catch (Exception ex)
        {
            Debug.Write(Log.Content);
            throw;
        }
    }


    public static void RegisterServices()
    {

        IObjectsFactory factory = null;
        
#if (ISOLATION_REQUIRED)
        //aBaTaPbl4:unit tests => register fake services (during local build at developer workstation)
        
        factory = new UnitTestsObjectsFactory();
                
#else
        //aBaTaPbl4:system tests => register real services (during nighty build at build agent)

        factory = new SystemTestsObjectsFactory();
#endif

        AppContext.RegisterService(factory, typeof(IObjectsFactory));
        AppContext.RegisterService(factory.CreateFileSystem(), typeof(IFileSystem));
        AppContext.RegisterService(factory.CreatePluginManager(), typeof(IPluginManager));
        AppContext.RegisterService(factory.CreateSettings(), typeof(ISearchSettings));

    }

    public static void RestoreTestData()
    {
        var sourcePath = Path.Combine(Environment.CurrentDirectory, "TestData");
        var destPath = Environment.CurrentDirectory;
        CopyFolder(sourcePath, destPath);
    }

    public static void CopyFolder(string sourceFolder, string destFolder)
    {
        if (!Directory.Exists(destFolder))
            Directory.CreateDirectory(destFolder);
        string[] files = Directory.GetFiles(sourceFolder);
        foreach (string file in files)
        {
            string name = Path.GetFileName(file);
            string dest = Path.Combine(destFolder, name);
            File.Copy(file, dest, true);
        }
        string[] folders = Directory.GetDirectories(sourceFolder);
        foreach (string folder in folders)
        {
            string name = Path.GetFileName(folder);
            string dest = Path.Combine(destFolder, name);
            CopyFolder(folder, dest);
        }
    }


    [TearDown]
    public static void RunAfterAnyTests()
    {

    }
}