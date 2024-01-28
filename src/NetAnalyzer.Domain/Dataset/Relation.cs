namespace NetAnalyzer.Domain.Dataset;

public enum State
{
    New,
    InProcess,
    Saved,
    Deleted
}


public record class Relation(int DatasetID, int MemberOne, int MemberTwo)
{
    public int Id { get; set; }
}

public record class DatasetInfo(State State)
{
    public int Id { get; set; }
}