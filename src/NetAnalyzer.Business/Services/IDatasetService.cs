using NetAnalyzer.Domain.Dataset;

namespace NetAnalyzer.Business;

public interface IDatasetService {

    int CreateDataset(string name);
    List<DatasetInfoStatistic>? LoadDatasetStatistics();

    DatasetInfoStatistic LoadDatasetStatistic(int datasetId);
    GraphModel LoadDatasetWithMaxDistance(int datasetId);

    AverageLinksModel GetAverageLinksByDistance(int datasetId, int distance);

    ReachableNodesModel GetReachableNodesForNode(int datasetId, int node, int distance);
    void ProcessDataset(int datasetId, Stream stream);

    void CleanData();
}
