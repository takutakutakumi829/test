using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class GraphEditorElement : VisualElement
{
    GraphAsset m_GraphAsset;
    List<NodeElement> m_Nodes;

    public GraphEditorElement(GraphAsset graphAsset)
    {
        m_GraphAsset = graphAsset;

        style.flexGrow = 1;
        style.overflow = Overflow.Hidden;

        this.AddManipulator(new ContextualMenuManipulator(OnContextMenuPopulate));

        m_Nodes = new List<NodeElement>();

        foreach (var node in graphAsset.nodes)
        {
            CreateNodeElement(node);
        }
    }
    void CreateNodeElement(SerializableNode node)
    {
        var nodeElement = new NodeElement(node);

        Add(nodeElement);
        m_Nodes.Add(nodeElement);
    }

    void OnContextMenuPopulate(ContextualMenuPopulateEvent evt)
    {
        evt.menu.AppendAction("Add Node", AddNodeMenuAction, DropdownMenuAction.AlwaysEnabled);
    }
 
    void AddNodeMenuAction(DropdownMenuAction menuAction)
    {
        Vector2 mousePosition = menuAction.eventInfo.localMousePosition;
        var node = new SerializableNode() { position = mousePosition };

        m_GraphAsset.nodes.Add(node);  // アセットに追加する

        CreateNodeElement(node);
    }
}
