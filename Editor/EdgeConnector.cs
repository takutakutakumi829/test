using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EdgeConnector : MouseManipulator
{
    bool m_Active = false;
    GraphEditorElement m_Graph;
    EdgeElement m_ConnectingEdge;

    ContextualMenuManipulator m_AddEdgeMenu;

    public EdgeConnector()
    {
        // ノードの接続は左クリックで行う
        activators.Add(new ManipulatorActivationFilter() { button = MouseButton.LeftMouse });

        m_Active = false;

        // メニュー選択マニピュレータは作っておくが、この時点ではターゲットが確定していないので、
        // RegisterCallbacksOnTarget()で追加する
        m_AddEdgeMenu = new ContextualMenuManipulator(OnContextualMenuPopulate);
    }

    private void OnContextualMenuPopulate(ContextualMenuPopulateEvent evt)
    {
        if (evt.target is NodeElement node)
        {
            evt.menu.AppendAction(
                "Add Edge",
                (DropdownMenuAction menuItem) =>
                {
                    m_Active = true;

                    // 親をたどってGraphEditorElementを取得する
                    m_Graph = target.GetFirstAncestorOfType<GraphEditorElement>();
                    m_ConnectingEdge = m_Graph.CreateEdgeElement(node, menuItem.eventInfo.mousePosition);

                    target.CaptureMouse();
                },
                DropdownMenuAction.AlwaysEnabled);
        }
    }

    protected override void RegisterCallbacksOnTarget()
    {
        target.RegisterCallback<MouseDownEvent>(OnMouseDown);
        target.RegisterCallback<MouseUpEvent>(OnMouseUp);
        target.RegisterCallback<MouseMoveEvent>(OnMouseMove);
        target.RegisterCallback<MouseCaptureOutEvent>(OnCaptureOut);

        target.AddManipulator(m_AddEdgeMenu);
    }

    protected override void UnregisterCallbacksFromTarget()
    {
        target.RemoveManipulator(m_AddEdgeMenu);

        target.UnregisterCallback<MouseDownEvent>(OnMouseDown);
        target.UnregisterCallback<MouseUpEvent>(OnMouseUp);
        target.UnregisterCallback<MouseMoveEvent>(OnMouseMove);
        target.UnregisterCallback<MouseCaptureOutEvent>(OnCaptureOut);
    }

    protected void OnMouseDown(MouseDownEvent evt)
    {
        if (!CanStartManipulation(evt))
            return;

        // マウス押下では他のイベントが起きてほしくないのでPropagationを中断する
        if (m_Active)
            evt.StopImmediatePropagation();
    }

    protected void OnMouseUp(MouseUpEvent evt)
    {
        if (!CanStopManipulation(evt))
            return;

        if (!m_Active)
            return;

        var node = m_Graph.GetDesignatedNode(evt.originalMousePosition);

        if (node == null
            || node == target
            || m_Graph.ContainsEdge(m_ConnectingEdge.From, node))
        {
            m_Graph.RemoveEdgeElement(m_ConnectingEdge);
        }
        else
        {
            m_ConnectingEdge.ConnectTo(node);
            m_Graph.SerializeEdge(m_ConnectingEdge);  // つないだ時にシリアライズする
        }
        m_Active = false;
        m_ConnectingEdge = null;  // 接続終了
        target.ReleaseMouse();
    }

    protected void OnMouseMove(MouseMoveEvent evt)
    {
        if (!m_Active)
        {
            return;
        }

        // 2020/01/09 追記：Worldの座標系からGraphEditorElementの座標系に変換して渡すことにする
        //                 この例でいうと、ウィンドウの上のタブ領域の分だけずれることになる

        // m_ConnectingEdge.ToPosition = evt.originalMousePosition;  // 位置更新
        //  ↓↓ 変更
        m_ConnectingEdge.ToPosition = m_Graph.WorldToLocal(evt.mousePosition);  // 位置更新
        // 2020/01/09 追記ここまで
    }

    private void OnCaptureOut(MouseCaptureOutEvent evt)
    {
        if (!m_Active)
            return;

        // 中断時の処理
        m_Graph.RemoveEdgeElement(m_ConnectingEdge);
        m_ConnectingEdge = null;

        m_Active = false;
        target.ReleaseMouse();
    }
}
