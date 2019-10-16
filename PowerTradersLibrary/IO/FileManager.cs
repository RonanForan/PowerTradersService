using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerTradersLibrary
{
    public class FileManager
    { 
        private string _fileFolder;
        private ILogger _log = new LoggerConfiguration().WriteTo.Console().CreateLogger();
        public FileManager(ConfigManager configManager)
        {
            //just get this once from config 
            _fileFolder = GetFolder(configManager);

        }
        //Public for unit test
        public string GetFileName(DateTime dt)
        {
            return String.Format(Globals.FILE_NAME_TEMPLATE, dt.ToString(Globals.FILE_DATE_FORMAT), dt.ToString(Globals.FILE_TIME_FORMAT));
        }

        private string GetFolder(ConfigManager configManager)
        {
            return configManager.GetAppSettingsValue(Globals.FILE_CONFIG_KEY);
        }
        //Public for unit test
        public string GetPath(DateTime dt)
        {
            return Path.Combine(_fileFolder, GetFileName(dt));
        }

        //Take the power period positions and write to file 
        public void WriteToFile(double[] positions, DateTime dt)
        {
            _log.Information("Write the Power Period Positions to file");
            //add one to array for header
            String[] lines = new string[Globals.NUMBER_OF_PERIODS + 1];
            lines[0] = Globals.FILE_HEADER;
            //Report starts at hour 23 the previous day
            //Put this hour into globals or config potentially
            DateTime dtOffset = new DateTime(1, 1, 1, 23, 00, 00);
            for(int i=1;i<positions.Count();i++)
            {
                lines[i] = PositionToString(i - 1, positions[i], dtOffset);
            }
            var path = GetPath(dt);
            File.WriteAllLines(path, lines);
            _log.Information("Finished writing data to path {0}", path);
        }
        //Converts to hour with specified format
        private String PositionToString(int index, double value, DateTime dt)
        {
            return String.Format("{0},{1}", dt.AddHours(index).ToString("HH:mm"), value);
        }
        //Write the last run date to a state file
        //Required to backfill missing reports
        public void WriteLastRunDate(DateTime dt)
        {
            using (var sw = File.CreateText(Globals.FILE_PATH_STATE))
            {
                sw.WriteLine(dt.ToString());
            }
        }
        //Read the last run date
        //Returns nothing if there hasn't been an extract yet
        public DateTime? ReadLastRunDate()
        {
            if (!System.IO.File.Exists(Globals.FILE_PATH_STATE)) return null;

            using (var sr = new System.IO.StreamReader(Globals.FILE_PATH_STATE))
            {
                var line = sr.ReadLine();
                DateTime lastRun;
                if (DateTime.TryParse(line, out lastRun)) return DateTime.Parse(line);
                else throw new Exception("State file does not contain valid date");
            }
        }
    }
       
}
