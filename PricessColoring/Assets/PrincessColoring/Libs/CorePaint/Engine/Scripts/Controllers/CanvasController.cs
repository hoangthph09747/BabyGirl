using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using PaintCraft.Utils;
using PaintCraft.Canvas;
using PaintCraft.Canvas.Configs;
using PaintCraft;
using System;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

namespace PaintCraft.Controllers
{

	public class CanvasController : MonoBehaviour
	{
		public float Width { get; private set; }

		public float Height { get; private set; }

		public Vector2 Size
		{
			get
			{
				return new Vector2((float)Width, (float)Height);
			}
		}


		public static CanvasController instance;
        private Scene scene;
        [Header("Camera Offset")] public Vector3 offsetMobile;
        public Vector3 offsetTablet;

		public float CamMaxZoomInPercent = 500.0f;

		public PageConfig PageConfig;

		[HideInInspector]
		public Texture2D OutlineTexture;
		[HideInInspector]
		public Texture2D RegionTexture;
		public float OutlineLayerOffset = -25.0f;

		[HideInInspector]
		public BackLayerController BackLayerController;
		[HideInInspector]
		public OutlineLayerController OutlineLayerController;

		[HideInInspector]
		public Material BackLayerMaterial;
		public CanvasCameraController CanvasCameraController;
		public float BackLayerOffset = 100.0f;

		[HideInInspector]
		public Material OutlineMaterial;
		public float BrushOffset = 50.0f;
		public int BrushLayerId = 9;
		public int TempRenderLayerId = 10;
		public UndoManager UndoManager;

		public int HistorySize = 10;

		public int PreviewIconWidth = 440;
		public int PreviewIconHeight = 330;

		public Color DefaultBGColor = Color.white;

		public Vector2 RenderTextureSize { get; private set; }

		public bool ForceClearOnStart = false;

		public int InputTresholdMargin = 300;
		//terminate line if input is farther than 100 pixel from the edge

		public Bounds inputBounds;

		public Action<PageConfig> OnPageChange;

		[NonSerialized]
		public bool hasOutline = false;

		public TypePen typePen;
		public int layerNormal = 25;
		public int layerGiltter = 26;

		void Awake()
		{
			if (instance == null)
			{
				instance = this;
			}

			//if(LoadSceneManager.Instance.nameMinigame != NameMinigame.DIY)
			if (AppData.SelectedPageConfig != null)
			{
				PageConfig = AppData.SelectedPageConfig;
			}

			if (PageConfig == null)
			{
				Debug.LogError("You have to provide page config for this component or set AppData.SelectedPageConfig", gameObject);
				return;
			}

			// if (DrawPictureController.Instance.Aspect is >= 1.33f and <= 1.7f)
			// 	transform.position = offsetTablet;
			// else
			// 	transform.position = offsetMobile;
			
			//ngocdu comment ẩn cái này để tránh nó reset lại vị trí canvas sẽ sửa sau
			//transform.position = DrawPictureController.Instance.Aspect is >= 1.33f and <= 1.45f
			//	? offsetTablet
			//	: offsetMobile;
			
            scene = SceneManager.GetActiveScene();
			//SetActivePageConfig(PageConfig);
		}


