using Serilog;
using Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace PowerTradersLibrary
{
    public class Workflow
    {
        //split out into classes anticipating more services/reports
        private readonly IPowerService _powerService;
        private readonly FileManager _fileManager;
        private readonly Aggregate _aggregate;
        private readonly ConfigManager _configManager;
        private Timer _timer;
        private readonly long _intervalInMiliseconds;
        //Using serilog
        //Outputting to console here but would switch to file for a live application
        private ILogger _log = new LoggerConfiguration().WriteTo.Console().CreateLogger();

        public Workflow()
        {
            //Use a dependency injection container (for unit/integration test) 
            //and a factory class to delegate initialisation
            _powerService = new PowerService();
            _configManager = new ConfigManager();
            _fileManager = new FileManager(_configManager);
            _aggregate = new Aggregate();
            _intervalInMiliseconds = GetIntervalFromConfig();
        }
        //Get interval from config file and convert from minutes to miliseconds
        public long GetIntervalFromConfig()
        {
            string val = _configManager.GetAppSettingsValue(Globals.INTERVAL_KEY);
            long minutes = Globals.DEFAULT_MINUTES ;
            long.TryParse(val, out minutes);
            long milis = minutes * Globals.RATIO_TO_MILISECONDS;
            _log.Information(String.Format("Interval set to {0}", milis));
            return milis; 
        }
        //Run the report and start the timer
        public void OnStart()
        {
            //required for logging and to raise alerts
            AppDomain.CurrentDomain.UnhandledException += MyHandler;
            _log.Information(String.Format("Service starting"));
            //run the report on start up but don't check for missing 
            CheckForMissingAndRunReport(DateTime.Now, false);
            _timer = new Timer(_intervalInMiliseconds);
            _timer.Elapsed += OnTimedEvent;
            _timer.AutoReset = true;
            _timer.Enabled = true;
            _timer.Start();
        }
        //Unhandled exceptions
        //Required for logging and raising alerts
        private void MyHandler(object sender, UnhandledExceptionEventArgs args)
        {
            Exception e = (Exception)args.ExceptionObject;
            _log.Error(e, "Unhandled Exception");
            _log.Error("Runtime terminating: {0}", args.IsTerminating);
        }
        //Shut down the timer and dispose
        public void OnEnd()
        {
            _log.Information(String.Format("Service Ending"));
            if (_timer != null) _timer.Stop();
            _timer.Dispose();
        }
       
        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            _log.Information(String.Format("Interval Report Start"));
            CheckForMissingAndRunReport(e.SignalTime, true);
            _log.Information(String.Format("Interval Report End"));
        }
        //Check for missing reports if there is a last run date.
        //Run any missing reports
        //Run the scheduled report
        public void CheckForMissingAndRunReport(DateTime reportTime, bool processMissing)
        {
            try
            {
                //Wrap in a try statement as we dont want the service to fail here.
                //Following events will fill in the missing extracts if the power service is 
                //temporarily down
                var lastRun = _fileManager.ReadLastRunDate();
                if (lastRun.HasValue & processMissing) ProcessMissing(lastRun.Value, reportTime);
                RunReport(reportTime);
            }
            catch (PowerServiceException psEx)
            {
                Log.Error(psEx, "Exception occurred whilst calling the PowerService");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Excpetion occurred whilst generating the report");
            }
        }
         //Look through missing reports and generate them.
         //Assumes that the power trading service can generate the positions
         //retrospectively
         public void ProcessMissing(DateTime last, DateTime reportTime)
         {
            _log.Information(String.Format("Checking for missing files"));

            while ((reportTime - last).Milliseconds > _intervalInMiliseconds+Globals.SCHEDULE_TOLERANCE)
            {
                var missingExtractTime = last.AddMilliseconds(_intervalInMiliseconds);
                _log.Information(String.Format("Running missing extract for: {0}", missingExtractTime));
                if (RunReport(missingExtractTime))
                {
                    last = missingExtractTime;
                }
            }
        }
        //Run the report. 
        //Capture the elapsed time for logging purposes.
        //I would also split the elapsed time by time taken to get the trades
        //aggregation time and time to write out the results. 
        //Bool required for the routine that generates missing extracts 
        public bool RunReport(DateTime reportTime)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            Boolean success = false;
            var trades = _powerService.GetTrades(reportTime);
            var aggregated = _aggregate.GetAggregatedVolumes(trades);
            _fileManager.WriteToFile(aggregated, reportTime);
            _fileManager.WriteLastRunDate(reportTime);
            success = true;
            stopWatch.Stop();
            _log.Information(String.Format("Report took {0} miliseconds to run", stopWatch.ElapsedMilliseconds));
            return success;
        }
    }
}
