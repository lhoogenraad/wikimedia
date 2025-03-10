using Util.Log;

namespace Util.Log
{
    public class ConsoleLogger : ILog
    {
        private const string NAME = "ConsoleLogger";

		public void Log(params object[] args)
        {
            Console.WriteLine(string.Join(" ", args));
        }
        

        public string Name()
        {
			return NAME;
        }
    }
}

