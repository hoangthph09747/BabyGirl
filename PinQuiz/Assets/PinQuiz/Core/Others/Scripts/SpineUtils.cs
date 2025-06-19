//---------------------------------------------
//---------DAC NGUYEN / HONG QUAN--------------
//---------------------------------------------

using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Spine;
using Spine.Unity;
using Spine.Unity.AttachmentTools;
using UnityEngine;
using AnimationState = Spine.AnimationState;

namespace QuanUtilities
{
    public static class SpineUtils
    {
        public static bool SkeletonDataAssetIsValid(SkeletonDataAsset asset)
        {
            return asset != null && asset.GetSkeletonData(quiet: true) != null;
        }

        public static void ReloadSkeletonDataAsset(SkeletonDataAsset skeletonDataAsset)
        {
            if (skeletonDataAsset != null)
            {
                foreach (AtlasAssetBase aa in skeletonDataAsset.atlasAssets)
                {
                    if (aa != null) aa.Clear();
                }

                skeletonDataAsset.Clear();
            }

            skeletonDataAsset.GetSkeletonData(true);
        }

        public static void ReinitializeComponent(SkeletonRenderer component)
        {
            if (component == null) return;
            if (!SkeletonDataAssetIsValid(component.SkeletonDataAsset)) return;
            var stateComponent = component as IAnimationStateComponent;
            AnimationState oldAnimationState = null;
            if (stateComponent != null)
            {
                oldAnimationState = stateComponent.AnimationState;
            }

            component.Initialize(true); // implicitly clears any subscribers

            if (oldAnimationState != null)
            {
                stateComponent.AnimationState.AssignEventSubscribersFrom(oldAnimationState);
            }

            component.LateUpdate();
        }

        public static void ReinitializeComponent(SkeletonGraphic component)
        {
            if (component == null) return;
            if (!SkeletonDataAssetIsValid(component.SkeletonDataAsset)) return;
            component.Initialize(true);
            component.LateUpdate();
        }

        public static void SetSkin(this SkeletonAnimation anim, params string[] nameSkins)
        {
            Skeleton skeleton = anim.skeleton;
            SkeletonData skeletonData = skeleton.Data;
            // Get the source skins.
            Skin myEquipsSkin = new Skin("my new skin");
            for (int i = 0; i < nameSkins.Length; i++)
            {
                var findSkin = skeletonData.FindSkin(nameSkins[i]);
                if (findSkin == null)
                {
                    Debug.LogError("not found skin=" + nameSkins[i]);
                }
                myEquipsSkin.AddSkin(findSkin);
            }

            // Set and apply the Skin to the skeleton.
            skeleton.SetSkin(myEquipsSkin);
            skeleton.SetSlotsToSetupPose();
            anim.Update(0);
        }

        public static void SetSkin(this SkeletonGraphic anim, params string[] nameSkins)
        {
            Skeleton skeleton = anim.Skeleton;
            SkeletonData skeletonData = skeleton.Data;
            // Get the source skins.
            Skin myEquipsSkin = new Skin("my new skin");
            for (int i = 0; i < nameSkins.Length; i++)
            {
                var findSkin = skeletonData.FindSkin(nameSkins[i]);
                if (findSkin == null)
                {
                    Debug.LogError("not found skin=" + nameSkins[i]);
                }
                myEquipsSkin.AddSkin(findSkin);
            }

            // Set and apply the Skin to the skeleton.
            skeleton.SetSkin(myEquipsSkin);
            skeleton.SetSlotsToSetupPose();
            anim.Update(0);
        }

        public static void SetSkin(this SkeletonAnimation anim, string nameSkin)
        {
            anim.Skeleton.SetSkin(nameSkin);
            anim.Skeleton.SetSlotsToSetupPose();
        }

        public static void SetSkin(this SkeletonGraphic anim, string nameSkin)
        {
            anim.Skeleton.SetSkin(nameSkin);
            anim.Skeleton.SetSlotsToSetupPose();
        }

        public static void SetSkin(this ISkeletonAnimation anim, string nameSkin)
        {
            anim.Skeleton.SetSkin(nameSkin);
            anim.Skeleton.SetSlotsToSetupPose();
        }

        public static void SetAttachment(this SkeletonAnimation baby, string slotName, string attachmentPath, string regionName,
            SpineAtlasAsset atlasRegion)
        {
            var atlas = atlasRegion.GetAtlas();
            float scale = baby.skeletonDataAsset.scale;
            baby.Skeleton.SetAttachment(slotName, attachmentPath/* + regionName*/);
            Slot slot = baby.Skeleton.FindSlot(slotName);
            //slot.SetColor(Color.white);
            Attachment originalAttachment = slot.Attachment;

            AtlasRegion region = atlas.FindRegion(regionName);
            slot.Attachment = originalAttachment.GetRemappedClone(region, true, true, scale);
            //slot.SetColor(Color.green);
            //slot.Attachment = newRegionAttachment;
        }

