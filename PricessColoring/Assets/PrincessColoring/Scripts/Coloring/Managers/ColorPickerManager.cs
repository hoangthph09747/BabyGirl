using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorPickerManager : Singleton<ColorPickerManager>
{
    public List<Color32> listColors;

    public void SetColors(List<Image> listImgColor)
    {
        for (int i = 0; i < listColors.Count; i++)
        {
            listImgColor[i].color = listColors[i];
        }
    }
    
}
