using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.Callbacks;

public class GraphEditor : EditorWindow
{
    [MenuItem("Window/GraphEditor")]
    public static void ShowWindow()
    {
        GraphEditor graphEditor = CreateInstance<GraphEditor>();
        graphEditor.Show();  // ウィンドウを表示
        graphEditor.titleContent = new GUIContent("Graph Editor");
        if (Selection.activeObject is GraphAsset graphAsset)
        {
            graphEditor.Initialize(graphAsset);
        }
    }

    [OnOpenAsset()]
    static bool OnOpenAsset(int instanceId, int line)
    {
        if (EditorUtility.InstanceIDToObject(instanceId) is GraphAsset)
        {
            ShowWindow();
            return true;
        }

        return false;
    }

    GraphAsset m_GraphAsset;
    GraphEditorElement m_Element;
    public void OnEnable()
    {
        if (m_GraphAsset != null)
        {
            Initialize(m_GraphAsset);
        }
    }
    public void Initialize(GraphAsset graphAsset)
    {
        m_GraphAsset = graphAsset;

        VisualElement root = this.rootVisualElement;

        m_Element = new GraphEditorElement(graphAsset);  // アセットを渡す
        root.Add(m_Element);
    }

}
