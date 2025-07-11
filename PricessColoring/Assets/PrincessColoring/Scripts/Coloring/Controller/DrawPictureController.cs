using System;
using System.Collections;
using System.Collections.Generic;
using PaintCraft.Canvas.Configs;
using PaintCraft.Controllers;
using PaintCraft.Tools;
using UnityEngine;
using Observer;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
public enum ToolsType
{
    Colors,
    Glitter,
    Fills,
    Patterns,
    Stamps,
    Easer,
    MainColor
}
public class DrawPictureController : Singleton<DrawPictureController>
{
    [SerializeField] private ToolsType toolsType;

    [Header("Elements")] [SerializeField] private ScreenCameraController screenCameraController, screenCameraControllerGitller;

    [SerializeField] private CanvasController _canvasController, _canvasControllerGitller;

    [Header("List")] [SerializeField] private List<PageConfig> listPageConfig;
    [SerializeField] private List<LineConfig> listLineConfig, listLineConfigGitller;

    [SerializeField] private int indexPageConfig = 0;
    public List<LineConfig> ListLineConfig => listLineConfig;
    public float Aspect => (float) Screen.height / (float) Screen.width;

    public ToolsType ToolsType
    {
        get => toolsType;
        set => toolsType = value;
    }

    public TypePen typePen;
    [SerializeField]
    Button buttonSwitchTypePen, buttonGotoScreenshot, buttonTakeScreenshot, buttonChangeBg, buttonExit, buttonGallery,buttonHideGallery,
        buttonNextToChoosePicture, buttonBackToDraw, buttonGotoChoosePicture, buttonSaveGallery, buttonBackHome;
    [SerializeField]
    GameObject bgOrigin, uiDraw, uiTakeScreenshot, uiSaveGallery;
    [SerializeField]
    RawImage screnshotRawImage;
    
    [SerializeField]
    Sprite spNormalColor, spGitller;
    [SerializeField]
    Image imageButtonSwitchColor, bgSwitchPage;
    [SerializeField]
    GameObject uiColor, uiGitller, uiGallery, buttonNext, buttonPrev, handTouchGiltter;

    public ScreenCameraController ScreenCameraController => screenCameraController;

    [Header("Audio")] [SerializeField] private AudioClip bgClip;

    string keyIndexPageConfig = "keyIndexPageConfig";
    string keyTypePen = "keyTypePen";
    string keyNumberPlayColoring = "keyNumberPlayColoring";

    bool clickSwitchPage = true;

    [SerializeField]
    List<GameObject> bgs;
    [SerializeField]
    int indexBg = 0;

    [SerializeField]
    ZoomCamera[] zoomCameras;
    [SerializeField]
    CameraController_Move[] cameraController_Moves;
    [SerializeField]
    Camera cameraScreenshot;
    [SerializeField]
    SpriteRenderer spriteRendererScreenshot;
    [SerializeField]
    GameObject drawNormal, drawGitller;
    Action<object> _OnSwitchTypePen, _OnChangeColor;
    [SerializeField]
    AudioClip audioClip;
    private void Awake()
    {
        //indexPageConfig = PlayerPrefs.GetInt(keyIndexPageConfig, 0);
        //_canvasController.PageConfig = listPageConfig[indexPageConfig];
        //_canvasController.SetActivePageConfig(_canvasController.PageConfig);

        //_canvasControllerGitller.PageConfig = listPageConfig[indexPageConfig];
        //_canvasControllerGitller.SetActivePageConfig(_canvasControllerGitller.PageConfig);

       // _canvasController.PageConfig = AppData.SelectedPageConfig;
        _canvasController.SetActivePageConfig(_canvasController.PageConfig);

        //_canvasControllerGitller.PageConfig = AppData.SelectedPageConfig;
        _canvasControllerGitller.SetActivePageConfig(_canvasControllerGitller.PageConfig);

        // _canvasControllerGitller.ClearHistory();
        bgSwitchPage.color = Color.white;
    }

