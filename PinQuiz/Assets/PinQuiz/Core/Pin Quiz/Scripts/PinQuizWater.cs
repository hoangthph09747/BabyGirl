using PinQuiz;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PinQuiz
{
    public class PinQuizWater : PinQuizFluid
    {
        //private void OnCollisionEnter2D(Collision2D collision)
        //{
        //    Check(collision.transform);
        //}

        //private void OnTriggerEnter2D(Collider2D collision)
        //{
        //    Check(collision.transform);
        //}

        //private void Check(Transform col)
        //{
        //    if (!col.transform.TryGetComponent(out PinQuizEntity entity)) return;
        //    if (entity.EntityType == EntityType.Water) return;
        //    entity.TouchLava();
        //}

        public override void TouchLava(PinQuizLava lava)
        {
            base.TouchLava(lava);
            lava.ToStone();
            Die();
        }
    }
}