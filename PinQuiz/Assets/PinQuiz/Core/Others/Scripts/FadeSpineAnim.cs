using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using QuanUtilities;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HongQuan
{
    public class FadeSpineAnim : FadeObject
    {
        public List<SkeletonAnimation> anims = new List<SkeletonAnimation>();
        public bool autoGetComponents;

        private Tweener tweeen;

        private void Awake()
        {
            if (autoGetComponents)
            {
                if (TryGetComponent(out SkeletonAnimation spriteRenderer))
                {
                    anims.Add(spriteRenderer);
                }
                anims.AddRange(GetComponentsInChildren<SkeletonAnimation>());
            }
        }

        public override void Fade(float to, float duration = 0.5f, System.Action onDone = null)
        {
            KillTween();
            onDone += KillTween;
            DOVirtual.Float(0, to, duration, value =>
            {
                foreach (var animation in anims)
                {
                    var c = animation.Skeleton.GetColor();
                    c.a = value;
                    animation.Skeleton.SetColor(c);
                }
            }).OnComplete(() => onDone?.Invoke());
        }

        public override void Fade(float from, float to, float duration = 0.5f, System.Action onDone = null)
        {
            KillTween();
            onDone += KillTween;
            DOVirtual.Float(from, to, duration, value =>
            {
                foreach (var animation in anims)
                {
                    var c = animation.Skeleton.GetColor();
                    c.a = value;
                    animation.Skeleton.SetColor(c);
                }
            }).OnComplete(() => onDone?.Invoke());
        }

        private void KillTween()
        {
            tweeen?.Kill();
        }
    }

}