        public static void SetAttachment(this SkeletonAnimation baby, string slotName, SpineAtlasAsset atlasRegion)
        {
            if (atlasRegion == null)
                return;
            string attachmentPath = atlasRegion.name.Replace("_Atlas", "");
            var atlas = atlasRegion.GetAtlas();
            float scale = baby.skeletonDataAsset.scale;
            baby.Skeleton.SetAttachment(slotName, attachmentPath);
            Slot slot = baby.Skeleton.FindSlot(slotName);
            //slot.SetColor(new Color(1, 1, 1, 1));
            Attachment originalAttachment = slot.Attachment;

            AtlasRegion region = atlas.FindRegion(atlasRegion.materials[0].mainTexture.name);
            if (region == null)
                Debug.LogError("null region");
            slot.Attachment = originalAttachment.GetRemappedClone(region, true, true, scale);

            //  Debug.LogError("setattackment " + attachmentPath);
        }

        public static void SetOffAttachment(this SkeletonAnimation baby, string slotName)
        {
            baby.Skeleton.SetAttachment(slotName, null);
        }

        public static void SetColor(this SkeletonAnimation baby, string slotName, Color newColor)
        {
            Slot slot = baby.Skeleton.FindSlot(slotName);
            slot.SetColor(newColor);
        }

        public static void SetColor(this SkeletonAnimation baby, Color color)
        {
            baby.Skeleton.SetColor(color);
        }
        public static void SetColor(this SkeletonGraphic baby, Color color)
        {
            baby.color = color;
        }

        public static void SetAttachment(this ISkeletonAnimation baby, float scale, string slotName, string attachmentPath,
            SpineAtlasAsset atlasRegion)
        {
            var atlas = atlasRegion.GetAtlas();
            baby.Skeleton.SetAttachment(slotName, attachmentPath);
            Slot slot = baby.Skeleton.FindSlot(slotName);
            slot.SetColor(new Color(1, 1, 1, 1));
            Attachment originalAttachment = slot.Attachment;

            AtlasRegion region = atlas.FindRegion(atlasRegion.materials[0].mainTexture.name);
            slot.Attachment = originalAttachment.GetRemappedClone(region, true, true, scale);
        }

        public static void SetAttachment(this SkeletonAnimation baby, string slotName, string attachmentPath, SpineAtlasAsset atlasRegion)
        {
            var atlas = atlasRegion.GetAtlas();
            float scale = baby.skeletonDataAsset.scale;
            baby.Skeleton.SetAttachment(slotName, attachmentPath);
            Slot slot = baby.Skeleton.FindSlot(slotName);
            //slot.SetColor(new Color(1, 1, 1, 1));
            Attachment originalAttachment = slot.Attachment;

            AtlasRegion region = atlas.FindRegion(atlasRegion.materials[0].mainTexture.name);
            slot.Attachment = originalAttachment.GetRemappedClone(region, true, true, scale);

            //  Debug.LogError("setattackment " + attachmentPath);
        }
        public static void SetAttachment(this SkeletonAnimation baby, string slotName, string attachmentPath, SpineSpriteAtlasAsset atlasRegion)
        {
            var atlas = atlasRegion.GetAtlas();
            float scale = baby.skeletonDataAsset.scale;
            baby.Skeleton.SetAttachment(slotName, attachmentPath);
            Slot slot = baby.Skeleton.FindSlot(slotName);
            //slot.SetColor(new Color(1, 1, 1, 1));
            Attachment originalAttachment = slot.Attachment;

            AtlasRegion region = atlas.FindRegion(atlasRegion.materials[0].mainTexture.name);
            slot.Attachment = originalAttachment.GetRemappedClone(region, true, true, scale);

            //  Debug.LogError("setattackment " + attachmentPath);
        }

        public static void SetAttachment(this SkeletonGraphic baby, string slotName, string attachmentPath, SpineAtlasAsset atlasRegion)
        {
            var atlas = atlasRegion.GetAtlas();
            float scale = baby.skeletonDataAsset.scale;
            baby.Skeleton.SetAttachment(slotName, attachmentPath);
            Slot slot = baby.Skeleton.FindSlot(slotName);
            slot.SetColor(new Color(1, 1, 1, 1));
            Attachment originalAttachment = slot.Attachment;

            AtlasRegion region = atlas.FindRegion(atlasRegion.materials[0].mainTexture.name);
            slot.Attachment = originalAttachment.GetRemappedClone(region, true, true, scale);
        }

