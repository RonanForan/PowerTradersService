using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerTradersLibrary
{
    public static class Globals
    {
        //CSV File Config
        public const string FILE_NAME_TEMPLATE = "PowerPosition_{0}_{1}.csv";
        public const string FILE_DATE_FORMAT = "yyyyMMdd";
        public const string FILE_TIME_FORMAT = "HHmm";
        public const string FILE_CONFIG_KEY = "FolderPath";
        public const string FILE_HEADER = "LocalTime,Volume";

        public const string FILE_PATH_STATE = @"c:\temp\LastRun.txt";

        //Power Periods
        public const short NUMBER_OF_PERIODS = 24;

        //interval conversion
        public const int RATIO_TO_MILISECONDS = 60000;
        public const int SCHEDULE_TOLERANCE = 1000;
        public const string INTERVAL_KEY = "Interval";
        public const int DEFAULT_MINUTES = 60;

    }
}
