using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkSimulation : MonoBehaviour
{
    public Router router;

    private void Start()
    {
        Node node1 = new Node(1, router);
        Node node2 = new Node(2, router);
        Node node3 = new Node(3, router);

        node1.SendMessage(2, "Amigo! Salen esos prohibidos??");
        node2.SendMessage(3, "Me volvio a escribir este pesado para jugar");
        node3.SendMessage(1, "Flaco sos re pesado, no jugamos mas con vos");
        node1.SendMessage(4, "Me bloquearon :(");
        node1.SendMessage(4, "Queres jugar conmigo?");
    }
}
