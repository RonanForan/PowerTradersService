using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Services;
using System.Linq;
using PowerTradersLibrary;

namespace PowerTradersTest
{
    [TestClass]
    //With more time I would mock the powerService and create a set of test trades
    //and predefined totals
    public class AggregationTests
    {
        [TestMethod]
        public void TestPowerService()
        {
            DateTime dt = new DateTime(2019, 12, 24, 13, 44, 33);
            IPowerService ps = new PowerService();
            var results = ps.GetTrades(dt);
            Assert.IsNotNull(results);
        }
        [TestMethod]
        public void TestPowerPeriodAggregation()
        {
            DateTime dt = new DateTime(2019, 12, 24, 13, 44, 33);
            IPowerService ps = new PowerService();
            var results = ps.GetTrades(dt);
            double totalP1 = 0;
            foreach(var trade in results)
            {
                totalP1 += trade.Periods[0].Volume;
            }
            Aggregate aggregate = new Aggregate();
            var aggresults = aggregate.GetAggregatedVolumes(results);
            Assert.AreEqual(totalP1, aggresults[1]);

        }

       
    }
}