    // Start is called before the first frame update
    void Start()
    {
        //MyAdsBabyGirl.GetInstance().ShowBannerAd(LoadSceneManager.Instance.nameMinigame.ToString());

        bgs = new List<GameObject>();
        foreach (Transform item in bgOrigin.transform)
        {
            bgs.Add(item.gameObject);
        }

       

        //buttonPrev.SetActive(indexPageConfig > 0);
        //buttonNext.SetActive(indexPageConfig < listPageConfig.Count - 1);

        //typePen = (TypePen)(PlayerPrefs.GetInt(keyTypePen, 2));
       
        //if(typePen == TypePen.Giltter)
        //{
        //    typePen = TypePen.Color;
        //}    
        //else
        //{
        //    typePen = TypePen.Giltter;
        //}

        typePen = TypePen.Color;

        //Invoke(nameof(ClickSwitchTypePen), 0.25f);

        //this.RegisterListener(EventID.OnChangeColor, (param) => OnChangeColor((Color)param));
        _OnChangeColor = (param) => OnChangeColor((Color)param);
        this.RegisterListener(EventID.OnChangeColor, _OnChangeColor);
        buttonSwitchTypePen.onClick.AddListener(ClickSwitchTypePen);

        _OnSwitchTypePen = (param) => OnSwitchTypePen((TypePen)param);
        this.RegisterListener(EventID.OnSwitchTypePen, _OnSwitchTypePen);

        //if (PlayerPrefs.GetInt(keyNumberPlayColoring) < 3)
        //{
            
        //    Invoke(nameof(ShowHandTouchGiltter), 6);
        //}

        PlayerPrefs.SetInt(keyNumberPlayColoring, PlayerPrefs.GetInt(keyNumberPlayColoring) + 1);

        bgSwitchPage.DOFade(0, 0.5f).SetEase(Ease.OutBack).OnComplete(()=>
        {
            bgSwitchPage.raycastTarget = false;
        });

        //ToySoundManager.instance.PlayLoopSound(ToySoundManager.instance.allSoundEffect[UnityEngine.Random.Range(3, 5)]);
        buttonGotoScreenshot.onClick.AddListener(GotoScreenshot);
        buttonBackToDraw.onClick.AddListener(BackToDraw);
        buttonGotoChoosePicture.onClick.AddListener(BackToChoosePicture);
        buttonNextToChoosePicture.onClick.AddListener(BackToChoosePicture);
        buttonSaveGallery.onClick.AddListener(ClickSaveGallery);
        buttonTakeScreenshot.onClick.AddListener(ClickTakeScreenshot);
        buttonExit.onClick.AddListener(Exit);
        buttonGallery.onClick.AddListener(ClickGallery);
        buttonHideGallery.onClick.AddListener(HideGallery);
        buttonChangeBg.onClick.AddListener(ClickChangeBg);
        Invoke(nameof(ShowButtonBackHome), 3);
        buttonGotoScreenshot.gameObject.SetActive(false);
    }

    void ShowButtonBackHome()
    {
        buttonGotoScreenshot.gameObject.SetActive(true);
        buttonBackHome.gameObject.SetActive(true);
    }

    public void PlaySoundTouch()
    {
        SoundManager_BabyGirl.Instance.PlayOneShot(audioClip);
    }

    private void OnDestroy()
    {
        base.OnDestroy();
        this.RemoveListener(EventID.OnSwitchTypePen, _OnSwitchTypePen);
        this.RemoveListener(EventID.OnChangeColor, _OnChangeColor);
    }

