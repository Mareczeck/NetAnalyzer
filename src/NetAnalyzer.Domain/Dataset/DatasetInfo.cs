namespace NetAnalyzer.Domain.Dataset;

public record class DatasetInfo(State State, string Name)
{
    public int Id { get; set; }
}