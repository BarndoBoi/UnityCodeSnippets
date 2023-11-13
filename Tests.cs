public class Tests
{
    public static void RunPathfindingExample()
    {
        // Create a Graph
        Graph marketGraph = new Graph();

        // Create MarketInstances
        MarketInstance marketA = new MarketInstance { Name = "Market A", LocationId = 1 };
        MarketInstance marketB = new MarketInstance { Name = "Market B", LocationId = 2 };
        MarketInstance marketC = new MarketInstance { Name = "Market C", LocationId = 3 };

        // Create Nodes for each MarketInstance
        Node nodeA = new Node { LocationId = marketA.LocationId, MarketInstance = marketA };
        Node nodeB = new Node { LocationId = marketB.LocationId, MarketInstance = marketB };
        Node nodeC = new Node { LocationId = marketC.LocationId, MarketInstance = marketC };

        // Connect Nodes with weights based on desired relationships
        nodeA.ConnectedNodes.Add(nodeB, 1);
        nodeA.ConnectedNodes.Add(nodeC, 3);

        nodeB.ConnectedNodes.Add(nodeA, 1);
        nodeB.ConnectedNodes.Add(nodeC, 1);

        nodeC.ConnectedNodes.Add(nodeA, 3);
        nodeC.ConnectedNodes.Add(nodeB, 1);

        // Add Nodes to the Graph
        marketGraph.Nodes.Add(nodeA);
        marketGraph.Nodes.Add(nodeB);
        marketGraph.Nodes.Add(nodeC);

        // Example: Using Dijkstra's algorithm to find the shortest path
        Console.WriteLine("Shortest Path from Market A to Market C:");
        List<Node> shortestPath = Helpers.Dijkstra(nodeA, nodeC, marketGraph);

        if (shortestPath != null)
        {
            foreach (var node in shortestPath)
            {
                Console.WriteLine($"{node.MarketInstance.Name} (LocationId {node.LocationId})");
            }
        }
        else
        {
            Console.WriteLine("No path found.");
        }
    }
}