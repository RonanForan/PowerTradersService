
using PowerTradersLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Workflow workflow = new Workflow();
            workflow.OnStart();
            Console.ReadLine();
            workflow.OnEnd();
        }
    }
}
