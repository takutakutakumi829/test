using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;

public class TestNode : Node
{
    public SerializableNode serializableNode;
    private Port StringPort;
    public Port gPort { get { return StringPort; } }

    public TestNode()
    {
        //入力用ポートの作成
        StringPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(string)); // 第三引数をPort.Capacity.Multipleにすると複数のポートへの接続が可能になる
        StringPort.portName = "State Key";
        inputContainer.Add(StringPort); // 入力用ポートはinputContainerに追加する

        //入力用ポートの作成
        Port inputIsBase = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(bool));
        inputIsBase.portName = "Is Base";
        inputContainer.Add(inputIsBase);

        //出力する値その１
        var outputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(float));
        outputPort.portName = "Value";
        outputContainer.Add(outputPort); // 出力用ポートはoutputContainerに追加する

    }

    public TestNode(SerializableNode node)
    {
        serializableNode = node;
        title = node.name;

        //入力用ポートの作成
        Port inputStateKey = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(string)); // 第三引数をPort.Capacity.Multipleにすると複数のポートへの接続が可能になる
        inputStateKey.portName = "State Key";
        inputContainer.Add(inputStateKey); // 入力用ポートはinputContainerに追加する

        //入力用ポートの作成
        Port inputIsBase = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(bool));
        inputIsBase.portName = "Is Base";
        inputContainer.Add(inputIsBase);

        //出力する値その１
        var outputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(float));
        outputPort.portName = "Value";
        outputContainer.Add(outputPort); // 出力用ポートはoutputContainerに追加する

    }
}
