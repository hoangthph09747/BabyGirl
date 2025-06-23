//using DG.Tweening;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToiletRush
{
    public class ToiletRushSnowman : MonoBehaviour
    {
        [SerializeField] GameObject eff;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.CompareTag(ToiletRushManager.CharacterTag)) return;
            if (!collision.TryGetComponent(out ToiletRushCharacter character)) return;
            character.TouchSnowman();
            Instantiate(eff, character.transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
