using System.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NetAnalyzer.Domain.Dataset;
using NetAnalyzer.Infrastructure.Persistence;

namespace NetAnalyzer.Infrastructure.Tests;

[TestClass]
public class DatabaseQueriesTest
{
    public const string DB_CONNECTION_STRING = "DataSource=/Users/kristyna.lukesova/Projekty/Database/DataStorage.db";



    [TestMethod]
    public void TestInsertData()
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
                context.Database.EnsureCreated();
            }
            // Run the test against one instance of the context
            using (var context = new AppDbContext(options))
            {
                var dataSet = new DatasetInfo(State.New, "Some name");
                var e = context.DataSets.Add(dataSet);
                context.SaveChanges();
                
                context.Relations.Add(new Relation(dataSet.Id,1,2));
                context.Relations.Add(new Relation(dataSet.Id,2,3));
                context.Relations.Add(new Relation(dataSet.Id,3,1));
                
                context.SaveChanges();
            }
        }
        finally
        {
            connection.Close();
        }
    }


    
}