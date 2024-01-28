using Microsoft.Extensions.Configuration;
using NetAnalyzer.Infrastructure.Persistence;

namespace NetAnalyzer.Infrastructure;

public class SqlLiteDatabaseInitializer(AppDbContext dbContext) : IDatabaseInitializer
{
    private readonly AppDbContext dbContext = dbContext;

    public void InitializeDatabase()
    {
        Console.WriteLine("Creating the database...");
        if (dbContext.Database.EnsureCreated())
        {


            Console.WriteLine("Database created successfully.");
        }
        else
        {
            Console.WriteLine("Database already exists.");
        }
    }
}
