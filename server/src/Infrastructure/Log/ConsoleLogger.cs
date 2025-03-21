namespace Infrastructure.Log

{
    public static class ConsoleLogger: ILogger
    {
		public static void Log(params object[] args)
        {
            Console.WriteLine(string.Join(" ", args));
        }
    }
}