		public void SetActivePageConfig(PageConfig pageConfig)
		{
			PageConfig = pageConfig;

			Width = PageConfig.GetSize().x;
			Height = PageConfig.GetSize().y;
			inputBounds = new Bounds(transform.position, new Vector3(Width + InputTresholdMargin * 2, Height + InputTresholdMargin * 2, 1));


			if (Application.platform == RuntimePlatform.WSAPlayerARM || Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.WSAPlayerARM ||
				Application.platform == RuntimePlatform.IPhonePlayer)
			{
				//Debug.LogFormat("Scren size: {0} {1}", Screen.width, Screen.height);
				float maxWidth = Mathf.Max(Screen.width, Screen.height);
				if (maxWidth < (float)Width)
				{
					Debug.Log("im on slow mobile. Limit canvas size to the screen size");
					float maxHeight = maxWidth * (float)Height / (float)Width;
					RenderTextureSize = new Vector2(maxWidth, maxHeight);
					HistorySize = 5;
				}
				else
				{
					RenderTextureSize = new Vector2(Width, Height);
				}
			}
			else
			{
				RenderTextureSize = new Vector2(Width, Height);
			}

            //if (tmpTexture2D == null)
            //{
            //    tmpTexture2D = new Texture2D((int)RenderTextureSize.x, (int)RenderTextureSize.y, TextureFormat.RGB24, false);
            //}
            //else
            //{
            //    tmpTexture2D.Reinitialize((int)RenderTextureSize.x, (int)RenderTextureSize.y);
            //    tmpTexture2D.Apply();
            //}

            //sửa màu nhũ trên samsung s23 không được thay hàm tmpTexture2D.Resize bằng tmpTexture2D.Reinitialize
            if (tmpTexture2D == null)
            {
                tmpTexture2D = new Texture2D((int)RenderTextureSize.x, (int)RenderTextureSize.y, TextureFormat.RGB24, false);
            }
            else
            {
# if UNITY_EDITOR
				tmpTexture2D.Reinitialize((int)RenderTextureSize.x, (int)RenderTextureSize.y);
#else
				tmpTexture2D.Resize((int)RenderTextureSize.x, (int)RenderTextureSize.y);
#endif
                tmpTexture2D.Apply();
            }


            if (typeof(AdvancedPageConfig).IsAssignableFrom(PageConfig.GetType()))
			{
				OutlineTexture = (PageConfig as AdvancedPageConfig).OutlineTexture;
				RegionTexture = (PageConfig as AdvancedPageConfig).RegionTexture;
				hasOutline = OutlineTexture != null;
			}
			else
			{
				hasOutline = false;
				OutlineTexture = null;
				RegionTexture = null;
			}


			if (BackLayerController == null)
			{
				BackLayerController = GOUtil.CreateGameObject<BackLayerController>("BackLayer", gameObject, BackLayerOffset);
				BackLayerController.Init(this);
				//BackLayerController.gameObject.layer = 0;

				//ngocdu set layer backlayer
				if (typePen == TypePen.Glow)
				{
					//BackLayerController.gameObject.layer = layerGlow;
				}
				else if (typePen == TypePen.Giltter)
				{
					BackLayerController.gameObject.layer = layerGiltter;
				}
				else if (typePen == TypePen.Color)
				{
					BackLayerController.gameObject.layer = layerNormal;
				}
			}
			else
			{
				BackLayerController.SetNewSize();
			}

			if (hasOutline)
			{
				if (OutlineLayerController == null)
				{
					//Assert.IsNotNull(OutlineMaterial, "outline material must be set");
					OutlineLayerController = GOUtil.CreateGameObject<OutlineLayerController>("Outline", gameObject, OutlineLayerOffset);
					//OutlineLayerController.gameObject.layer = 0;
					if (typePen == TypePen.Color)
					{
						OutlineLayerController.gameObject.layer = layerNormal;
					}
					else if (typePen == TypePen.Giltter)
					{
						OutlineLayerController.gameObject.layer = layerGiltter;
					}
					OutlineLayerController.Init(this);
				}
				else
				{
					OutlineLayerController.gameObject.SetActive(true);
					OutlineLayerController.SetNewSize();
				}
			}
			else
			{
				if (OutlineLayerController != null)
				{
					OutlineLayerController.gameObject.SetActive(false);
				}
			}


			if (CanvasCameraController.Initialized == false)
			{
				CanvasCameraController.Init(this);
			}
			else
			{
				CanvasCameraController.SetNewSize();
			}

			if (UndoManager == null)
			{
				UndoManager = new UndoManager(this, HistorySize);
			}
			else
			{
				UndoManager.Reinit(RenderTextureSize);
			}

			//ngocdu
			Invoke(nameof(LoadFromDiskOrClear), 0.5f);
			// LoadFromDiskOrClear();
			if (OnPageChange != null)
			{
				OnPageChange.Invoke(pageConfig);
			}
		}

		//ngocdu clear history
		public void ClearHistory()
        {
			foreach(SnapshotData snapshotData in UndoManager.SnapshotPool.GetSnapshotData())
            {
				snapshotData.RenderTexture.Release();
            }				

		}

