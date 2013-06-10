using Common;
using Models.ScanStrategies;
using NUnit.Framework;

namespace SearcherTests.Models
{
    [TestFixture]
    public class SingleThreadScanTest : ScanTest
    {
        protected override ScanStrategyBase CreateStrategy()
        {
            return AppContext.GetObject<SingleThreadScan>();
        }
    }
}