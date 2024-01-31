using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NetAnalyzer.Domain.Dataset;
using NetAnalyzer.Infrastructure;
using NetAnalyzer.Infrastructure.Persistence;

namespace NetAnalyzer.Business.Tests;

[TestClass]
public class GraphProcessorTests
{
    [TestMethod]
    public void CalculateMaximalDistance()
    {
        var graph = new GraphModel()
        {
            Nodes = new List<Node> { new Node(1, 1), new Node(2, 1), new Node(3, 1) },
            Links = new List<Link> { new Link(1, 2), new Link(2, 3) }
        };

        var processor = new GraphProcessor();
        int maximalDistance = processor.MaximalDistanceBetweenNodes(graph);

        Assert.AreEqual(2,maximalDistance);

    }
    [TestMethod]
    public void CalculateMaximalDistance_Cycle()
    {
        var graph = new GraphModel()
        {
            Nodes = new List<Node> { new Node(1, 1), new Node(2, 1), new Node(3, 1) },
            Links = new List<Link> { new Link(1, 2), new Link(2, 3), new Link(3, 1) }
        };

        var processor = new GraphProcessor();
        int maximalDistance = processor.MaximalDistanceBetweenNodes(graph);

        Assert.AreEqual(1,maximalDistance);

    }
}