using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Collections.Specialized;
using Serilog;

namespace PowerTradersLibrary
{
    //Config manager class so clients are unaware of how or where the config resides    
    public class ConfigManager 
    {
        private ILogger _log = new LoggerConfiguration().WriteTo.Console().CreateLogger();
        private NameValueCollection AppSettingsConfig
        {
            get
            {
                return ConfigurationManager.AppSettings;
            }
        }

        public String GetAppSettingsValue(string index)
        {
            _log.Information("Retrieving value for {0}", index);
            return AppSettingsConfig[index];
        }

      
    }

    
}
