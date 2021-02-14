using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using System;

public class OutlineNode : Node
{
    private FloatField floatField;
    private TextField stateField;
    public string GetStateKey { get { return stateField.value; } }

    private TextField clipName;
    private List<string> clipField;

    private EnumField enumField;
    public System.Enum GetEnum {  get { return enumField.value; } }
 
    
    public BoolField boolField;

    //conditionField
    private EnumField conditionField;

    //contentField
    private EnumField contentField;

    //conditionDataField
    public ConditionDataField conditionDataField;

    //contentDataField
    public ContentDataField contentDataField;

    //ファイルパス
    private string pathName;
    public string GetPathName {  get { return pathName; } }

    //表示座標
    private Vector2 position;
    public Vector2 GetPosition { get { return position; } }


    public struct EventsData
    {
        public EnumField conditionField;
        public TextField conditionData;

        //contentField
        public EnumField contentField;
        public TextField contentData;
    }

    private List<EventsData> eventsData;

    private List<ActionData> actionField;

    public struct ClipData
    {
        public TextField clipName;
    }
    private List<ClipData> clipData;



    public enum BoolField
    {
        False,
        True
    }

    //boolの判別
    public bool GetIsBase()
    {
        bool rtnValue = false;

        var boolfield = enumField.value.ToString();
        if(boolfield == "True")
        {
            rtnValue = true;
        }
        else
        {
            rtnValue = false;
        }

        return rtnValue;
    }

    public List<ActionData> GetEventsData()
    {
        //更新
        for (int j = 0; j < eventsData.Count; j++)
        {
            var data = eventsData[j];
            ActionData action;
            action.condition = (ConditionDataField)data.conditionField.value;
            action.conditionData = data.conditionData.value;
            action.content = (ContentDataField)data.contentField.value;
            action.contentData = data.contentData.value;

            actionField.RemoveAt(j);
            actionField.Insert(j, action);
            //actionField[j].SetActionData((ConditionDataField)data.conditionField.value, data.conditionData.value, (ContentDataField)data.contentField.value, data.contentData.value);
        }


        return actionField;
    }

    public List<string> GetClipPath()
    {
        //更新
        for (int j = 0; j < clipData.Count; j++)
        {
            var data = clipData[j];
            ClipData clip;
            clip.clipName = data.clipName;

            clipField.RemoveAt(j);
            clipField.Insert(j, clip.clipName.value);
        }
        return clipField;
    }


    public OutlineNode()
    {
        title = "Outline";

    }

    public OutlineNode(SaveData data)
    {
        pathName = data.pathName;
        position = data.position;
        transform.position = data.position;
        var file = new SaveManager();
        var titleName = file.GetExtensionFileNameSaveData(data.pathName);
        title = titleName;

        AddStringField(data.stateKey);
        AddBoolField(data.isBase);

        eventsData = new List<EventsData>();
        actionField = new List<ActionData>();
        for (int j = 0; j < data.events.Count; j++)
        {
            AddEventsData(data.events[j], j);
            AddEventsField(j);
        }

        clipData = new List<ClipData>();
        clipField = new List<string>();
        for (int i = 0; i < data.clipPath.Count; i++)
        {
            AddClipPathData(data.clipPath[i], i);
            AddClipPathField(i);
        }

  
    }

    public void AddStringField(string stateKey)
    {
        stateField = new TextField();
        stateField.label = "State Key";
        if (stateKey != null)
        {
            stateField.value = stateKey;
        }
        inputContainer.Add(stateField);

    }

    public void AddBoolField(bool isBase)
    {
        //値の追加
        enumField = new EnumField();
        enumField.label = "Is Base";
        enumField.Init(boolField);
        enumField.value = (isBase == true ? BoolField.True : BoolField.False);
        inputContainer.Add(enumField);
        RefreshExpandedState();

    }


    private void AddEventsData(ActionData action, int num)
    {
        EventsData events;
        //値の追加
        events.conditionField = new EnumField();
        events.conditionField.label = "condition" + num;
        events.conditionField.Init(conditionDataField);
        events.conditionField.value = (ConditionDataField)action.condition;

        events.conditionData = new TextField();
        events.conditionData.label = "conditionData";
        events.conditionData.value = action.conditionData;


        //値の追加
        events.contentField = new EnumField();
        events.contentField.label = "content";
        events.contentField.Init(contentDataField);
        events.contentField.value = (ContentDataField)action.content;

        events.contentData = new TextField();
        events.contentData.label = "contentData";
        events.contentData.value = action.contentData;

        eventsData.Add(events);
    }

    public void AddEventsField(int num)
    {
        //値の追加
        outputContainer.Add(eventsData[num].conditionField);

        outputContainer.Add(eventsData[num].conditionData);


        //値の追加
        outputContainer.Add(eventsData[num].contentField);

        outputContainer.Add(eventsData[num].contentData);

        RefreshExpandedState();

        ActionData data;

        data.condition = (ConditionDataField)eventsData[num].conditionField.value;
        data.conditionData = eventsData[num].conditionData.value;
        data.content = (ContentDataField)eventsData[num].contentField.value;
        data.contentData = eventsData[num].contentData.value;

        actionField.Add(data);
        //RefreshExpandedState();

    }

    public void AddEventsField(ActionData action, int num)
    {
        //値の追加
        conditionField = new EnumField();
        conditionField.label = "condition" + num;
        conditionField.Init(conditionDataField);
        conditionField.value = (ConditionDataField)action.condition;
        outputContainer.Add(conditionField);

        var conditionData = new TextField();
        conditionData.label = "conditionData";
        conditionData.value = action.conditionData;
        outputContainer.Add(conditionData);


        //値の追加
        contentField = new EnumField();
        contentField.label = "content";
        contentField.Init(contentDataField);
        contentField.value = (ContentDataField)action.content;
        outputContainer.Add(contentField);
        RefreshExpandedState();

        var contentData = new TextField();
        contentData.label = "contentData";
        contentData.value = action.contentData;
        outputContainer.Add(contentData);

        ActionData data;

        data.condition = (ConditionDataField)conditionField.value;
        data.conditionData = conditionData.value;
        data.content = (ContentDataField)contentField.value;
        data.contentData = contentData.value;

        actionField.Add(data);
        //RefreshExpandedState();

    }

    private void AddClipPathData(string path, int num)
    {
        ClipData data;
        //値の追加
        data.clipName = new TextField();
        data.clipName.label = "Clip Path" + num;
        if (data.clipName != null)
        {
            data.clipName.value = path;
        }
        clipData.Add(data);
    }

    public void AddClipPathField(int num)
    {
        mainContainer.Add(clipData[num].clipName);
        clipField.Add(clipData[num].clipName.value);
    }
}

