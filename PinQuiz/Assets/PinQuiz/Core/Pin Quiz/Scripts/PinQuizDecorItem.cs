using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PinQuiz
{
    public class PinQuizDecorItem : MonoBehaviour
    {
        public float moveDuration = 5;
        public float moveHeight = 3;
        public float rotateDuration = 5;
        public float rotateAdd = 90;
        public float maxDelay = 2;

        private void Start()
        {
            //transform.DOMoveY(transform.position.y + moveHeight, moveDuration).SetEase(Ease.InOutQuart).SetLoops(-1, LoopType.Yoyo).SetDelay(Random.Range(0,maxDelay));
            transform.DORotate(new Vector3(0, 0, rotateAdd), rotateDuration, RotateMode.LocalAxisAdd).SetEase(Ease.InOutQuart).SetLoops(-1, LoopType.Yoyo).SetDelay(Random.Range(0, maxDelay));
        }
    }

}