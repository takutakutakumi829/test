
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using System;

public class ExampleGraphView : GraphView
{
    private EditorWindow editorWindow;

    public SerializableNode serializableNode;
    public SearchMenuWindow searchWindow;
    public SearchMenuWindow getSearchWindow { get { return searchWindow; } }

    public GraphAsset graphAsset;
    public EdgeDragHelper mouse;

    //1つ以上の要素をマウスでドラッグすることができるマニピュレータ
    public ContentDragger contentDragger;

    //選択ドラッグのマニピュレーター
    public SelectionDragger selectionDragger;

    //Saveの管理
    public bool SaveFlag = false;

    public ExampleGraphView(EditorWindow editor, ref GraphAsset asset)
    {
        graphAsset = asset;
        editorWindow = editor;

        //AddElement(new ExampleNode());  //ノードの追加

        //諸々の設定
        this.StretchToParentSize();

        contentDragger = new ContentDragger();
        selectionDragger = new SelectionDragger();

        SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
        this.AddManipulator(new ContextualMenuManipulator(OnContextMenuPopulate));

        this.AddManipulator(contentDragger);
        this.AddManipulator(selectionDragger);
        //this.AddManipulator(new RectangleSelector());
 
        //右クリックでのメニューの表示
        searchWindow = ScriptableObject.CreateInstance<SearchMenuWindow>();
        searchWindow.Init(this, editor, ref graphAsset);

         nodeCreationRequest += context =>
        {
            SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), searchWindow);
            
        };
    }

    void OnContextMenuPopulate(ContextualMenuPopulateEvent evt)
    {
        evt.menu.AppendAction("アセットの保存", SaveFileAction, DropdownMenuAction.AlwaysEnabled);
        evt.menu.AppendAction("出力", AddUploadFileAction, DropdownMenuAction.AlwaysEnabled);
        evt.menu.AppendAction("読み込み", LoadTextDataFile, DropdownMenuAction.AlwaysEnabled);

    }

    //アセットのセーブ
    void SaveFileAction(DropdownMenuAction menuAction)
    {
        // マウスの位置にノードを追加
        SaveFlag = true;
    }

    //Nodeの追加
    void AddUploadFileAction(DropdownMenuAction menuAction)
    {
        // 出力
        getSearchWindow.UploadTextDataFile();
    }

    //ファイルの読み込み
    void LoadTextDataFile(DropdownMenuAction menuAction)
    {
        //読み込むノードの分岐
        getSearchWindow.LoadTextDataFile(menuAction.eventInfo.localMousePosition);
    }

    public GraphAsset GetGraphAsset()
    {
        return graphAsset;
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter adapter = null)
    {
        var compatible = new List<Port>();

        foreach(var port in ports.ToList())
        {
            //分岐
            // 同じノードには繋げない
            if (startPort.node == port.node)
            {
                continue;
            }

            // Input同士、Output同士は繋げない
            if (port.direction == startPort.direction)
            {
                continue;
            }

            // ポートの型が一致していない場合は繋げない
            if (port.portType != startPort.portType)
            {
                continue;
            }

            compatible.Add(port);
        }
 
        return compatible;
    }

}

