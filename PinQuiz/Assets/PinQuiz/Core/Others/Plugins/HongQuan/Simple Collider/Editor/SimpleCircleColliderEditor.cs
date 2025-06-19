using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SimpleCircleCollider))]
public class SimpleCircleColliderEditor : Editor
{
    private SimpleCircleCollider col;
    private void OnEnable()
    {
        col = (SimpleCircleCollider)target;
    }
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Get Bound from Renderer"))
        {
            GetBoundFromRenderer();
        }
    }

    private void OnSceneGUI()
    {
        SimpleCircleCollider col = (SimpleCircleCollider)target;
        //Handles.Label(col.transform.position,col.name);
        col.SetRadius(Handles.RadiusHandle(Quaternion.identity, col.GetCenter(), col.GetRadius()));
        var fmh_29_62_638636356137568553 = Quaternion.identity; col.SetCenter(Handles.FreeMoveHandle(col.GetCenter(), 0.08f, Vector3.one, Handles.DotHandleCap) - col.transform.position);
    }

    private void GetBoundFromRenderer()
    {
        Renderer renderer = col.gameObject.GetComponent<Renderer>();
        if (renderer != null)
        {
            col.SetCenter(renderer.bounds.center-col.transform.position);
            col.SetRadius(Mathf.Max(renderer.bounds.size.x, renderer.bounds.size.y) / 2f);
        }
    }
}
