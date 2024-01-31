using NetAnalyzer.Domain.Dataset;

public interface IGraphProcessor
{
    void CalculateMaximalDistanceBetweenNodes(GraphModel graph);
    decimal GetAverageLinks(GraphModel graph, int distance);

    Dictionary<Node, int> ReachableLinksForNode(Node node, int definedDistance);
}
