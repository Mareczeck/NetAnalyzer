namespace NetAnalyzer.Domain.Dataset;


public record class Relation(int DatasetID, int MemberOne, int MemberTwo)
{
    public int Id { get; set; }
}
