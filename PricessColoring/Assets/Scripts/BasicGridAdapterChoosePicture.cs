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
using System.Linq;
using PaintCraft.Utils;
using PaintCraft.Canvas.Configs;
using UnityEngine.SceneManagement;
// You should modify the namespace to your own or - if you're sure there won't ever be conflicts - remove it altogether
namespace PrincessColoring
{
	// There is 1 important callback you need to implement, apart from Start(): UpdateCellViewsHolder()
	// See explanations below
	public class BasicGridAdapterChoosePicture : GridAdapter<GridParams, MyGridItemViewsHolder>
	{
		// Helper that stores data and notifies the adapter when items count changes
		// Can be iterated and can also have its elements accessed by the [] operator
		public SimpleDataHelper<MyGridItemModel> Data { get; private set; }

		string keyIndexPicturePrincess = "keyIndexPicturePrincess";
		public int indexStartServer = 10;
		 int maxPicture = 68;
		#region GridAdapter implementation
		protected override void Start()
		{
			Data = new SimpleDataHelper<MyGridItemModel>(this);

			// Calling this initializes internal data and prepares the adapter to handle item count changes
			base.Start();

            // Retrieve the models from your data source and set the items count

            RetrieveDataAndUpdate();

        }

        // This is called anytime a previously invisible item become visible, or after it's created, 
        // or when anything that requires a refresh happens
        // Here you bind the data from the model to the item's views
        // *For the method's full description check the base implementation
        protected override void UpdateCellViewsHolder(MyGridItemViewsHolder newOrRecycled)
		{
            // In this callback, "newOrRecycled.ItemIndex" is guaranteed to always reflect the
            // index of item that should be represented by this views holder. You'll use this index
            // to retrieve the model from your data set

            MyGridItemModel model = Data[newOrRecycled.ItemIndex];

			// newOrRecycled.backgroundImage.color = model.color;
			// newOrRecycled.titleText.text = model.pageConfig.name;

			if (model.pageConfig != null)
			{
				//newOrRecycled.rawImage.texture = SaveSystem.LoadTexture2D(SaveSystem.SAVE_FOLDER_PrincessColoring_Screenshot + "/" + model.pageConfig.name + ".jpg");
                // TODO: Load texture from Resources or another method, since SaveSystem is removed.
                newOrRecycled.rawImage.texture = null;
				//newOrRecycled.rawImage.texture = model.pageConfig.OutlineTexture;
				newOrRecycled.pageConfig = model.pageConfig;
				newOrRecycled.imageIconDownload.gameObject.SetActive(false);
				newOrRecycled.backgroundImage.color = Color.white;
				newOrRecycled.textLoading.gameObject.SetActive(false);
			}
			else
            {
				//if (newOrRecycled.rawImage.texture == null)
				{
					newOrRecycled.rawImage.texture = Resources.Load<Texture2D>($"IconPictures/" + (model.index + 1));
				}
				/*int check = AddressableManager.Instance.CheckLoadPage(model.index + 1);
				if (check == 1)
				{
					model.pageConfig = (AddressableManager.Instance.LoadPage(model.index + 1));
					newOrRecycled.pageConfig = model.pageConfig;
					newOrRecycled.textLoading.gameObject.SetActive(false);
				}
				//else if (check == 2)
				//{
				//	newOrRecycled.imageIconDownload.gameObject.SetActive(false);
				//	newOrRecycled.textLoading.gameObject.SetActive(true);
				//	newOrRecycled.pageConfig = null;
				//}
				else
                {
					newOrRecycled.imageIconDownload.gameObject.SetActive(true);
					newOrRecycled.backgroundImage.color = Color.gray;
					newOrRecycled.textLoading.gameObject.SetActive(false);
					newOrRecycled.pageConfig = null;
				}	*/				
			}				
		}

        // This is the best place to clear an item's views in order to prepare it from being recycled, but this is not always needed, 
        // especially if the views' values are being overwritten anyway. Instead, this can be used to, for example, cancel an image 
        // download request, if it's still in progress when the item goes out of the viewport.
        // <newItemIndex> will be non-negative if this item will be recycled as opposed to just being disabled
        // *For the method's full description check the base implementation
        /*
		protected override void OnBeforeRecycleOrDisableCellViewsHolder(MyGridItemViewsHolder inRecycleBinOrVisible, int newItemIndex)
		{
			base.OnBeforeRecycleOrDisableCellViewsHolder(inRecycleBinOrVisible, newItemIndex);
		}
		*/
        #endregion

        // These are common data manipulation methods
        // The list containing the models is managed by you. The adapter only manages the items' sizes and the count
        // The adapter needs to be notified of any change that occurs in the data list. 
        // For GridAdapters, only Refresh and ResetItems work for now
        #region data manipulation
        public void AddItemsAt(int index, IList<MyGridItemModel> items)
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

		public void SetItems(IList<MyGridItemModel> items)
		{
			Data.ResetItems(items);
		}
		#endregion


		// Here, we're requesting <count> items from the data source
		void RetrieveDataAndUpdate()
		{
			StartCoroutine(FetchMoreItemsFromDataSourceAndUpdate());
		}

