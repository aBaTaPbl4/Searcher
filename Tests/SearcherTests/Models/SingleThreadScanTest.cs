﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Models.ScanStrategies;
using NUnit.Framework;

namespace SearcherTests.Models
{
    [TestFixture]
    public class SingleThreadScanTest : ScanTest
    {
        protected override ScanStrategyBase CreateStrategy()
        {
            return TestsConfiguration.ObjectsFactory.CreateSingleThreadStrategy();
        }
    }
}
