using PostSharp.Aspects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace TennisWithMe_WebApi.Aspects
{
    [Serializable]
    public class TimerAspect : OnMethodBoundaryAspect
    {
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
            Console.WriteLine("Method {0}: duration={1}", args.Method.Name, _stopwatch.Elapsed);
        }

    }
}