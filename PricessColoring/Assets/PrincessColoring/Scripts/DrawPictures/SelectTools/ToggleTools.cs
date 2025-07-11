using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class ToggleTools : MonoBehaviour, IPointerDownHandler
{
    public ToolsType toolsType;
    public Toggle toggleTool;
    [SerializeField] protected Image glow;
    [SerializeField] protected Image background;
    public float offSetX;
    public Vector2 posInit;

    [Header("Draw Picture Scene")] private ToggleColors toggleColors;
    private ToggleTextures toggleTextures;
    [SerializeField] private Vector2 offSetPanelScalePen;

    public ToggleColors ToggleColors
    {
        get => toggleColors;
        set => toggleColors = value;
    }

    public ToggleTextures ToggleTextures
    {
        get => toggleTextures;
        set => toggleTextures = value;
    }

    // Start is called before the first frame update
    void Start()
    {
        toggleTool = GetComponent<Toggle>();
        glow = transform.GetChild(0).GetComponent<Image>();
        background = transform.GetChild(1).GetComponent<Image>();
        toggleTool.onValueChanged.AddListener(OnValueChange);
        if (toggleTool.isOn)
            OnValueChange(true);
    }


    protected virtual void OnValueChange(bool isOn)
    {
        if (isOn)
        {
            //if (LoadSceneManager.Instance.nameMinigame == NameMinigame.PrincessColoring)
            //    DrawPictureController.Instance.ToolsType = toolsType;
            //else
            //    DrawPictureControllerASMR.Instance.ToolsType = toolsType;

            DrawPictureController.Instance.ToolsType = toolsType;
            if (toolsType is ToolsType.Colors or ToolsType.Fills)
                toggleColors?.OnValueChange(true);
            else
                toggleTextures?.OnValueChange(true);


            background.transform.DOScale(Vector2.one, 1f);
            glow.transform.DOScale(Vector2.one, 1f);
            glow.DOFade(1, 1f);
        }
        else
        {
            background.transform.DOScale(new Vector2(0.9f, 0.9f), 1f);
            glow.transform.DOScale(new Vector2(0.9f, 0.9f), 1f);
            glow.DOFade(0, 1f);
        }
    }

    public void MoveTool(float to)
    {
        gameObject.GetComponent<RectTransform>().DOAnchorPosX(to, 0.5f);
    }

    public void FadeTool(int value, bool interact)
    {
        background.DOFade(value, 0.5f).OnComplete(() => toggleTool.interactable = interact);
        glow.DOFade(0, 1f);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // SoundManager.Instance?.PlayUISound(SoundType.Button);
        // GameAnalytics.Instance?.SwitchTypeTool(toolsType);
    }
}