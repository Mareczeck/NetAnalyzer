using System.Diagnostics;

namespace NetAnalyzer.Infrastructure.Tests;

[TestClass]
public class NetworkDataLoaderServiceTest
{
    [TestMethod]
    public void TestLoadingFromData_NoDuplicity()
    {
        string dataset = "1 2\n2 3\n3 1\n2 5\n";
        var dataLoader = new NetworkDataLoaderService();
        
        var result = dataLoader.ReadData(new MemoryStream(System.Text.Encoding.UTF8.GetBytes(dataset)));

        Assert.AreEqual(4, result.Count);
    }
    
    [TestMethod]
    public void TestLoadingFromData_Duplicities()
    {
        string dataset = "1 2\n2 3\n3 1\n1 3\n";
        var dataLoader = new NetworkDataLoaderService();
        
        var result = dataLoader.ReadData(new MemoryStream(System.Text.Encoding.UTF8.GetBytes(dataset)));

        Assert.AreEqual(3, result.Count);
    }
    
    [TestMethod]
    public void TestLoadingFromData_Mismatch()
    {
        string dataset = "1 2\na b\n3 1\n1 3\n";
        var dataLoader = new NetworkDataLoaderService();

        Assert.ThrowsException<InvalidFormatException>(() => dataLoader.ReadData(new MemoryStream(System.Text.Encoding.UTF8.GetBytes(dataset))));
    }
    
    [TestMethod]
    public void TestLoadingFromData_LoadFromFile()
    {
        var fs = File.OpenRead("network-data.txt");
        var dataLoader = new NetworkDataLoaderService();
        var result = dataLoader.ReadData(fs);

        // Instead don't throw exception
        Assert.IsNotNull(result);
     }

     
    // [TestMethod]
    // public void TestLoadingFromData_OptimalizationTest()
    // {
    //     var sw = new Stopwatch();
    //     sw.Start();
    //     var fs = File.OpenRead("network-data.txt");
    //     var dataLoader = new NetworkDataLoaderService();
    //     var result = dataLoader.ReadData(fs);
    //     sw.Stop();
        
    //     Console.WriteLine(sw.Elapsed);
    //  }
}