using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PowerTradersLibrary;

namespace PowerTradersTest
{
    [TestClass]
    public class ConfigTests
    {
        [TestMethod]
        public void GetFolder()
        {
            ConfigManager configManager = new ConfigManager();
            Assert.AreEqual(@"c:\temp\", configManager.GetAppSettingsValue(Globals.FILE_CONFIG_KEY));
        }

        [TestMethod]
        public void GetIntervalInMiliseconds()
        {
            Workflow workflow = new Workflow();
            Assert.AreEqual(60000, workflow.GetIntervalFromConfig());
        }
    }
}
