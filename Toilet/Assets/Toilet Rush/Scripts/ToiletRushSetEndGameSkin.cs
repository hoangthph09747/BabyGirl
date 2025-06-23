using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToiletRush
{
    public class ToiletRushSetEndGameSkin : MonoBehaviour
    {
        [SerializeField] bool dontSetGender = false;
        SkeletonGraphic anim;

        private void Awake()
        {
            anim = GetComponent<SkeletonGraphic>();
        }

        private void Start()
        {
            string skin = "";
            if (dontSetGender)
            {
                if (anim.Skeleton.Skin.Name.Contains("boy"))
                {
                    skin = "boy";
                }
                else
                {
                    skin = "girl";
                }
            }
            else
            {
                switch (ToiletRushManager.instance.LoseGender)
                {
                    case Gender.Boy:
                        skin = "boy";
                        break;
                    case Gender.Girl:
                        skin = "girl";
                        break;
                }
            }

            switch (ToiletRushManager.instance.Map)
            {
                case 1:
                    skin += "1";
                    break;
                case 2:
                case 3:
                    skin += "2";
                    break;
                case 4:
                case 5:
                    skin += "3";
                    break;
                case 6:
                case 7:
                    skin += "4";
                    break;
                case 10:
                case 11:
                    skin += "5";
                    break;
                default:
                    skin += "0";
                    break;
            }
            anim.Skeleton.SetSkin(skin);
            anim.Skeleton.SetSlotsToSetupPose();
            anim.LateUpdate();
        }
    }
}
