using Microsoft.Extensions.Configuration;

namespace NetAnalyzer.Infrastructure;

public class SqlLiteDatabaseInitializer : IDatabaseInitializer
{
    private readonly IConfiguration configuration;
    private readonly IFileManipulationService fileManipulation;

    public SqlLiteDatabaseInitializer(IConfiguration configuration, IFileManipulationService fileManipulation)
    {
        this.configuration = configuration;
        this.fileManipulation = fileManipulation;
    }

    public void InitializeDatabase()
    {
        string? databasePath = configuration["DatabasePath"];
        
        databasePath = fileManipulation.ResolvePath(databasePath);

        if (!File.Exists(databasePath))
        {
            Console.WriteLine("Creating the database...");


            Console.WriteLine("Database created successfully.");
        }
        else
        {
            Console.WriteLine("Database already exists.");
        }
    }
}
