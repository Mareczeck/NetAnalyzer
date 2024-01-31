using System.Text.Json.Serialization;

namespace NetAnalyzer.Domain.Dataset;

public class GraphModel 
{
    public List<Node> Nodes { get; set; } = new();
    public List<Link> Links { get; set; } =  new();
    
    public int MaximumDistance { get; set; }

    [JsonIgnore]
    public bool IsProcessed { get; set; }


    public void Preprocess()
    {
        if(!IsProcessed)
        {
            // Creates dictionary for quick access
            var nodeDictionary = Nodes.ToDictionary(node => node.Id);

            // Create neighbours for each node
            foreach (var link in Links)
            {
                if (nodeDictionary.TryGetValue(link.Source, out var sourceNode) && 
                    nodeDictionary.TryGetValue(link.Target, out var targetNode))
                {
                    sourceNode.Neighbors.Add(targetNode);
                    targetNode.Neighbors.Add(sourceNode); // reverse link for unordered graph
                }
            }
        }
        IsProcessed = true;
    }
}

public record struct Node(int Id, int Group){

    [JsonIgnore]
    public List<Node> Neighbors { get; set; } = new List<Node>();
}

public record struct Link(int Target, int Source){}