using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;

public class ValueNode : Node
{
    public ValueNode()
    {
        title = "Value";

        var port = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(float));
        port.portName = "Value";
        outputContainer.Add(port);

        //値の追加
        extensionContainer.Add(new FloatField());
        RefreshExpandedState();
    }
}
