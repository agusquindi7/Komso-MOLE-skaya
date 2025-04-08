using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Router : MonoBehaviour
{
    Dictionary<int, Node> nodes = new Dictionary<int, Node>();

    public void RegisterNode(Node node)
    {
        if (!nodes.ContainsKey(node.ID))
        {
            nodes[node.ID] = node;
            Debug.Log($"Node {node.ID} registered in the router");
        }
    }

    public void RouteMessage(int sourceID, int destinationID, string message)
    {
        if (nodes.ContainsKey(destinationID))
        {
            nodes[destinationID].ReceiveMessage(sourceID, message);
        }
        else
        {
            Debug.Log($"Router: Node {destinationID} not found in the network");
        }
    }
}
