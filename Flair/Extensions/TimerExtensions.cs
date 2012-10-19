using System.Timers;

namespace Flair.Extensions
{
    public static class TimerExtensions
    {
         public static void Reset(this Timer timer)
         {
             if (timer == null) return;

             timer.Stop();
             timer.Start();
         }
    }
}