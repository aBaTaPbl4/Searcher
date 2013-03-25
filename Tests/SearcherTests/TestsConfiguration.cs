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
            TestHelper.CopyFolder(
                Path.GetFullPath(@"..\..\..\..\Bin\Plugins"), 
                Path.Combine(Environment.CurrentDirectory, "Plugins"));
            var pm = AppContext.PluginManager as PluginManager;
            pm.ScanPluginsFolder();
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

#if (ISOLATION_REQUIRED)//aBaTaPbl4:unit tests => register fake services(using by local build at developer workstation)
                
        factory = new UnitTestsObjectsFactory();
                
#else//aBaTaPbl4:system tests => register real services(using by nighty build, to check compatiblity with net-services, db etc)

        factory = new SystemTestsObjectsFactory();

#endif
        factory.Initialize();
        AppContext.RegisterService(factory, typeof(IObjectsFactory));
        AppContext.RegisterService(factory.CreateFileSystem(), typeof(IFileSystem));
        AppContext.RegisterService(factory.CreatePluginManager(), typeof(IPluginManager));
        AppContext.RegisterService(factory.CreateSettings(), typeof(ISearchSettings));

    }

    public static void RestoreTestData()
    {
        ObjectsFactory.RestoreObjects();
    }


    //[TearDown]
    //public static void RunAfterAnyTests()
    //{
        
    //}
}