using System;
using System.Diagnostics;
using System.IO;
using NUnit.Framework;
using log4net.Config;



[SetUpFixture]
public class TestsConfiguration
{

    [SetUp]
    public static void RunBeforeAnyTests()
    {
        try
        {
            XmlConfigurator.Configure();
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
        string[] files = Directory.GetFiles("TestData");
        foreach (string file in files)
        {
            File.Copy(file, Path.GetFileName(file), true);
        }
    }


    [TearDown]
    public static void RunAfterAnyTests()
    {
    }
}