using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using PaintCraft.Tools;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Observer;
public class ToggleTextures : MonoBehaviour, IPointerClickHandler
{
    public Toggle toggleTextures;
    [SerializeField] Texture2D textureBrush;
    [SerializeField] private Image select;
    [SerializeField] private Image background;
    [SerializeField] GameObject giltterImage;
    [SerializeField] private Image activeIcon;

    [Header("Draw Picture Scene")] [SerializeField]
    private LineConfig lineConfig;

    [SerializeField] private Brush brush;

    [SerializeField] private ToggleTools toggleTools;

    public Image Background
    {
        get => background;
        set => background = value;
    }

    public Texture2D TextureBrush
    {
        get => textureBrush;
        set => textureBrush = value;
    }

    public TypePen typePen;
    Action<object> _OnSwitchTypePen;
    private void Awake()
    {
        toggleTextures = GetComponent<Toggle>();
        select = transform.GetChild(0).GetComponent<Image>();
        background = transform.GetChild(1).GetComponent<Image>();
        activeIcon = transform.GetChild(2).GetComponent<Image>();

        
    }

    // Start is called before the first frame update
    void Start()
    {
        select.transform.localScale = Vector2.zero;

        toggleTextures.onValueChanged.AddListener(OnValueChange);
        if (toggleTextures.isOn)
            OnValueChange(true);

        AddIconGitller();
        giltterImage = background.transform.GetChild(0).gameObject;
        _OnSwitchTypePen = (param) => OnSwitchTypePen((TypePen)param);
        this.RegisterListener(EventID.OnSwitchTypePen, _OnSwitchTypePen);

       // if (LoadSceneManager.Instance.nameMinigame == NameMinigame.ColoringASMR || LoadSceneManager.Instance.nameMinigame == NameMinigame.PrincessColoring)
        {
            if (typePen == TypePen.Parttern)
            {
                giltterImage.gameObject.SetActive(false);
                textureBrush = Resources.Load<Texture2D>($"PatternBucket/" + transform.GetSiblingIndex());
                background.sprite = Resources.Load<Sprite>($"IconPattern/" + transform.GetSiblingIndex());
                background.SetNativeSize();
                background.color = Color.white;
            }
        }
        //else if (LoadSceneManager.Instance.nameMinigame == NameMinigame.DIY)
        //{
        //    if (typePen == TypePen.Parttern)
        //    {
        //        giltterImage.gameObject.SetActive(false);
        //        AddressableManager.Instance.Load<Texture2D>("Pattern_DIY/" + transform.GetSiblingIndex() + ".png",
        //       gameObject, OnLoadedTexturePattern);
        //        AddressableManager.Instance.Load<Sprite>("Pattern_DIY_Icon/" + transform.GetSiblingIndex() + ".png",
        //       gameObject, OnLoadedSpritePattern);

        //        background.SetNativeSize();
        //        background.color = Color.white;
        //    }
        //    else if (typePen == TypePen.Giltter)
        //    {
        //        giltterImage.gameObject.SetActive(false);
        //        //textureBrush = AddressableManager.Instance.LoadTexture("Glitters_DIY/" + transform.GetSiblingIndex() + ".png");
        //        //background.sprite = AddressableManager.Instance.LoadSprite("Glitters_DIY_Icon/" + transform.GetSiblingIndex() + ".png");

        //        AddressableManager.Instance.Load<Texture2D>("Glitters_DIY/" + transform.GetSiblingIndex() + ".png",
        //      gameObject, OnLoadedTextureGitller);
                

        //       // background.SetNativeSize();
        //        background.color = Color.white;
        //    }
        //    else if (typePen == TypePen.Ombre)
        //    {
        //        giltterImage.gameObject.SetActive(false);
        //        //textureBrush = AddressableManager.Instance.LoadTexture("Ombre_DIY/" + transform.GetSiblingIndex() + ".png");
        //        //background.sprite = AddressableManager.Instance.LoadSprite("Ombre_DIY_Icon/" + transform.GetSiblingIndex() + ".png");

        //        AddressableManager.Instance.Load<Texture2D>("Ombre_DIY/" + transform.GetSiblingIndex() + ".png",
        //       gameObject, OnLoadedTextureOmbre);
        //        AddressableManager.Instance.Load<Sprite>("Ombre_DIY_Icon/" + transform.GetSiblingIndex() + ".png",
        //       gameObject, OnLoadedSpriteOmbre);

        //        background.SetNativeSize();
        //        background.color = Color.white;
        //    }
        //}    
    }

    private void OnLoadedSpriteOmbre(Sprite obj)
    {
        background.sprite = obj;
        background.SetNativeSize();
    }

    private void OnLoadedTextureOmbre(Texture2D obj)
    {
        textureBrush = obj;
    }

    private void OnLoadedTextureGitller(Texture2D obj)
    {
        textureBrush = obj;
        background.transform.GetChild(0).GetComponent<RawImage>().texture = obj;
        background.transform.GetChild(0).GetComponent<RawImage>().SetNativeSize();
        background.transform.GetChild(0).gameObject.SetActive(true);
    }

    private void OnLoadedSpritePattern(Sprite obj)
    {
        background.sprite = obj;
        background.SetNativeSize();
    }

    private void OnLoadedTexturePattern(Texture2D obj)
    {
        textureBrush = obj;
    }

    private void OnDestroy()
    {
        this.RemoveListener(EventID.OnSwitchTypePen, _OnSwitchTypePen);
    }

