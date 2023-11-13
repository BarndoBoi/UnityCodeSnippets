using System;
public static class Helpers
{
    public static int GenerateRandomRange(int minValue, int maxValue)
    {
        // Simulate a random range function using System.Random
        Random random = new Random();
        return random.Next(minValue, maxValue);
    }

    public static float GenerateRandomRange(double minValue, double maxValue)
    {
        Random random = new Random();
        return (float)(minValue + (maxValue - minValue) * random.NextDouble());
    }

    public static List<Node> Dijkstra(Node startNode, Node endNode, Graph marketGraph)
    {
        Dictionary<Node, int> distance = new Dictionary<Node, int>();
        Dictionary<Node, Node> previous = new Dictionary<Node, Node>();
        HashSet<Node> unvisitedNodes = new HashSet<Node>(startNode.ConnectedNodes.Keys);

        foreach (var node in marketGraph.Nodes)
        {
            distance[node] = int.MaxValue;
            previous[node] = null;
        }

        distance[startNode] = 0;

        while (unvisitedNodes.Count > 0)
        {
            Node currentNode = GetMinimumDistanceNode(unvisitedNodes, distance);
            unvisitedNodes.Remove(currentNode);

            foreach (var neighbor in currentNode.ConnectedNodes)
            {
                int alternativeRoute = distance[currentNode] + neighbor.Value;

                if (alternativeRoute < distance[neighbor.Key])
                {
                    distance[neighbor.Key] = alternativeRoute;
                    previous[neighbor.Key] = currentNode;
                }
            }
        }

        return ReconstructPath(previous, endNode);
    }

    private static Node GetMinimumDistanceNode(HashSet<Node> nodes, Dictionary<Node, int> distance)
    {
        Node minNode = null;
        int minDistance = int.MaxValue;

        foreach (var node in nodes)
        {
            if (distance.TryGetValue(node, out int nodeDistance) && nodeDistance < minDistance)
            {
                Console.WriteLine("Assigning node");
                minDistance = nodeDistance;
                minNode = node;
            }
        }

        Console.WriteLine("Value of minNode is: " + minNode);
        return minNode;
    }

    // Helper method to reconstruct the shortest path
    public static List<Node> ReconstructPath(Dictionary<Node, Node> previous, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node current = endNode;

        while (current != null)
        {
            path.Add(current);
            current = previous[current];
        }

        path.Reverse();
        return path;
    }

    public static float CalculatePercentage(float part, float whole)
    {
        if (whole == 0)
        {
            // Avoid division by zero
            return float.NaN;
        }

        return (part / whole) * 100.0f;
    }

    public static float CalculatePercentageOf(float number, float percentage)
    {
        return (percentage / 100.0f) * number;
    }
}