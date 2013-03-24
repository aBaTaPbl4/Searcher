using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Models.ScanStrategies;
using NUnit.Framework;

namespace SearcherTests.Models
{
    [TestFixture]
    public class MultiThreadScanTest : ScanTest
    {
        protected override ScanStrategyBase CreateStrategy()
        {
            return TestsConfiguration.ObjectsFactory.CreateMultiThreadStrategy();
        }
    }
}
