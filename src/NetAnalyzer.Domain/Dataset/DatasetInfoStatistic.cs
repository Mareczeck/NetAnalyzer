using System.ComponentModel.DataAnnotations;
using NetAnalyzer.Domain.Dataset;

namespace NetAnalyzer.Domain.Dataset;

public class DatasetInfoStatistic
{
    public int DatasedId { get; set; } 
    
    public State State { get; set; }

    public int Members { get; set; }

    [DisplayFormat(DataFormatString = "{0:0.00}")]
    public decimal AverageRelation { get; set; }
}
