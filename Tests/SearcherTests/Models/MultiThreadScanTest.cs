using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using Models.ScanStrategies;
using NUnit.Framework;

namespace SearcherTests.Models
{
    [TestFixture]
    public class MultiThreadScanTest : ScanTest
    {
        protected override ScanStrategyBase CreateStrategy()
        {
            return AppContext.GetObject<MultiThreadScan>();
        }
    }
}
