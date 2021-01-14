using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;

public class EdgeElement : VisualElement
{
    public SerializableEdge serializableEdge;

    public NodeElement From { get; private set; }
    public NodeElement To { get; private set; }

    Vector2 m_ToPosition;
    public Vector2 ToPosition
    {
        get { return m_ToPosition; }
        set
        {
            // 2020/01/09 追記：GraphEditorElementの座標系で渡されることを想定するように変更

             m_ToPosition = this.WorldToLocal(value);  // ワールド座標で渡されることを想定
            //  ↓↓ 変更
            // 2020/01/09 追記ここまで

            MarkDirtyRepaint();  // 再描画をリクエスト
        }
    }
    public EdgeElement()
    {
        // 2020/01/09 追記：マウスのイベントを呼ばれるためにはVisualElementのRectがマウス座標を含む必要がある
        //                 よって、エッジも大きさを持つため、自由な位置を取れるようにPosition.Absoluteを指定する
        style.position = Position.Absolute;
        // 2020/01/09 追記ここまで

        this.AddManipulator(new ContextualMenuManipulator(evt =>
        {
            if (evt.target is EdgeElement)
            {
                evt.menu.AppendAction(
                "Remove Edge",
                (DropdownMenuAction menuItem) =>
                {
                    // 親をたどってGraphEditorElementに削除リクエストを送る
                    var graph = GetFirstAncestorOfType<GraphEditorElement>();
                    graph.RemoveEdgeElement(this);
                },
                DropdownMenuAction.AlwaysEnabled);
            }
        }));
    }

    public EdgeElement(NodeElement fromNode, Vector2 toPosition):this()
    {
        From = fromNode;
        ToPosition = toPosition;
    }
    public EdgeElement(SerializableEdge edge, NodeElement fromNode, NodeElement toNode):this()
    {
        serializableEdge = edge;
        From = fromNode;
        To = toNode;
    }

    // つなげるときに呼ぶ
    public void ConnectTo(NodeElement node)
    {
        To = node;
        MarkDirtyRepaint();  // 再描画をリクエスト
    }

    // 2020/01/09 追記：バウンディングボックスの取得を別関数に分ける
    //                 また、幅が狭くなりすぎないように少し大きめに取ることにする
    //                 その大きさをDrawEdgeのタイミングで適用することにする
    private Rect GetBoundingBox()
    {
        Vector2 start = From.GetStartPosition();
        Vector2 end = To.GetEndPosition();

        Vector2 rectPos = new Vector2(Mathf.Min(start.x, end.x) - 12f, Mathf.Min(start.y, end.y) - 12f);
        Vector2 rectSize = new Vector2(Mathf.Abs(start.x - end.x) + 24f, Mathf.Abs(start.y - end.y) + 24f);
        Rect bound = new Rect(rectPos, rectSize);

        return bound;
    }

    // 2020/01/09 追記
    private void UpdateLayout()
    {
        Rect bound = GetBoundingBox();

        // レイアウトをバウンディングボックスに合わせて調整
        style.left = bound.x;
        style.top = bound.y;
        style.right = float.NaN;
        style.bottom = float.NaN;
        style.width = bound.width;
        style.height = bound.height;
    }

    readonly float INTERCEPT_WIDHT = 15f;

    public override bool ContainsPoint(Vector2 localPoint)
    {
        if (From == null || To == null)
            return false;

        // 2020/01/09 追記：EdgeElementのレイアウトに大きさを持たせたことにより
        //                 base.ContainsPoint()がバウンディングボックスを見る処理として利用できるようになった

        /*
        Vector2 start = From.GetStartPosition();
        Vector2 end = To.GetEndPosition();

        // ノードを覆うRectを作成
        Vector2 rectPos = new Vector2(Mathf.Min(start.x, end.x), Mathf.Min(start.y, end.y));
        Vector2 rectSize = new Vector2(Mathf.Abs(start.x - end.x), Mathf.Abs(start.y - end.y));
        Rect bound = new Rect(rectPos, rectSize);

        if (!bound.Contains(localPoint))
        {
            return false;
        }
        */
        //  ↓↓ 変更
        if (!base.ContainsPoint(localPoint))
        {
            return false;
        }

        // 2020/01/09 追記：localPointはEdgeElementの座標系で与えられる
        //                 それをGraphEditorElementの座標系に合わせる必要がある
        localPoint = this.ChangeCoordinatesTo(parent, localPoint);
        // 2020/01/09 追記ここまで

        // 近似線分ab
        Vector2 a = From.GetStartPosition() + 12f * From.GetStartNorm();
        Vector2 b = To.GetEndPosition() + 12f * To.GetEndNorm();

        // 一致した場合はaからの距離
        if (a == b)
        {
            return Vector2.Distance(localPoint, a) < INTERCEPT_WIDHT;
        }

        // 直線abとlocalPointの距離
        float distance = Mathf.Abs(
            (b.y - a.y) * localPoint.x
            - (b.x - a.x) * localPoint.y
            + b.x * a.y - b.y * a.x
            ) / Vector2.Distance(a, b);

        return distance < INTERCEPT_WIDHT;
    }

    public void DrawEdge()
    {
        if (From != null && To != null)
        {
            UpdateLayout();
            DrawEdge(
                startPos: From.GetStartPosition(),
                startNorm: From.GetStartNorm(),
                endPos: To.GetEndPosition(),
                endNorm: To.GetEndNorm());
        }
        else
        {
            // 追加中の描画用
            if (From != null)
            {
                DrawEdge(
                    startPos: From.GetStartPosition(),
                    startNorm: From.GetStartNorm(),
                    endPos: ToPosition,
                    endNorm: Vector2.zero);
            }
        }
    }

    private void DrawEdge(Vector2 startPos, Vector2 startNorm, Vector2 endPos, Vector2 endNorm)
    {
        Handles.color = Color.blue;
        Handles.DrawBezier(
            startPos,
            endPos,
            startPos + 50f * startNorm,
            endPos + 50f * endNorm,
            color: Color.blue,
            texture: null,
            width: 2f);

        Vector2 arrowAxis = 10f * endNorm;
        Vector2 arrowNorm = 5f * Vector3.Cross(endNorm, Vector3.forward);

        Handles.DrawAAConvexPolygon(endPos,
            endPos + arrowAxis + arrowNorm,
            endPos + arrowAxis - arrowNorm);
        Handles.color = Color.white;
    }
}
