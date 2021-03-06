﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;

public class NodeElement : Box
{
    public SerializableNode serializableNode;

    public NodeElement(SerializableNode node)
    {
        serializableNode = node;

        style.position = Position.Absolute;
        style.height = 100;
        style.width = 100;
        transform.position = node.position;

        this.AddManipulator(new NodeDragger());
        this.AddManipulator(new EdgeConnector());
        this.AddManipulator(new ContextualMenuManipulator(OnContextualMenuPopulate));
        Add(new Label(node.name));

    }
    private void OnContextualMenuPopulate(ContextualMenuPopulateEvent evt)
    {
        if (evt.target is NodeElement)
        {
            evt.menu.AppendAction(
                "Remove Node",
                RemoveNodeMenuAction,
                DropdownMenuAction.AlwaysEnabled);
        }
    }

    private void RemoveNodeMenuAction(DropdownMenuAction menuAction)
    {
        // 親をたどって削除をリクエスト
        var graph = GetFirstAncestorOfType<GraphEditorElement>();
        graph.RemoveNodeElement(this);
    }

    public Vector2 GetStartPosition()
    {
        return (Vector2)transform.position + new Vector2(style.width.value.value / 2f, style.height.value.value);
    }
    public Vector2 GetEndPosition()
    {
        return (Vector2)transform.position + new Vector2(style.width.value.value / 2f, 0f);
    }
    public Vector2 GetStartNorm()
    {
        return new Vector2(0f, 1f);
    }
    public Vector2 GetEndNorm()
    {
        return new Vector2(0f, -1f);
    }
}
