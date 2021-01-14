using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class GraphEditorElement : VisualElement
{
    GraphAsset m_GraphAsset;
    List<NodeElement> m_Nodes;
    List<EdgeElement> m_Edges;

    public void SerializeEdge(EdgeElement edge)
    {
        var serializableEdge = new SerializableEdge()
        {
            toId = m_Nodes.IndexOf(edge.To)  // ここで先ノードのIDを数える
        };

        edge.From.serializableNode.edges.Add(serializableEdge);  // 実際に追加
        edge.serializableEdge = serializableEdge;  // EdgeElementに登録しておく
    }

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

        m_Edges = new List<EdgeElement>();

        foreach (var node in m_Nodes)
        {
            foreach (var edge in node.serializableNode.edges)
            {
                CreateEdgeElement(edge, node, m_Nodes);
            }
        }
    }
    void CreateNodeElement(SerializableNode node)
    {
        var nodeElement = new NodeElement(node);

        Add(nodeElement);
        m_Nodes.Add(nodeElement);
    }
    public EdgeElement CreateEdgeElement(SerializableEdge edge, NodeElement fromNode, List<NodeElement> nodeElements)
    {
        var edgeElement = new EdgeElement(edge, fromNode, nodeElements[edge.toId]);
        Add(edgeElement);
        m_Edges.Add(edgeElement);

        return edgeElement;
    }

    public EdgeElement CreateEdgeElement(NodeElement fromNode, Vector2 toPosition)
    {
        var edgeElement = new EdgeElement(fromNode, toPosition);
        Add(edgeElement);
        m_Edges.Add(edgeElement);

        return edgeElement;
    }

    public void RemoveEdgeElement(EdgeElement edge)
    {
        // 消すエッジにSerializableEdgeがあれば、それを消す
        if (edge.serializableEdge != null)
        {
            edge.From.serializableNode.edges.Remove(edge.serializableEdge);
        }

        Remove(edge);
        m_Edges.Remove(edge);
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
    public void DrawEdge()
    {
        foreach (var edge in m_Edges)
        {
            edge.DrawEdge();
        }
    }

    // マウスの位置にあるノードを返す
    public NodeElement GetDesignatedNode(Vector2 position)
    {
        foreach (NodeElement node in m_Nodes)
        {
            if (node.ContainsPoint(node.WorldToLocal(position)))
                return node;
        }

        return null;
    }

    // すでに同じエッジがあるかどうか
    public bool ContainsEdge(NodeElement from, NodeElement to)
    {
        return m_Edges.Exists(edge =>
        {
            return edge.From == from && edge.To == to;
        });
    }
}
