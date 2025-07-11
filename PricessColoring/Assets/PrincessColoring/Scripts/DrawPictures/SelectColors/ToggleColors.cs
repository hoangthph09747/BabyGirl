using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using PaintCraft.Tools;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Observer;
public class ToggleColors : MonoBehaviour, IPointerClickHandler
{
    protected Toggle toggleColor;
    [SerializeField] protected RectTransform selectRing;
    [SerializeField] protected Image background;
    [SerializeField] protected Image activeIcon;

    [Header("Draw Picture Scene")] [SerializeField]
    private LineConfig lineConfigColor;

    [SerializeField] private LineConfig lineConfigBucket;
    [SerializeField] private Brush brushColor;
    [SerializeField] private Brush brushBucket;
    [SerializeField] private ToggleTools toggleToolsColor;
    [SerializeField] private ToggleTools toggleToolsFill;

    [SerializeField] protected string codeColor;

    Action<object> _OnSwitchTypePen;
    public Image Background
    {
        get => background;
        set => background = value;
    }
    private void Awake()
    {
        toggleColor = GetComponent<Toggle>();
    }
    // Start is called before the first frame update
    protected virtual void Start()
    {
        
        background = transform.GetChild(1).GetComponent<Image>();
        activeIcon = transform.GetChild(2).GetComponent<Image>();
        toggleColor.onValueChanged.AddListener(OnValueChange);
        if (toggleColor.isOn)
            OnValueChange(true);

        _OnSwitchTypePen = (param) => OnSwitchTypePen((TypePen)param);
        this.RegisterListener(EventID.OnSwitchTypePen, _OnSwitchTypePen);

        Initialize();
    }

    private void OnSwitchTypePen(TypePen param)
    {
        //if (toggleColor.isOn && gameObject.activeSelf)
        //    OnValueChange(true);
    }

    protected virtual void OnEnable()
    {
        if (toggleColor.isOn)
            OnValueChange(true);
    }

    private void OnDestroy()
    {
        this.RemoveListener(EventID.OnSwitchTypePen, _OnSwitchTypePen);
    }

    protected void Initialize()
    {
        toggleColor.group = GetComponentInParent<ToggleGroup>();
        selectRing.localScale = Vector2.zero;
    }

    public virtual void OnValueChange(bool isOn)
    {
        if (isOn)
        {
            toggleToolsColor.ToggleColors = this;
            toggleToolsFill.ToggleColors = this;
            SwitchColorByTool();
            selectRing.DOScale(Vector2.one, .5f);
            this.PostEvent(EventID.OnSwitchTypePen, TypePen.Color);
            this.PostEvent(EventID.OnChangeColor, background.color);

            //if(LoadSceneManager.Instance.nameMinigame == NameMinigame.DIY)
            //{
            //    DIY.GameManager_DIY.Instance.SetIndexColor(transform.GetSiblingIndex(), TypePen.Color);
            //    DIY.GameManager_DIY.Instance.ScrollToItem(gameObject.GetComponent<RectTransform>());
            //}
        }
        else
        {
            selectRing.DOScale(Vector2.zero, .5f);
        }
    }

    public void FadeColor(int value, float duration)
    {
        selectRing.GetComponent<Image>().DOFade(value, duration);
        background.DOFade(value, duration);
        activeIcon.DOFade(value, duration);
    }

    void SetupLineConfig(LineConfig lineConfig, Brush brush)
    {
        //
        //Debug.LogError("switch line config 1");
        //if (LoadSceneManager.Instance.nameMinigame == NameMinigame.PrincessColoring)
        //{
        //    DrawPictureController.Instance.ScreenCameraController.LineConfig = lineConfig;
        //}
        //else if (LoadSceneManager.Instance.nameMinigame == NameMinigame.ColoringASMR)
        //{
        //    DrawPictureControllerASMR.Instance.ScreenCameraController.LineConfig = lineConfig;
        //}
        DrawPictureController.Instance.ScreenCameraController.LineConfig = lineConfig;
        lineConfig.Brush = brush;
        lineConfig.Color.Color = background.color;
        lineConfig.Color.Alpha = 1f;
    }

    private void SwitchColorByTool()
    {
        ToolsType toolsType1 = ToolsType.Colors;
        //if (LoadSceneManager.Instance.nameMinigame == NameMinigame.PrincessColoring)
        {
            toolsType1 = DrawPictureController.Instance.ToolsType;
        }
        //else if (LoadSceneManager.Instance.nameMinigame == NameMinigame.ColoringASMR)
        //{
        //    toolsType1 = DrawPictureControllerASMR.Instance.ToolsType;
        //}
        switch (toolsType1)
        {
            case ToolsType.Colors:
                SetupLineConfig(lineConfigColor, brushColor);
                break;
            case ToolsType.Fills:
                SetupLineConfig(lineConfigBucket, brushBucket);
                break;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // SoundManager.Instance?.PlayUISound(SoundType.Button);
    }
}