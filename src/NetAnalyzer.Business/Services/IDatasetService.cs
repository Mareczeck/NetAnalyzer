using NetAnalyzer.Domain.Dataset;

namespace NetAnalyzer.Business;

public interface IDatasetService {

    int CreateDataset(string name);
    List<DatasetInfoStatistic>? LoadDatasetStatistics();

    DatasetInfoStatistic LoadDatasetStatistic(int datasetId);
    GraphModel LoadDataset(int datasetId);
    void ProcessDataset(int datasetId, Stream stream);
}