		IEnumerator Start()
		{
			if (ForceClearOnStart)
			{
				yield return null;
				ClearCanvas();
			}
		}

		public void ClearCanvas()
		{
			CanvasCameraController.ClearRenderTexture();
		}

	
		public void ClearCanvasAndSavePicture()
		{
			//CanvasCameraController.ClearRenderTextureWithAction(() => SaveChangesToDisk());
		}

		public void Undo()
		{
            UndoManager.Undo();
        }

		public void Redo()
		{
            UndoManager.Redo();
        }

		string SaveDirectory
		{
			get
			{
				string dir = Path.Combine(Application.persistentDataPath, "Saves");
				if (!Directory.Exists(dir))
				{
					Directory.CreateDirectory(dir);
				}
				return dir;
			}
		}

		string SaveFilePath
		{
			get
			{
				//return Path.Combine(SaveDirectory, PageConfig.UniqueId + ".jpg");

				//ngocdu comment
				//return Path.Combine(SaveDirectory, PageConfig.UniqueId + typePen.ToString() + ".png");
				//new code
				//return Path.Combine(SaveDirectory, PageConfig.name + typePen.ToString() + ".png");
				return Path.Combine(SaveDirectory, PageConfig.name + "_" + typePen.ToString()  + ".png");
			}
		}

		string SaveFilePathWithIdImage(string idImage)
		{
			//get
			{
				//ngocdu comment
				//return Path.Combine(SaveDirectory, PageConfig.UniqueId + typePen.ToString() + ".png");
				//new code
				return Path.Combine(SaveDirectory, idImage + "_" + typePen.ToString() + ".png");
			}
		}

		Texture2D tmpTexture2D;

		public Texture2D TmpTexture2D
		{
			get
			{
				return tmpTexture2D;
			}
		}

		Texture2D tmpTextureIcon2D;

		public Texture2D TmpTextureIcon2D
		{
			get
			{
				if (tmpTextureIcon2D == null)
				{
					tmpTextureIcon2D = new Texture2D(PreviewIconWidth
						, PreviewIconHeight, TextureFormat.RGB24, false);
				}
				return tmpTextureIcon2D;
			}
		}

		public void SaveChangesToDisk()
		{
			StartCoroutine(DoSaveChangesToDisk());
		}


        IEnumerator DoSaveChangesToDisk()
        {

            yield return new WaitForEndOfFrame();
			//RenderTexture tmp = RenderTexture.active;
			//RenderTexture.active = BackLayerController.RenderTexture;

			//TmpTexture2D.ReadPixels(new Rect(0, 0, BackLayerController.RenderTexture.width, BackLayerController.RenderTexture.height), 0, 0, false);

			//yield return new WaitForSeconds(1f);

			//File.WriteAllBytes(SaveFilePath, TmpTexture2D.EncodeToJPG(100));

			//RenderTexture downscaledRT = RenderTexture.GetTemporary(440, 330);
			//Graphics.Blit(CanvasCameraController.Camera.targetTexture, downscaledRT);
			//RenderTexture.active = downscaledRT;

			//TmpTextureIcon2D.ReadPixels(new Rect(0, 0, 440, 330), 0, 0, false);

			//File.WriteAllBytes(PageConfig.IconSavePath, TmpTextureIcon2D.EncodeToJPG(100));
			//RenderTexture.ReleaseTemporary(downscaledRT);
			//RenderTexture.active = tmp;

			//ngocdu
			Texture mainTexture = BackLayerController.GetComponent<MeshRenderer>().material.mainTexture;
			Texture2D texture2D = new Texture2D(mainTexture.width, mainTexture.height, TextureFormat.RGBA32, false);

			RenderTexture currentRT = RenderTexture.active;

			RenderTexture renderTexture = new RenderTexture(mainTexture.width, mainTexture.height, 32);
			Graphics.Blit(mainTexture, renderTexture);

			RenderTexture.active = renderTexture;
			texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
			texture2D.Apply();
			Debug.LogError("path save: " + SaveFilePath);
			//  Color[] pixels = texture2D.GetPixels();
			File.WriteAllBytes(SaveFilePath, texture2D.EncodeToPNG());
			RenderTexture.active = currentRT;
		}

