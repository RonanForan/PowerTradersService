
using PowerTradersLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace PowerTradersWindowsService
{
    public partial class ReportService : ServiceBase
    {
        private Workflow _workflow;
        public ReportService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            _workflow = new Workflow();
            _workflow.OnStart();
        }

        protected override void OnStop()
        {
            _workflow.OnEnd();
        }
    }
}
