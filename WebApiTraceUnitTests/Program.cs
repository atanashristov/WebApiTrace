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
        }
    }
}
