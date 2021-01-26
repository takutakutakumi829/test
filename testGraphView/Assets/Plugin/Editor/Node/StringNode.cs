using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;

public class StringNode : Node
{
    private TextField textField;
    public string Text { get { return textField.value; } }

    private Port port;
    public Port gPort { get { return port; } }

    public StringNode()
    {
        title = "String";
        port = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(string));
        port.portName = "StringValue";
        
        outputContainer.Add(port);

        Init();
    }

    public StringNode(string path)
    {
        title = "String";
        port = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(string));
        port.portName = "StringValue";

        outputContainer.Add(port);

        Init(path);
    }

    public void Init(string textData = null)
    {
        textField = new TextField();
        if (textData != null)
        {
            textField.value = textData;
        }
        mainContainer.Add(textField);

    }

}