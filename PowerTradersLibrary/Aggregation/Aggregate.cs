
using Serilog;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerTradersLibrary
{
    public class Aggregate
    {
        private ILogger _log = new LoggerConfiguration().WriteTo.Console().CreateLogger();
        //Aggegate positions into power periods 
        //Consider using an adaptor class to bridge between external objects 
        //thereby protecting this implementation from changes in the power service api/objects
        public double[] GetAggregatedVolumes(IEnumerable<PowerTrade> powerTrades)
        {
            //I would log elapsed time here for a live application
            _log.Information(String.Format("Start aggregating power trade of count {0}", powerTrades.Count()));
            //add one to capacity so we're not required to zero index the array
            double[] power = new double[Globals.NUMBER_OF_PERIODS+1];
            foreach(PowerTrade powerTrade in powerTrades)
            {
                foreach(PowerPeriod powerPeriod in powerTrade.Periods)
                {
                    power[powerPeriod.Period] += powerPeriod.Volume;
                }
            }
            _log.Information(String.Format("Finished aggregating power trades"));
            return power;
        }
    }
}
