using NetAnalyzer.Domain.Dataset;

namespace NetAnalyzer.Business;

public interface IDatasetService {

    int CreateDataset();
    List<DatasetInfoStatistic>? LoadDatasets();
    void ProcessDataset(int datasetId, Stream stream);
}
