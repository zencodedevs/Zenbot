using System;
using System.Collections.Generic;
using System.Timers;

namespace Zenbot.Services
{
    internal static class Interval
    {
        public const int
        SECOND = 1000,
        MINUTE = 60000,
        HOUR = 3600000,
        DAY = 86400000,
        WEEK = 604800000;
    }

    /**
     * Customizeable timerwrapper with exposed Elapsed event
     * 
     * Incomplete and currently not in use
     */
    public class TimerWrapper : IDisposable
    {
        private Timer timer;

        public TimerWrapper(int periodInMilliseconds = Interval.DAY)
        {
            timer = new Timer(periodInMilliseconds);
        }

        public event ElapsedEventHandler TimerEvent
        {
            add { timer.Elapsed += value; }
            remove { timer.Elapsed -= value; }
        }

        public void Dispose()
        {
            timer.Dispose();
        }
    }


    /**
     * Timer Factory
     */
    public class TimerFactory : IDisposable
    {
        // Dictionary of pairs <period, timer(period)>
        private Dictionary<int, Timer> timers = new();

        // If we already have a timer for this period - re-use it instead of creating a new one
        public Timer CreateTimer(int periodInMilliseconds)
        {
            Timer timer = null;

            if (!timers.TryGetValue(periodInMilliseconds, out timer))
            {
                timer = new(periodInMilliseconds);
                timers.Add(periodInMilliseconds, timer);
            }

            Console.WriteLine($"Timer initialized - {timer.Interval.ToString()} milliseconds");
            return timer;
        }

        public void Dispose()
        {
            foreach (var entry in timers)
            {
                entry.Value.Dispose();
            }
        }
    }
}
