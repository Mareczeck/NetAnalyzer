using System.Data.Common;
using System.Linq;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NetAnalyzer.Domain.Dataset;
using NetAnalyzer.Infrastructure;
using NetAnalyzer.Infrastructure.Persistence;

namespace NetAnalyzer.Business;

public class DatasetService : IDatasetService
{
    private readonly INetworkDataLoaderService networkDataLoader;
    private readonly AppDbContext dbContext;

    private const string MEMBER_COUNT_COMMAND = @"SELECT COUNT(*) FROM (
        SELECT [MemberOne] FROM [Relations] WHERE DatasetID = @DatasetId
        UNION
        SELECT [MemberTwo] FROM [Relations] WHERE DatasetID = @DatasetId);";

        
    private const string SUM_RELATIONS_COMMAND = @"SELECT SUM(cnt) FROM (
        SELECT count(MemberOne) as cnt FROM [Relations] WHERE DatasetID = @DatasetId GROUP BY MemberOne 
        UNION ALL
        SELECT count(MemberTwo) as cnt FROM [Relations] WHERE DatasetID = @DatasetId GROUP BY MemberTwo)";


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

    public List<DatasetInfoStatistic>? LoadDatasets()
    {
        var dataSets = dbContext.DataSets.Select(x => new DatasetInfoStatistic()
        {
            DatasedId = x.Id,
            State = x.State
        }).ToList();

        var connection = dbContext.Database.GetDbConnection();
        connection.Open();

        using var command = connection.CreateCommand();
        var parameter = command.CreateParameter();
        parameter.ParameterName = "@DatasetId";
        command.Parameters.Add(parameter);

        foreach (var item in dataSets)
        {
            parameter.Value = item.DatasedId;
            command.CommandText = MEMBER_COUNT_COMMAND;

            var result = command.ExecuteScalar();
            var count = result != null ? Convert.ToInt32(result) : throw new Exception();
            item.Members = count;

            if(count > 0){
                command.CommandText = SUM_RELATIONS_COMMAND;

                result = command.ExecuteScalar();
                var sum = result != null ? Convert.ToInt32(result) : throw new Exception();
                item.AverageRelation = (decimal)sum / count;
            }        
        }
        
        connection.Close();
        return dataSets;
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
