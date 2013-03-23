﻿using System;
using System.Diagnostics;
using System.IO;
using Common;
using Common.Interfaces;
using NUnit.Framework;
using SearcherTests.ServiceImpls;
using ServiceImpls;
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
            RegisterFakeServices();
            RestoreTestData();
        }
        catch (Exception ex)
        {
            Debug.Write(Log.Content);
            throw;
        }
    }

    public static void RegisterFakeServices()
    {
#if ISOLATION_REQUIRED
        //unit tests => register fake services (local build at developer workstation)
#else
        //system tests => register real services (nighty build at build agent)
        var fs = new FileSystem();
        //fs
        AppContext.RegisterService(fs, typeof(IFileSystem));
        AppContext.RegisterService(fs, fs.GetType());
#endif
        AppContext.RegisterService(FakeObjectsFactory.CreateSettings(), typeof(ISearchSettings));

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