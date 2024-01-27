namespace NetAnalyzer.Infrastructure;

public interface INetworkDataLoaderService
{
    HashSet<(int n1, int n2)> ReadData(Stream stream);
}
