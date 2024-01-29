using NetAnalyzer.Domain.Dataset;

namespace NetAnalyzer.Web.Models;

public class DatasetStatisticsViewModel
{
    public List<DatasetInfoStatistic>? Statistics { get; set;} = new();
}
