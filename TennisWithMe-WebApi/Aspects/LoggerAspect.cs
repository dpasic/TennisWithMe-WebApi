using PostSharp.Aspects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TennisWithMe_WebApi.Aspects
{
    [Serializable]
    public class LoggerAspect : OnMethodBoundaryAspect
    {
        private static string FILE_PATH = @"C:\temp\TennisWithMe\logger-data.txt";

        public override void OnEntry(MethodExecutionArgs args)
        {
            using (var writter = new StreamWriter(FILE_PATH, true))
            {
                writter.WriteLine("Date = {0}; Entered method call = {1}", DateTime.Now, args.Method.Name);
            }
        }

        public override void OnSuccess(MethodExecutionArgs args)
        {
            using (var writter = new StreamWriter(FILE_PATH, true))
            {
                writter.WriteLine("Date = {0}; Successfully executed method call = {1}", DateTime.Now, args.Method.Name);
            }
        }

        public override void OnException(MethodExecutionArgs args)
        {
            using (var writter = new StreamWriter(FILE_PATH, true))
            {
                writter.WriteLine("Date = {0}; Exception was thrown while executing method call = {1}", DateTime.Now, args.Method.Name);
            }
        }
    }
}
