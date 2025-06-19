using HongQuan;
using PinQuiz;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace PinQuiz
{
    public class PinQuizLava : PinQuizFluid
    {
        protected override void Awake()
        {
            base.Awake();

        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            Check(collision.transform);
        }

        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            Check(collision.transform);
        }

        private void Check(Transform col)
        {
            if (!col.transform.TryGetComponent(out PinQuizEntity entity)) return;
            if (entity.EntityType == EntityType.Lava) return;
            var eff = QuanUtilities.SimplePool.Spawn(PinQuizManager.instance.lavaSteamEffect, transform.position, Quaternion.identity);
            eff.DespawnDelay(1);
            eff.effect.Play();
            SoundManager_BabyGirl.Instance.PlaySoundEffectOneShot(PinQuizManager.instance.lavaEvaporationSound);
            entity.TouchLava(this);
        }

        public void ToStone()
        {
            Instantiate(PinQuizManager.instance.lavaStone, transform.position, transform.rotation, transform.parent);
            Die();
        }
    }
}