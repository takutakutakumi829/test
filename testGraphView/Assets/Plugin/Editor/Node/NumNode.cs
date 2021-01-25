using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;

public class NumNode : Node
{
    public NumNode()
    {
        title = "Num";
        var port = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(float));
        port.portName = "Value";
        inputContainer.Add(port);
    }

    void Update()
    {
        int a = 1;
    }
}
