using DG.Tweening;
using QuanUtilities;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace PinQuiz
{
    public class PinQuizTNT : PinQuizEntity
    {
        [SerializeField] ParticleSystem explosionEff;
        [SerializeField] float explosionRadius;
        [SerializeField] float explosionForce;
        //[SerializeField] float explosionDelay = 1;
        [SerializeField] Vector3 offset;

        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.TryGetComponent(out PinQuizEntity entity)) return;
            //Invoke(nameof(Die), explosionDelay);
            Explose();
        }

        protected void OnCollisionEnter2D(Collision2D collision)
        {
            if (!collision.transform.TryGetComponent(out PinQuizEntity entity)) return;
            //Invoke(nameof(Die), explosionDelay);
            Explose();
        }

        bool canExplose = true;

        public void Explose()
        {
            if (!canExplose) return;
            canExplose = false;

            var explosionEffect = Instantiate(explosionEff).transform;
            explosionEffect.position = transform.position;
            Destroy(explosionEffect.gameObject, 5);
            Camera.main.transform.DOShakePosition(0.5f, 0.2f);

            var hits = Physics2D.OverlapCircleAll(transform.position + offset, explosionRadius, LayerMask.GetMask("Item", "Character"));

            foreach (var hit in hits)
            {
                if (hit.transform.TryGetComponent(out PinQuizEntity entity))
                {
                    //var rigid = hit.GetComponent<Rigidbody2D>();
                    //rigid.isKinematic = false;
                    //rigid.gravityScale = 1;
                    //rigid.GetComponent<Collider2D>().isTrigger = false;
                    //var dir = new Vector2(Random.Range(-1f, 1f), 1f).normalized;
                    //rigid.AddForce(dir * explosionForce);
                    //rigid.AddTorque(Random.Range(0, 180));
                    if (entity != this)
                        entity.BeingExplosed(this);
                }
            }
            Destroy(gameObject);
        }

        protected override void Start()
        {
            base.Start();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position + offset, explosionRadius);
        }
    }
}