        //public bool LoadFromDiskOrClear()
        //{
            //if (File.Exists(SaveFilePath) && !string.IsNullOrEmpty(PageConfig.name))
            //{
            //	if (TmpTexture2D.LoadImage(File.ReadAllBytes(SaveFilePath)))
            //	{
            //		CanvasCameraController.Camera.targetTexture.DiscardContents();
            //		Graphics.Blit(TmpTexture2D, CanvasCameraController.Camera.targetTexture);
            //		return true;
            //	}
            //}
            //ClearCanvas();
            //return false;
        //}

		public bool LoadFromDiskOrClear()
		{
			if (File.Exists(SaveFilePathWithIdImage(PageConfig.UniqueId)) && !string.IsNullOrEmpty(PageConfig.name))
			{
				//Debug.LogError("load from disk");
				//Debug.LogError(SaveFilePathWithIdImage(PageConfig.UniqueId));
				if (TmpTexture2D.LoadImage(File.ReadAllBytes(SaveFilePathWithIdImage(PageConfig.UniqueId))))
				{
					CanvasCameraController.Camera.targetTexture.DiscardContents();
					Graphics.Blit(TmpTexture2D, CanvasCameraController.Camera.targetTexture);
					//Debug.LogError("LoadFromDiskOrClear 0");
					Debug.LogError("path load: " + SaveFilePath);
					Destroy(TmpTexture2D);
					return true;
				}
			}
			// if (GameManager.Instance.isEditPicture == false)
			// {
			// 	if (File.Exists(SaveFilePath) && !string.IsNullOrEmpty(PageConfig.name))
			// 	{
			// 		if (TmpTexture2D.LoadImage(File.ReadAllBytes(SaveFilePath)))
			// 		{
			// 			CanvasCameraController.Camera.targetTexture.DiscardContents();
			// 			Graphics.Blit(TmpTexture2D, CanvasCameraController.Camera.targetTexture);
			// 			//Debug.LogError("LoadFromDiskOrClear 0");
			// 			//Debug.LogError("path load: " + SaveFilePath);
			// 			Destroy(TmpTexture2D);
			// 			return true;
			// 		}
			// 	}
			// 	//  ClearCanvas();
			// 	//Debug.LogError("LoadFromDiskOrClear 1");
			// }
			// else
			// {
			// 	Debug.LogError("0---------:" + SaveFilePathWithIdImage(GameManager.Instance.imageDetailEditting.ii));
			// 	if (File.Exists(SaveFilePathWithIdImage(GameManager.Instance.imageDetailEditting.ii)) && !string.IsNullOrEmpty(PageConfig.name))
			// 	{
			// 		Debug.LogError(SaveFilePathWithIdImage(GameManager.Instance.imageDetailEditting.ii));
			// 		if (TmpTexture2D.LoadImage(File.ReadAllBytes(SaveFilePathWithIdImage(GameManager.Instance.imageDetailEditting.ii))))
			// 		{
			// 			CanvasCameraController.Camera.targetTexture.DiscardContents();
			// 			Graphics.Blit(TmpTexture2D, CanvasCameraController.Camera.targetTexture);
			// 			//Debug.LogError("LoadFromDiskOrClear 0");
			// 			//Debug.LogError("path load: " + SaveFilePath);
			// 			Destroy(TmpTexture2D);
			// 			return true;
			// 		}
			// 	}
			// }

			return false;
		}

		void Update()
		{
			HandleChangeScreenSize();
		}

		int oldWIdth = -1, oldHeight = -1;

		void HandleChangeScreenSize()
		{
			if (Screen.width != oldWIdth || Screen.height != oldHeight)
			{
				oldWIdth = Screen.width;
				oldHeight = Screen.height;
				ClearCanvas();
				//LoadFromDiskOrClear();
			}
		}

		public bool isCoordWithinRect(Vector3 position)
		{
			position.z = transform.position.z;
			return inputBounds.Contains(position);

		}

		private void OnDestroy()
		{
			instance = null;
		}
	}

	public enum CanvasSizeType
	{
		ScreenSize,
		FixedSize,
		OutlineImageSize
	}
}
