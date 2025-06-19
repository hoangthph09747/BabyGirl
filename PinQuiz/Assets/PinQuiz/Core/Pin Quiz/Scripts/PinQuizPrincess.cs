using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using QuanUtilities;
using Spine.Unity;
using UnityEngine;
using Observer;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace PinQuiz
{
    public class PinQuizPrincess : PinQuizEntity 
    {
        public VectorPath path;
        public int pathIndex;
        public bool isMoving;
        public float speed = 10;

        public bool isHaveTreasure = true;
        public float findTreasureRadius = 2;
        public float princessCheckTresureRadius = 2;
        public int minTreasureCodition = 6;

        public bool islavaCheck;
        public float lavaCheckRadius = 1;
        public Vector3 lavaCheckPos;
        public int minLavaCondition = 2;
        public LayerMask lavaLayerMask;
        public bool mayTouchLava;
        public float minHeightToUseParachute = 1f;
        public LayerMask checkMonsterLayermask;

        public bool endGame;
        public bool canWinGame = true;
        public LayerMask characterLayerMask = 7;
        public PinQuizPrincessDream dream;
        public ParticleSystem fireAnim;

        [Header("Can Lose for Treasure Stuck")]
        public bool canLoseForTreasureStuck;
        public Vector3 stuckPoint;

        [SerializeField] SkeletonAnimation anim;
        [SerializeField, SpineAnimation] string idleAnim;
        [SerializeField, SpineAnimation] string moveAnim;
        [SerializeField, SpineAnimation] string scareAnim;
        [SerializeField, SpineAnimation] string beAttackedAnim;
        [SerializeField, SpineAnimation] string[] winAnim;
        [SerializeField, SpineAnimation] string[] beDirtyAnim;
        [SerializeField, SpineAnimation] string slimeTouchTreasure;
        [SerializeField, SpineAnimation] string[] exploseAnim;
        [SerializeField, SpineAnimation] string coinStuckAnim;
        [SerializeField, SpineAnimation] string parachuteAnim;
        [SerializeField, SpineAnimation] string touchLavaAnim;

        private TweenerCore<Vector3, Vector3, VectorOptions> moveTween;

        protected override void Start()
        {
            base.Start();
            PinQuizManager.instance.princess = this;
            PlayIdleAnim();
            Dream();
        }

        protected override void Update()
        {
            base.Update();
            LavaCheck();
            Move();
            CheckMonsterAndCriminal();
            CheckTreasureStuck();
        }

        private void LateUpdate()
        {
            TreasureChecking();
        }

        private void CheckMonsterAndCriminal()
        {
            if (isMoving) return;
            if (endGame) return;
            if (isFalling) return;
            //var hits = Physics2D.RaycastAll(transform.position + Vector3.left * 10 + Vector3.up, Vector3.right, 20, characterLayerMask);
            //foreach (var hit in hits)
            //{
            //    if (hit.transform.TryGetComponent(out PinQuizEntity entity))
            //    {
            //        if (entity.EntityType == EntityType.Criminal || entity.EntityType == EntityType.Monster)
            //        {
            //            PlayScareAnim();
            //            //print("Check dog");
            //            canWinGame = false;
            //            return;
            //        }
            //    }
            //}
            //canWinGame = true;
            //PlayIdleAnim();

            var hitLeft = Physics2D.Raycast(transform.position + Vector3.up + Vector3.left, Vector3.left, 10, checkMonsterLayermask);
            if (hitLeft)
            {
                if (hitLeft.transform.TryGetComponent(out PinQuizEntity e))
                {
                    if (e.EntityType == EntityType.Criminal || e.EntityType == EntityType.Monster)
                    {
                        PlayScareAnim();
                        //print("Check dog");
                        canWinGame = false;
                        return;
                    }
                }
            }

            var hitRight = Physics2D.Raycast(transform.position + Vector3.up + Vector3.right, Vector3.right, 10, checkMonsterLayermask);
            if (hitRight)
            {
                if (hitRight.transform.TryGetComponent(out PinQuizEntity e1))
                {
                    if (e1.EntityType == EntityType.Criminal || e1.EntityType == EntityType.Monster)
                    {
                        PlayScareAnim();
                        //print("Check dog");
                        canWinGame = false;
                        return;
                    }
                }
            }
            canWinGame = true;

        }

        protected override void OnStartFalling()
        {
            base.OnStartFalling();
            moveTween?.Kill();
            if (fallHitObj.distance > minHeightToUseParachute)
                PlayParachuteJumpAnim();
            else
                PlayIdleAnim();
#if UNITY_EDITOR
            Debug.Log("Fall Distance" + fallHitObj.distance);
#endif
        }

        protected override void OnDoneFalling()
        {
            PlayIdleAnim();
            RecaculatePathIndex();
            base.OnDoneFalling();
            CheckMonsterAndCriminal();
        }

        private void RecaculatePathIndex()
        {
            isMoving = false;
            moveTween?.Kill();
            float minDis = Mathf.Infinity;
            int res = 0;

            for (int i = 0; i < path.path.Length; i++)
            {
                float dis = Vector2.SqrMagnitude(path.path[i] - transform.position);
                if (dis < minDis)
                {
                    minDis = dis;
                    res = i;
                }
            }

            pathIndex = res;
        }

        public void PlayScareAnim()
        {
            if (anim.AnimationName != scareAnim)
                anim.PlayAnimation(scareAnim, true, 0.2f);
        }

        private void PlayIdleAnim()
        {
            if (anim.AnimationName != idleAnim)
                anim.PlayAnimation(idleAnim, true, 0.2f);
        }

        private void PlayParachuteJumpAnim()
        {
            if (anim.AnimationName != parachuteAnim)
                anim.PlayAnimation(parachuteAnim, true, 0.2f);
        }


        private void PlayWinAnimation()
        {
            SoundManager_BabyGirl.Instance.PlayOneShot("Sounds/success1");
            Vector3 pos = new Vector3(anim.transform.position.x, anim.transform.position.y + 1, anim.transform.position.z);
            PinQuizManager.instance.ShowEff(3, pos, PinQuizManager.TypeEff.ChangeSkinPrincessWin);
            //GameObject effect = Instantiate(PinQuizManager.instance.effectChangeSkinPrincessWin);
            //effect.transform.position = new Vector3(anim.transform.position.x, anim.transform.position.y + 1, anim.transform.position.z);
            //effect.transform.localScale = Vector3.one * 3;
            //Destroy(effect, 3);

            //anim.SetSkin("dress2");
            //anim.PlayAnimation(winAnim[Random.Range(0, winAnim.Length)], true, 0.2f);
            //anim.gameObject.SetActive(false);
            anim.transform.localScale = Vector3.zero;

            PinQuizManager.instance.endPrincessAnim.gameObject.SetActive(true);
            PinQuizManager.instance.endPrincessAnim.transform.position = anim.transform.position;
            PinQuizManager.instance.winAnimLocalScale = new Vector3(Random.Range(0, 2) == 0 ? 1 : -1, 1, 1);
            PinQuizManager.instance.endPrincessAnim.transform.localScale = PinQuizManager.instance.winAnimLocalScale * 0.26f;
            PinQuizManager.instance.endPrincessAnim.RandomWinAnim();
        }

        private void LavaCheck()
        {
            if (!islavaCheck) return;

            var lava = Physics2D.OverlapCircleAll(lavaCheckPos, lavaCheckRadius, lavaLayerMask);
            mayTouchLava = (lava.Length - minLavaCondition) > 0;
            if (mayTouchLava)
            {
                ForceStopMoving();
            }
        }

        private void PlayCoinStuckAnim()
        {
            if (anim.AnimationName != coinStuckAnim)
                anim.PlayAnimation(coinStuckAnim, true, 0.2f);
        }

        private void Move()
        {
            if (endGame) return;
            if (!canWinGame) return;
            if (isFalling) return;
            if (isMoving) return;
            if (mayTouchLava) return;
            if (!IsFreePath()) return;
            RecaculatePathIndex();
            if (pathIndex >= path.path.Length - 1) return;
            isMoving = true;
            PinQuizManager.instance.DisableRemovePins();
            anim.PlayAnimation(moveAnim, true, 0.2f);
            moveTween = transform.DOMoveX(path.path[pathIndex + 1].x, Mathf.Abs(transform.position.x - path.path[pathIndex + 1].x) / speed).OnComplete(() =>
            {
                isMoving = false;
                PinQuizManager.instance.EnableRemovePin();
                anim.PlayAnimation(idleAnim, true, 0.2f);
            }).SetEase(Ease.Linear);
        }

        private void ForceStopMoving()
        {
            if (!isMoving) return;
            isMoving = false;
            moveTween?.Kill();
            PinQuizManager.instance.EnableRemovePin();
            anim.PlayAnimation(idleAnim, true, 0.2f);
            CheckMonsterAndCriminal();
        }

        private bool IsFreePath()
        {
            if (path.path.Length < 2) return true;

            for (int i = 0; i < path.path.Length - 1; i++)
            {
                Vector3 dir = path.path[i + 1] - path.path[i];
                float dis = Vector3.Distance(path.path[i + 1], path.path[i]);
                var hit = Physics2D.Raycast(path.path[i], dir.normalized, dis, wallLayerMask);
                if (hit)
                {
                    return false;
                }
            }

            if (isHaveTreasure)
            {
                Vector3 pos = path.path[path.path.Length - 1];
                return IsGetTreasure(pos, findTreasureRadius);
            }

            return true;
        }

        private bool IsGetTreasure(Vector3 pos, float radius)
        {
            var treasure = Physics2D.OverlapCircleAll(pos, radius, treasureLayermask);
            //Debug.Log("Coin Count = " + treasure.Length);
            if (treasure.Length < minTreasureCodition) return false;
            return true;
        }

        private void TreasureChecking()
        {
            if (!isHaveTreasure) return;
            if (!canWinGame) return;
            if (isFalling) return;
            //if (isMoving) return;
            if (endGame) return;
            if (IsGetTreasure(transform.position, princessCheckTresureRadius))
            {
                Time.timeScale = 1.5f;
                endGame = true;
                moveTween?.Kill();

                PlayIdleAnim();
                this.PostEvent(EventID.OnWinPinQuizPrincess, gameObject);

                //CoinManager.instance.GetListCoin();
                //CoinManager.instance.MoveTo(transform);
                Invoke(nameof(PlayWinAnimation), 1);
                Invoke(nameof(Win), 3.5f);
            }
        }

        public void Win()
        {
            PinQuizManager.instance.WinGame();
            //PlayWinAnimation();
        }

        private void OnDrawGizmos()
        {
            //Gizmos.DrawLine(transform.position - Vector3.right * 10 + Vector3.up, transform.position - Vector3.left * 10 + Vector3.up);
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position + Vector3.up + Vector3.right, transform.position + Vector3.up + Vector3.right * 10);
            Gizmos.DrawLine(transform.position + Vector3.up + Vector3.left, transform.position + Vector3.up + Vector3.left * 10);

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, princessCheckTresureRadius);
            if (path != null)
                if (path.path.Length > 0)
                    Gizmos.DrawWireSphere(path.path[path.path.Length - 1], findTreasureRadius);

            if (canLoseForTreasureStuck)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(stuckPoint, findTreasureRadius);
            }
        }

        public void MeetPrince(PinQuizPrince prine)
        {
            moveTween?.Kill();
            PinQuizManager.instance.WinGame();
        }

        public override void Die()
        {
            PinQuizManager.instance.LoseGame();
            base.Die();
        }

        public override void Die(float delay)
        {
            canWinGame = false;
            endGame = true;
            base.Die(delay);
        }

        private void CheckTreasureStuck()
        {
            if (!canLoseForTreasureStuck) return;
            if (endGame) return;
            if (IsGetTreasure(stuckPoint, findTreasureRadius))
            {
                canWinGame = false;
                endGame = true;
                PinQuizManager.instance.LoseGame(EndGameType.CoinStuck);
                canLoseForTreasureStuck = false;
                PlayCoinStuckAnim();
                SoundManager_BabyGirl.Instance.PlayOneShot("FishingSounds/Sob");
            }
        }

        public void BeTakenShoe()
        {
            endGame = true;
            moveTween?.Kill();
            anim.PlayAnimation(beAttackedAnim, true, 0.2f);
            this.DelayFunction(1, () => PinQuizManager.instance.LoseGame(Random.Range(0, 2) == 0 ? EndGameType.DogTakeShoe : EndGameType.DogLick));
          //  SoundManager_BabyGirl.Instance.PlayOneShot("FishingSounds/Sob");
        }

        public override void TouchSlime()
        {
            if (endGame) return;
            endGame = true;
            moveTween?.Kill();
            anim.PlaySequanceAnimations(beDirtyAnim[0], beDirtyAnim[1]);
            PinQuizManager.instance.LoseGame(EndGameType.Slime);
          //  SoundManager_BabyGirl.Instance.PlayOneShot("FishingSounds/Sob");
            base.TouchSlime();
        }

        public void SlimeTouchTreasure()
        {
            if (endGame) return;
            endGame = true;
            moveTween?.Kill();
            anim.PlayAnimation(slimeTouchTreasure, true, 0.5f);
            PinQuizManager.instance.LoseGame(EndGameType.SlimeTouchTreasure);
        //    SoundManager_BabyGirl.Instance.PlayOneShot("FishingSounds/Sob");
        }

        public void LavaTouchTreasure()
        {
            if (endGame) return;
            endGame = true;
            moveTween?.Kill();
            anim.PlayAnimation(slimeTouchTreasure, true, 0.5f);
            PinQuizManager.instance.LoseGame(EndGameType.CoinStuck);
            SoundManager_BabyGirl.Instance.PlayOneShot("FishingSounds/Sob");
        }

        public override void BeingExplosed(PinQuizTNT bomb)
        {
            if (endGame) return;
            endGame = true;
            anim.PlaySequanceAnimations(exploseAnim[0], exploseAnim[1]);
            SoundManager_BabyGirl.Instance.PlayOneShot("FishingSounds/Sob");
            PinQuizManager.instance.LoseGame(Random.Range(0, 2) == 0 ? EndGameType.Bomb2 : EndGameType.Bomb);
        }

        public void TreasureDestroyed()
        {
            if (endGame) return;
            endGame = true;
            moveTween?.Kill();
            PlayCoinStuckAnim();
            PinQuizManager.instance.LoseGame(EndGameType.CoinStuck);
        }

        public override void TouchLava(PinQuizLava lava)
        {
            if (endGame) return;
            endGame = true;
            moveTween?.Kill();
            anim.PlayAnimation(touchLavaAnim, true, 0.1f);
            PinQuizManager.instance.LoseGame(EndGameType.Fire);
            SoundManager_BabyGirl.Instance.PlayOneShot("FishingSounds/Sob");
            fireAnim.gameObject.SetActive(true);
            fireAnim.Play();
            base.TouchLava(lava);
        }

        public void DogTakeCoins(PinQuizCriminal dog)
        {
            if (endGame) return;
            endGame = true;
            moveTween?.Kill();
            PlayCoinStuckAnim();
            PinQuizManager.instance.LoseGame(dog.transform, EndGameType.MonsterGetCoins, 5);
            SoundManager_BabyGirl.Instance.PlayOneShot("FishingSounds/Sob");
        }

        public void Dream()
        {
            if (PinQuizManager.instance.LevelIndex != 0) return;
            if (PinQuizManager.instance.isDream) return;

            if (PinQuizManager.instance.isDream) return;
            PinQuizManager.instance.isDream = true;

            endGame = true;
            PinQuizManager.instance.DisableRemovePins();

            SetOverlay();

            PinQuizManager.instance.EnableRemovePin();
            endGame = false;
            ResetSortingGroup();

           /* dream.Dream(() =>
            {
                PinQuizManager.instance.EnableRemovePin();
                endGame = false;
                ResetSortingGroup();
            });*/
        }

        int baseSortingOrder;
        public void SetOverlay()
        {
            baseSortingOrder = anim.GetComponent<MeshRenderer>().sortingOrder;
            anim.GetComponent<MeshRenderer>().sortingOrder = 9;
        }

        public void ResetSortingGroup()
        {
            anim.GetComponent<MeshRenderer>().sortingOrder = baseSortingOrder;
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(PinQuizPrincess)), CanEditMultipleObjects]
    public class PinQuizPrincessEditor : Editor
    {
        private void OnSceneGUI()
        {
            var princess = target as PinQuizPrincess;

            if (princess.islavaCheck)
            {
                Undo.RecordObject(target, "Change Check Lava Point");
                Handles.color = new Color(1f, .5f, .314f, 1);

                 var fmh_507_88_638636356165794273 = Quaternion.identity; princess.lavaCheckPos = Handles.FreeMoveHandle(princess.lavaCheckPos,0.2f, Vector3.one * 0.1f, Handles.DotHandleCap);
                princess.lavaCheckRadius = Handles.RadiusHandle(Quaternion.identity,
                    princess.lavaCheckPos, princess.lavaCheckRadius);
            }

            if (princess.canLoseForTreasureStuck)
            {
                Handles.color = Color.red;
                Undo.RecordObject(target, "Change Check Stuck Point");
                 var fmh_516_84_638636356165797453 = Quaternion.identity; princess.stuckPoint = Handles.FreeMoveHandle(princess.stuckPoint,0.2f, Vector3.one * 0.1f, Handles.DotHandleCap);
            }

            Undo.RecordObject(target, "Change Check Radius");
            princess.findTreasureRadius = Handles.RadiusHandle(Quaternion.identity,
                princess.transform.position, princess.findTreasureRadius);

            Undo.RecordObject(target, "Change Princess Check Radius");
            princess.princessCheckTresureRadius = Handles.RadiusHandle(Quaternion.identity,
                princess.transform.position, princess.princessCheckTresureRadius);
        }
    }
#endif
}


