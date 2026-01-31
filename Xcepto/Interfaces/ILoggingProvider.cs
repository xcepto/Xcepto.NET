namespace Xcepto.Interfaces
{
    public interface ILoggingProvider
    {
        void LogDebug(string message);

        public void Flush();
    }
}