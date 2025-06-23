using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToiletRush
{
    public class ToiletRushBanana : MonoBehaviour
    {
        [SerializeField] AudioClip soundEffect;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.CompareTag(ToiletRushManager.CharacterTag)) return;
            if(!collision.TryGetComponent(out ToiletRushCharacter character)) return;
            character.TouchBanana();
            SoundManager_BabyGirl.Instance.PlaySoundEffectOneShot(soundEffect);
            Destroy(gameObject);
        }
    }

}