    private void OnSwitchTypePen(TypePen param)
    {
        typePen = param;

        if (typePen == TypePen.Color)
        {
            screenCameraController.LineConfig = listLineConfig[0];
            screenCameraControllerGitller.LineConfig = listLineConfigGitller[1];
        }
        else if (typePen == TypePen.Giltter)
        {
            screenCameraController.LineConfig = listLineConfig[1];
            screenCameraControllerGitller.LineConfig = listLineConfigGitller[0];
        }
        else if (typePen == TypePen.Parttern)
        {
            screenCameraController.LineConfig = listLineConfig[0];
            screenCameraControllerGitller.LineConfig = listLineConfigGitller[1];
        }
    }

    public void ClickChangeBg()
    {
        indexBg++;
        if (indexBg == bgs.Count)
            indexBg = 0;
        ChangeBg();
    }

    void Exit()
    {
        SoundManager_BabyGirl.Instance.PlayOneShot("Sounds/UI/Button");
        //LoadSceneManager.Instance.LoadScene(Constant.SceneMenu);
        SceneManager.LoadScene(Constant.SceneMenu);
    }

    void BackToChoosePicture()
    {
        SoundManager_BabyGirl.Instance.PlayOneShot("Sounds/UI/Button");
        //LoadSceneManager.Instance.LoadScene(NameMinigame.PrincessColoring.ToString());
        SceneManager.LoadScene(NameMinigame.PrincessColoring.ToString());
    }    

    void GotoScreenshot()
    {
        //MyAdsBabyGirl.GetInstance().HideBannerAd(LoadSceneManager.Instance.nameMinigame.ToString());
        SoundManager_BabyGirl.Instance.PlayOneShot("Sounds/UI/Button");
        _canvasController.SaveChangesToDisk();
        _canvasControllerGitller.SaveChangesToDisk();
        screenCameraController.transform.localPosition = new Vector3(0, 0, 10);
        screenCameraControllerGitller.transform.localPosition = new Vector3(0, 0, 10);
        //screenCameraController.GetComponent<Camera>().orthographicSize = screenCameraController.GetComponent<ZoomCamera>().ZoomMaxBound;
        //screenCameraControllerGitller.GetComponent<Camera>().orthographicSize = screenCameraController.GetComponent<ZoomCamera>().ZoomMaxBound;
        screenCameraController.GetComponent<Camera>().orthographicSize = 880;
        screenCameraControllerGitller.GetComponent<Camera>().orthographicSize = 880;
        // TakeScreenshot.instance.CreateTextureScreenshot(SaveSystem.SAVE_FOLDER_PrincessColoring_Screenshot, _canvasController.PageConfig.name + ".jpg");
        // TODO: Implement screenshot logic if needed. TakeScreenshot reference removed.
        uiDraw.SetActive(false);
        uiTakeScreenshot.SetActive(true);
        
        Invoke(nameof(ShowBg), 0.25f);

        for (int i = 0; i < cameraController_Moves.Length; i++)
        {
            cameraController_Moves[i].moveAble = false;
        }

        for (int i = 0; i < zoomCameras.Length; i++)
        {
            zoomCameras[i].zoomAble = false;
        }
    }

    void BackToDraw()
    {
        SoundManager_BabyGirl.Instance.PlayOneShot("Sounds/UI/Button");
        uiDraw.SetActive(true);
        uiTakeScreenshot.SetActive(false);
        
        drawGitller.SetActive(true);
        drawNormal.SetActive(true);
        HideBg();

        for (int i = 0; i < cameraController_Moves.Length; i++)
        {
            cameraController_Moves[i].moveAble = true;
        }

        for (int i = 0; i < zoomCameras.Length; i++)
        {
            zoomCameras[i].zoomAble = true;
        }
    }

    void ChangeBg()
    {
        SoundManager_BabyGirl.Instance.PlayOneShot("Sounds/UI/Button");
        for (int i = 0; i < bgs.Count; i++)
        {
            bgs[i].SetActive(false);
        }
        bgs[indexBg].SetActive(true);
    }    