        public static void OffSlot_ByAttackment(this SkeletonAnimation baby, string slotName, string attackmentPath, SpineAtlasAsset spineAtlasAsset)
        {
            SetAttachment(baby, slotName, attackmentPath, spineAtlasAsset);
        }

        public static void OffSlot_ByAttackment(this SkeletonGraphic baby, string slotName, string attackmentPath, SpineAtlasAsset spineAtlasAsset)
        {
            SetAttachment(baby, slotName, attackmentPath, spineAtlasAsset);
        }

        public static void OnSlot_ByAttackment(this SkeletonAnimation baby, string slotName, string attackmentPath, SpineAtlasAsset spineAtlasAsset)
        {
            SetAttachment(baby, slotName, attackmentPath, spineAtlasAsset);
        }

        public static void OnSlot_ByAttackment(this SkeletonGraphic baby, string slotName, string attackmentPath, SpineAtlasAsset spineAtlasAsset)
        {
            SetAttachment(baby, slotName, attackmentPath, spineAtlasAsset);
        }

        public static void PlayBackwards(this SkeletonAnimation baby, ref TrackEntry myTrackEntry, ref float targetTime)
        {
            myTrackEntry.TimeScale = 0f; // so time isn't moved forward by SkeletonAnimation/AnimationState
            myTrackEntry.AnimationLast = 0f; // this may cause multiple events to fire if you have those.
            myTrackEntry.TrackTime =
                targetTime; // you should decrement targetTime by a delta time (to act as the animation time cursor that moves backwards.)
            baby.state.Apply(baby
                .skeleton); // may be required, depending on project script execution order settings. It would be more efficient if this script runs before the SkeletonAnimation.cs and you won't need this.
            targetTime -= Time.deltaTime;

            //loop use code
            // if (targetTime <= 0)
            //     targetTime = myTrackEntry.AnimationEnd; // You need to make sure the trackTime is within the limits of the animation, or will not work

            //Debug.LogError(targetTime);
        }

        //ngocdu play anim spine
        public static void PlayAnimation(this SkeletonAnimation anim, string nameAnimation, bool loop = false, float mix = 0.0f)
        {

            anim.state.SetAnimation(0, nameAnimation, loop).MixDuration = mix;
        }

        public static void PlayAnimation(this SkeletonGraphic anim, string nameAnimation, bool loop = false, float mix = 0.25f)
        {
            anim.AnimationState.SetAnimation(0, nameAnimation, loop).MixDuration = mix;
        }

        public static float GetAnmationDuration(this SkeletonGraphic anim, string nameAnimation)
        {
            return anim.Skeleton.Data.FindAnimation(nameAnimation).Duration;
        }
        public static float GetAnmationDuration(this SkeletonAnimation anim, string nameAnimation)
        {
            return anim.Skeleton.Data.FindAnimation(nameAnimation).Duration;
        }

        public static string[] GetAnimationNames(this SkeletonAnimation anim)
        {
            var anims = anim.Skeleton.Data.Animations;
            List<string> names = new List<string>();
            foreach (var tanim in anims)
                names.Add(tanim.Name);
            return names.ToArray();
        }
        public static string[] GetAnimationNames(this SkeletonGraphic anim)
        {
            var anims = anim.Skeleton.Data.Animations;
            List<string> names = new List<string>();
            foreach (var tanim in anims)
                names.Add(tanim.Name);
            return names.ToArray();
        }

        public static float GetAnimationDuration(this SkeletonGraphic anim, string animName)
        {
            return anim.Skeleton.Data.FindAnimation(animName).Duration;
        }
        public static float GetAnimationDuration(this SkeletonAnimation anim, string animName)
        {
            return anim.Skeleton.Data.FindAnimation(animName).Duration;
        }
        public static float GetAnimationsDuration(this SkeletonAnimation anim, params string[] animName)
        {
            float duration = 0f;
            for (int i = 0; i < animName.Length; i++)
                duration += anim.GetAnimationDuration(animName[i]);
            return duration;
        }
        public static float GetAnimationsDuration(this SkeletonGraphic anim, params string[] animName)
        {
            float duration = 0f;
            for (int i = 0; i < animName.Length; i++)
                duration += anim.GetAnimationDuration(animName[i]);
            return duration;
        }

