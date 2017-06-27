using PostSharp.Aspects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;

namespace TennisWithMe_WebApi.Aspects
{
    [Serializable]
    public class TimerAspect : OnMethodBoundaryAspect
    {
        private static string FILE_PATH = @"C:\temp\TennisWithMe\timer-data.txt";

        [NonSerialized]
        private Stopwatch _stopwatch;

        public override void RuntimeInitialize(System.Reflection.MethodBase method)
        {
            _stopwatch = new Stopwatch();
        }

        public override void OnEntry(MethodExecutionArgs args)
        {
            _stopwatch.Start();
        }

        public override void OnSuccess(MethodExecutionArgs args)
        {
            _stopwatch.Stop();

            using (var writter = new StreamWriter(FILE_PATH, true))
            {
                writter.WriteLine("Method = {0}: Duration = {1}", args.Method.Name, _stopwatch.Elapsed);
            }
        }

    }
}