//using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ToiletRush
{
    public class ToiletRushToilet : MonoBehaviour
    {
        [SerializeField] Transform endPoint;
        [SerializeField] Gender gender;
        [SerializeField] BoxCollider2D drawZoneBoxCollider;

        public bool isDoneDraw;

        public Gender Gender => gender;
        public BoxCollider2D DrawZoneBoxCollider => drawZoneBoxCollider;
        public UnityEvent<ToiletRushToilet> onGoToToilet;

        private void Awake()
        {
            switch (gender)
            {
                case Gender.Boy:
                    tag = ToiletRushManager.BoyToiletTag;
                    break;
                default:
                    tag = ToiletRushManager.GirlToiletTag;
                    break;
            }
        }

        public void SetDrawDone()
        {
            isDoneDraw = true;
        }

        public void GoToToilet(ToiletRushCharacter character)
        {
            /*character.transform.DOMove(endPoint.position, .5f).OnComplete(() =>
            {
                onGoToToilet?.Invoke(this);
            });
*/
            SoundManager_BabyGirl.Instance.DoMove(character.transform, endPoint.position, .5f, () => { onGoToToilet?.Invoke(this); });
        }
    }
}
