using DG.Tweening;
using QuanUtilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PinQuiz
{
    public class PinQuizCannon : PinQuizEntity
    {
        [SerializeField] private Transform point;
        [SerializeField] private PinQuizBullet bullet;
        [SerializeField] private float bulletSpeed = 10;
        [SerializeField] private int bulletCount = 10;

        private bool canFire = true;

        protected override void Update()
        {
            base.Update();

            if (!canFire) return;

            var hit = Physics2D.Raycast(point.position, transform.right, Mathf.Infinity);
            if (!hit) return;
            if (!hit.transform.TryGetComponent(out PinQuizEntity e)) return;

            canFire = false;
            bulletCount--;

            var b = Instantiate(bullet, transform.position, Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z), transform);
            b.transform.DOMove(hit.point, hit.distance / bulletSpeed);
            b.parentCannon = this;

            if (bulletCount <= 0)
                this.DelayFunction(3, Die);
            else
                this.DelayFunction(2, ()=>canFire = true);
        }

    }
}