
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.Experimental.GraphView;

public class ExampleGraphView : GraphView
{
    public SerializableNode serializableNode;
    public SearchMenuWindow searchWindow;
    public SearchMenuWindow getSearchWindow { get { return searchWindow; } }

    public GraphAsset graphAsset;
    public EdgeDragHelper mouse;

    //1つ以上の要素をマウスでドラッグすることができるマニピュレータ
    public ContentDragger contentDragger;

    //選択ドラッグのマニピュレーター
    public SelectionDragger selectionDragger;

    public ExampleGraphView(EditorWindow editor, ref GraphAsset asset)
    {
        graphAsset = asset;
       
        //AddElement(new ExampleNode());  //ノードの追加

        //諸々の設定
        this.StretchToParentSize();

        contentDragger = new ContentDragger();
        selectionDragger = new SelectionDragger();
        SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
        this.AddManipulator(contentDragger);
        this.AddManipulator(selectionDragger);
        this.AddManipulator(new RectangleSelector());
        //this.AddManipulator(new ContextualMenuManipulator(OnContextMenuPopulate));

        //右クリックでのメニューの表示
        searchWindow = ScriptableObject.CreateInstance<SearchMenuWindow>();
        searchWindow.Init(this, editor, ref graphAsset);

  
        nodeCreationRequest += context =>
        {
            SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), searchWindow);
            
        };
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

    public void CheckPortList()
    {
        var edge = edges.ToList();
        var node = nodes.ToList();
        var list = ports.ToList();
        Port setPort;
        int num = 0;

        //inPut検索
        for(int j = 0;j < list.Count; j++)
        {
            if(list[j].direction == Direction.Input)
            {
                if(list[j].portName == "State Key")
                {
                    setPort = list[j];

                    num = j;
                }
            }
        }

        //outPut検索
        for (int j = 0; j < list.Count; j++)
        {
            if (list[j].direction == Direction.Output)
            {
                if (list[j].portName == "StringValue")
                {
                    var conecting = list[j].connections;
                    int i = 0;

                }
            }
        }

    }

}
