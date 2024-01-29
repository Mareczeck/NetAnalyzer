using NetAnalyzer.Domain.Dataset;

namespace NetAnalyzer.Business;

public interface IDatasetService {

    int CreateDataset(string name);
    List<DatasetInfoStatistic>? LoadDatasets();
    void ProcessDataset(int datasetId, Stream stream);
}
