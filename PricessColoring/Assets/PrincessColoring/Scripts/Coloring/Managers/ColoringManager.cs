using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PaintCraft.Controllers;
using UnityEngine.UI;
using DG.Tweening;
public class ColoringManager : MonoBehaviour
{
    public Button buttonNextPicture ;
    public Transform buttonTakeScreenshot, buttonHome;
    public CanvasController [] canvasControllers;
    public List< Texture2D> texture2Ds;
    public List<Color32> colorsDefaultCanvas;
    bool checkAble = false;
    [SerializeField]
    GameObject oldUISelectColor, newUISelectColor;

    //số này để tính user vẽ được bao nhiêu phần trăm tranh , 0.5 là một nửa
    float percentDrawComplete = 0.9f;
    // Start is called before the first frame update
    void Start()
    {
        // Invoke(nameof(GetTexture), 0.5f);
       

       // newUISelectColor.SetActive(GameManager.instance.isNewUISelectColor);
       // oldUISelectColor.SetActive(!GameManager.instance.isNewUISelectColor);

        colorsDefaultCanvas = new List<Color32>();
        for (int i = 0; i < canvasControllers.Length; i++)
        {
            colorsDefaultCanvas.Add(canvasControllers[i].DefaultBGColor);
        }

        if (texture2Ds.Count <= 0)
            texture2Ds = new List<Texture2D>();

        StartCoroutine(UpdateTimeCheck());

        // buttonTakeScreenshot.gameObject.GetComponent<Button>().onClick.AddListener(ClickTakeScreenshot);
       // buttonTakeScreenshot.gameObject.SetActive(false);
    }

    void ClickTakeScreenshot()
    {
        DOTween.Kill(buttonTakeScreenshot.transform);
    }

    WaitForSeconds time = new WaitForSeconds(1);
    int t = 0, maxTime = 5, timeDone = 15;
    bool doneTime = false;
    IEnumerator UpdateTimeCheck()
    {
        while (true)
        {
            yield return time;
            t++;
            if (t >= maxTime)
            {
                checkAble = true;
            }
            if(t >= timeDone)
            {
                doneTime = true;
            }    
        }
    }

    Color[] pixels;
    void GetTexture()
    {
       
        for(int i =0; i < canvasControllers.Length; i++)
        {
            if(canvasControllers[i].transform.parent.gameObject.activeSelf)
            {
                if (canvasControllers[i].transform.Find("BackLayer").gameObject.GetComponent<Renderer>().material.mainTexture)
                {
                   // Texture2D texture = (Texture2D)(canvasControllers[i].transform.Find("BackLayer").gameObject.GetComponent<Renderer>().material.mainTexture);

                    Texture mainTexture = canvasControllers[i].transform.Find("BackLayer").gameObject.GetComponent<Renderer>().material.mainTexture;
                    Texture2D texture2D = new Texture2D(mainTexture.width, mainTexture.height, TextureFormat.RGBA32, false);

                    RenderTexture currentRT = RenderTexture.active;

                    RenderTexture renderTexture = new RenderTexture(mainTexture.width, mainTexture.height, 32);
                    Graphics.Blit(mainTexture, renderTexture);

                    RenderTexture.active = renderTexture;
                    texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
                    texture2D.Apply();

                     pixels = texture2D.GetPixels();

                    RenderTexture.active = currentRT;
                    if (texture2D)
                    {
                        if (texture2Ds.Count <= i)
                            texture2Ds.Add(texture2D);
                        else
                            texture2Ds[i] = texture2D;
                    }

                    renderTexture.Release();
                    renderTexture.DiscardContents(true, true);
                }
            }
        } 
    }

    bool completeDraw = false;
    private void Update()
    {
        if (Input.GetMouseButtonUp(0) && checkAble && !completeDraw && doneTime)
        {
            if (CheckDrawComplete())
            {
                completeDraw = true;

                buttonTakeScreenshot.gameObject.SetActive(true);
                // buttonTakeScreenshot.interactable = true;
                buttonTakeScreenshot.transform.DOScale(Vector2.one * 1.30f, 0.5f).SetLoops(-1, LoopType.Yoyo);

                //buttonHome.transform.DOScale(Vector2.one * 1.30f, 0.5f).SetLoops(-1, LoopType.Yoyo);
            }
        }
    }

