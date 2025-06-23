//using DG.Tweening;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
//using Unity.VisualScripting;
using UnityEngine;

namespace ToiletRush
{
    public class ToiletRushGhost : MonoBehaviour
    {
        [SerializeField]private AudioClip soundEffect;
        private SkeletonAnimation anim;
        private Collider2D col;
        private ToiletRushVerticalMove move;

        private void Awake()
        {
            anim = GetComponentInChildren<SkeletonAnimation>();
            col = GetComponent<Collider2D>();
            move = GetComponent<ToiletRushVerticalMove>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.CompareTag(ToiletRushManager.CharacterTag)) return;
            if (!collision.TryGetComponent(out ToiletRushCharacter character)) return;
            col.enabled = false;
            anim.state.SetAnimation(0, "ani", true);
            move.canMove = false;

           // transform.DOMove(character.GhostPos.position, .5f);
            SoundManager_BabyGirl.Instance.DoMove(transform, character.GhostPos.position, .5f);
            SoundManager_BabyGirl.Instance.PlaySoundEffectOneShot(soundEffect);
            character.TouchGhost();
        }
    }
}