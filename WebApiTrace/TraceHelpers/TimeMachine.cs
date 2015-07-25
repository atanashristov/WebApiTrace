using System;

namespace WebApiTrace.TraceHelpers
{
    /// <summary>
    /// Used to easy testability.
    /// </summary>
    public class TimeMachine
    {
        public virtual DateTime Now
        {
            get { return DateTime.Now; }
        }

        public virtual DateTime UtcNow
        {
            get { return DateTime.UtcNow; }
        }

    }
}