    void ShowBg()
    {
        drawGitller.SetActive(false);
        drawNormal.SetActive(false);
        cameraScreenshot.gameObject.SetActive(true);
        ChangeBg();
        SpriteRenderer bgScreenshot = bgs[indexBg].GetComponent<SpriteRenderer>();
        

        spriteRendererScreenshot.sprite = null;
        // TODO: Replace SaveSystem usage for sprite loading.
       
        if (bgScreenshot.sprite.rect.size.x < spriteRendererScreenshot.sprite.rect.size.x)
            spriteRendererScreenshot.transform.localScale = Vector3.one * (bgScreenshot.sprite.rect.size.x / spriteRendererScreenshot.sprite.rect.size.x) * 0.95f;
        else
            spriteRendererScreenshot.transform.localScale = Vector3.one * (bgScreenshot.sprite.rect.size.x / spriteRendererScreenshot.sprite.rect.size.x) * 0.95f;
    }  
    
    void HideBg()
    {
        if (indexBg == 0)
        {
            bgs[bgs.Count - 1].SetActive(false);
        }
        else
        {
            bgs[indexBg - 1].SetActive(false);
        }
        cameraScreenshot.gameObject.SetActive(false);
        
    }    

    public void BackHome()
    {
        buttonGotoScreenshot.gameObject.SetActive(false);
        SoundManager_BabyGirl.Instance.PlayOneShot("Sounds/UI/Button");
        // ToySoundManager.instance.PlaySoundEffect(ToySoundManager.instance.allSoundEffect[1]);
        _canvasController.SaveChangesToDisk();
        _canvasControllerGitller.SaveChangesToDisk();

        //bgSwitchPage.DOFade(1, 1).SetEase(Ease.OutBack);

        screenCameraController.transform.localPosition = new Vector3(0,0, 10);
        screenCameraControllerGitller.transform.localPosition = new Vector3(0, 0, 10);
        screenCameraController.GetComponent<Camera>().orthographicSize = screenCameraController.GetComponent<ZoomCamera>().ZoomMaxBound;
        screenCameraControllerGitller.GetComponent<Camera>().orthographicSize = screenCameraController.GetComponent<ZoomCamera>().ZoomMaxBound;
        FindObjectOfType<PrincessColoring.SliderZoomCamera>().gameObject.SetActive(false);
        // TakeScreenshot.instance.CreateTextureScreenshot(SaveSystem.SAVE_FOLDER_PrincessColoring_Screenshot, _canvasController.PageConfig.name + ".jpg");
        // TODO: Implement screenshot logic if needed. TakeScreenshot reference removed.

        bgSwitchPage.DOFade(1, 1).SetEase(Ease.OutBack).SetDelay(0.5f);
        StartCoroutine(GoHome());
    }    

    IEnumerator GoHome()
    {
        SoundManager_BabyGirl.Instance.PlayOneShot("Sounds/UI/Button");
        yield return new WaitForSeconds(2.5f);
        //LoadSceneManager.Instance.LoadScene(NameMinigame.PrincessColoring.ToString());
        SceneManager.LoadScene(NameMinigame.PrincessColoring.ToString());
    }    

    void ShowHandTouchGiltter()
    {
        if (typePen == TypePen.Color)
        {
            handTouchGiltter.SetActive(true);
            handTouchGiltter.transform.DOScale(0.85f, 0.5f).SetEase(Ease.OutBack).SetLoops(-1, LoopType.Yoyo);
        }
    }    

