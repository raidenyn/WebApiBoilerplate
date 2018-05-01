using System.Linq;

namespace WebApiBoilerplate.Core.Tests
{
    class Program
    {
        public static int Main(string[] args)
        {
            args = new [] { typeof(Program).Assembly.Location }.Concat(args).ToArray();

            return Xunit.ConsoleClient.Program.Main(args);
        }
    }
}
