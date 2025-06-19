using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using QuanUtilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HongQuan
{
    public class FadeSpriteRenderer : FadeObject
    {
        public List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>();
        public bool autoGetComponents;

        private List<TweenerCore<Color, Color, ColorOptions>> tweeens = new List<TweenerCore<Color, Color, ColorOptions>>();

        private void Awake()
        {
            if (autoGetComponents)
            {
                if(TryGetComponent(out SpriteRenderer spriteRenderer))
                {
                    spriteRenderers.Add(spriteRenderer);
                }
                spriteRenderers.AddRange(GetComponentsInChildren<SpriteRenderer>());
            }
        }

        public override void Fade(float to, float duration = 0.5f, System.Action onDone = null)
        {
            RemoveAllTweens();
            onDone += RemoveAllTweens;
            foreach (var sprite in spriteRenderers)
            {
                tweeens.Add(sprite.DOFade(to, duration));
            }
            this.DelayFunction(duration, onDone);
        }

        public override void Fade(float from, float to, float duration = 0.5f, System.Action onDone = null)
        {
            RemoveAllTweens();
            onDone += RemoveAllTweens;
            foreach (var sprite in spriteRenderers)
            {
                sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, from);
                tweeens.Add(sprite.DOFade(to, duration));
            }
            this.DelayFunction(duration, onDone);
        }

        private void RemoveAllTweens()
        {
            foreach (var tweener in tweeens)
            {
                tweener.Kill();
            }
            tweeens.Clear();
        }
    }
}
