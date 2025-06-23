//using DG.Tweening;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToiletRush
{
    public class ToiletRushSpider : MonoBehaviour
    {
        SkeletonAnimation anim;

        private void Awake()
        {
            anim = GetComponentInChildren<SkeletonAnimation>();
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.CompareTag(ToiletRushManager.CharacterTag)) return;
            if (!collision.TryGetComponent(out ToiletRushCharacter character)) return;
            anim.state.SetAnimation(0, "rang_to", true).MixDuration = .5f;
            //transform.DOMove(character.SpderPos.position, .5f);
            character.TouchSpider();
        }
    }

}