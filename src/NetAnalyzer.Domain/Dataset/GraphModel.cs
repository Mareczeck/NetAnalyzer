using System.Text.Json.Serialization;

namespace NetAnalyzer.Domain.Dataset;

public class GraphModel 
{
    public List<Node> Nodes { get; set; } = new();
    public List<Link> Links { get; set; } =  new();
    
    public int MaximumDistance { get; set; }

    public void Preprocess()
    {
        // Vytvořte slovník pro rychlý přístup k uzlům podle jejich id
        var nodeDictionary = Nodes.ToDictionary(node => node.Id);

        // Pro každý uzel vytvořte seznam jeho sousedů na základě vazeb
        foreach (var link in Links)
        {
            if (nodeDictionary.TryGetValue(link.Source, out var sourceNode) && 
                nodeDictionary.TryGetValue(link.Target, out var targetNode))
            {
                sourceNode.Neighbors.Add(targetNode);
                targetNode.Neighbors.Add(sourceNode); // Přidání zpětného spojení pro neorientovaný graf
            }
        }
    }
}

public record struct Node(int Id, int Group){

    [JsonIgnore]
    public List<Node> Neighbors { get; set; } = new List<Node>();
}

public record struct Link(int Target, int Source){}