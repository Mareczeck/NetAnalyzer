using Microsoft.EntityFrameworkCore;
using NetAnalyzer.Domain.Dataset;
using NetAnalyzer.Infrastructure;
using NetAnalyzer.Infrastructure.Persistence;

namespace NetAnalyzer.Business;

public interface IDatasetService {

    public int CreateDataset();
    public void ProcessDataset(int datasetId, Stream stream);
}

public class DatasetService : IDatasetService
{
    private readonly INetworkDataLoaderService networkDataLoader;
    private readonly AppDbContext dbContext;


    public DatasetService(INetworkDataLoaderService networkDataLoader, AppDbContext dbContext)
    {
        this.networkDataLoader = networkDataLoader;
        this.dbContext = dbContext;
    }


    public int CreateDataset()
    {
        var ds = new DatasetInfo(State.New);
        var dataset = dbContext.DataSets.Add(ds);

        dbContext.SaveChanges();

        return ds.Id;
    }
    
    public void ProcessDataset(int datasetId, Stream stream)
    {
        using var transaction = dbContext.Database.BeginTransaction();

        try
        {
            var dataSet = dbContext.DataSets.Find(datasetId);
            var relations = networkDataLoader.ReadData(stream);

            foreach (var item in relations)
            {
                dbContext.Relations.Add(new Relation(datasetId, item.n1, item.n2));
            }

            dbContext.SaveChanges();
            
            transaction.Commit();
        }
        catch{

            transaction.Rollback();
        }
    }
}
