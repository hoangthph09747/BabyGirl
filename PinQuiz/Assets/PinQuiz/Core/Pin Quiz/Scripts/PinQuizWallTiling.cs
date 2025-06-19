using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace PinQuiz
{
    public class PinQuizWallTiling : MonoBehaviour
    {
        private void Start()
        {
            Tile();
        }

        public void Tile()
        {
            var mat = GetComponent<Renderer>().material;
            var texture = mat.GetTexture("_MainTex");
            var ratio = texture.width * 1f / texture.height;
            mat.SetVector("_Tiling_Value", transform.localScale / new Vector2(ratio, 1f));
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(PinQuizWallTiling)), CanEditMultipleObjects]
    public class PinQuizWallTilingEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("Auto Tiling"))
            {
                foreach (var target in targets)
                {
                    (target as PinQuizWallTiling).Tile();
                }
            }
        }
    }
#endif
}
