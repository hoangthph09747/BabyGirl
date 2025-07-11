using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TakeScreenshot : MonoBehaviour
{
    public static TakeScreenshot instance;
    public Transform posBottomLeft,  posTopRight;
    Vector2 posScreen1;
    Vector2 posScreen3;
    [SerializeField]
    Camera camera;
    [SerializeField]
    int antiAliasing = 2;

    private void Awake()
    {
        if (instance == null)
            //if not, set instance to this
            instance = this;
        //If instance already exists and it's not this:
        else if (instance != this)
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnDestroy()
    {
        Destroy(textureScreen);
        Destroy(textureScreenThumb);
        Destroy(rt);
        instance = null;
    }

    public Texture2D textureScreen, textureScreenThumb;
    RenderTexture rt;
    public IEnumerator TakeSnapshot(RawImage imageScreenshot)
    {
        //camera.gameObject.SetActive(true);
        camera.depth = 0;
        yield return new WaitForEndOfFrame();
        rt = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32);
        rt.antiAliasing = antiAliasing;


        //camera.targetTexture = rt;
        //RenderTexture.active = rt;
        //camera.clearFlags = CameraClearFlags.SolidColor;

        //// Simple: use a clear background
        camera.backgroundColor = Color.clear;
        camera.Render();

        if (posBottomLeft != null && posTopRight != null)
        {
            posScreen1 = camera.WorldToScreenPoint(posBottomLeft.position);
            posScreen3 = camera.WorldToScreenPoint(posTopRight.position);

            if (textureScreen == null)
                textureScreen = new Texture2D((int)(posScreen3.x - posScreen1.x), (int)(posScreen3.y - posScreen1.y),
                    TextureFormat.ARGB32, false);

            textureScreen.ReadPixels(new Rect(posScreen1.x, posScreen1.y, (int)(posScreen3.x - posScreen1.x), (int)(posScreen3.y - posScreen1.y)), 0, 0, false);
        }
        else
        {
            if (textureScreen == null)
                textureScreen = new Texture2D(Screen.width, Screen.height,
                    TextureFormat.RGB24, false);
            textureScreen.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        }

        // textureScreen.LoadRawTextureData(textureScreen.GetRawTextureData());
        textureScreen.Apply();
        if(imageScreenshot != null)
        imageScreenshot.texture = textureScreen;

        if (textureScreenThumb == null)
            textureScreenThumb = new Texture2D((int)(textureScreen.width), (int)(textureScreen.height),
                TextureFormat.ARGB32, false);

        if (textureScreenThumb.width < textureScreen.width)
        {
            textureScreenThumb.Reinitialize(textureScreen.width, textureScreen.height);
        }

        textureScreenThumb.SetPixels(textureScreen.GetPixels());
        textureScreenThumb.Apply();

        SaveSystem.scale(textureScreenThumb, (int)(textureScreen.width * 0.5f), (int)(textureScreen.height * 0.5f));

        camera.depth = -2;
    }
    // Chuyển đổi từ toạ độ UI (Canvas) sang toạ độ màn hình
    public Vector2 ConvertCanvasToScreenPoint(RectTransform uiElement, Camera uiCamera)
    {
        // Lấy vị trí trong thế giới của đối tượng UI
        Vector3 worldPosition = uiElement.position;

        // Chuyển vị trí thế giới ra tọa độ màn hình
        Vector2 screenPosition = RectTransformUtility.WorldToScreenPoint(uiCamera, worldPosition);

        return screenPosition;
    }
    public IEnumerator TakeSnapshot(string pathSaveFolder, string nameFile)
    {
        yield return new WaitForEndOfFrame();

        if (posBottomLeft != null && posTopRight != null)
        {
            posScreen1 = camera.WorldToScreenPoint(posBottomLeft.position);
            posScreen3 = camera.WorldToScreenPoint(posTopRight.position);

            if(posBottomLeft.GetComponent<RectTransform>() != null)
            {
                posScreen1 = ConvertCanvasToScreenPoint(posBottomLeft.GetComponent<RectTransform>(), camera);
            }

            if (posTopRight.GetComponent<RectTransform>() != null)
            {
                posScreen3 = ConvertCanvasToScreenPoint(posTopRight.GetComponent<RectTransform>(), camera);
            }
            int w = Mathf.Abs( (int)(posScreen3.x - posScreen1.x));
            int h = Mathf.Abs( (int)(posScreen3.y - posScreen1.y));
            if (textureScreen == null)
                textureScreen = new Texture2D(w, h,
                    TextureFormat.RGB24, false);
            textureScreen.ReadPixels(new Rect(posScreen1.x, posScreen1.y, w, h), 0, 0);
        }
        else
        {
            if (textureScreen == null)
                textureScreen = new Texture2D(Screen.width, Screen.height,
                    TextureFormat.RGB24, false);
            textureScreen.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        }    
        // textureScreen.LoadRawTextureData(textureScreen.GetRawTextureData());
        textureScreen.Apply();

        if (textureScreenThumb == null)
            textureScreenThumb = new Texture2D((int)(textureScreen.width), (int)(textureScreen.height),
                TextureFormat.RGBA32, false);

        if (textureScreenThumb.width < textureScreen.width)
        {
            textureScreenThumb.Reinitialize(textureScreen.width, textureScreen.height);
        }

        textureScreenThumb.SetPixels(textureScreen.GetPixels());
        textureScreenThumb.Apply();

       // SaveSystem.scale(textureScreenThumb, (int)(textureScreen.width * 0.5f), (int)(textureScreen.height * 0.5f));

        SaveSystem.SaveTexture(textureScreen.EncodeToJPG(), pathSaveFolder, nameFile);
    }

    public IEnumerator TakeSnapshotThumb(string pathSaveFolder, string nameFile)
    {
        yield return new WaitForEndOfFrame();

        if (posBottomLeft != null && posTopRight != null)
        {
            posScreen1 = camera.WorldToScreenPoint(posBottomLeft.position);
            posScreen3 = camera.WorldToScreenPoint(posTopRight.position);

            if (posBottomLeft.GetComponent<RectTransform>() != null)
            {
                posScreen1 = ConvertCanvasToScreenPoint(posBottomLeft.GetComponent<RectTransform>(), camera);
            }

            if (posTopRight.GetComponent<RectTransform>() != null)
            {
                posScreen3 = ConvertCanvasToScreenPoint(posTopRight.GetComponent<RectTransform>(), camera);
            }
            int w = Mathf.Abs((int)(posScreen3.x - posScreen1.x));
            int h = Mathf.Abs((int)(posScreen3.y - posScreen1.y));
            if (textureScreen == null)
                textureScreen = new Texture2D(w, h,
                    TextureFormat.RGB24, false);
            textureScreen.ReadPixels(new Rect(posScreen1.x, posScreen1.y, w, h), 0, 0);
        }
        else
        {
            if (textureScreen == null)
                textureScreen = new Texture2D(Screen.width, Screen.height,
                    TextureFormat.RGB24, false);
            textureScreen.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        }
        // textureScreen.LoadRawTextureData(textureScreen.GetRawTextureData());
        textureScreen.Apply();

        if (textureScreenThumb == null)
            textureScreenThumb = new Texture2D((int)(textureScreen.width), (int)(textureScreen.height),
                TextureFormat.RGB24, false);

        if (textureScreenThumb.width < textureScreen.width)
        {
            textureScreenThumb.Reinitialize(textureScreen.width, textureScreen.height);
        }

        textureScreenThumb.SetPixels(textureScreen.GetPixels());
        textureScreenThumb.Apply();

        SaveSystem.scale(textureScreenThumb, (int)(textureScreen.width * 0.3f), (int)(textureScreen.height * 0.3f));

        SaveSystem.SaveTexture(textureScreenThumb.EncodeToJPG(), pathSaveFolder, nameFile);
    }

    public IEnumerator TakeSnapshotThumbAndScreenshot(string pathSaveFolderThumb, string pathSaveFolderScreenshot, string nameFile, bool showToat = true)
    {
        yield return new WaitForEndOfFrame();

        if (posBottomLeft != null && posTopRight != null)
        {
            posScreen1 = camera.WorldToScreenPoint(posBottomLeft.position);
            posScreen3 = camera.WorldToScreenPoint(posTopRight.position);

            if (posBottomLeft.GetComponent<RectTransform>() != null)
            {
                posScreen1 = ConvertCanvasToScreenPoint(posBottomLeft.GetComponent<RectTransform>(), camera);
            }

            if (posTopRight.GetComponent<RectTransform>() != null)
            {
                posScreen3 = ConvertCanvasToScreenPoint(posTopRight.GetComponent<RectTransform>(), camera);
            }
            int w = Mathf.Abs((int)(posScreen3.x - posScreen1.x));
            int h = Mathf.Abs((int)(posScreen3.y - posScreen1.y));
            if (textureScreen == null)
                textureScreen = new Texture2D(w, h,
                    TextureFormat.RGB24, false);
            textureScreen.ReadPixels(new Rect(posScreen1.x, posScreen1.y, w, h), 0, 0);
        }
        else
        {
            if (textureScreen == null)
                textureScreen = new Texture2D(Screen.width, Screen.height,
                    TextureFormat.RGB24, false);
            textureScreen.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        }
        // textureScreen.LoadRawTextureData(textureScreen.GetRawTextureData());
        textureScreen.Apply();

        if (textureScreenThumb == null)
            textureScreenThumb = new Texture2D((int)(textureScreen.width), (int)(textureScreen.height),
                TextureFormat.RGB24, false);

        if (textureScreenThumb.width < textureScreen.width)
        {
            textureScreenThumb.Reinitialize(textureScreen.width, textureScreen.height);
        }

        textureScreenThumb.SetPixels(textureScreen.GetPixels());
        textureScreenThumb.Apply();

        SaveSystem.scale(textureScreenThumb, (int)(textureScreen.width * 0.3f), (int)(textureScreen.height * 0.3f));
        SaveSystem.SaveTexture(textureScreen.EncodeToJPG(), pathSaveFolderScreenshot, nameFile);
        SaveSystem.SaveTexture(textureScreenThumb.EncodeToJPG(), pathSaveFolderThumb, nameFile);
        if (showToat)
        {
            yield return new WaitForSeconds(1);
            PopupManager.Instance.ShowToat("save success!");
        }
    }

    public void CreateTextureScreenshot(string pathSaveFolder, string nameFile)
    {
        StartCoroutine(TakeSnapshot(pathSaveFolder, nameFile));
    }

    public void CreateTextureThumb(string pathSaveFolder, string nameFile)
    {
        StartCoroutine(TakeSnapshotThumb(pathSaveFolder, nameFile));
    }

    public void SavePicture_ThumbAndScreenshot(string pathSaveFolderThumb, string pathSaveFolderScreenshot, string nameFile, bool showToat = true)
    {
        StartCoroutine(TakeSnapshotThumbAndScreenshot(pathSaveFolderThumb, pathSaveFolderScreenshot, nameFile, showToat));
    }

    void SaveScreenshotCake()
    {
        //int numberScreenshotCake = PlayerPrefs.GetInt(CakeConstant.keyNumberScreenshotCake, 0);
        //SaveSystem.SaveTexture(textureScreen.EncodeToJPG(), SaveSystem.SAVE_FOLDER_CAKE_Screenshot, numberScreenshotCake.ToString() + ".jpg");
        //SaveSystem.SaveTexture(textureScreenThumb.EncodeToJPG(), SaveSystem.SAVE_FOLDER_CAKE_Thumb, numberScreenshotCake.ToString() + ".jpg");
        //PlayerPrefs.SetInt(CakeConstant.keyNumberScreenshotCake, numberScreenshotCake + 1);
    }    

    public void StartTakeScreenshotCake(RawImage rawImage)
    {
        StartCoroutine(TakeSnapshot(rawImage));
        Invoke(nameof(SaveScreenshotCake), 0.5f);
    } 
    
    public void StartTakeScreenshot(RawImage rawImage)
    {
        StartCoroutine(TakeSnapshot(rawImage));
    }    
}
