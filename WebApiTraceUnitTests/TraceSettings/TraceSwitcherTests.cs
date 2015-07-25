using WebApiTrace.TraceSettings;
using NUnit.Framework;

namespace WebApiTraceUnitTests.TraceSettings
{
    [TestFixture]
    public class TraceSwitcherTests
    {
        TraceSwitcher.TraceLevel _originalTraceLevel;
        TraceSwitcher.TraceVerbosity _originalTraceVerbosity;

        [SetUp]
        public void SetUp()
        {
            _originalTraceLevel = TraceSwitcher.Level;
            _originalTraceVerbosity = TraceSwitcher.Verbosity;
        }

        [TearDown]
        public void TearDown()
        {
            TraceSwitcher.Level = _originalTraceLevel;
            TraceSwitcher.Verbosity = _originalTraceVerbosity;
        }

        [Test]
        public void CanSwitchLevels()
        {
            TraceSwitcher.Level = TraceSwitcher.TraceLevel.None;
            Assert.AreEqual(TraceSwitcher.TraceLevel.None, TraceSwitcher.Level);

            TraceSwitcher.Level = TraceSwitcher.TraceLevel.Error;
            Assert.AreEqual(TraceSwitcher.TraceLevel.Error, TraceSwitcher.Level);

            TraceSwitcher.Level = TraceSwitcher.TraceLevel.Debug;
            Assert.AreEqual(TraceSwitcher.TraceLevel.Debug, TraceSwitcher.Level);
        }

        [Test]
        public void CanSwitchVerbosity()
        {
            TraceSwitcher.Verbosity = TraceSwitcher.TraceVerbosity.Minimal;
            Assert.AreEqual(TraceSwitcher.TraceVerbosity.Minimal, TraceSwitcher.Verbosity);

            TraceSwitcher.Verbosity = TraceSwitcher.TraceVerbosity.General;
            Assert.AreEqual(TraceSwitcher.TraceVerbosity.General, TraceSwitcher.Verbosity);

            TraceSwitcher.Verbosity = TraceSwitcher.TraceVerbosity.Minimal;
            Assert.AreEqual(TraceSwitcher.TraceVerbosity.Minimal, TraceSwitcher.Verbosity);
        }

    }
}
