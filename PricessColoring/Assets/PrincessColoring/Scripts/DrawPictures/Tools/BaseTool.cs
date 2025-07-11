using System.Collections;
using System.Collections.Generic;
using PaintCraft.Tools;
using UnityEngine;

public class BaseTool : MonoBehaviour
{
    [SerializeField] private LineConfig lineConfig;
    [SerializeField] private  Brush brush;

    protected LineConfig LineConfig => lineConfig;
    
    // Start is called before the first frame update
    protected virtual void Start()
    {
        
    }
    // Update is called once per frame

    public void BrushSelected()
    {
        SetupBrush();
    }
    
    protected virtual void SetupBrush()
    {
        Debug.LogError("switch line config 3");

        //if (LoadSceneManager.Instance.nameMinigame == NameMinigame.PrincessColoring)
        //    DrawPictureController.Instance.ScreenCameraController.LineConfig = lineConfig;
        //else
        //    DrawPictureControllerASMR.Instance.ScreenCameraController.LineConfig = lineConfig;
        DrawPictureController.Instance.ScreenCameraController.LineConfig = lineConfig;
        lineConfig.Brush = brush;
    }

}