    void ClickSwitchTypePen()
    {
        if(handTouchGiltter.activeSelf)
        handTouchGiltter.SetActive(false);

        buttonSwitchTypePen.transform.DOPunchScale(Vector2.one * 0.2f, 0.5f).SetEase(Ease.OutBack);
        
        if (typePen == TypePen.Color)
        {
            typePen = TypePen.Giltter;
            imageButtonSwitchColor.sprite = spGitller;
            uiGitller.SetActive(true);
            if (uiColor)
                uiColor.SetActive(false);
        }
        else
        {
            typePen = TypePen.Color;
            imageButtonSwitchColor.sprite = spNormalColor;
            uiGitller.SetActive(false);
            if(uiColor != null)
            uiColor.SetActive(true);
        }

       // uiColor.SetActive(typePen == TypePen.Color);
       // uiGitller.SetActive(typePen == TypePen.Giltter);

        if (typePen == TypePen.Color)
        {
            screenCameraController.LineConfig = listLineConfig[0];
            screenCameraControllerGitller.LineConfig = listLineConfigGitller[1];
        }
        else if (typePen == TypePen.Giltter)
        {
            screenCameraController.LineConfig = listLineConfig[1];
            screenCameraControllerGitller.LineConfig = listLineConfigGitller[0];
        }

        this.PostEvent(EventID.OnSwitchTypePen, typePen);

        //ToySoundManager.instance.PlaySoundEffect(ToySoundManager.instance.allSoundEffect[2]);
        SoundManager_BabyGirl.Instance.PlayOneShot("Sounds/UI/Button");
    }   
    
    public void UndoRedoSound()
    {
        //ToySoundManager.instance.PlaySoundEffect(ToySoundManager.instance.allSoundEffect[11]);
        SoundManager_BabyGirl.Instance.PlayOneShot("Sounds/UI/Button");
    }    

    private void OnChangeColor(Color param)
    {
        if(typePen == TypePen.Color)
        {
            screenCameraController.LineConfig = listLineConfig[0];
            screenCameraControllerGitller.LineConfig = listLineConfigGitller[1];
        }
        else if (typePen == TypePen.Giltter)
        {
            screenCameraController.LineConfig = listLineConfig[1];
            screenCameraControllerGitller.LineConfig = listLineConfigGitller[0];
        }

       // ToySoundManager.instance.PlaySoundEffect(ToySoundManager.instance.allSoundEffect[UnityEngine.Random.Range(5, 8)]);
    }

    public void NextPicture()
    {
        //if (indexPageConfig >= listPageConfig.Count - 1)
        //    return;
        //StartCoroutine(IENextPicture());
        //ToySoundManager.instance.PlaySoundEffect(ToySoundManager.instance.allSoundEffect[1]);

        buttonNext.transform.DOPunchScale(Vector2.one * 0.15f, 0.5f).SetEase(Ease.OutBack);

        if (!clickSwitchPage)
            return;

        clickSwitchPage = false;

        if (indexPageConfig >= listPageConfig.Count - 1)
        {
            indexPageConfig = 0;
        }   
        else
            indexPageConfig++;
        PlayerPrefs.SetInt(keyIndexPageConfig, indexPageConfig);
        PlayerPrefs.SetInt(keyTypePen, (int)typePen);

        StartCoroutine(IENextPicture());

        bgSwitchPage.raycastTarget = true;
        bgSwitchPage.DOFade(1, 1).SetEase(Ease.OutBack).OnComplete(ReloadScene);

        //BonBonAdvertising.ShowInterstitialAd("Coloring_Next");
    }

    IEnumerator IENextPicture()
    {
        
        yield return new WaitForSeconds(0.01f);
        _canvasController.SaveChangesToDisk();
        _canvasControllerGitller.SaveChangesToDisk();
        AppData.SelectedPageConfig = listPageConfig[indexPageConfig];
        //indexPageConfig++;
        //_canvasController.PageConfig = listPageConfig[indexPageConfig];
        //_canvasController.ClearCanvas();
        //_canvasController.SetActivePageConfig(_canvasController.PageConfig);
        //_canvasController.ClearHistory();

        //_canvasControllerGitller.PageConfig = listPageConfig[indexPageConfig];
        //_canvasControllerGitller.ClearCanvas();
        //_canvasControllerGitller.SetActivePageConfig(_canvasControllerGitller.PageConfig);

        //_canvasControllerGitller.ClearHistory();

        //buttonPrev.SetActive(indexPageConfig > 0);
        //buttonNext.SetActive(indexPageConfig < listPageConfig.Count - 1);
    }

