
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using System.Reflection;

public class SearchMenuWindow : ScriptableObject, ISearchWindowProvider
{
    private ExampleGraphView graphView;
    private EditorWindow editorWindow;
    private Action<GraphAsset> onCreated;
    public SaveData saveData;
    public bool dataLoadFlag = false;
    public Port readPort;

    public void Init(ExampleGraphView graph, EditorWindow edit, ref GraphAsset asset)
    {
        graphView = graph;
        editorWindow = edit;
        foreach (var node in asset.data)
        {
            saveData = node;
        }
    }
    public ExampleGraphView GetGraphView()
    {
        return graphView;
    }

    public void SetDataAsset(ref GraphAsset asset)
    {
        asset.data.Add(saveData);
        dataLoadFlag = false;
    }
    public void SetLoadData(GraphAsset asset)
    {
        foreach (var save in asset.data)
        {
            //ここでfileNameの入力
            LoadTextDataFile(save.position, false, save.pathName);
        }
    }

    List<SearchTreeEntry> ISearchWindowProvider.CreateSearchTree(SearchWindowContext context)
    {
        dataLoadFlag = false;
        var entries = new List<SearchTreeEntry>();
        entries.Add(new SearchTreeGroupEntry(new GUIContent("何か作る")));

        entries.Add(new SearchTreeGroupEntry(new GUIContent("Read Node")) { level = 1 });
        entries.Add(new SearchTreeEntry(new GUIContent(nameof(TestNode))) { level = 2, userData = typeof(ReadNode) });


        entries.Add(new SearchTreeGroupEntry(new GUIContent("Create New Node")) { level = 1 });

        //追加するノードの設定
        entries.Add(new SearchTreeEntry(new GUIContent(nameof(BoolNode))) { level = 2, userData = typeof(BoolNode) });
        entries.Add(new SearchTreeEntry(new GUIContent(nameof(NumNode))) { level = 2, userData = typeof(NumNode) });
        entries.Add(new SearchTreeEntry(new GUIContent(nameof(StringNode))) { level = 2, userData = typeof(StringNode) });
        entries.Add(new SearchTreeEntry(new GUIContent(nameof(ValueNode))) { level = 2, userData = typeof(ValueNode) });

        return entries;
    }

    bool ISearchWindowProvider.OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
    {
        var type = searchTreeEntry.userData as Type;
        String typeName = type.ToString();
        // マウスの位置にノードを追加
        var worldMousePosition = editorWindow.rootVisualElement.ChangeCoordinatesTo(editorWindow.rootVisualElement.parent, context.screenMousePosition - editorWindow.position.position);
        var localMousePosition = graphView.contentViewContainer.WorldToLocal(worldMousePosition);

        //読み込むノードの分岐
        if (typeName == "ReadNode")
        {
            LoadTextDataFile(localMousePosition);
            return true;
        }
        
        //呼び出し
        var node = Activator.CreateInstance(type) as Node;

        node.SetPosition(new Rect(localMousePosition, new Vector2(100, 100)));
        graphView.AddElement(node);

        return true;
    }

    void LoadTextDataFile(Vector2 mousePos, bool addFlag = true, string pathName = null)
    {
        var file = new SaveManager();
        if (pathName == null)
        {
            file.OpenFile();
        }
        else
        {
            file.LoadFile(pathName);
        }

        var node = new SerializableNode() { position = mousePos, data = file.readData, name = file.saveDataName };
        var data = new SaveData() { stateKey = file.readData.stateKey, isBase = file.readData.isBase, clipPath = file.readData.clipPath, events = file.readData.events, position = node.position, pathName = file.pathName };
        var view = GetGraphView();
        if (addFlag == true)
        {
            CreateNodeElement(node, data);
        }
        else
        {
            LoadNodeElement(node, data);

        }
    }

    void CreateNodeElement(SerializableNode node, SaveData data)
    {
        object objNode;
        // 引数の存在するコンストラクタを呼び出したい場合先に生成する
        var args = new object[] { node };
        objNode = Activator.CreateInstance(typeof(ReadNode), BindingFlags.CreateInstance, null, args, null);

        saveData = data;
        AddElements(objNode, saveData);

    }
    //ロード時
    void LoadNodeElement(SerializableNode node, SaveData data)
    {
        object objNode;
        // 引数の存在するコンストラクタを呼び出したい場合先に生成する
        var args = new object[] { node };
        objNode = Activator.CreateInstance(typeof(ReadNode), BindingFlags.CreateInstance, null, args, null);

        saveData = data;
        AddElements(objNode, saveData, false);

    }

    void AddElements(object obj, SaveData data, bool setFlag = true)
    {
        var objBase = obj as Node;
        objBase.SetPosition(new Rect(data.position, new Vector2(100, 100)));
        graphView.AddElement(objBase);

        Node baseNode;
        Vector2 pos = new Vector2(100, 50);
        var args = new object[] { data.stateKey };

        //stringNode
        if (data.stateKey != "")
        {
            baseNode = Activator.CreateInstance(typeof(StringNode), BindingFlags.CreateInstance, null, args, null) as Node;
            baseNode.SetPosition(new Rect(data.position - pos, new Vector2(100, 100)));
            graphView.AddElement(baseNode);
        }


        //boolNode
        pos = new Vector2(100, -50);
        args = new object[] { data.isBase };
        baseNode = Activator.CreateInstance(typeof(BoolNode), BindingFlags.CreateInstance, null, args, null) as Node;
        baseNode.SetPosition(new Rect(data.position - pos, new Vector2(100, 100)));
        graphView.AddElement(baseNode);

        //actionNode
        pos = new Vector2(-250, 50);
        args = new object[] { data.events };

        baseNode = Activator.CreateInstance(typeof(ActionNode), BindingFlags.CreateInstance, null, args, null) as Node;
        baseNode.SetPosition(new Rect(data.position - pos, new Vector2(100, 100)));
        graphView.AddElement(baseNode);

        //eventNode
        pos -= new Vector2(250, -400);
        for (int i = 0; i < data.events.Count; i++)
        {
            pos -= new Vector2(0, 150);

            args = new object[] { data.events[i] };
            baseNode = Activator.CreateInstance(typeof(EventNode), BindingFlags.CreateInstance, null, args, null) as Node;
            baseNode.SetPosition(new Rect(data.position - pos, new Vector2(100, 100)));
            graphView.AddElement(baseNode);
        }

        dataLoadFlag = (setFlag == true ? true : false);
    }
}
