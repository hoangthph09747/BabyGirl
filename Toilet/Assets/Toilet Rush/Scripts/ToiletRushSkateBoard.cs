//using DG.Tweening;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToiletRush
{
    public class ToiletRushSkateBoard : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.CompareTag(ToiletRushManager.CharacterTag)) return;
            if (!collision.TryGetComponent(out ToiletRushCharacter character)) return;
            character.TouchSkateBoard();
            transform.SetParent(character.SkateBoardPos, true);
            //transform.DOLocalMove(Vector3.zero, .5f);
            GetComponent<Collider2D>().enabled = false;
        }
    }
}

