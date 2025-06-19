using System;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class VectorPath : MonoBehaviour 
{
    public Vector3[] path;
    public bool isDraw;
    public bool isClosed;
    public float lineThickness = 1;
    public Color color = Color.red;
    public float dotHandleSize = 0.2f;

    [Tooltip("Toggle between World and Local coordinates for path points.")]
    public bool isLocalSpace = false;
    private bool previousLocalSpace = false;

    private void OnValidate()
    {
        if (path == null) return;

        if (isLocalSpace != previousLocalSpace)
        {
            for (int i = 0; i < path.Length; i++)
            {
                if (isLocalSpace)
                {
                    // Convert from world to local
                    path[i] = transform.InverseTransformPoint(path[i]);
                }
                else
                {
                    // Convert from local to world
                    path[i] = transform.TransformPoint(path[i]);
                }
            }

            previousLocalSpace = isLocalSpace;
        }
    }

    public void PushBackPoint(Vector3 point)
    {
        var tList = new List<Vector3>(path);
        tList.Add(point);
        path = tList.ToArray();
    }

    public Vector3[] ShiftPath(int pos)
    {
        Vector3[] shiftedPath = new Vector3[path.Length];
        Array.Copy(path, pos, shiftedPath, 0, path.Length - pos);
        Array.Copy(path, 0, shiftedPath, path.Length - pos, pos);
        return shiftedPath;
    }
    /// <summary>
    /// Trả về mảng các điểm path dưới dạng world position.
    /// </summary>
    public Vector3[] GetWorldPath()
    {
        if (path == null) return new Vector3[0];

        if (!isLocalSpace)
        {
            return path;
        }

        Vector3[] result = new Vector3[path.Length];
        for (int i = 0; i < path.Length; i++)
        {
            result[i] = transform.TransformPoint(path[i]);
        }
        return result;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(VectorPath))]
public class VectorPathsEditor : Editor
{
    private VectorPath wp;

    private void OnEnable()
    {
        wp = (VectorPath)target;
    }

    private void OnSceneGUI()
    {
        if (!wp.isDraw || wp.path == null) return;

        Undo.RecordObject(wp, "Change Path");
        Handles.color = wp.color;

        for (int i = 0; i < wp.path.Length; i++)
        {
            Vector3 worldPos = wp.isLocalSpace ? wp.transform.TransformPoint(wp.path[i]) : wp.path[i];
            Handles.Label(worldPos + Vector3.up * 0.5f, i.ToString());

            Vector3 nextPos = Vector3.zero;
            if (i != wp.path.Length - 1)
                nextPos = wp.isLocalSpace ? wp.transform.TransformPoint(wp.path[i + 1]) : wp.path[i + 1];
            else if (wp.isClosed)
                nextPos = wp.isLocalSpace ? wp.transform.TransformPoint(wp.path[(i + 1) % wp.path.Length]) : wp.path[(i + 1) % wp.path.Length];

            if (i != wp.path.Length - 1 || wp.isClosed)
                Handles.DrawLine(worldPos, nextPos, wp.lineThickness);

            Vector3 movedWorld = Handles.FreeMoveHandle(worldPos, wp.dotHandleSize, Vector3.one * 0.1f, Handles.DotHandleCap);

            if (worldPos != movedWorld)
            {
                wp.path[i] = wp.isLocalSpace ? wp.transform.InverseTransformPoint(movedWorld) : movedWorld;
            }
        }
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.HelpBox("Toggle Local Space to use points relative to object origin.", MessageType.Info);

        if (GUILayout.Button("Paths to center of Scene"))
        {
            PathsToCenter();
        }
    }

    private void PathsToCenter()
    {
        foreach (var item in targets)
        {
            var path = item as VectorPath;
            for (int i = 0; i < path.path.Length; i++)
            {
                path.path[i] = path.isLocalSpace ?
                    path.transform.InverseTransformPoint(SceneView.lastActiveSceneView.pivot) :
                    SceneView.lastActiveSceneView.pivot;
            }
        }
    }
}
#endif