    public void PreviousPicture()
    {

        //if (indexPageConfig <= 0)
        //    return;
        //StartCoroutine(IEPreviousPicture());
        //ToySoundManager.instance.PlaySoundEffect(ToySoundManager.instance.allSoundEffect[1]);
        buttonPrev.transform.DOPunchScale(Vector2.one * 0.15f, 0.5f).SetEase(Ease.OutBack);
        if (!clickSwitchPage)
            return;

        clickSwitchPage = false;

        if (indexPageConfig <= 0)
        {
            indexPageConfig = listPageConfig.Count - 1;
        }
        else
            indexPageConfig--;

        PlayerPrefs.SetInt(keyIndexPageConfig, indexPageConfig);
        PlayerPrefs.SetInt(keyTypePen, (int)typePen);
        StartCoroutine(IEPreviousPicture());
        bgSwitchPage.raycastTarget = true;
        bgSwitchPage.DOFade(1, 1).SetEase(Ease.OutBack).OnComplete(ReloadScene);

       // BonBonAdvertising.ShowInterstitialAd("Coloring_Back");
    }

    IEnumerator IEPreviousPicture()
    {
        
        yield return new WaitForSeconds(0.01f);
        _canvasController.SaveChangesToDisk();
        _canvasControllerGitller.SaveChangesToDisk();
        AppData.SelectedPageConfig = listPageConfig[indexPageConfig];
        //indexPageConfig--;
        //_canvasController.PageConfig = listPageConfig[indexPageConfig];
        //_canvasController.ClearCanvas();
        //_canvasController.SetActivePageConfig(_canvasController.PageConfig);
        //_canvasController.ClearHistory();

        //_canvasControllerGitller.PageConfig = listPageConfig[indexPageConfig];
        //_canvasControllerGitller.ClearCanvas();
        //_canvasControllerGitller.SetActivePageConfig(_canvasControllerGitller.PageConfig);
        //_canvasControllerGitller.ClearHistory();


        //buttonPrev.SetActive(indexPageConfig > 0);
        //buttonNext.SetActive(indexPageConfig < listPageConfig.Count - 1);

    }

    void ReloadScene()
    {
        //LoadSceneManager.Instance.LoadScene(SceneManager.GetActiveScene().name);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
    }

    public void ClickTakeScreenshot()
    {
        StartCoroutine(StartTakeScreenshot());
       
    }

