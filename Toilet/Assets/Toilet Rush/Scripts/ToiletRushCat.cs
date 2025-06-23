//using DG.Tweening;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToiletRush
{
    public class ToiletRushCat : MonoBehaviour
    {
        [SerializeField] private AudioClip soundEffect;
        SkeletonAnimation anim;

        private void Awake()
        {
            anim = GetComponentInChildren<SkeletonAnimation>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.CompareTag(ToiletRushManager.CharacterTag)) return;
            if (!collision.TryGetComponent(out ToiletRushCharacter character)) return;
            character.TouchCat();
            //transform.DOMove(character.CatBoardPos.position, .5f);
            SoundManager_BabyGirl.Instance.DoMove(transform, character.CatBoardPos.position, .5f);
            GetComponent<Collider2D>().enabled = false;
            anim.state.SetAnimation(0, "ani", true).MixDuration = .2f;
            SoundManager_BabyGirl.Instance.PlaySoundEffectOneShot(soundEffect);
        }
    }

}