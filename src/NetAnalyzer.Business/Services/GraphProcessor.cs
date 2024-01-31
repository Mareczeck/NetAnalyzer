using NetAnalyzer.Domain.Dataset;

public class GraphProcessor : IGraphProcessor
{
    public void CalculateMaximalDistanceBetweenNodes(GraphModel graph)
    {
        int maxDistance = 0;
        graph.Preprocess();

        // Process each node in parallel
        Parallel.ForEach(graph.Nodes, node =>
        {
            // Perform Dijkstra's algorithm to find maximal distance
            int distance = Dijkstra(node, graph);
            
            // Synchronize access to maxDistance
            lock (this)
            {
                if (distance > maxDistance)
                    maxDistance = distance;
            }
        });

        graph.MaximumDistance = maxDistance;
    }

    public decimal GetAverageLinks(GraphModel graph, int distance)
    {
        graph.Preprocess();
        
        var nodes = graph.Nodes;
        int totalLinks = 0;
        int totalNodes = nodes.Count;

        Parallel.ForEach(nodes, node =>
        {
            lock (this)
            {
                totalLinks += ReachableLinksForNode(node, distance).Count - 1;
            }
        });

        return totalNodes > 0 ? (decimal)totalLinks / totalNodes : 0;
    }

    public Dictionary<Node, int> ReachableLinksForNode(Node node, int definedDistance)
    {
        HashSet<Node> visited = new HashSet<Node>();
        Dictionary<Node, int> distances = new Dictionary<Node, int>();
        distances[node] = 0;

        visited.Add(node);

        for (int step = 0; step < definedDistance; step++)
        {
            bool newNeighbours = false;
            Dictionary<Node, int> newDistances = new Dictionary<Node, int>();

            foreach (var currentNode in distances.Where(x => x.Value == step))
            {
                foreach (var neighbor in currentNode.Key.Neighbors)
                {
                    if (!visited.Contains(neighbor))
                    {
                        visited.Add(neighbor);
                        newDistances[neighbor] = step + 1;
                        newNeighbours = true;
                    }
                }
            }

            foreach (var entry in newDistances)
            {
                distances[entry.Key] = entry.Value;
            }

            // If last step did not add any neighbour, we can stop calculation
            if(!newNeighbours)
                break;
        }

        return distances; 
    }


    private int Dijkstra(Node startNode, GraphModel graph)
    {
        int maxDistance = 0;
        var distances = new Dictionary<Node, int>();
        foreach (var node in graph.Nodes)
            distances[node] = int.MaxValue;

        distances[startNode] = 0;

        var queue = new Queue<Node>();
        var visited = new HashSet<Node>(); // Uložení již navštívených uzlů
        queue.Enqueue(startNode);

        while (queue.Count > 0)
        {
            var currentNode = queue.Dequeue();
            visited.Add(currentNode); // Přidání uzlu do již navštívených

            foreach (var neighbor in currentNode.Neighbors)
            {
                var newDistance = distances[currentNode] + 1;

                if (newDistance < distances[neighbor])
                {
                    distances[neighbor] = newDistance;
                    queue.Enqueue(neighbor);
                }
            }
        }

        // Counting only reachable distances
        int currentMaxDistance = distances.Where(x => x.Value != int.MaxValue).Max(kv => kv.Value);
        if (currentMaxDistance > maxDistance)
            maxDistance = currentMaxDistance;
    

        return maxDistance;
    }
}