
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
    public GraphAsset graphAsset;

    public ExampleGraphView(EditorWindow editor, GraphAsset asset)
    {
        graphAsset = asset;
       
        //AddElement(new ExampleNode());  //ノードの追加

        //諸々の設定
        this.StretchToParentSize();

        SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());
        //this.AddManipulator(new ContextualMenuManipulator(OnContextMenuPopulate));


        //右クリックでのメニューの表示
        searchWindow = ScriptableObject.CreateInstance<SearchMenuWindow>();
        searchWindow.Init(this, editor);

        nodeCreationRequest += context =>
        {
            SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), searchWindow);
        };

    }

    public GraphAsset GetGraphAsset()
    {
        return graphAsset;
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter adapter)
    {
        var compatible = new List<Port>();

        compatible.AddRange(ports.ToList().Where(port =>
        {
            var test = port.ToString();

            //分岐
            // 同じノードには繋げない
            if (startPort.node == port.node)
            {
                return false;
            }

            // Input同士、Output同士は繋げない
            if (port.direction == startPort.direction)
            {
                return false;
            }

            // ポートの型が一致していない場合は繋げない
            if (port.portType != startPort.portType)
            {
                return false;
            }

            return true;
        }));

        return compatible;
    }

    void Update()
    {
        
    }
}
