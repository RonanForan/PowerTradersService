using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PowerTradersLibrary;


namespace PowerTradersTest
{
    [TestClass]
    public class FileTests
    {
        [TestMethod]
        public void TestFileName()
        {
            ConfigManager configManager = new ConfigManager();
            FileManager fm = new FileManager(configManager);
            DateTime dt = new DateTime(2019, 12, 24, 13, 44, 33);
            Assert.AreEqual("PowerPosition_20191224_1344.csv", fm.GetFileName(dt));
        }
       
        [TestMethod]
        public void TestPath()
        {
            ConfigManager configManager = new ConfigManager();
            FileManager fm = new FileManager(configManager);
            DateTime dt = new DateTime(2019, 12, 24, 13, 44, 33);
            Assert.AreEqual(@"c:\temp\PowerPosition_20191224_1344.csv", fm.GetPath(dt));
        }

        
    }
}
