using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PinQuiz
{
    public class PinQuizBullet : MonoBehaviour
    {
        public PinQuizCannon parentCannon;

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(10);
            Destroy(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.TryGetComponent(out PinQuizEntity entity))
            {
                if (entity == parentCannon) return;
                entity.Die();
                Destroy(gameObject);
            }
        }
    }
}
