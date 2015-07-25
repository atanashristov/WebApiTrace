using System;
using WebApiTraceUnitTests.TraceSettings;

namespace WebApiTraceUnitTests
{
    class Program
    {
        static void Main(string[] args)
        {
            var suite = new TraceSwitcherTests();
            suite.SetUp();
            suite.CanSwitchLevels();
            suite.CanSwitchVerbosity();
            suite.TearDown();

            Console.WriteLine("OK.");

            if (System.Diagnostics.Debugger.IsAttached)
            {
                Console.WriteLine("Press <Enter> to close.");
                Console.ReadLine();
            }

        }
    }
}
