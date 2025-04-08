using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleLinkedList<T>
{
    NodeMyA<T> _first;
    NodeMyA<T> _last;
    int _count;

    //INTENTAR HACER LISTA DOBLEMENTE ENLAZADA

    public void Add(T item)
    {
        var node = new NodeMyA<T>();
        node.value = item;

        if (_first == null)
            _first = node;
        else
            _last.next = node;

        _last = node;
        
        _count++;
    }

    public T Get(int index)
    {
        if (index < 0 || index >= _count) //TIRE EL ERROR Y CORTA EL CODIGO
            throw new System.Exception("ArgumentOutRangeException");

        var currentNode = _first;

        for (int i = 0; i < index; i++)
            currentNode = currentNode.next;

        return currentNode.value;
    }

    public void Remove(int index)
    {
        if (index < 0 || index >= _count) //TIRE EL ERROR Y CORTA EL CODIGO
        {
            Debug.LogError("ArgumentOutRangeException");
            return;
        }

        if(index == 0) //EXCEPCION SI ME PASAN CERO
        {
            _first = _first.next;

            if (_first == null)
                _last = null;
        }
        else
        {
            var currentNode = _first;
            //VOY HASTA EL ANTERIOR QUE LE PEDI O LE PUEDO PEDIR A GET(INDEX - 1)
            for (int i = 0; i < index - 1; i++)
                currentNode = currentNode.next;

            currentNode.next = currentNode.next.next;

            if (currentNode.next == null)
                _last = currentNode;
        }


        _count--;
    }
}
