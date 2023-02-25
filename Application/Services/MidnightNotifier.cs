using System.Timers;
using Timer = System.Timers.Timer;

namespace Application.Services
{
    public class MidnightNotifier
    {
        public readonly Timer Timer;

        public MidnightNotifier()
        {
            // Create a timer that ticks every minute
            Timer = new Timer(GetSleepTime());
            Timer.Elapsed += OnTimerElapsed;
            Timer.Enabled = true;
        }

        private static double GetSleepTime()
        {
            var midnightTonight = DateTime.Today.AddDays(1);
            var differenceInMilliseconds = (midnightTonight - DateTime.Now).TotalMilliseconds;
            return differenceInMilliseconds;
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            Timer.Interval = GetSleepTime();
        }
    }
}

