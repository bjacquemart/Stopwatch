using System;
using System.Reactive.Linq;

namespace Stopwatch
{
   class Program
   {
      static void Main(string[] args)
      {
         int top = Console.CursorTop + 2;
         TimeSpan elapsedTime = TimeSpan.Zero;
         TimeSpan lapTime = TimeSpan.Zero;
         int lap = 0;

         var observable = Observable.Interval(TimeSpan.FromSeconds(1)).Timestamp().Publish();
         IDisposable stopwatch = observable.Subscribe(seconds =>
            {
               elapsedTime = TimeSpan.FromSeconds(seconds.Value);
               Console.SetCursorPosition(0, top);
               Console.Out.Write("elapsed time: {0}, current local time: {1}", elapsedTime, seconds.Timestamp.ToLocalTime());
               Console.Out.Flush();
            });

         Console.WriteLine();
         Console.WriteLine("Press [Enter] to start [L] to lap and [Esc] to stop the stopwatch...");

         bool loop = true;
         while (loop)
         {
            if (Console.KeyAvailable)
            {
               var key = Console.ReadKey(true);

               switch (key.Key)
               {
                  case ConsoleKey.Escape:
                     if (stopwatch != null)
                        stopwatch.Dispose();
                     loop = false;
                     Console.SetCursorPosition(0, top + ++lap);
                     break;
                  case ConsoleKey.L:
                     var currentlapTime = elapsedTime.Subtract(lapTime);
                     lapTime += currentlapTime;
                     Console.SetCursorPosition(0, top + ++lap);
                     Console.WriteLine("lap {0}, {1}", lap, currentlapTime);
                     break;
                  case ConsoleKey.Enter:
                     observable.Connect();
                     break;
                  default:
                     break;
               }               
            }
         }
      }
   }
}
