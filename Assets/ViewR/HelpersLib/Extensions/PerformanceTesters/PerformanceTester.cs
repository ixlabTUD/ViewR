using System;
using System.Diagnostics;
using System.Linq;
using ViewR.HelpersLib.Extensions.General;

namespace ViewR.HelpersLib.Extensions.PerformanceTesters
{
    /// <summary>
    /// A basic performance tester, can be adopted for different tasks.
    /// </summary>
    public class PerformanceTester
    {
        public static void Main()
        {
            const int numberMainLoops = 10000;
            const int numberPerSubLoop = 10000;

            var durations = new System.TimeSpan[numberMainLoops];

            for (var j = 0; j < numberMainLoops; j++)
            {
                var timer = new Stopwatch(); // creating new instance of the stopwatch
                int[] array = {1, 2, 3, 4};
                var item = 5;

                timer.Start();

                for (var i = 0; i < numberPerSubLoop; i++)
                {
                    var result = array.Append(item);
                }

                timer.Stop();

                // Console.WriteLine(timer.Elapsed);
                durations[j] = timer.Elapsed;
            }

            var mode = durations.GroupBy(v => v)
                .OrderByDescending(g => g.Count())
                .First()
                .Key;
            Console.WriteLine("Mode: " + mode);

            var timeSpans = TimeSpan.Zero;
            foreach (var timeSpan in durations)
                timeSpans += timeSpan;

            Console.WriteLine("timeSpans: " + timeSpans);
            Console.WriteLine("timeSpans avg: " + new TimeSpan(timeSpans.Ticks / numberMainLoops));
        }
    }
}