using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UpdateColors : MonoBehaviour
{
    [Header("Colors")]
    public List<Color32> listColor;

    [Header("Textures")]
    public List<Texture> listTextures;

    public List<ToggleTextures> togglesTexture;
    public List<ToggleColors> toggleColors;

    private void Start()
    {
        UpdateColorsImages();
    }

    public void UpdateColorsImages()
    {
        for (int i = 0; i < toggleColors.Count; i++)
        {
            toggleColors[i].Background.color = listColor[i];
            // ColorPickerManager.Instance.listColors[i] = listColor[i];
        }
    }

    public void UpdateTextures()
    {
        for (int i = 0; i < togglesTexture.Count; i++)
        {
            togglesTexture[i].TextureBrush = (Texture2D) listTextures[i];
        }
    }
    
    
}

#if UNITY_EDITOR
[CustomEditor(typeof(UpdateColors))]
public class UpdateColorsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        UpdateColors updateColors = (UpdateColors)target;
        if (GUILayout.Button("Update Colors"))
        {
            updateColors.UpdateColorsImages();
        }
        
        if (GUILayout.Button("Update Textures"))
        {
            updateColors.UpdateTextures();
        }
    }
}
#endif
