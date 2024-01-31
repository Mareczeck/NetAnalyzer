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
    private readonly IGraphProcessor graphProcessor;
    private readonly AppDbContext dbContext;

    private const string MEMBER_COUNT_COMMAND = @"SELECT COUNT(*) FROM (
        SELECT [MemberOne] FROM [Relations] WHERE DatasetID = @DatasetId
        UNION
        SELECT [MemberTwo] FROM [Relations] WHERE DatasetID = @DatasetId);";

    private const string SUM_RELATIONS_COMMAND = @"SELECT SUM(cnt) FROM (
        SELECT count(MemberOne) as cnt FROM [Relations] WHERE DatasetID = @DatasetId GROUP BY MemberOne 
        UNION ALL
        SELECT count(MemberTwo) as cnt FROM [Relations] WHERE DatasetID = @DatasetId GROUP BY MemberTwo)";


    private const string CLEAN_COMMAND = "DELETE FROM Relations; DELETE FROM DatasetInfos;";

    public DatasetService(INetworkDataLoaderService networkDataLoader, AppDbContext dbContext, IGraphProcessor graphProcessor)
    {
        this.networkDataLoader = networkDataLoader;
        this.dbContext = dbContext;
        this.graphProcessor = graphProcessor;
    }


    public int CreateDataset(string name)
    {
        var ds = new DatasetInfo(State.New, name);
        
        dbContext.DataSets.Add(ds);
        dbContext.SaveChanges();

        return ds.Id;
    }

    public GraphModel LoadDatasetWithMaxDistance(int datasetId)
    {
        var model = LoadDataset(datasetId);

        // Count max distance
        graphProcessor.CalculateMaximalDistanceBetweenNodes(model);

        return model;
    }

    private GraphModel LoadDataset(int datasetId)
    {        
        var memberOneQuery = dbContext.Relations.Where(r => r.DatasetID == datasetId).Select(r => r.MemberOne);
        var memberTwoQuery = dbContext.Relations.Where(r => r.DatasetID == datasetId).Select(r => r.MemberTwo);

        var model = new GraphModel()
        {
            // Unique nodes
            Nodes = memberOneQuery.Union(memberTwoQuery).Select(x => new Node(x,1)).ToList(),
            Links = dbContext.Relations.Where(r => r.DatasetID == datasetId).Select(x => new Link(x.MemberOne, x.MemberTwo)).ToList()
        };

        return model;
    } 

    public DatasetInfoStatistic LoadDatasetStatistic(int datasetId)
    {
        var dataset = dbContext.DataSets.FirstOrDefault(x => x.Id == datasetId);
        if(dataset == null)
            throw new Exception("Dataset not found");
            
        
        var connection = dbContext.Database.GetDbConnection();
        connection.Open();

        using var command = connection.CreateCommand();
        var parameter = command.CreateParameter();
        parameter.ParameterName = "@DatasetId";
        command.Parameters.Add(parameter);

        var statistic = new DatasetInfoStatistic()
        {
            DatasetId = datasetId,
            DatasetName = dataset.Name,
            State = dataset.State
        };

        GetDbStatisticForDatasets(statistic);
        return statistic;
    }

    public List<DatasetInfoStatistic>? LoadDatasetStatistics()
    {
        var dataSets = dbContext.DataSets.Select(x => new DatasetInfoStatistic()
        {
            DatasetId = x.Id,
            DatasetName = x.Name,
            State = x.State
        }).ToArray();

        GetDbStatisticForDatasets(dataSets);
        return dataSets.ToList();
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
            throw;
        }
    }
    
    public void CleanData()
    {
        dbContext.Database.ExecuteSqlRaw(CLEAN_COMMAND);

    }

    public AverageLinksModel GetAverageLinksByDistance(int datasetId, int distance)
    {        
        var model = LoadDataset(datasetId);
        
        // Count average links by distance
        return new AverageLinksModel() 
        { 
            AverageLinks = graphProcessor.GetAverageLinks(model, distance) 
        };
    }

    public ReachableNodesModel GetReachableNodesForNode(int datasetId, int node, int distance)
    {
        var model = LoadDataset(datasetId);
        model.Preprocess();

        var nodesDictionary = graphProcessor.ReachableLinksForNode(model.Nodes.FirstOrDefault(x => x.Id == node), distance);
        return new ReachableNodesModel() 
        { 
            Nodes = nodesDictionary.ToDictionary(pair => pair.Key.Id, pair => pair.Value)
        };
    }
    private void GetDbStatisticForDatasets(params DatasetInfoStatistic[] statistics)
    {
        var connection = dbContext.Database.GetDbConnection();
        connection.Open();

        using var command = connection.CreateCommand();
        var parameter = command.CreateParameter();
        parameter.ParameterName = "@DatasetId";
        command.Parameters.Add(parameter);

        foreach(var item in statistics)
        {
            parameter.Value = item.DatasetId;
            command.CommandText = MEMBER_COUNT_COMMAND;

            var result = command.ExecuteScalar();
            var count = result != null ? Convert.ToInt32(result) : throw new Exception();
            item.Members = count;

            if (count > 0)
            {
                command.CommandText = SUM_RELATIONS_COMMAND;

                result = command.ExecuteScalar();
                var sum = result != null ? Convert.ToInt32(result) : throw new Exception();
                item.AverageRelation = (decimal)sum / count;
            }
        }

        connection.Close();
    }

}