    private void OnSwitchTypePen(TypePen param)
    {
        //Debug.LogError("OnSwitchTypePen");
        //if(LoadSceneManager.Instance.nameMinigame != NameMinigame.DIY)
        giltterImage.SetActive(param == TypePen.Giltter);

        
    }

    void AddIconGitller()
    {
        if (toggleTools)
        {
            if (toggleTools.toolsType == ToolsType.Glitter)
            {
                GameObject rawImage = new GameObject("iconRawImage");
                rawImage.AddComponent<RawImage>();
                rawImage.transform.parent = background.transform;
                rawImage.transform.localPosition = Vector3.zero;
                rawImage.transform.localScale = Vector3.one;
                rawImage.transform.SetSiblingIndex(0);
                rawImage.GetComponent<RawImage>().texture = textureBrush;
                rawImage.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
                background.gameObject.AddComponent<Mask>();
            }
        }
        else
        {
            if(gameObject.name.Contains("Glitter"))
            {
                GameObject rawImage = new GameObject("iconRawImage");
                rawImage.AddComponent<RawImage>();
                rawImage.transform.parent = background.transform;
                rawImage.transform.localPosition = Vector3.zero;
                rawImage.transform.localScale = Vector3.one;
                rawImage.transform.SetSiblingIndex(0);
                rawImage.GetComponent<RawImage>().texture = textureBrush;
                rawImage.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
                background.gameObject.AddComponent<Mask>();
            }    
        }    
    }    

    public void OnValueChange(bool isOn)
    {
        if (isOn)
        {
            select.transform.DOScale(Vector2.one, 0.5f).SetEase(Ease.OutExpo);

            Debug.LogError("switch line config 2");
            //DrawPictureController.Instance.ScreenCameraController.LineConfig = lineConfig;

            this.PostEvent(EventID.OnSwitchTypePen, typePen);

            //if (LoadSceneManager.Instance.nameMinigame == NameMinigame.DIY)
            //{
            //    DIY.GameManager_DIY.Instance.SetIndexColor(transform.GetSiblingIndex(), typePen);
            //    DIY.GameManager_DIY.Instance.ScrollToItem(gameObject.GetComponent<RectTransform>());
            //}

            //if (LoadSceneManager.Instance.nameMinigame == NameMinigame.ColoringASMR)
            //{
            //    if (DrawPictureControllerASMR.Instance.typePen == TypePen.Parttern)
            //    {
            //        this.PostEvent(EventID.OnChangePattern, transform.GetSiblingIndex());
            //    }
            //    else if (DrawPictureControllerASMR.Instance.typePen == TypePen.Giltter)
            //    {
            //        this.PostEvent(EventID.OnChangeColorGlliter, background.color);
            //    }
            //    else if (DrawPictureControllerASMR.Instance.typePen == TypePen.Color)
            //    {
            //        this.PostEvent(EventID.OnChangeColor, background.color);
            //    }
            //}
            //else if (LoadSceneManager.Instance.nameMinigame == NameMinigame.PrincessColoring)
            {
                if (DrawPictureController.Instance.typePen == TypePen.Parttern)
                {
                    this.PostEvent(EventID.OnChangePattern, transform.GetSiblingIndex());
                }
                else if (DrawPictureController.Instance.typePen == TypePen.Giltter)
                {
                    this.PostEvent(EventID.OnChangeColorGlliter, background.color);
                }
                else if (DrawPictureController.Instance.typePen == TypePen.Color)
                {
                    this.PostEvent(EventID.OnChangeColor, background.color);
                }
            }
            //else if (LoadSceneManager.Instance.nameMinigame == NameMinigame.DIY)
            //{
            //    if (DIY.GameManager_DIY.Instance.typePen == TypePen.Parttern ||
            //        DIY.GameManager_DIY.Instance.typePen == TypePen.Ombre)
            //    {
            //        this.PostEvent(EventID.OnChangePattern, transform.GetSiblingIndex());
            //        lineConfig.Color = PointColor.White;
            //    }
            //    else if (DIY.GameManager_DIY.Instance.typePen == TypePen.Color)
            //    {
            //        this.PostEvent(EventID.OnChangeColor, background.color);
            //    }
            //    else if (DIY.GameManager_DIY.Instance.typePen == TypePen.Giltter)
            //    {
            //        lineConfig.Color = PointColor.White;
            //    }

            //}

            toggleTools.ToggleTextures = this;
            SetupLineConfig();
            //SoundManager_BabyGirl.Instance.PlayOneShot("Sounds/UI/Bucket");
        }
        else
        {
            select.transform.DOScale(Vector2.zero, 0.5f).SetEase(Ease.OutExpo);
        }
    }

    public void FadeColor(int value, float duration)
    {
        select.DOFade(value, duration);
        background.DOFade(value, duration);
        activeIcon.DOFade(value, duration);
    }

    void SetupLineConfig()
    {
        lineConfig.Brush = brush;
        //if (LoadSceneManager.Instance.nameMinigame == NameMinigame.ColoringASMR)
        //{

        //}
        //else
        //    lineConfig.Texture = textureBrush;

        
        lineConfig.Texture = textureBrush;
    }


    private void OnEnable()
    {
        if (toggleTextures.isOn)
            OnValueChange(true);
        // if (ToolsDrawManager.Instance.nameScene is GameConstant.DRAWLINE)
        //     ToolsController.Instance.SetTextureBrush(textureBrush);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // SoundManager.Instance?.PlayUISound(SoundType.Button);
    }
}