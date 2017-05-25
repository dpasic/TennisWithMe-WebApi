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
        private Stopwatch stopwatch = new Stopwatch();

        public override void RuntimeInitialize(System.Reflection.MethodBase method)
        {
            stopwatch = new Stopwatch();
        }

        public override void OnEntry(MethodExecutionArgs args)
        {
            stopwatch.Start();
        }

        public override void OnSuccess(MethodExecutionArgs args)
        {
            stopwatch.Stop();
            Console.WriteLine("Method {0}: duration={1}", args.Method.Name, stopwatch.Elapsed);
        }

    }
}