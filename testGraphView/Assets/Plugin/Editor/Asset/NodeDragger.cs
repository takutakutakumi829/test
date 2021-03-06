﻿using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;

public class NodeDragger : SelectionDragger
{
    private bool m_Focus;

    public NodeDragger()
    {
        // 左クリックで有効化する
        activators.Add(new ManipulatorActivationFilter { button = MouseButton.MiddleMouse });
    }

    /// Manipulatorにターゲットがセットされたときに呼ばれる
    protected override void RegisterCallbacksOnTarget()
    {
        m_Focus = false;

        target.RegisterCallback<MouseDownEvent>(OnMouseDown);
        target.RegisterCallback<MouseUpEvent>(OnMouseUp);
        target.RegisterCallback<MouseMoveEvent>(OnMouseMove);
        target.RegisterCallback<MouseCaptureOutEvent>(OnMouseCaptureOut);
    }

    /// Manipulatorのターゲットが変わる直前に呼ばれる
    protected override void UnregisterCallbacksFromTarget()
    {
        target.UnregisterCallback<MouseDownEvent>(OnMouseDown);
        target.UnregisterCallback<MouseUpEvent>(OnMouseUp);
        target.UnregisterCallback<MouseMoveEvent>(OnMouseMove);
        target.UnregisterCallback<MouseCaptureOutEvent>(OnMouseCaptureOut);
    }

    protected void OnMouseDown(MouseDownEvent evt)
    {
        // 設定した有効化条件をみたすか (= 左クリックか)
        if (CanStartManipulation(evt))
        {
            m_Focus = true;
            target.BringToFront();
            target.CaptureMouse();
        }
    }

    protected void OnMouseUp(MouseUpEvent evt)
    {
        // CanStartManipulation()で条件を満たしたActivationのボタン条件と、
        // このイベントを発火させているボタンが同じか
        // (= 左クリックを離したときか)
        if (CanStopManipulation(evt))
        {
            target.ReleaseMouse();
            m_Focus = false;
        }
    }

    protected void OnMouseCaptureOut(MouseCaptureOutEvent evt)
    {
        m_Focus = false;
    }

    protected void OnMouseMove(MouseMoveEvent evt)
    {
        if (m_Focus)
        {
            target.transform.position += (Vector3)evt.mouseDelta;
        }
    }
}
