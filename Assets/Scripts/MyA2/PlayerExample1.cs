using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerExample1 : MonoBehaviour
{
    LinkedList<int> _list = new LinkedList<int>();
    private void Start()
    {
        _list.Get(0);
    }
}
