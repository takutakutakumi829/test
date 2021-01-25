﻿using System.Collections;
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
        port.portName = "すとりんぐ";
        
        outputContainer.Add(port);

        Init();
    }

    public StringNode(Port p)
    {
        title = "String";
        port = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(string));
        port.portName = "Value";
        port.ConnectTo(p);

        outputContainer.Add(port);

        Init();
    }

    public void Init()
    {
        textField = new TextField();
        mainContainer.Add(textField);

    }

}