        public static void PlayAnimation(this SkeletonAnimation anim, string nameAnimation, int loopTimes)
        {
            anim.state.SetAnimation(0, nameAnimation, false);
            for (int i = 1; i < loopTimes; i++)
            {
                anim.state.AddAnimation(0, nameAnimation, false, 0);
            }
        }

        public static void PlayAnimation(this SkeletonGraphic anim, string nameAnimation, int loopTimes)
        {
            anim.AnimationState.SetAnimation(0, nameAnimation, false);
            for (int i = 1; i < loopTimes; i++)
            {
                anim.AnimationState.AddAnimation(0, nameAnimation, false, 0);
            }
        }

        public static void SetTimeScale(this SkeletonAnimation anim, float timeScale)
        {
            anim.AnimationState.TimeScale = timeScale;
        }

        public static void PlayAnimation(this SkeletonGraphic anim, float timeScale)
        {
            anim.AnimationState.TimeScale = timeScale;
        }

        public static void PlaySequanceAnimations(this SkeletonAnimation anim, params string[] anims)
        {
            if (anims.Length == 0) return;
            anim.PlayAnimation(anims[0]);
            for (int i = 1; i < anims.Length - 1; i++)
            {
                anim.AnimationState.AddAnimation(0, anims[i], false, 0);
            }
            anim.AnimationState.AddAnimation(0, anims[anims.Length - 1], true, 0);
        }

        public static void PlaySequanceAnimtions(this SkeletonGraphic anim, params string[] anims)
        {
            if (anims.Length == 0) return;
            anim.PlayAnimation(anims[0]);
            for (int i = 1; i < anims.Length - 1; i++)
            {
                anim.AnimationState.AddAnimation(0, anims[i], false, 0);
            }
            anim.AnimationState.AddAnimation(0, anims[anims.Length - 1], true, 0);
        }
        public static void PlaySequanceAnimations(this SkeletonAnimation anim, bool isLoopEndAnim, params string[] anims)
        {
            if (anims.Length == 0) return;
            anim.PlayAnimation(anims[0]);
            for (int i = 1; i < anims.Length - 1; i++)
            {
                anim.AnimationState.AddAnimation(0, anims[i], false, 0);
            }
            anim.AnimationState.AddAnimation(0, anims[anims.Length - 1], isLoopEndAnim, 0);
        }

        public static Tweener DoColor(this SkeletonAnimation anim, Color to, float duration)
        {
            return DOVirtual.Color(anim.skeleton.GetColor(), to, duration, color =>
            {
                anim.SetColor(color);
            });
        }

        public static Tweener DoColor(this SkeletonGraphic anim, Color to, float duration)
        {
            return DOVirtual.Color(anim.Skeleton.GetColor(), to, duration, color =>
            {
                anim.SetColor(color);
            });
        }
        public static Tweener DoFade(this SkeletonAnimation anim, float to, float duration)
        {
            return DOVirtual.Float(anim.Skeleton.GetColor().a, to, duration, value =>
            {
                var color = anim.Skeleton.GetColor();
                color.a = value;
                anim.SetColor(color);
            });
        }
        public static Tweener DoFade(this SkeletonAnimation anim, float from,float to, float duration)
        {
            return DOVirtual.Float(from, to, duration, value =>
            {
                var color = anim.Skeleton.GetColor();
                color.a = value;
                anim.SetColor(color);
            });
        }

        public static Tweener DoFade(this SkeletonGraphic anim, float to, float duration)
        {
            return DOVirtual.Float(anim.Skeleton.GetColor().a, to, duration, value =>
            {
                var color = anim.Skeleton.GetColor();
                color.a = value;
                anim.SetColor(color);
            });
        }
        public static Tweener DoFade(this SkeletonGraphic anim,float from, float to, float duration)
        {
            return DOVirtual.Float(from, to, duration, value =>
            {
                var color = anim.Skeleton.GetColor();
                color.a = value;
                anim.SetColor(color);
            });
        }

        public static void OnSlot(this Skeleton skeleton, string slotName,string attactmentName)
        {
            Slot slot = skeleton.FindSlot(slotName);
            Attachment attachment = skeleton.GetAttachment(slot.Data.Index, attactmentName);
            slot.Attachment = attachment;
        }
        public static Slot OffSlot(this Skeleton skeleton, string slotName)
        {
            Slot slot = skeleton.FindSlot(slotName);
            slot.Attachment = null;
            return slot;
        }
        public static void SetLayerOder(this SkeletonAnimation anim, int oderLayer)
        {
            anim.gameObject.GetComponent<MeshRenderer>().sortingOrder = oderLayer;
        }
    }
}