    Color32[] colors;
    //số này để giảm số lượng pixel cần check, 10 tương ứng giảm 10 lần
    int numberDownCheck = 10;
    bool drawDone = false;
    bool CheckDrawComplete()
    {
        checkAble = false;
        t = 0;
        GetTexture();

        bool result = false;
        int [] counts = { 0, 0, 0};
        int maxPixel = 0;
       
        for(int i =0; i< texture2Ds.Count;i++)
        {
             colors = texture2Ds[i].GetPixels32();
            maxPixel = colors.Length;
            //Debug.LogError("1111:" + colors[0]);
            //Debug.LogError("222:" + colors[colors.Length/2]);
            //Debug.LogError("df:" + colorsDefaultCanvas[i]);
            for (int j =0; j< colors.Length; j++)
            {
                //check hết pixel thì giật quá nên giảm số lượng pixe cần check đi bằng cách lấy pixel chia hết cho 3 thôi
                //giảm dc 3 lần
                if (j % numberDownCheck == 0)
                {
                    if (colors[j].a != 0 && !colors[j].Equals(colorsDefaultCanvas[i]))
                    {
                        counts[i]++;
                    }
                }
            }

           // Debug.LogError("count " + i + " : " + counts[i]);
        }

        int dem = 0;
        for(int i =0; i< counts.Length; i++)
        {
            dem += counts[i];
            
        }
        //Debug.LogError("dem: " + dem);
        //Debug.LogError("pecent: " + (dem * 1.0f) / (maxPixel * 1.0f / numberDownCheck));
        if ((dem * 1.0f) / (maxPixel * 1.0f / numberDownCheck) > percentDrawComplete)
        {
            result = true;
           // Debug.LogError("complete draw: " + (dem * 1.0f) / (maxPixel * 1.0f / numberDownCheck));
           if(!drawDone)
            {
                drawDone = true;
               

                //if(GameManager.instance.showGlow)
                //{
                //    if (!PlayerPrefs.HasKey("coloring_glow_" + AppData.namePageIconDrawTutorial))
                //    {
                //        PlayerPrefs.SetInt("coloring_glow_" + AppData.namePageIconDrawTutorial, 1);
                //        PlayerPrefs.SetInt(MyConstant.keyTotalColoringGlow, PlayerPrefs.GetInt(MyConstant.keyTotalColoringGlow) + 1);
                //        BonBonAnalytics.GetInstance().LogEvent("coloring_glow_total_done_" + PlayerPrefs.GetInt(MyConstant.keyTotalColoringGlow));
                //    }
                //}
                //else
                //{
                //    if (!PlayerPrefs.HasKey("coloring_normal_" + AppData.namePageIconDrawTutorial))
                //    {
                //        PlayerPrefs.SetInt("coloring_normal_" + AppData.namePageIconDrawTutorial, 1);
                //        PlayerPrefs.SetInt(MyConstant.keyTotalColoringNormal, PlayerPrefs.GetInt(MyConstant.keyTotalColoringNormal) + 1);
                //        BonBonAnalytics.GetInstance().LogEvent("coloring_normal_total_done_" + PlayerPrefs.GetInt(MyConstant.keyTotalColoringNormal));
                //    }
                //}

                PlayerPrefs.SetInt("coloring_done_" + canvasControllers[0].PageConfig.name, 1);
               // PlayerPrefs.SetInt(MyConstant.keyTotalColoringNormal, PlayerPrefs.GetInt(MyConstant.keyTotalColoringNormal) + 1);
               // BonBonAnalytics.GetInstance().LogEvent("coloring_done_" + canvasControllers[0].PageConfig.name);
            }
        }
        ClearTextures();
        return result;
    }

    void ClearTextures()
    {
        for(int i=0;i<texture2Ds.Count;i++)
        {
           Texture2D.DestroyImmediate( texture2Ds[i] , true);
        }

        texture2Ds.Clear();
    }    
}
