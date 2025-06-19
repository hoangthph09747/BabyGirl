using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SimpleBoxCollider))]
public class SimpleBoxColliderEditor : Editor
{
    private SimpleBoxCollider col;
    private void OnEnable()
    {
        col = (SimpleBoxCollider)target;
    }
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Get Bound from Renderer"))
        {
            GetBoundFromRenderer();
        }
    }

    Vector3 left, right, up, down;
    Vector3 topLeft, bottomRight;

    private void OnSceneGUI()
    {
        //Handles.Label(col.transform.position, col.name);

        Undo.RecordObject(col, "ChangeBoxColliderSize");

        left = Handles.Slider(new Vector3(col.GetMinPoint().x, (col.GetMaxPoint().y + col.GetMinPoint().y) / 2), Vector3.left, 0.08f, Handles.DotHandleCap, 0.1f);
        right = Handles.Slider(new Vector3(col.GetMaxPoint().x, (col.GetMaxPoint().y + col.GetMinPoint().y) / 2), Vector3.right, 0.08f, Handles.DotHandleCap, 0.1f);
        up = Handles.Slider(new Vector3((col.GetMaxPoint().x + col.GetMinPoint().x) / 2, col.GetMaxPoint().y), Vector3.up, 0.08f, Handles.DotHandleCap, 0.1f);
        down = Handles.Slider(new Vector3((col.GetMaxPoint().x + col.GetMinPoint().x) / 2, col.GetMinPoint().y), Vector3.down, 0.08f, Handles.DotHandleCap, 0.1f);

        col.SetMaxPoint(new Vector2(right.x, up.y) - (Vector2)col.transform.position);
        col.SetMinPoint(new Vector2(left.x, down.y) - (Vector2)col.transform.position);

        var fmh_41_75_638636356137568664 = Quaternion.identity; col.SetMaxPoint(Handles.FreeMoveHandle((Vector3)col.GetMaxPoint(), 0.08f, Vector3.one, Handles.DotHandleCap) - col.transform.position);
        var fmh_42_75_638636356137605135 = Quaternion.identity; col.SetMinPoint(Handles.FreeMoveHandle((Vector3)col.GetMinPoint(), 0.08f, Vector3.one, Handles.DotHandleCap) - col.transform.position);
         var fmh_43_104_638636356137610738 = Quaternion.identity; col.SetTopLeft( Handles.FreeMoveHandle(new Vector3(col.GetMinPoint().x, col.GetMaxPoint().y),0.08f, Vector3.one, Handles.DotHandleCap) - col.transform.position);
         var fmh_44_106_638636356137615579 = Quaternion.identity; col.SetBottonRight(Handles.FreeMoveHandle(new Vector3(col.GetMaxPoint().x, col.GetMinPoint().y), 0.08f, Vector3.one, Handles.DotHandleCap) - col.transform.position);
    }   

    private void GetBoundFromRenderer()
    {
        Renderer renderer = col.gameObject.GetComponent<Renderer>();
        if (renderer != null)
        {
            col.SetMaxPoint(new Vector2(renderer.bounds.center.x + renderer.bounds.size.x / 2, renderer.bounds.center.y + renderer.bounds.size.y / 2) - (Vector2)col.transform.position);
            col.SetMinPoint(new Vector2(renderer.bounds.center.x - renderer.bounds.size.x / 2, renderer.bounds.center.y - renderer.bounds.size.y / 2) - (Vector2)col.transform.position);
        }
    }
}
