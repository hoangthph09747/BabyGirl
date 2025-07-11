﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public static class SaveSystem
{
    public static readonly string SAVE_FOLDER = Application.persistentDataPath + "/Saves";
    public static readonly string SAVE_FOLDER_CAKE_Screenshot = Application.persistentDataPath + "/Cake_Screenshot";
    public static readonly string SAVE_FOLDER_CAKE_Thumb = Application.persistentDataPath + "/Cake_Thumb";

    public static readonly string SAVE_FOLDER_HappyBirthday_Screenshot = Application.persistentDataPath + "/HappyBirthday_Screenshot";
    public static readonly string SAVE_FOLDER_HappyBirthday_Thumb = Application.persistentDataPath + "/HappyBirthday_Thumb";

    public static readonly string SAVE_FOLDER_DIY_Screenshot = Application.persistentDataPath + "/DIY_Screenshot";
    public static readonly string SAVE_FOLDER_DIY_Thumb = Application.persistentDataPath + "/DIY_Thumb";

    public static readonly string SAVE_FOLDER_PrincessColoring_Screenshot = Application.persistentDataPath + "/PrincessColoring_Screenshot";
    public static readonly string SAVE_FOLDER_PrincessColoring_Thumb = Application.persistentDataPath + "/PrincessColoring_Thumb";
    public static readonly string SAVE_FOLDER_PrincessColoring_Gallery = Application.persistentDataPath + "/PrincessColoring_Gallery";

    public static readonly string SAVE_FOLDER_ColoringASMR_Screenshot = Application.persistentDataPath + "/ColoringASMR_Screenshot";
    public static readonly string SAVE_FOLDER_ColoringASMR_Thumb = Application.persistentDataPath + "/ColoringASMR_Thumb";
    public static readonly string SAVE_FOLDER_ColoringASMR_Gallery = Application.persistentDataPath + "/ColoringASMR_Gallery";

    public static readonly string SAVE_FOLDER_Makeup_Screenshot = Application.persistentDataPath + "/Makeup_Screenshot";
    public static readonly string SAVE_FOLDER_Make_Thumb = Application.persistentDataPath + "/Makeup_Thumb";

     public static readonly string SAVE_FOLDER_PETMakeUp_Screenshot = Application.persistentDataPath + "/PetMakeUp_Screenshot";
    public static readonly string SAVE_FOLDER_PETMakeUp_Thumb = Application.persistentDataPath + "/PetMakeUp_Thumb";

    public static readonly string SAVE_FOLDER_Tailor_Screenshot = Application.persistentDataPath + "/Tailor_Screenshot";
    public static readonly string SAVE_FOLDER_Tailor_Thumb = Application.persistentDataPath + "/Tailor_Thumb";

    public static readonly string SAVE_FOLDER_HomeDecor_Screenshot = Application.persistentDataPath + "/HomeDecor_Screenshot";
    public static readonly string SAVE_FOLDER_HomeDecor_Thumb = Application.persistentDataPath + "/HomeDecor_Thumb";

    public static readonly string SAVE_FOLDER_PrincessDrawLineThumbs = Application.persistentDataPath + "/PrincessDrawLineThumbs";

     public static readonly string SAVE_FOLDER_MakeUpKit_Screenshot = Application.persistentDataPath + "/MakeUpKit_Screenshot";
    public static readonly string SAVE_FOLDER_MakeUpKit_Thumb = Application.persistentDataPath + "/MakeUpKit_Thumb";
    public static readonly string SAVE_FOLDER_MakeUpKit_Board = Application.persistentDataPath + "/MakeUpKit_Board";

    public static readonly string SAVE_FOLDER_ColorNumber86_Screenshot = Application.persistentDataPath + "/ColorNumber86_Screenshot";
    public static readonly string SAVE_FOLDER_ColorNumber86_ThumbMenu = Application.persistentDataPath + "/ColorNumber86_ThumbMenu";
    public static readonly string SAVE_FOLDER_ColorNumber86_ThumbGallery = Application.persistentDataPath + "/ColorNumber86_ThumbGallery";

    public static readonly string SAVE_FOLDER_DressupPK_Screenshot = Application.persistentDataPath + "/DressupPK_Screenshot";
    public static readonly string SAVE_FOLDER_DressupPK_Thumb = Application.persistentDataPath + "/DressupPK_Thumb";
    public static readonly string SAVE_FOLDER_DressupPK_Data = Application.persistentDataPath + "/DressupPK_Data";
    public static void Init()
    {
        if (!Directory.Exists(SAVE_FOLDER))
        {
            Directory.CreateDirectory(SAVE_FOLDER);
        }

        if (!Directory.Exists(SAVE_FOLDER_CAKE_Screenshot))
        {
            Directory.CreateDirectory(SAVE_FOLDER_CAKE_Screenshot);
        }

        if (!Directory.Exists(SAVE_FOLDER_CAKE_Thumb))
        {
            Directory.CreateDirectory(SAVE_FOLDER_CAKE_Thumb);
        }

        if (!Directory.Exists(SAVE_FOLDER_PrincessColoring_Screenshot))
        {
            Directory.CreateDirectory(SAVE_FOLDER_PrincessColoring_Screenshot);
        }

        if (!Directory.Exists(SAVE_FOLDER_PrincessColoring_Thumb))
        {
            Directory.CreateDirectory(SAVE_FOLDER_PrincessColoring_Thumb);
        }

        if (!Directory.Exists(SAVE_FOLDER_PrincessColoring_Gallery))
        {
            Directory.CreateDirectory(SAVE_FOLDER_PrincessColoring_Gallery);
        }

        //
        if (!Directory.Exists(SAVE_FOLDER_ColoringASMR_Screenshot))
        {
            Directory.CreateDirectory(SAVE_FOLDER_ColoringASMR_Screenshot);
        }

        if (!Directory.Exists(SAVE_FOLDER_ColoringASMR_Gallery))
        {
            Directory.CreateDirectory(SAVE_FOLDER_ColoringASMR_Gallery);
        }

        if (!Directory.Exists(SAVE_FOLDER_ColoringASMR_Thumb))
        {
            Directory.CreateDirectory(SAVE_FOLDER_ColoringASMR_Thumb);
        }

        if (!Directory.Exists(SAVE_FOLDER_Makeup_Screenshot))
        {
            Directory.CreateDirectory(SAVE_FOLDER_Makeup_Screenshot);
        }
        if (!Directory.Exists(SAVE_FOLDER_PETMakeUp_Screenshot))
        {
            Directory.CreateDirectory(SAVE_FOLDER_PETMakeUp_Screenshot);
        }

        if (!Directory.Exists(SAVE_FOLDER_PETMakeUp_Thumb))
        {
            Directory.CreateDirectory(SAVE_FOLDER_PETMakeUp_Thumb);
        }

        if (!Directory.Exists(SAVE_FOLDER_Make_Thumb))
        {
            Directory.CreateDirectory(SAVE_FOLDER_Make_Thumb);
        }

        if (!Directory.Exists(SAVE_FOLDER_Tailor_Screenshot))
        {
            Directory.CreateDirectory(SAVE_FOLDER_Tailor_Screenshot);
        }

        if (!Directory.Exists(SAVE_FOLDER_Tailor_Thumb))
        {
            Directory.CreateDirectory(SAVE_FOLDER_Tailor_Thumb);
        }

        if (!Directory.Exists(SAVE_FOLDER_HomeDecor_Screenshot))
        {
            Directory.CreateDirectory(SAVE_FOLDER_HomeDecor_Screenshot);
        }

        if (!Directory.Exists(SAVE_FOLDER_HomeDecor_Thumb))
        {
            Directory.CreateDirectory(SAVE_FOLDER_HomeDecor_Thumb);
        }

        if (!Directory.Exists(SAVE_FOLDER_HappyBirthday_Screenshot))
        {
            Directory.CreateDirectory(SAVE_FOLDER_HappyBirthday_Screenshot);
        }

        if (!Directory.Exists(SAVE_FOLDER_HappyBirthday_Thumb))
        {
            Directory.CreateDirectory(SAVE_FOLDER_HappyBirthday_Thumb);
        }

        if (!Directory.Exists(SAVE_FOLDER_DIY_Screenshot))
        {
            Directory.CreateDirectory(SAVE_FOLDER_DIY_Screenshot);
        }
        if (!Directory.Exists(SAVE_FOLDER_DIY_Thumb))
        {
            Directory.CreateDirectory(SAVE_FOLDER_DIY_Thumb);
        }

        //86
        if (!Directory.Exists(SAVE_FOLDER_ColorNumber86_Screenshot))
        {
            Directory.CreateDirectory(SAVE_FOLDER_ColorNumber86_Screenshot);
        }

        if (!Directory.Exists(SAVE_FOLDER_ColorNumber86_ThumbMenu))
        {
            Directory.CreateDirectory(SAVE_FOLDER_ColorNumber86_ThumbMenu);
        }

        if (!Directory.Exists(SAVE_FOLDER_ColorNumber86_ThumbGallery))
        {
            Directory.CreateDirectory(SAVE_FOLDER_ColorNumber86_ThumbGallery);
        }

        //67
        if (!Directory.Exists(SAVE_FOLDER_DressupPK_Screenshot))
        {
            Directory.CreateDirectory(SAVE_FOLDER_DressupPK_Screenshot);
        }

        if (!Directory.Exists(SAVE_FOLDER_DressupPK_Thumb))
        {
            Directory.CreateDirectory(SAVE_FOLDER_DressupPK_Thumb);
        }

        if (!Directory.Exists(SAVE_FOLDER_DressupPK_Data))
        {
            Directory.CreateDirectory(SAVE_FOLDER_DressupPK_Data);
        }
    }

    public static void DeleteFile(string path)
    {
        if (File.Exists(path))
        {
            // Debug.LogError("delete: " + path);
            File.Delete(path);
        }
    }

    /// <summary>
    /// lưu nội dung string vào file
    /// </summary>
    /// <param name="saveString"> nội dung cần ghi</param>
    /// <param name="pathFile"> tên đường dẫn file có cả phần mở rộng .txt hoặc .json</param>
    public static void SaveString(string saveString, string pathFile)
    {
        Init();
        File.WriteAllText(pathFile, saveString);
    }

    /// <summary>
    /// load nội dung string trong file
    /// </summary>
    /// <param name="pathFile">tên đường dẫn file có cả phần mở rộng .txt hoặc .json</param>
    /// <returns></returns>
    public static string LoadString(string pathFile)
    {
        Init();
        
        if (File.Exists(pathFile))
        {
            string saveString = File.ReadAllText(pathFile);
            return saveString;
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// lưu ảnh screenshot
    /// </summary>
    /// /// <param name="texture">ảnh cần lưu</param>
    /// <param name="filePath">đường dẫn lưu file screenshot</param>
    /// <param name="filePathThumb">đường dẫn lưu file ảnh thumb</param>
    public static void SaveScrenshot(Texture2D texture, string filePath, string filePathThumb)
    {
        Init();
        byte[] bytes;
        bytes = texture.EncodeToJPG();
        //Debug.Log(filePath);
        System.IO.File.WriteAllBytes(filePath, bytes);

        //save thumb texture
        scale(texture, (int)(texture.width * 0.3f), (int)(texture.height * 0.3f));
        byte[] bytesThumb;
        bytesThumb = texture.EncodeToJPG();
        System.IO.File.WriteAllBytes(filePathThumb, bytesThumb);

        bytes = null;
        bytesThumb = null;

        texture = null;
    }

    static string CapitalizeFirstLetterOnly(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return input;
        }

        // Chuyển chữ cái đầu thành chữ hoa và kết hợp lại với phần còn lại
        string firstLetterUpper = char.ToUpper(input[0]) + input.Substring(1);

        return firstLetterUpper;
    }



    private static Texture2D ScaleTexture(Texture2D source, int targetWidth, int targetHeight)
    {
        Texture2D result = new Texture2D(targetWidth, targetHeight, source.format, true);
        Color[] rpixels = result.GetPixels(0);
        float incX = (1.0f / (float)targetWidth);
        float incY = (1.0f / (float)targetHeight);
        for (int px = 0; px < rpixels.Length; px++)
        {
            rpixels[px] = source.GetPixelBilinear(incX * ((float)px % targetWidth), incY * ((float)Mathf.Floor(px / targetWidth)));
        }
        result.SetPixels(rpixels, 0);
        result.Apply();
        return result;
    }

    /// <summary>
    /// Returns a scaled copy of given texture. 
    /// </summary>
    /// <param name="tex">Source texure to scale</param>
    /// <param name="width">Destination texture width</param>
    /// <param name="height">Destination texture height</param>
    /// <param name="mode">Filtering mode</param>
    public static Texture2D scaled(Texture2D src, int width, int height, FilterMode mode = FilterMode.Trilinear)
    {
        Rect texR = new Rect(0, 0, width, height);
        _gpu_scale(src, width, height, mode);

        //Get rendered data back to a new texture
        Texture2D result = new Texture2D(width, height, TextureFormat.ARGB32, true);
        result.Reinitialize(width, height);
        result.ReadPixels(texR, 0, 0, true);
        return result;
    }

    /// <summary>
    /// Scales the texture data of the given texture.
    /// </summary>
    /// <param name="tex">Texure to scale</param>
    /// <param name="width">New width</param>
    /// <param name="height">New height</param>
    /// <param name="mode">Filtering mode</param>
    public static void scale(Texture2D tex, int width, int height, FilterMode mode = FilterMode.Trilinear)
    {
        Rect texR = new Rect(0, 0, width, height);
        _gpu_scale(tex, width, height, mode);

        // Update new texture
        tex.Reinitialize(width, height);
        tex.ReadPixels(texR, 0, 0, true);
        tex.Apply(true);    //Remove this if you hate us applying textures for you :)
        rtt.Release();
    }

    static RenderTexture rtt;
    static void _gpu_scale(Texture2D src, int width, int height, FilterMode fmode)
    {
        //We need the source texture in VRAM because we render with it
        src.filterMode = fmode;
        src.Apply(true);

        //Using RTT for best quality and performance. Thanks, Unity 5
        rtt = new RenderTexture(width, height, 32);

        //Set the RTT in order to render to it
        Graphics.SetRenderTarget(rtt);

        //Setup 2D matrix in range 0..1, so nobody needs to care about sized
        GL.LoadPixelMatrix(0, 1, 1, 0);

        //Then clear & draw the texture to fill the entire RTT.
        GL.Clear(true, true, new Color(0, 0, 0, 0));
        Graphics.DrawTexture(new Rect(0, 0, 1, 1), src);
    }

    public static void SaveTextureToPNG(Texture2D snap, string filePath)
    {
        Init();
        string iter = System.DateTime.UtcNow.ToLongTimeString().Replace(":", "_");
        byte[] bytes;
        bytes = snap.EncodeToPNG();
        Debug.Log(filePath);
        System.IO.File.WriteAllBytes(filePath, bytes);
    }

    public static Sprite LoadSprite(string filePath)
    {
        Init();

        Debug.Log("load: " + filePath);
        if (string.IsNullOrEmpty(filePath)) return null;
        if (System.IO.File.Exists(filePath))
        {
            //Debug.LogError("load: " + filePath);
            byte[] bytes = System.IO.File.ReadAllBytes(filePath);
            Texture2D texture = new Texture2D(1, 1);
            texture.LoadImage(bytes);
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            return sprite;
        }
        return null;
    }
    public static Sprite LoadSprite(string path, string name)
    {
        Init();
        string filePath = path;
        
        if (string.IsNullOrEmpty(filePath)) return null;
        if (File.Exists(filePath))
        {
            byte[] bytes = File.ReadAllBytes(filePath);
            Texture2D texture = new Texture2D(1, 1);
            texture.LoadImage(bytes);
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            sprite.name = name;
            return sprite;
        }
        return null;
    }

    public static Texture2D LoadTexture2D(string path)
    {
        Init();


        // Debug.Log("load: " + path);
        if (string.IsNullOrEmpty(path)) return null;
        if (System.IO.File.Exists(path))
        {
            // Debug.LogError("load: " + path);
            byte[] bytes = System.IO.File.ReadAllBytes(path);
            Texture2D texture = new Texture2D(1, 1);
            texture.LoadImage(bytes);

            return texture;
        }
        return null;
    }


    public static void SaveTexture(byte[] bytes, string pathFolder, string nameFile)
    {
        if (!Directory.Exists(pathFolder))
        {
            Directory.CreateDirectory(pathFolder);
        }

        string path = pathFolder + "/" + nameFile;
        System.IO.File.WriteAllBytes(path, bytes);
    }

    public static bool CheckExit(string path)
    {
        return File.Exists(path);
    }

 
}