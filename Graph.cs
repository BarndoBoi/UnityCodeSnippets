public class Graph
{
    public List<Node> Nodes { get; } = new List<Node>();
}

public class Node
{
    public int LocationId { get; set; }
    public MarketInstance MarketInstance { get; set; }
    public Dictionary<Node, int> ConnectedNodes { get; } = new Dictionary<Node, int>();
}