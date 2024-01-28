using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NetAnalyzer.Infrastructure;
using NetAnalyzer.Infrastructure.Persistence;

namespace NetAnalyzer.Business.Tests;

[TestClass]
public class DatasetServiceTests
{
    public const string DB_CONNECTION_STRING = "DataSource=/Users/kristyna.lukesova/Projekty/Database/DataStorage.db";

    [TestMethod]
    public void TestInsertDataAll()
    {
        // In-memory database only exists while the connection is open
        var connection = new SqliteConnection(DB_CONNECTION_STRING);
        connection.Open();

        try
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite(connection)
                .Options;

            // Create the schema in the database
            using (var context = new AppDbContext(options))
            {
                var fs = File.OpenRead("network-data.txt");
                var dataLoader = new NetworkDataLoaderService();

                var service = new DatasetService(dataLoader, context);

                service.ProcessDataset(1, fs);
            }
        }
        finally
        {
            connection.Close();
        }
    }
}