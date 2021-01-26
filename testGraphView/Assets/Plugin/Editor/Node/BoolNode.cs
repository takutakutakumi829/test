using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;

public class BoolNode : Node
{
    private EnumField enumField;
    public int Text { get { return (int)enumField.userData; } }
    public BoolField boolField;

    public enum BoolField
    {
        False,
        True
    }

    public BoolNode()
    {
        title = "Bool";
        var port = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
        port.portName = "Value";
        inputContainer.Add(port);
        AddField();
    }
    public BoolNode(bool flag)
    {
        title = "Bool";
        var port = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
        port.portName = "Value";
        inputContainer.Add(port);
        AddField(flag);
    }

    void AddField(bool flag = false)
    {
        //値の追加
        enumField = new EnumField();
        enumField.Init(boolField);
        enumField.value = (flag == true ? BoolField.True : BoolField.False);
        mainContainer.Add(enumField);
        RefreshExpandedState();

    }

}
