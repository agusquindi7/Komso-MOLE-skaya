using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public int ID { get; private set; }
    private Router router;

    public Node(int id, Router router)
    {
        ID = id;
        this.router = router;
        router.RegisterNode(this);
    }

    public void SendMessage(int destinationId, string message)
    {
        Debug.Log($"Node {ID} sending message  to Node {destinationId}: {message}");
        router.RouteMessage(ID, destinationId, message);
    }

    public void ReceiveMessage(int sourceID, string message)
    {
        Debug.Log($"Node {ID} received message from Node {sourceID}: {message}");
    }
}
