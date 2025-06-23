using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToiletRush
{
    public class ToiletRushGum : MonoBehaviour
    {
        SkeletonAnimation anim;
        ParticleSystem eff;
        
        private void Awake()
        {
            anim = GetComponentInChildren<SkeletonAnimation>();
            eff = GetComponentInChildren<ParticleSystem>();
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.CompareTag(ToiletRushManager.CharacterTag)) return;
            if (!collision.TryGetComponent(out ToiletRushCharacter character)) return;
            character.TouchGum();
            anim.state.SetAnimation(0, "haha", true).MixDuration = .2f;
            eff.Play();
        }
    }

}