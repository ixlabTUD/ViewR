using System;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace ViewR.HelpersLib.Universals.Performance.StopWatch
{
    public class StopWatchMeasurementTool : MonoBehaviour
    {
        private readonly Stopwatch _watch = new Stopwatch();
        
        public void StartWatch() => _watch.Start();

        public void StopWatch() => _watch.Stop();

        public void RestartWatch() => _watch.Restart();
        
        public void ResetWatch() => _watch.Reset();

        public TimeSpan CurrentWatchTime => _watch.Elapsed;

        public void PrintCurrentTime()
        {
            Debug.Log("Time active: " + CurrentWatchTime);
        }
    }
}
