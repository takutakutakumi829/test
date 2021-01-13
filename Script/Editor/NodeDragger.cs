using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class NodeDragger : MouseManipulator
{
    private bool Focus_Mouse;

    public NodeDragger()
    {
        activators.Add(new ManipulatorActivationFilter { button = MouseButton.LeftMouse });
    }

    protected override void RegisterCallbacksOnTarget()
    {
        Focus_Mouse = false;

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
        if (CanStartManipulation(evt))
        {
            Focus_Mouse = true;
            target.BringToFront();
            target.CaptureMouse();
        }
    }

    protected void OnMouseUp(MouseUpEvent evt)
    {
        if (CanStopManipulation(evt))
        {
            target.ReleaseMouse();

            if (target is NodeElement node)
            {
                //NodeElementに保存しておいたシリアライズ対象のポジションをいじる
                node.serializableNode.position = target.transform.position;
            }

            Focus_Mouse = false;
        }
    }

    protected void OnMouseCaptureOut(MouseCaptureOutEvent evt)
    {
        Focus_Mouse = false;
    }

    protected void OnMouseMove(MouseMoveEvent evt)
    {
        if (Focus_Mouse)
        {
            target.transform.position += (Vector3)evt.mouseDelta;
        }
    }
}
