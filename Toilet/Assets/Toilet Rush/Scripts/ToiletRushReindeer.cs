//using DG.Tweening;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToiletRush
{
    public class ToiletRushReindeer : MonoBehaviour
    {
        [SerializeField] Transform characterPos; 
        private ToiletRushHorizonMove move;
        private Collider2D col;

        private void Awake()
        {
            col = GetComponent<Collider2D>();
            move = GetComponent<ToiletRushHorizonMove>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.CompareTag(ToiletRushManager.CharacterTag)) return;
            if (!collision.TryGetComponent(out ToiletRushCharacter character)) return;
            col.enabled = false;
            move.SetCanMove(false);
            transform.localScale = Vector3.one;
            character.transform.localScale = Vector3.one;
            character.transform.SetParent(characterPos, true);
            //character.transform.DOLocalMove(Vector3.zero, .5f);
            //SoundManager_BabyGirl.Instance.DoMove(character.transform, Vector3.zero, .5f);
            character.transform.localPosition = Vector3.zero;
            character.TouchReindeer();
        }
    }
}