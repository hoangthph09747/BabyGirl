using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PinQuiz
{
    public class PinQuizMonster : PinQuizEntity
    {
        [SerializeField] private Transform leftPoint;
        [SerializeField] private Transform rightPoint;
        [SerializeField] private float speed;

        private bool isMoving;

        protected override void Update()
        {
            base.Update();

            if (isFalling) return;
            if (isMoving) return;

            var hit = Physics2D.Raycast(leftPoint.position, Vector2.left, 100);

            if (hit.transform.TryGetComponent(out PinQuizEntity et))
            {
                if (et.EntityType < EntityType.Monster)
                {
                    isMoving = true;
                    transform.DOMoveX(hit.point.x, hit.distance / speed).OnComplete(() => isMoving = false);
                }
            }
            else
            {
                hit = Physics2D.Raycast(rightPoint.position, Vector2.right, 100);
                if (hit.transform.TryGetComponent(out PinQuizEntity e))
                {
                    if (e.EntityType < EntityType.Monster)
                    {
                        isMoving = true;
                        transform.DOMoveX(hit.point.x, hit.distance / speed).OnComplete(() => isMoving = false);
                    }
                }
            }
        }

        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out PinQuizEntity entity))
            {
                if (entity.EntityType < EntityType.Monster)
                {
                    entity.Die();
                }
            }
        }
    }
}