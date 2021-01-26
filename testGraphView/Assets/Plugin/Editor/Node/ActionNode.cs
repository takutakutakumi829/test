//ObjectNode.cs
using UnityEngine;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEditor.Experimental.GraphView;

public class ActionNode : Node
{
    private ObjectField objectField;
    public object Text { get { return objectField.value; } }


    public ActionNode() : base()
    {
        title = "ActionData";
        //入力用ポートの作成(events)
        Port inputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(List<ActionData>));
        inputPort.portName = "events";
        inputContainer.Add(inputPort); // 入力用ポートはinputContainerに追加する
        var outputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(object));
        outputContainer.Add(outputPort);

        objectField = new ObjectField();
        objectField.objectType = typeof(ActionData);
        outputContainer.Add(objectField);

    }
    public ActionNode(List<ActionData> data) : base()
    {
        title = "ActionData";
        //入力用ポートの作成(events)
        Port inputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(List<ActionData>));
        inputPort.portName = "events";
        inputContainer.Add(inputPort); // 入力用ポートはinputContainerに追加する
        Init(data);
    }

    void Init(List<ActionData> data)
    {
        Port outputPort;
        for(int j = 0; j < data.Count; j++)
        {
            outputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(ActionData));
            outputPort.portName = "data" + j;
            outputContainer.Add(outputPort);

            //objectField = new ObjectField();
            //objectField.objectType = typeof(ActionData);
            //outputContainer.Add(objectField);

        }

    }
}