    [SerializeField]
    Texture2D screnshotTexture, textureScreenThumb;
    RenderTexture rt;
    [SerializeField]
    Transform pBottomLeft, pTopRight, bgScreenshot, bgSaveGallery;
    [SerializeField]
    GameObject canvasUI;
    IEnumerator StartTakeScreenshot()
    {
        SoundManager_BabyGirl.Instance.PlayOneShot($"Sounds/chup_anh");
        bgScreenshot.gameObject.SetActive(false);
        canvasUI.SetActive(false);
        yield return new WaitForEndOfFrame();

        int tw = Screen.width; //thumb width
        int th = Screen.height; //thumb height
        rt = new RenderTexture(tw, th, 24, RenderTextureFormat.ARGB32);
        rt.antiAliasing = 4;

       Vector2 posScreen1 = cameraScreenshot.WorldToScreenPoint(pBottomLeft.position);
       Vector2 posScreen3 = cameraScreenshot.WorldToScreenPoint(pTopRight.position);

        if (screnshotTexture == null)
            screnshotTexture = new Texture2D((int)(posScreen3.x - posScreen1.x), (int)(posScreen3.y - posScreen1.y),
                TextureFormat.ARGB32, false);
       

        //if (screnshotTexture == null)
        //    screnshotTexture = new Texture2D(tw, th, TextureFormat.ARGB32, false);

        //cameraScreenshot.targetTexture = rt;
        //RenderTexture.active = rt;
        //cameraScreenshot.clearFlags = CameraClearFlags.SolidColor;

        //// Simple: use a clear background
        //cameraScreenshot.backgroundColor = Color.clear;
        //cameraScreenshot.Render();

        //screnshotTexture.ReadPixels(new Rect(0, 0, tw, th), 0, 0, false);

        screnshotTexture.ReadPixels(new Rect(posScreen1.x, posScreen1.y, (int)(posScreen3.x - posScreen1.x), (int)(posScreen3.y - posScreen1.y)), 0, 0, false);

        screnshotTexture.Apply();

        string dateTime = System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        string name = _canvasController.PageConfig.name + dateTime + ".png";
        // SaveSystem.SaveTexture(screnshotTexture.EncodeToPNG(), SaveSystem.SAVE_FOLDER_PrincessColoring_Gallery , name);
        // TODO: Replace SaveSystem usage for texture saving.

        if (textureScreenThumb == null)
            textureScreenThumb = new Texture2D((int)(screnshotTexture.width), (int)(screnshotTexture.height),
                TextureFormat.RGBA32, false);

        if (textureScreenThumb.width < screnshotTexture.width)
        {
            textureScreenThumb.Reinitialize(screnshotTexture.width, screnshotTexture.height);
        }

        textureScreenThumb.SetPixels(screnshotTexture.GetPixels());
        textureScreenThumb.Apply();

        // SaveSystem.scale(textureScreenThumb, (int)(screnshotTexture.width * 0.5f), (int)(screnshotTexture.height * 0.5f));
        // SaveSystem.SaveTexture(textureScreenThumb.EncodeToPNG(), SaveSystem.SAVE_FOLDER_PrincessColoring_Thumb, name);
        // TODO: Replace SaveSystem usage for scaling and saving texture.

        bgSaveGallery.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.2f);
        canvasUI.SetActive(true);
        uiTakeScreenshot.SetActive(false);
        uiSaveGallery.SetActive(true);
        screnshotRawImage.texture = screnshotTexture;
        //screnshotRawImage.SetNativeSize();
    }

    void SaveToGallery(Texture2D texture2D)
    {
        buttonSaveGallery.transform.DOScale(Vector3.zero, 0.15f);
        string dateTime = System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        string name = string.Format(_canvasController.PageConfig.name + ".png", Application.productName, dateTime);
        string fileName = "PrincessColoring_" + name;
        if (NativeGallery.CheckPermission(NativeGallery.PermissionType.Write, NativeGallery.MediaType.Image) == NativeGallery.Permission.Granted)
        {
            NativeGallery.SaveImageToGallery(texture2D, NameMinigame.PrincessColoring.ToString() + "_Girl", fileName, (success, path) =>
            //Debug.Log("Saved success !"));
            PopupManager.Instance.ShowToat("Saved Gallery"));
        }
        else
        {
            NativeGallery.RequestPermission(NativeGallery.PermissionType.Write, NativeGallery.MediaType.Image);
            if (NativeGallery.CheckPermission(NativeGallery.PermissionType.Write, NativeGallery.MediaType.Image) != NativeGallery.Permission.Granted)
            {
                Debug.Log("OnPopupAllowPermission");
            }
            else
            {
                NativeGallery.SaveImageToGallery(texture2D, NameMinigame.PrincessColoring.ToString() + "_Girl", fileName, (success, path) =>
                //Debug.Log("Saved success !"));
                PopupManager.Instance.ShowToat("Saved Gallery"));
            }
            // OnPopupAllowPermission();
        }

    }

    void ClickSaveGallery()
    {
        buttonSaveGallery.gameObject.SetActive(false);
        SaveToGallery(screnshotTexture);
    }    

    void ClickGallery()
    {
        uiGallery.SetActive(true);
    }
    
    void HideGallery()
    {
        uiGallery.SetActive(false);
    }    
}