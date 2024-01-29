namespace NetAnalyzer.Domain.Dataset;

public record class DatasetInfo(State State)
{
    public int Id { get; set; }
}