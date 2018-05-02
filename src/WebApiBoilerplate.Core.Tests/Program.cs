using System;
using NUnit.Common;

namespace WebApiBoilerplate.Core.Tests
{
    class Program
    {
        public static int Main(string[] args)
        {
            var runner = new NUnitLite.AutoRun(typeof(Program).Assembly);

            return runner.Execute(args, new ColorConsoleWriter(colorEnabled: true), Console.In);
        }
    }
}
