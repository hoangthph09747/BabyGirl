using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PinQuiz
{
    public class PinQuizPrince : PinQuizEntity
    {
        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.TryGetComponent(out PinQuizPrincess princess))
            {
                princess.MeetPrince(this);
            }
        }

        public override void Die()
        {
            PinQuizManager.instance.LoseGame();
            base.Die();
        }
    }
}