		// Retrieving <count> models from the data source and calling OnDataRetrieved after.
		// In a real case scenario, you'd query your server, your database or whatever is your data source and call OnDataRetrieved after
		IEnumerator FetchMoreItemsFromDataSourceAndUpdate()
		{
			// Simulating data retrieving delay
			yield return new WaitForSeconds(.15f);

			//UnityEngine.Object[] resources = Resources.LoadAll($"PagesConfig").Where(resource => resource is ColoringPageConfig).ToArray();
			List<UnityEngine.Object> resources = Resources.LoadAll($"PagesConfig")
			.Where(resource => resource is ColoringPageConfig)
			.OrderBy(resource => int.Parse(System.IO.Path.GetFileNameWithoutExtension(resource.name)))//sắp xếp các file theo thứ tự số
			.ToList();

            //List<ColoringPageConfig> listPageServer = AddressableManager.Instance.StartLoadPageConfig();
            //for (int i = 0; i < listPageServer.Count; i++)
            //{
            //    resources.Add(listPageServer[i]);
            //}

          /*  for (int i = indexStartServer; i < maxPicture; i++)
            {
				int check = AddressableManager.Instance.CheckLoadPage(i);
				if (check == 1)
                {
					resources.Add(AddressableManager.Instance.LoadPage(i));
				}					
				else 
                {

                }					
			}*/


            List<MyGridItemModel> list = new List<MyGridItemModel>();
   //         for (int i = 0; i < resources.Count; i++)
   //         {
			//	var model = new MyGridItemModel()
			//	{
			//		pageConfig = (ColoringPageConfig)resources[i]
			//	};
			//	list.Add(model);
			//}

			for (int i = 0; i < maxPicture - 1; i++)
			{
				if (CheckHavePage(resources, i))
				{
					if (resources.Count < i - 1)
					{
						var model = new MyGridItemModel()
						{
							pageConfig = null,
							index = i
						};
						list.Add(model);
					}
					else
					{
						//var model = new MyGridItemModel()
						//{
						//	pageConfig = (ColoringPageConfig)resources[i],
						//	index = i
						//};
						//list.Add(model);

						if (i < resources.Count)
						{
							ColoringPageConfig coloringPageConfig = (ColoringPageConfig)resources[i];
							if (coloringPageConfig)
							{
								var model = new MyGridItemModel()
								{
									pageConfig = (ColoringPageConfig)resources[i],
									index = i
								};
								list.Add(model);
							}
							else
							{
								var model = new MyGridItemModel()
								{
									pageConfig = null,
									index = i
								};
								list.Add(model);
							}
						}
						else
						{
							var model = new MyGridItemModel()
							{
								pageConfig = null,
								index = i
							};
							list.Add(model);
						}
					}
					
				}
				else
                {
					var model = new MyGridItemModel()
					{
						pageConfig = null,
						index = i
					};
					list.Add(model);
				}					
			}

			OnDataRetrieved(list.ToArray());
			Invoke(nameof(AutoScroll), 0.25f);
		}

		bool CheckHavePage(List<UnityEngine.Object> resources,int index)
        {
			if (resources.Count < index - 1)
				return false;

            for (int i = 0; i < resources.Count; i++)
            {
				if (resources[i].name.Equals((index + 1).ToString()))
					return true;
            }
			return false;
        }			

		void AutoScroll()
        {
			//ScrollTo(PlayerPrefs.GetInt("keyIndexPicturePrincess"));
			SmoothScrollTo(PlayerPrefs.GetInt("keyIndexPicturePrincess"), 0.5f);
        }			

		void OnDataRetrieved(MyGridItemModel[] newItems)
		{
			//Commented: this only works with Lists. ATM, Insert for Grids only works by manually changing the list and calling NotifyListChangedExternally() after
			// Data.InsertItemsAtEnd(newItems);

			Data.List.AddRange(newItems);
			Data.NotifyListChangedExternally();
		}
	}


	// Class containing the data associated with an item
	public class MyGridItemModel
	{

		public ColoringPageConfig pageConfig;
		public int index;

    }


    // This class keeps references to an item's views.
    // Your views holder should extend BaseItemViewsHolder for ListViews and CellViewsHolder for GridViews
    // The cell views holder should have a single child (usually named "Views"), which contains the actual 
    // UI elements. A cell's root is never disabled - when a cell is removed, only its "views" GameObject will be disabled
    public class MyGridItemViewsHolder : CellViewsHolder
	{

        public RawImage rawImage;
        public Image backgroundImage;
		public Button button;
		public PageConfig pageConfig;
		public Image imageIconDownload;
		public Text textLoading;

		// Retrieving the views from the item's root GameObject
		public override void CollectViews()
		{
			base.CollectViews();

            // GetComponentAtPath is a handy extension method from frame8.Logic.Misc.Other.Extensions
            // which infers the variable's component from its type, so you won't need to specify it yourself

            views.GetComponentAtPath("RawImage", out rawImage);
            views.GetComponentAtPath("BackgroundImage", out backgroundImage);
			views.GetComponentAtPath("BackgroundImage", out button);
			views.GetComponentAtPath("ImageIconDownload", out imageIconDownload);
			views.GetComponentAtPath("TextLoading", out textLoading);
			button.onClick.AddListener(Click);
		}

		void Click()
        {
			if (pageConfig != null)
			{
				//MyAdsBabyGirl.GetInstance().ShowInterstitialAd();
				//BonBonAnalytics.GetInstance().LogEvent("game_" + LoadSceneManager.Instance.nameMinigame.ToString() + "_type_" + pageConfig.name);
				AppData.SelectedPageConfig = pageConfig;
				//LoadSceneManager.Instance.LoadScene(Constant.SceneDrawPicture);
				SceneManager.LoadScene(Constant.SceneDrawPicture);
                PlayerPrefs.SetInt("keyIndexPicturePrincess", this.ItemIndex);
			}
			else
            {
				/*if (AddressableManager.Instance.isLoadingSinglePage == false)
				{
					imageIconDownload.gameObject.SetActive(false);
					backgroundImage.color = Color.white;
					textLoading.gameObject.SetActive(true);
					AddressableManager.Instance.LoadSinglePageConfig(this, this.ItemIndex);
				}*/
            }				
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
}
