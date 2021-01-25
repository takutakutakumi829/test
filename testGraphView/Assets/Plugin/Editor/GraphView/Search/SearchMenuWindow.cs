
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


    public void Init(ExampleGraphView graph, EditorWindow edit)
    {
        graphView = graph;
        editorWindow = edit;
    }
    public ExampleGraphView GetGraphView()
    {
        return graphView;
    }


    List<SearchTreeEntry> ISearchWindowProvider.CreateSearchTree(SearchWindowContext context)
    {
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
            LoadTextDataFile(localMousePosition, type);
            return true;
        }
        
        //呼び出し
        var node = Activator.CreateInstance(type) as Node;

        node.SetPosition(new Rect(localMousePosition, new Vector2(100, 100)));
        graphView.AddElement(node);

        return true;
    }

    void LoadTextDataFile(Vector2 mousePos, Type type)
    {
        var file = new SaveManager();
        file.OpenFile();

        var node = new SerializableNode() { position = mousePos, data = file.readData, name = file.saveDataName };

        var view = GetGraphView();

        CreateNodeElement(node, type);
    }

    void CreateNodeElement(SerializableNode node, Type type)
    {
        object objNode;
        // 引数の存在するコンストラクタを呼び出したい場合先に生成する
        var args = new object[] { node };
        objNode = Activator.CreateInstance(typeof(ReadNode), BindingFlags.CreateInstance, null, args, null);

        AddElements(objNode, node);
    }

    void AddElements(object obj, SerializableNode node)
    {
        var objBase = obj as Node;
        objBase.SetPosition(new Rect(node.position, new Vector2(100, 100)));
        graphView.AddElement(objBase);
    }
}
