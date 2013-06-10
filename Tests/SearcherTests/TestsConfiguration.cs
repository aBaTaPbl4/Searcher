using System;
using System.Diagnostics;
using System.IO;
using Common;
using NUnit.Framework;
using SearcherTests;
using log4net.Config;

[SetUpFixture]
public class TestsConfiguration
{
    private static readonly string _pluginsFolder = Path.Combine(Environment.CurrentDirectory, "Plugins");

    public static IObjectsFactory ObjectsFactory
    {
        get { return AppContext.GetObject<IObjectsFactory>(); }
    }

    [SetUp]
    public static void RunBeforeAnyTests()
    {
        try
        {
            XmlConfigurator.Configure();
            TestHelper.CopyFolder(
                Path.GetFullPath(@"..\..\..\..\Bin\Plugins"),
                _pluginsFolder
                );
            RestoreTestData();
        }
        catch (Exception ex)
        {
            Debug.Write(Log.Content);
            throw;
        }
    }

    public static void RestoreTestData()
    {
        ObjectsFactory.RestoreObjects();
    }


    [TearDown]
    public static void RunAfterAnyTests()
    {
        UnlockPluginFolder();
        Directory.Delete(_pluginsFolder, true);
    }

    private static void UnlockPluginFolder()
    {
        var pm = AppContext.PluginManager as IDisposable;
        if (pm != null)
        {
            pm.Dispose();
        }
    }
}