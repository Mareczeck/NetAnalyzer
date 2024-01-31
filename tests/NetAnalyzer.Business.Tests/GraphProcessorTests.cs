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
        processor.CalculateMaximalDistanceBetweenNodes(graph);

        Assert.AreEqual(2,graph.MaximumDistance);

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
        processor.CalculateMaximalDistanceBetweenNodes(graph);

        Assert.AreEqual(1,graph.MaximumDistance);

    }

    
    [TestMethod]
    public void CalculateNodeLinks_Basic()
    {
        var node = new Node(1, 1);
        var graph = new GraphModel()
        {
            Nodes = new List<Node> { node , new Node(2, 1), new Node(3, 1) },
            Links = new List<Link> { new Link(1, 2), new Link(2, 3)}
        };

        graph.Preprocess();

        var processor = new GraphProcessor();
        var links = processor.ReachableLinksForNode(node, 2).Count - 1;

        Assert.AreEqual(2,links);
    }

    
    [TestMethod]
    public void CalculateNodeLinks_Zero()
    {
        var node = new Node(4, 1);
        var graph = new GraphModel()
        {
            Nodes = new List<Node> { node , new Node(1, 1) , new Node(2, 1), new Node(3, 1) },
            Links = new List<Link> { new Link(1, 2), new Link(2, 3)}
        };

        graph.Preprocess();

        var processor = new GraphProcessor();
        var links = processor.ReachableLinksForNode(node, 2).Count - 1;

        Assert.AreEqual(0,links);

    }

    
    [TestMethod]
    public void CalculateNodeLinks_Cycle()
    {
        var node = new Node(1, 1);
        var graph = new GraphModel()
        {
            Nodes = new List<Node> { node, new Node(2, 1), new Node(3, 1) },
            Links = new List<Link> { new Link(1, 2), new Link(2, 3), new Link(3, 1) }
        };

        var processor = new GraphProcessor();
        graph.Preprocess();
        var links = processor.ReachableLinksForNode(node, 2).Count() - 1;

        Assert.AreEqual(2,links);

    }
}