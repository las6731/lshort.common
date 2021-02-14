namespace LShort.Common.Database
{
    public interface IDatabaseConnectivityProvider<out IDatabase>
    {
        IDatabase Connect(string filePath);
    }
}