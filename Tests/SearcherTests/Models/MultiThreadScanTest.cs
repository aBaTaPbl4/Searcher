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