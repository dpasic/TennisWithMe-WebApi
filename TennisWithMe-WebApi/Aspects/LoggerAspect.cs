using PostSharp.Aspects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TennisWithMe_WebApi.Aspects
{
    [Serializable]
    public class LoggerAspect : OnMethodBoundaryAspect
    {
        public override void OnEntry(MethodExecutionArgs args)
        {
            Console.WriteLine("Entered method call: {0}", args.Method.Name);
        }

        public override void OnSuccess(MethodExecutionArgs args)
        {
            Console.WriteLine("Successfully executed method call: {0}", args.Method.Name);
        }

        public override void OnException(MethodExecutionArgs args)
        {
            Console.WriteLine("Exception was thrown while executing method call: {0}", args.Method.Name);
        }
    }
}
