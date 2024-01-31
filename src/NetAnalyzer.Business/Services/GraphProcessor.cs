using NetAnalyzer.Domain.Dataset;

public class GraphProcessor : IGraphProcessor
{

    public int MaximalDistanceBetweenNodes(GraphModel graph)
    {
        int maxDistance = 0;

        graph.Preprocess();

        // Process each connected component in parallel
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

        return maxDistance;
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

        int currentMaxDistance = distances.Max(kv => kv.Value);
        if (currentMaxDistance > maxDistance)
            maxDistance = currentMaxDistance;
    

        return maxDistance;
    }
}