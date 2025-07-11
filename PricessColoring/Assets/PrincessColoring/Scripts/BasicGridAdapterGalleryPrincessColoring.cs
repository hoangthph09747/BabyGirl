/*
 * * * * This bare-bones script was auto-generated * * * *
 * The code commented with "/ * * /" demonstrates how data is retrieved and passed to the adapter, plus other common commands. You can remove/replace it once you've got the idea
 * Complete it according to your specific use-case
 * Consult the Example scripts if you get stuck, as they provide solutions to most common scenarios
 * 
 * Main terms to understand:
 *		Model = class that contains the data associated with an item (title, content, icon etc.)
 *		Views Holder = class that contains references to your views (Text, Image, MonoBehavior, etc.)
 * 
 * Default expected UI hiererchy:
 *	  ...
 *		-Canvas
 *		  ...
 *			-MyScrollViewAdapter
 *				-Viewport
 *					-Content
 *				-Scrollbar (Optional)
 *				-ItemPrefab (Optional)
 * 
 * Note: If using Visual Studio and opening generated scripts for the first time, sometimes Intellisense (autocompletion)
 * won't work. This is a well-known bug and the solution is here: https://developercommunity.visualstudio.com/content/problem/130597/unity-intellisense-not-working-after-creating-new-1.html (or google "unity intellisense not working new script")
 * 
 * 
 * Please read the manual under "/Docs", as it contains everything you need to know in order to get started, including FAQ
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using frame8.Logic.Misc.Other.Extensions;
using Com.TheFallenGames.OSA.CustomAdapters.GridView;
using Com.TheFallenGames.OSA.DataHelpers;
using Observer;
using System.IO;
// You should modify the namespace to your own or - if you're sure there won't ever be conflicts - remove it altogether
//namespace PrincessColoring
//{
	// There is 1 important callback you need to implement, apart from Start(): UpdateCellViewsHolder()
	// See explanations below
	public class BasicGridAdapterGalleryPrincessColoring : GridAdapter<GridParams, MyGridItemViewsHolder_PrincessColoring>
	{
		// Helper that stores data and notifies the adapter when items count changes
		// Can be iterated and can also have its elements accessed by the [] operator
		public SimpleDataHelper<MyGridItemModel_PrincessColoring> Data { get; private set; }

		public NameMinigame nameMinigame;
		#region GridAdapter implementation
		protected override void Start()
		{
			Data = new SimpleDataHelper<MyGridItemModel_PrincessColoring>(this);

			// Calling this initializes internal data and prepares the adapter to handle item count changes
			base.Start();

            // Retrieve the models from your data source and set the items count

           

        }

       /* private void OnEnable()
        {
			RetrieveDataAndUpdate();
		}*/

        // This is called anytime a previously invisible item become visible, or after it's created, 
        // or when anything that requires a refresh happens
        // Here you bind the data from the model to the item's views
        // *For the method's full description check the base implementation
        protected override void UpdateCellViewsHolder(MyGridItemViewsHolder_PrincessColoring newOrRecycled)
		{
            // In this callback, "newOrRecycled.ItemIndex" is guaranteed to always reflect the
            // index of item that should be represented by this views holder. You'll use this index
            // to retrieve the model from your data set

            MyGridItemModel_PrincessColoring model = Data[newOrRecycled.ItemIndex];

			//newOrRecycled.backgroundImage.color = model.color;
			Destroy(newOrRecycled.rawImage.texture);
			newOrRecycled.rawImage.texture = null;
			//StartCoroutine(newOrRecycled.LoadImageCoroutine());
			//newOrRecycled.rawImage.texture = SaveSystem.LoadTexture2D(model.pathThumb);
            // TODO: Load texture from Resources or another method, since SaveSystem is removed.
            newOrRecycled.rawImage.texture = null;
			newOrRecycled.pathScreenshot = model.pathScreenshot;
			newOrRecycled.pathThumb = model.pathThumb;
		}

        // This is the best place to clear an item's views in order to prepare it from being recycled, but this is not always needed, 
        // especially if the views' values are being overwritten anyway. Instead, this can be used to, for example, cancel an image 
        // download request, if it's still in progress when the item goes out of the viewport.
        // <newItemIndex> will be non-negative if this item will be recycled as opposed to just being disabled
        // *For the method's full description check the base implementation

        protected override void OnBeforeRecycleOrDisableCellViewsHolder(MyGridItemViewsHolder_PrincessColoring inRecycleBinOrVisible, int newItemIndex)
        {
            base.OnBeforeRecycleOrDisableCellViewsHolder(inRecycleBinOrVisible, newItemIndex);
        }

        #endregion

        // These are common data manipulation methods
        // The list containing the models is managed by you. The adapter only manages the items' sizes and the count
        // The adapter needs to be notified of any change that occurs in the data list. 
        // For GridAdapters, only Refresh and ResetItems work for now
        #region data manipulation
        public void AddItemsAt(int index, IList<MyGridItemModel_PrincessColoring> items)
		{
			//Commented: this only works with Lists. ATM, Insert for Grids only works by manually changing the list and calling NotifyListChangedExternally() after
			//Data.InsertItems(index, items);
			Data.List.InsertRange(index, items);
			Data.NotifyListChangedExternally();
		}

		public void RemoveItemsFrom(int index, int count)
		{
			//Commented: this only works with Lists. ATM, Remove for Grids only works by manually changing the list and calling NotifyListChangedExternally() after
			//Data.RemoveRange(index, count);
			Data.List.RemoveRange(index, count);
			Data.NotifyListChangedExternally();
		}

		public void SetItems(IList<MyGridItemModel_PrincessColoring> items)
		{
			Data.ResetItems(items);
		}
		#endregion


		// Here, we're requesting <count> items from the data source
		/*void RetrieveDataAndUpdate()
		{
			StartCoroutine(FetchMoreItemsFromDataSourceAndUpdate());
		}
*/
		/*List<string> GetJpgFilesInFolder(string path)
		{
			List<string> jpgFiles = new List<string>();

			if (Directory.Exists(path))
			{
				string[] files = Directory.GetFiles(path);

				foreach (string file in files)
				{
					if (file.ToLower().EndsWith(".jpg") || file.ToLower().EndsWith(".png"))
					{
						jpgFiles.Add(file);
						Debug.Log(file);
					}
				}
			}
			else
			{
				Debug.LogError("Folder not found: " + path);
			}

			jpgFiles.Sort((a, b) =>
			{
				FileInfo fileInfoA = new FileInfo(a);
				FileInfo fileInfoB = new FileInfo(b);
				return fileInfoB.CreationTime.CompareTo(fileInfoA.CreationTime);
			});

			return jpgFiles;
		}*/

		string GetPathFolderScreenshot()
        {
			switch(nameMinigame)
            {
				//case NameMinigame.CakeGame:
				//	return SaveSystem.SAVE_FOLDER_CAKE_Screenshot;
				//	break;
				//case NameMinigame.PrincessColoring:
				//	return SaveSystem.SAVE_FOLDER_PrincessColoring_Gallery;
				//	break;
				default:
					return null;
					break;
			}				
        }

		string GetPathFolderScreenshotThumb()
		{
			switch (nameMinigame)
			{
				//case NameMinigame.CakeGame:
				//	return SaveSystem.SAVE_FOLDER_CAKE_Thumb;
				//	break;
				//case NameMinigame.PrincessColoring:
				//	return SaveSystem.SAVE_FOLDER_PrincessColoring_Thumb;
				//	break;
				default:
					return null;
					break;
			}
		}

		// Retrieving <count> models from the data source and calling OnDataRetrieved after.
		// In a real case scenario, you'd query your server, your database or whatever is your data source and call OnDataRetrieved after
		/*IEnumerator FetchMoreItemsFromDataSourceAndUpdate()
		{
			// Simulating data retrieving delay
			yield return new WaitForSeconds(.15f);

			List<string> listFileScreenshot = GetJpgFilesInFolder(GetPathFolderScreenshot());
			List<string> listFileThumb = GetJpgFilesInFolder(GetPathFolderScreenshotThumb());



			var newItems = new MyGridItemModel_PrincessColoring[listFileScreenshot.Count];

            // Retrieve your data here

            for (int i = 0; i < listFileScreenshot.Count; ++i)
            {
				var model = new MyGridItemModel_PrincessColoring()
				{
					pathThumb = listFileThumb[i],
					pathScreenshot = listFileScreenshot[i]
				};
                newItems[i] = model;
            }


            OnDataRetrieved(newItems);
		}
*/
		void OnDataRetrieved(MyGridItemModel_PrincessColoring[] newItems)
		{
			//Commented: this only works with Lists. ATM, Insert for Grids only works by manually changing the list and calling NotifyListChangedExternally() after
			// Data.InsertItemsAtEnd(newItems);

			//ngocdu 
			Data.List.Clear();

			Data.List.AddRange(newItems);
			Data.NotifyListChangedExternally();
		}
	}


	// Class containing the data associated with an item
	public class MyGridItemModel_PrincessColoring
	{
        public string pathScreenshot, pathThumb;
    }


    // This class keeps references to an item's views.
    // Your views holder should extend BaseItemViewsHolder for ListViews and CellViewsHolder for GridViews
    // The cell views holder should have a single child (usually named "Views"), which contains the actual 
    // UI elements. A cell's root is never disabled - when a cell is removed, only its "views" GameObject will be disabled
    public class MyGridItemViewsHolder_PrincessColoring : CellViewsHolder
	{

        public RawImage rawImage;
        public Image backgroundImage;
		public Button button;
		public string pathScreenshot, pathThumb;

		public IEnumerator LoadImageCoroutine()
		{
			if (string.IsNullOrEmpty(pathThumb) || !System.IO.File.Exists(pathThumb))
			{
				yield break; // Nếu đường dẫn không hợp lệ hoặc tệp không tồn tại, thoát coroutine.
			}

			// Đọc dữ liệu hình ảnh từ tệp
			byte[] bytes = System.IO.File.ReadAllBytes(pathThumb);

			// Tạo một Texture2D mới
			Texture2D texture = new Texture2D(1, 1);
			yield return null; // Đợi một frame để đảm bảo texture được tạo trên luồng chính

			// Nạp hình ảnh vào texture
			texture.LoadImage(bytes);

			// Gán texture cho RawImage
			rawImage.texture = texture;
		}

		// Retrieving the views from the item's root GameObject
		public override void CollectViews()
		{
			base.CollectViews();

            // GetComponentAtPath is a handy extension method from frame8.Logic.Misc.Other.Extensions
            // which infers the variable's component from its type, so you won't need to specify it yourself

            views.GetComponentAtPath("RawImage", out rawImage);
            views.GetComponentAtPath("BackgroundImage", out backgroundImage);
			views.GetComponentAtPath("BackgroundImage", out button);
			button.onClick.AddListener(ClickPicture);
		}

		void ClickPicture()
        {
			PopupViewPicturePrincessColoring.instance.Show(pathScreenshot, pathThumb);
        }

        // This is usually the only child of the item's root and it's called "Views". 
        // That's what the default implementation will look for, but just for flexibility, 
        // this callback is provided, in case it's named differently or there's more than 1 child 
        // *See GridExample.cs for more info

        protected override RectTransform GetViews()
        { return root.Find("Views").transform as RectTransform; }


        // Override this if you have children layout groups. They need to be marked for rebuild when this callback is fired
        /*
		public override void MarkForRebuild()
		{
			base.MarkForRebuild();

			LayoutRebuilder.MarkLayoutForRebuild(yourChildLayout1);
			LayoutRebuilder.MarkLayoutForRebuild(yourChildLayout2);
			AChildSizeFitter.enabled = true;
		}
		*/

        // Override this if you've also overridden MarkForRebuild()
        /*
		public override void UnmarkForRebuild()
		{
			AChildSizeFitter.enabled = false;

			base.UnmarkForRebuild();
		}
		*/
    }
//}
