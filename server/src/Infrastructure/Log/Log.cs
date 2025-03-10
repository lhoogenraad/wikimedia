namespace Util.Log
{
    public interface ILog
    {
        void Log(params object[] args);
        string Name();
    }
}

