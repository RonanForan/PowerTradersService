using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PowerTradersLibrary;

namespace PowerTradersTest
{
    [TestClass]
    public class IntegrationTests
    {
        [TestMethod]
        
        public void MimicStartAndStopServiceWithoutErrors()
        {
            Workflow workflow = new Workflow();
            workflow.OnStart();
            Thread.Sleep(10000);
            workflow.OnEnd();
        }
    }
}
