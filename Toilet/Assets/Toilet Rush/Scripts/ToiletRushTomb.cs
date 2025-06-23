//using DG.Tweening;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToiletRush
{
    public class ToiletRushTomb : MonoBehaviour
    {
        [SerializeField] Transform characterPos;
        private SkeletonAnimation anim;
        private Collider2D col;
        [SerializeField] AudioClip soundEffect;

        private void Awake()
        {
            anim = GetComponentInChildren<SkeletonAnimation>();
            col = GetComponent<Collider2D>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.CompareTag(ToiletRushManager.CharacterTag)) return;
            if (!collision.TryGetComponent(out ToiletRushCharacter character)) return;
           // character.transform.DOMove(characterPos.position, .5f);
            character.transform.localScale = new Vector3(Mathf.Abs(character.transform.localScale.x), character.transform.localScale.y, character.transform.localScale.z);
            col.enabled = false;
            anim.state.SetAnimation(0, "ani", true).MixDuration = .5f;
            character.TouchTomb();
            SoundManager_BabyGirl.Instance.PlaySoundEffectOneShot(soundEffect);
        }
    }
}
