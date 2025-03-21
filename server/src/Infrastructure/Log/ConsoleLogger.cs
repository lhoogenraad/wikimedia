namespace Infrastructure.Log

{
    public class ConsoleLogger: ILogger
    {
		public void Log(params object[] args)
        {
            Console.WriteLine(string.Join(" ", args));
        }
    }
}

