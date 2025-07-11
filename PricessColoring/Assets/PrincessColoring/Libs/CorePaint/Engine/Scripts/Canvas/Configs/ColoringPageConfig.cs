using System;
using PaintCraft.Utils;
using UnityEngine;

namespace PaintCraft.Canvas.Configs
{
    [CreateAssetMenu(menuName = "PaintCraft/ColoringPageConfig")]
    public class ColoringPageConfig : AdvancedPageConfig
    {

        [TexturePath] public string outlinePath;
        [TexturePath] public string RegionPath;
		[TexturePath] public string ColorsPath;
		[TexturePath] public string IconPath;
		

		[NonSerialized]
        Texture2D outlineTexture;

        [SerializeField]
        bool isServer = false;

        public override Texture2D OutlineTexture
        {
            get
            {
                if (outlineTexture == null)
                {
                    if (isServer == false)
                        outlineTexture = Resources.Load<Texture2D>(OutlinePath);
                    else
                    {
                        //if(LoadSceneManager.Instance.nameMinigame == NameMinigame.PrincessColoring)
                        //outlineTexture = AddressableManager.Instance.LoadTexture("PictureNew/" + name + "/" + outlinePath.Split('/')[1] + ".png");
                        //else if (LoadSceneManager.Instance.nameMinigame == NameMinigame.ColoringASMR)
                        //    outlineTexture = AddressableManagerASMR.Instance.LoadTexture("PictureNewASMR/" + name + "/" + outlinePath.Split('/')[1] + ".png");
                        //else if (LoadSceneManager.Instance.nameMinigame == NameMinigame.DIY)
                        //    outlineTexture = AddressableManager.Instance.LoadTexture("Pictures/" + outlinePath + ".png");
                       // outlineTexture = AddressableManager.Instance.LoadTexture("PictureNew/" + name + "/" + outlinePath.Split('/')[1] + ".png");
                    }
                }
                return outlineTexture;
            }
        }

        public Sprite OutlineSprite
        {
            get
            {
                return Resources.Load<Sprite>(OutlinePath);
            }
        }

        [NonSerialized]
        Texture2D regionTexture;
        public override Texture2D RegionTexture
        {
            get
            {
                if (regionTexture == null)
                {
                    if (isServer == false)
                        regionTexture = Resources.Load<Texture2D>(RegionPath);
                    else
                    {
                        //if (LoadSceneManager.Instance.nameMinigame == NameMinigame.PrincessColoring)
                        //    regionTexture = AddressableManager.Instance.LoadTexture("PictureNew/" + name + "/" + RegionPath.Split('/')[1] + ".png");
                        //else if (LoadSceneManager.Instance.nameMinigame == NameMinigame.ColoringASMR)
                        //    regionTexture = AddressableManager.Instance.LoadTexture("PictureNewASMR/" + name + "/" + RegionPath.Split('/')[1] + ".png");
                        //else if (LoadSceneManager.Instance.nameMinigame == NameMinigame.DIY)
                        //    regionTexture = AddressableManager.Instance.LoadTexture( RegionPath + ".png");
                      //  regionTexture = AddressableManager.Instance.LoadTexture("PictureNew/" + name + "/" + RegionPath.Split('/')[1] + ".png");

                    }
                }
                return regionTexture;
            }
        }


		[NonSerialized]
		Texture2D colorsTexture;
		public override Texture2D ColorsTexture
		{
			get
			{
				if (colorsTexture == null)
				{
					colorsTexture = Resources.Load<Texture2D>(ColorsPath);
				}
				return colorsTexture;
			}
		}

		public Sprite Icon
        {
            get { return Resources.Load<Sprite>(IconPath); }
        }

		public string OutlinePath
        {
            get
            {
                return outlinePath;
            }

            set
            {
                outlinePath = value;
            }
        }


		public override Vector2 GetSize()
        {
            
            if (OutlineTexture == null)
            {
                if (StartImageTexture == null) {                    
                    if (RegionTexture == null){                        
                        Debug.Log("one of Outline, StartImage or Region picture must be set", this);
                        return Vector2.one;
                    } else {
                        return new Vector2(RegionTexture.width, RegionTexture.height);
                    }
                } else {
                    return new Vector2(StartImageTexture.width, StartImageTexture.height);
                }               
            }
            else
            {
                return new Vector2(OutlineTexture.width, OutlineTexture.height);
            }
        }
    }
}
