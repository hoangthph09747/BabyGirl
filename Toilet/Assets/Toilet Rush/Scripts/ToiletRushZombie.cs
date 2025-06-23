//using DG.Tweening;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
//using Unity.VisualScripting;
using UnityEngine;

namespace ToiletRush
{
    public class ToiletRushZombie : MonoBehaviour
    {
        [SerializeField] AudioClip soundEffect;
        private ToiletRushHorizonMove move;
        private SkeletonAnimation anim;
        private Collider2D col;

        private void Awake()
        {
            col = GetComponent<Collider2D>();
            move = GetComponent<ToiletRushHorizonMove>();
            anim = GetComponentInChildren<SkeletonAnimation>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.CompareTag(ToiletRushManager.CharacterTag)) return;
            if (!collision.TryGetComponent(out ToiletRushCharacter character)) return;
            col.enabled = false;
            anim.state.SetAnimation(0, "zoombie_ngoam", true);
            move.SetCanMove(false);
            transform.localScale = new Vector3(-1, 1, 1);
            transform.SetParent(character.ZombiePos, true);
            //transform.DOLocalMove(Vector3.zero, .5f);
            character.TouchZombie();
            SoundManager_BabyGirl.Instance.PlaySoundEffectOneShot(soundEffect);
        }
    }
}
