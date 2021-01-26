using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;

public class ReadNode : Node
{
    public SerializableNode serializableNode;
    public List<ActionData> events;
    public List<string> clipPath;
    private Port port;

    public ReadNode(SerializableNode node) : base()
    {
        title = node.name;

        serializableNode = node;
        //入力用ポートの作成(state key)
        Port inputStateKey = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(string)); // 第三引数をPort.Capacity.Multipleにすると複数のポートへの接続が可能になる
        inputStateKey.portName = "State Key";
        inputContainer.Add(inputStateKey); // 入力用ポートはinputContainerに追加する

        //入力用ポートの作成(is Base)
        Port inputIsBase = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(bool));
        inputIsBase.portName = "Is Base";
        inputContainer.Add(inputIsBase);



        //出力する値その1(events)
        Port outputEvents = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(List<ActionData>));
        outputEvents.portName = "events Value";
        outputContainer.Add(outputEvents);

        //出力する値その2(clipPath)
        Port outputClipPath = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(List<string>));
        outputClipPath.portName = "clipPath Value";
        outputContainer.Add(outputClipPath);

    }
    public virtual IEnumerable connections { get { return (IEnumerable)port.connections; } }

}