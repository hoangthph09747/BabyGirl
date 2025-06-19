#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace HongQuan
{
    [RequireComponent(typeof(LineRenderer))]
    public class MyLineRendererEditor : MonoBehaviour
    {

    }

#if UNITY_EDITOR
    [CustomEditor(typeof(MyLineRendererEditor))]
    public class LineRendererEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("Set Z to 0"))
            {
                LineRenderer lineRenderer = ((MyLineRendererEditor)target).GetComponent<LineRenderer>();

                Undo.RecordObject(lineRenderer, "Set Z to 0");

                Vector3[] positions = new Vector3[lineRenderer.positionCount];

                lineRenderer.GetPositions(positions);

                for (int i = 0; i < positions.Length; i++)
                {
                    positions[i].z = 0;
                }

                lineRenderer.SetPositions(positions);
                EditorUtility.SetDirty(lineRenderer);
            }
        }
    }
#endif
}
