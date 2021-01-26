using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine.UIElements;



public class EventNode : Node
{
    private FloatField floatField;
    private TextField textField;
    private EnumField enumField;

    private EnumField conditionField;
    private EnumField contentField;
    public int Text { get { return (int)enumField.userData; } }
    public ConditionDataField conditionDataField;
    public ContentDataField contentDataField;

    

    public int condition;
    public string conditionData;
    public int content;
    public string contentData;

    public ActionData actionData;



    public EventNode() : base()
    {
        title = "Event";
        //入力用ポートの作成
        Port inputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(ActionData));
        inputPort.portName = "inPut";
        inputContainer.Add(inputPort); // 入力用ポートはinputContainerに追加する
        Init();
    }
    public EventNode(ActionData action) : base()
    {
        actionData = action;
        title = "Event";
        //入力用ポートの作成
        Port inputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(ActionData));
        inputPort.portName = "inPut";
        inputContainer.Add(inputPort); // 入力用ポートはinputContainerに追加する
        Init(action);
    }

    void Init()
    {
        floatField = new FloatField();
        floatField.label = "condition";
        mainContainer.Add(floatField);

        textField = new TextField();
        textField.label = "conditionData";
        mainContainer.Add(textField);

        floatField = new FloatField();
        floatField.label = "content";
        mainContainer.Add(floatField);

        textField = new TextField();
        textField.label = "contentData";
        mainContainer.Add(textField);


    }
    void Init(ActionData action)
    {
        //var condition = new FloatField();
        //condition.label = "condition";
        //condition.value = action.condition;
        //mainContainer.Add(condition);

        //値の追加
        conditionField = new EnumField();
        conditionField.label = "condition";
        conditionField.Init(conditionDataField);
        conditionField.value = (ConditionDataField)action.condition;
        mainContainer.Add(conditionField);

        var conditionData = new TextField();
        conditionData.label = "conditionData";
        conditionData.value = action.conditionData;
        mainContainer.Add(conditionData);

        //var content = new FloatField();
        //content.label = "content";
        //content.value = action.content;
        //mainContainer.Add(content);

        //値の追加
        contentField = new EnumField();
        contentField.label = "content";
        contentField.Init(contentDataField);
        contentField.value = (ContentDataField)action.content;
        mainContainer.Add(contentField);
        RefreshExpandedState();

        var contentData = new TextField();
        contentData.label = "contentData";
        contentData.value = action.contentData;
        mainContainer.Add(contentData);

    }

}