using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PinQuiz
{
    public class PinQuizEntity : MonoBehaviour
    {
        public EntityType EntityType;

        [SerializeField] protected LayerMask wallLayerMask = 1;
        [SerializeField] protected bool canFall = true;
        [SerializeField] protected float fallSpeed = 5;
        [SerializeField] protected bool autoCaculateGroundDistance = true;
        [SerializeField] protected float groundDistance = 1;
        [SerializeField] protected LayerMask treasureLayermask = 1024;

        [SerializeField] protected bool canRockClimb;
        [SerializeField] protected float stoneClimbSpeed = 4;

        protected bool isFalling;
        protected BoxCollider2D col;
        protected TweenerCore<Vector3, Vector3, VectorOptions> fallingTween;

        protected virtual void Awake()
        {
            col = GetComponent<BoxCollider2D>();
        }

        public virtual bool CanTarget()
        {
            return !isFalling;
        }

        public virtual void Die()
        {
            canFall = false;
            if (isFalling)
            {
                fallingTween?.Kill();
                OnDoneFalling();
            }
            Destroy(gameObject);
        }

        public virtual void Die(float delay)
        {
            canFall = false;
            if (isFalling)
            {
                fallingTween?.Kill();
                OnDoneFalling();
            }

            Invoke(nameof(Die), delay);
        }

        protected virtual void Start()
        {
            if (autoCaculateGroundDistance)
            {
                groundDistance = Physics2D.Raycast(transform.position, Vector2.down, Mathf.Infinity, wallLayerMask).distance;
            }
        }

        protected virtual void Update()
        {
            if (!canFall) return;
            if (isFalling) return;

            var hit = Physics2D.Raycast(transform.position, Vector2.down, Mathf.Infinity, wallLayerMask);
            var hit1 = Physics2D.Raycast(transform.position + new Vector3(col.size.x * transform.localScale.x / 2f, 0), Vector2.down, Mathf.Infinity, wallLayerMask);
            var hit2 = Physics2D.Raycast(transform.position - new Vector3(col.size.x * transform.localScale.x / 2f, 0), Vector2.down, Mathf.Infinity, wallLayerMask);
            //Debug.Log(hit.distance + " " + name);
            if (hit.distance <= groundDistance + 0.1f || hit1.distance <= groundDistance + 0.1f || hit2.distance <= groundDistance + 0.1f) return;
            isFalling = true;

            fallHitObj = hit;
            OnStartFalling();
            fallingTween = transform.DOMoveY(hit.point.y + groundDistance - 0.1f, hit.distance / fallSpeed).OnUpdate(OnFalling).OnComplete(OnDoneFalling);
        }
        protected RaycastHit2D fallHitObj;
        protected virtual void OnFalling()
        {

        }

        protected virtual void OnDoneFalling()
        {
            isFalling = false;
            PinQuizManager.instance.EnableRemovePin();
        }

        protected virtual void OnStartFalling()
        {
            PinQuizManager.instance.DisableRemovePins();
            SoundManager_BabyGirl.Instance.PlayOneShot("QuanSounds/Whistle Down");
        }

        private void OnDestroy()
        {
            if (isFalling)
            {
                PinQuizManager.instance.EnableRemovePin();
                fallingTween?.Kill();
            }
        }

        public virtual void BeingExplosed(PinQuizTNT bomb)
        {

        }

        public virtual void TouchSlime()
        {

        }

        public virtual void TouchLava(PinQuizLava lava)
        {

        }

        public Vector2 GetBottomPoint()
        {
            Vector2 size = col.size;
            float bottomCenterX = col.bounds.center.x;
            float bottomCenterY = col.bounds.center.y - size.y / 2f;
            return new Vector2(bottomCenterX, bottomCenterY);
        }

        TweenerCore<Vector3, Vector3, VectorOptions> climbTween;
        float targetY;
        protected virtual void ClimbStone(CircleCollider2D collision)
        {
            if (!canRockClimb) return;
            if (collision == null) return;
            if (1 << collision.gameObject.layer != PinQuizManager.instance.StoneLayerMask) return;
            var rb = collision.GetComponent<Rigidbody2D>();
            if (rb == null) return;
            if (rb.velocity.magnitude > 0.1f) return;
            if (transform.position.y - collision.transform.position.y > groundDistance + collision.radius) return;
            var targetY = collision.transform.position.y + collision.radius + groundDistance - 0.2f;
            if (Mathf.Abs(targetY - this.targetY) < 0.1f) return;
            if (targetY > this.targetY)
            {
                climbTween?.Kill();
                this.targetY = targetY;
            }
            climbTween = transform.DOMoveY(targetY, 1 / stoneClimbSpeed);
        }

        protected virtual void CheckUnclimbStone(CircleCollider2D collision)
        {
            if (!canRockClimb) return;
            if (collision == null) return;
            if (1 << collision.gameObject.layer != PinQuizManager.instance.StoneLayerMask) return;
            targetY = float.MinValue;

        }
        protected virtual void OnTriggerStay2D(Collider2D collision)
        {
            ClimbStone(collision as CircleCollider2D);
        }
        protected virtual void OnTriggerExit2D(Collider2D collision)
        {
            CheckUnclimbStone(collision as CircleCollider2D);
        }
        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {

        }
    }
    public enum EntityType
    {
        None = 0,
        Prince = 1,
        Princess = 2,
        Criminal = 3,
        Monster = 4,
        Gun = 5,
        TNT = 6,
        Coin = 7,
        Lava = 8,
        Slime = 9,
        Water = 10,
    }
}