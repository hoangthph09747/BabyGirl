using QuanUtilities;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToiletRush {
    public class ToiletRushEndGameAnimUI : MonoBehaviour
    {
        [SerializeField] SkeletonGraphic[] anims;

        private void Awake()
        {
            anims = GetComponentsInChildren<SkeletonGraphic>();
        }

        public void Show(float showDuration = 1f)
        {
            gameObject.SetActive(true);
            foreach (var anim in anims)
            {
              //  anim.DoFade(0, 1, showDuration);
            }
        }
    }
}