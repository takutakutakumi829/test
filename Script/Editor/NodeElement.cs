using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class NodeElement : Box
{
    public SerializableNode serializableNode;

    public NodeElement(SerializableNode node)
    {
        serializableNode = node;

        style.position = Position.Absolute;
        style.height = 50;
        style.width = 100;
        transform.position = node.position;

        this.AddManipulator(new NodeDragger());
    }
}
