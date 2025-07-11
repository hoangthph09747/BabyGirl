using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR

public class MaterialGenerator : MonoBehaviour
{
    [MenuItem("Tools/Create Materials")]
    private static void CreateMaterials()
    {
 
        foreach (Object o in Selection.objects)
        {
 
            if (o.GetType() != typeof(Texture2D))
            {
                Debug.LogError("This isn't a texture: " + o);
                continue;
            }
 
            Debug.Log("Creating material from: " + o);
 
            Texture2D selected = o as Texture2D;
 
            Material material = new Material(Shader.Find("DrawTutorial/DrawShader"));
            material.mainTexture = (Texture)o;
 
            string savePath = AssetDatabase.GetAssetPath(selected);
            savePath = savePath.Substring(0, savePath.LastIndexOf('/') + 1);
 
            string newAssetName = savePath + selected.name + ".mat";
 
            AssetDatabase.CreateAsset(material, newAssetName);
 
            AssetDatabase.SaveAssets();
 
        }
        Debug.Log("Done!");
    }
}
#endif