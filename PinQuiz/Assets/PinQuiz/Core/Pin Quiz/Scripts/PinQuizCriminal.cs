using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using HongQuan;
using QuanUtilities;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace PinQuiz
{
    public class PinQuizCriminal : PinQuizEntity
    {
        [SerializeField] private Transform leftPoint;
        [SerializeField] private Transform rightPoint;
        [SerializeField] private float speed = 1;
        [SerializeField] private float physicSpeed = 5;
        [SerializeField] LayerMask checkWallLayermask;
        [SerializeField] float climbHeight = 0.2f;

        [SerializeField] public float findTreasureRadius = 2;
        [SerializeField] int minTreasureCodition = 10;
        [SerializeField] public Vector3 takeCoinOffset;

        [Header("Anim")]
        [SerializeField] SkeletonAnimation anim;
        [SerializeField, SpineAnimation] string moveAnim;
        [SerializeField, SpineAnimation] string idle;
        [SerializeField, SpineAnimation] string attack;
        [SerializeField, SpineAnimation] string richIdle;
        [SerializeField, SpineAnimation] string richMove;
        [SerializeField] ParticleSystem attackSmoke;

        [Header("Sound")]
        [SerializeField] AudioClip crySound;
        [SerializeField] AudioClip barkSound;

        [SerializeField] float delayCoin = 1f;
        [SerializeField] float delayChangeSkin = 3f;
        [SerializeField] float coinSpeed = 3f;
        [SerializeField] ParticleSystem smokeEff;

        private bool canCheck = true;
        private bool isMoving;
        private TweenerCore<Vector3, Vector3, VectorOptions> moveTween;
        private Rigidbody2D rb;

        private Transform _transform;
        protected override void Awake()
        {
            base.Awake();
            _transform = transform;
            rb = GetComponent<Rigidbody2D>();
        }

        protected override void Update()
        {
            base.Update();

            PhysicMove();

            if (!canCheck) return;
            if (isFalling) return;
            if (isMoving) return;
            if (getCoin) return;

            if (TakeCoin()) return;


            if (Check(leftPoint.position, Vector2.left)) return;
            if (Check(rightPoint.position, Vector2.right)) return;

            if (Check(leftPoint.position + new Vector3(0, 0.5f), Vector2.left)) return;
            if (Check(rightPoint.position + new Vector3(0, 0.5f), Vector2.right)) return;

            PlayIdleAnimation();
        }

        Vector2 physicMoveDir = Vector2.zero;
        PinQuizEntity targetEntity = null;
        private void PhysicMove()
        {
            if (canFall) return;
            if (!isMoving) return;
            if (physicMoveDir == Vector2.zero)
            {
                PlayIdleAnimation();
                return;
            }

            if (targetEntity != null)
            {
                bool canReach = Mathf.Abs(_transform.position.y - targetEntity.transform.position.y) < 2f;
                if (!targetEntity.CanTarget() || !canReach)
                {
                    targetEntity = null;
                }
            }
            else
            {
                isMoving = false;
                PlayIdleAnimation();
                PinQuizManager.instance.EnableRemovePin();
                return;
            }

            Vector2 moveTarget = rb.position + physicMoveDir * Time.deltaTime * physicSpeed;

            //Climb
            if (rb.velocity.y < 0.1f)
            {
                var bottom = GetBottomPoint();
                var checkPoint = bottom + physicMoveDir * col.bounds.size.x / 2;
                if (Physics2D.Raycast(checkPoint, physicMoveDir, 0.1f, wallLayerMask)
                    && !Physics2D.Raycast(checkPoint + Vector2.up * climbHeight, physicMoveDir, 0.1f, wallLayerMask))
                {
                    moveTarget += Vector2.up * Time.deltaTime * physicSpeed;
                }
            }

            rb.MovePosition(moveTarget);

            PlayMoveAnimation();
        }

        bool getCoin;

        private bool TakeCoin()
        {
            if (getCoin) return true;
            var treasure = Physics2D.OverlapCircleAll(_transform.position + takeCoinOffset, findTreasureRadius, treasureLayermask);
            if (treasure.Length < minTreasureCodition) return false;
            getCoin = true;
            moveTween?.Kill();
            foreach (var coin in PinQuizManager.instance.Coins)
            {
                coin.DisablePhysic();
                Sequence coinMove = DOTween.Sequence();
                Vector3 peakPoint = transform.position + Vector3.up * 3 +
                    Vector3.right * 2 * (transform.position.x < coin.transform.position.x ? 1f : -1f);
                coinMove.SetDelay(Vector2.Distance(coin.transform.position, _transform.position) * delayCoin);
                coinMove.Append(coin.transform.DOMove(peakPoint, Vector2.Distance(peakPoint, coin.transform.position) / coinSpeed).SetEase(Ease.Linear).OnComplete(() =>
                {
                    SoundManager_BabyGirl.Instance.PlayOneShot("unlock");
                    coin.FadeRenderer(0, Vector2.Distance(_transform.position, coin.transform.position) / coinSpeed);
                    Vector3 pos = new Vector3(_transform.position.x + Random.Range(-1.1f, 1.1f), _transform.position.y + Random.Range(0.1f, 3.0f), _transform.position.z);
                    PinQuizManager.instance.ShowEff(1.75f, pos, PinQuizManager.TypeEff.CoinCollect);
                    /*  var eff = Instantiate(PinQuizManager.instance.effectCoinCollect);
                      eff.transform.position = new Vector3(_transform.position.x + Random.Range(-1.1f, 1.1f), _transform.position.y + Random.Range(0.1f, 3.0f), _transform.position.z);
                      eff.transform.localScale = Vector3.one * 0.5f;
                      eff.SetActive(true);
                      Destroy(eff, 3);*/
                }));
                coinMove.Append(coin.transform.DOMove(_transform.position,
                    Vector2.Distance(_transform.position, peakPoint) / coinSpeed).SetEase(Ease.Linear));
                //coinMove.OnComplete(() =>
                //{
                //    SoundManager_BabyGirl.Instance.PlayOneShot($"Sounds/unlock");
                //});
                PinQuizManager.instance.princess.DogTakeCoins(this);
            }
            this.DelayFunction(delayChangeSkin, () =>
            {
                //Lose game
                PlayRichAnimation();
                smokeEff.Play();
                Debug.Log("Lose");
            });
            return true;
        }

        private void PlayRichAnimation()
        {
            if (anim.AnimationName != richIdle)
                anim.PlayAnimation(richIdle, true, 0.2f);
        }

        private void OnDrawGizmos()
        {
            //Gizmos.color = Color.yellow;
            //Gizmos.DrawWireSphere(transform.position + takeCoinOffset, findTreasureRadius);
        }

        private bool Check(Vector2 point, Vector2 dir)
        {
            var wall = Physics2D.Raycast(point, dir, 100, checkWallLayermask);

            if (!wall)
                return CheckEntity();
            else if (!wall.transform.CompareTag("Obstacle"))
                return CheckEntity();
            return false;

            bool CheckEntity()
            {
                var hits = Physics2D.RaycastAll(point, dir, 100, ~treasureLayermask);
               // print("Hits count = " + hits.Length);
                foreach (var hit in hits)
                {
                    if (hit.transform.TryGetComponent(out PinQuizEntity e))
                    {
                        if (e.CanTarget())
                        {
                            if (e.EntityType == EntityType.Prince || e.EntityType == EntityType.Princess)
                            {
                                isMoving = true;
                                if (canFall)
                                {
                                    moveTween = _transform.DOMoveX(hit.transform.position.x, hit.distance / speed).OnComplete(OnDoneMove).OnUpdate(() => CheckCanReachEntity(e)).SetEase(Ease.Linear);
                                }
                                else
                                {
                                    targetEntity = e;
                                    physicMoveDir = dir;
                                }
                                PlayMoveAnimation();
                                PinQuizManager.instance.DisableRemovePins();
                                if (e.EntityType == EntityType.Princess)
                                {
                                    princess = (PinQuizPrincess)e;
                                    ((PinQuizPrincess)e).canWinGame = false;
                                }
                                HeadToTarget(e.transform.position);
                                return true;
                            }
                        }
                        else if (e.EntityType == EntityType.Princess)
                        {
                            princess = (PinQuizPrincess)e;
                            ((PinQuizPrincess)e).canWinGame = false;
                        }
                    }
                }
                return false;
            }
        }

        private void PlayMoveAnimation()
        {
            if (anim.AnimationName != moveAnim)
                anim.PlayAnimation(moveAnim, true, 0.2f);
        }

        private void PlayIdleAnimation()
        {
            if (anim.AnimationName != idle)
                anim.PlayAnimation(idle, true);
        }
        private void PlayAttackAnimtion()
        {
            if (anim.AnimationName != attack)
                anim.PlayAnimation(attack, true, 0.2f);
        }

        private void HeadToTarget(Vector3 targetPos)
        {
            if (targetPos.x > _transform.position.x)
                _transform.localScale = new Vector3(-1, 1, 1);
            else
                _transform.localScale = Vector3.one;
        }

        PinQuizPrincess princess;

        private void OnDoneMove()
        {
            isMoving = false;
            PinQuizManager.instance.EnableRemovePin();
        }

        private void CheckCanReachEntity(PinQuizEntity entity)
        {
            bool canReach = Mathf.Abs(_transform.position.y - entity.transform.position.y) < .5f;
            if (entity.CanTarget() && canReach) return;
            moveTween?.Kill();
            isMoving = false;
            OnDoneMove();
        }


        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            base.OnTriggerEnter2D(collision);
            if (collision.TryGetComponent(out PinQuizEntity entity))
            {
                if (entity.EntityType == EntityType.Prince || entity.EntityType == EntityType.Princess)
                {
                    AttackPlayer((PinQuizPrincess)entity);
                }
            }
        }

        private void AttackPlayer(PinQuizPrincess princess)
        {
            // Manager.instance.soundManager.PlayOneShot(barkSound);
            
            SoundManager_BabyGirl.Instance.PlayOneShot(barkSound);
            moveTween?.Kill();
            physicMoveDir = Vector2.zero;
            //var smoke = Instantiate(attackSmoke);
            attackSmoke.transform.SetParent(princess.transform, false);
            attackSmoke.transform.localPosition = Vector3.zero;
            attackSmoke.Play();
            princess.PlayScareAnim();
            PlayAttackAnimtion();
            this.DelayFunction(1, () =>
            {
                col.enabled = false;
                canCheck = false;
                canFall = false;
                //_transform.DOMove(transform.position + new Vector3(15 * transform.localScale.x, -20), 4).SetEase(Ease.Linear);
                princess.BeTakenShoe();
                PlayIdleAnimation();
            });
        }


        public override void Die()
        {
            if (princess != null)
                princess.canWinGame = true;
            if (isMoving)
                PinQuizManager.instance.EnableRemovePin();
            base.Die();
        }

        public override void Die(float delay)
        {
            canCheck = false;
            base.Die(delay);
        }

        public override void BeingExplosed(PinQuizTNT bomb)
        {
            base.BeingExplosed(bomb);
            col.enabled = false;
            canCheck = false;
            canFall = false;

           // Manager.instance.soundManager.PlayOneShot(crySound);
            SoundManager_BabyGirl.Instance.PlayOneShot(crySound);
            transform.DOMoveY(transform.position.y + 10, 2);
            transform.DORotate(new Vector3(0, 0, 720), 2, RotateMode.LocalAxisAdd);
            DOVirtual.Color(Color.white, new Color(1, 1, 1, 0), 1, color =>
            {
                anim.SetColor(color);
            }).SetDelay(1).OnComplete(Die);

        }

        public override void TouchLava(PinQuizLava lava)
        {
            base.TouchLava(lava);
            Die();
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(PinQuizCriminal)), CanEditMultipleObjects]
    public class PinQuizCriminalEditor : Editor
    {
        private void OnSceneGUI()
        {
            var t = target as PinQuizCriminal;

            Handles.color = Color.yellow;
            Undo.RecordObject(target, "Change Check Radius and offset get coin criminal");
            t.findTreasureRadius = Handles.RadiusHandle(Quaternion.identity,
                t.transform.position + t.takeCoinOffset, t.findTreasureRadius);
            var fmh_371_96_638636356162262768 = Quaternion.identity; t.takeCoinOffset = Handles.FreeMoveHandle(t.transform.position + t.takeCoinOffset,0.2f, Vector3.one * 0.1f, Handles.DotHandleCap) - t.transform.position;
        }
    }

#endif
}