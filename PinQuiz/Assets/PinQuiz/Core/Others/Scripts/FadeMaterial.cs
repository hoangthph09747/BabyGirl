using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HongQuan
{
    public class FadeMaterial : FadeObject
    {
        [SerializeField] string alphaField = "_Alpha";


        List<Material> materials = new List<Material>();

        private void Awake()
        {
            if(TryGetComponent(out Renderer renderer))
            {
                materials.Add(renderer.material);
            }            
            var r = GetComponentsInChildren<Renderer>();
            foreach(var c in r)
            {
                materials.Add(c.material);
            }
        }

        public override void Fade(float from, float to, float duration = 0.5F, Action onDone = null)
        {
            DOVirtual.Float(from, to, duration, value =>
            {
                foreach(var m in materials)
                {
                    var mat = m;
                    mat.SetFloat(alphaField, value);
                }    
            }).OnComplete(()=>onDone?.Invoke());
        }

        public override void Fade(float to, float duration = 0.5F, Action onDone = null)
        {

            DOVirtual.Float(0, to, duration, value =>
            {
                foreach (var m in materials)
                {
                    var mat = m;
                    mat.SetFloat(alphaField, value);
                }
            }).OnComplete(() => onDone?.Invoke());
        }
    }

}

