using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using System;

public class ExampleNode : Node
{

    public ExampleNode()
    {
        title = "てすと";

        // 入力用のポートを作成
        var inputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(float)); // 第三引数をPort.Capacity.Multipleにすると複数のポートへの接続が可能になる
        inputPort.portName = "Input";
        inputContainer.Add(inputPort); // 入力用ポートはinputContainerに追加する

        // 出力用のポートを作る
        var outputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(float));
        outputPort.portName = "Value";
        outputContainer.Add(outputPort); // 出力用ポートはoutputContainerに追加する

    }
}
