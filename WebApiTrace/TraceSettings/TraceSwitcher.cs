using System.Threading;

namespace WebApiTrace.TraceSettings
{
    public class TraceSwitcher
    {
        static readonly ReaderWriterLockSlim Locker = new ReaderWriterLockSlim();


        public const TraceLevel DefaultLevel = TraceLevel.Error;
        public const TraceVerbosity DefaultVerbosity = TraceVerbosity.Minimal;

        static TraceLevel _level = DefaultLevel;
        static TraceVerbosity _verbosity = DefaultVerbosity;


        static TraceSwitcher()
        { }

        public static void ResetSettings()
        {
            Level = DefaultLevel;
            Verbosity = DefaultVerbosity;
        }

        public static void TurnOff()
        {
            Level = TraceLevel.None;
            Verbosity = TraceVerbosity.Minimal;
        }


        // Trace level settings

        /// <summary>
        /// Trace level: Get/set what events is to show in the trace.
        /// </summary>
        public static TraceLevel Level
        {
            get
            {
                try { return GetTraceLevel(); }
                catch { return DefaultLevel; }
            }
            set
            {
                try { SetTraceLevel(value); }
                catch { }
            }
        }

        private static void SetTraceLevel(TraceLevel newValue)
        {
            Locker.EnterWriteLock();
            try { _level = newValue; }
            finally { Locker.ExitWriteLock(); }
        }

        private static TraceLevel GetTraceLevel()
        {
            Locker.EnterReadLock();
            try { return _level; }
            finally { Locker.ExitReadLock(); }
        }



        // Verbosity settings

        /// <summary>
        /// Trace verbosity: Get/set how much information to show in the trace.l
        /// </summary>
        public static TraceVerbosity Verbosity
        {
            get
            {
                try { return GetVerbosity(); }
                catch { return DefaultVerbosity; }
            }
            set
            {
                try { SetVerbosity(value); }
                catch { }
            }
        }

        private static void SetVerbosity(TraceVerbosity newValue)
        {
            Locker.EnterWriteLock();
            try { _verbosity = newValue; }
            finally { Locker.ExitWriteLock(); }
        }

        private static TraceVerbosity GetVerbosity()
        {
            Locker.EnterReadLock();
            try { return _verbosity; }
            finally { Locker.ExitReadLock(); }
        }


        /// <summary>
        /// Trace levels: Controls what events is to show in the trace.
        /// </summary>
        public enum TraceLevel
        {
            /// <summary>
            /// Do not do tracing.
            /// </summary>
            None,

            /// <summary>
            /// Only show errors.
            /// </summary>
            Error,

            /// <summary>
            /// Show trace for every request.
            /// </summary>
            Debug
        }


        /// <summary>
        /// Trace verbosity: Controls how much information to show in the trace.
        /// </summary>
        public enum TraceVerbosity
        {
            /// <summary>
            /// Only show default information.
            /// </summary>
            Minimal,

            /// <summary>
            /// Show basic information, incl. http url.
            /// </summary>
            General,

            /// <summary>
            /// Show the  complete request pipeline and other information.
            /// </summary>
            Verbose
        }
    }
}
