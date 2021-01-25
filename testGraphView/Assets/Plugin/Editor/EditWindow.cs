using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.Callbacks;

public class EditWindow : EditorWindow
{
    ExampleGraphView graphView;
    GraphAsset graphAsset;

    [MenuItem("Window/なんかすごいえでぃた")]
    public static void Open()
    {
        var edit = GetWindow<EditWindow>(ObjectNames.NicifyVariableName(nameof(EditWindow)));

        if (Selection.activeObject is GraphAsset asset)
        {
            edit.Init(asset);
        }
    }

    //何らかのアセットを開いた時
    [OnOpenAsset()]
    static bool OnOpenAsset(int instance, int line)
    {
        //GraphAssetファイルを選んだ場合
        if(EditorUtility.InstanceIDToObject(instance) is GraphAsset)
        {
            Open();
            return true;
        }

        return false;
    }

    void OnEnable()
    {
        if (graphAsset != null)
        {
            Init(graphAsset);
        }
    }

    public void Init(GraphAsset asset)
    {
        graphAsset = asset;
        graphView = new ExampleGraphView(this, graphAsset);
        rootVisualElement.Add(graphView);
    }
   
}
