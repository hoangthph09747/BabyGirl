/*using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Core.PathCore;
using DG.Tweening.Plugins.Options;*/
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Events;

namespace ToiletRush
{
    public class ToiletRushCharacter : MonoBehaviour
    {
        [SerializeField] private float speed = 10f;
        [SerializeField] private float duration = 3f;
        [SerializeField] private ToiletRushToilet toilet;
        //[SerializeField] private ParticleSystem hitEff;
        [SerializeField] private ToiletRushGuideLine guideLine;
        [SerializeField] private Gender gender;
        [SerializeField] private GameObject water;
        [SerializeField] private ParticleSystem shitEff;
        [SerializeField] private BoxCollider2D drawZoneBoxCollider;

        [SerializeField, SpineAnimation] string idleAnimation;
        [SerializeField, SpineAnimation] string moveAnimation;
        [SerializeField, SpineAnimation] string goToToiletAnimation;
        [SerializeField, SpineAnimation] string winAnimation;
        [SerializeField, SpineAnimation] string bananaAnimation;
        [SerializeField, SpineAnimation] string gumAnimation;
        [SerializeField, SpineAnimation] string beetleAnimation;
        [SerializeField, SpineAnimation] string crabAnimation;
        [SerializeField, SpineAnimation] string skateBoardAnimation;
        [SerializeField, SpineAnimation] string catAnimation;
        [SerializeField, SpineAnimation] string ponyAnimation;
        [SerializeField, SpineAnimation] string spiderAnimation;
        [SerializeField, SpineAnimation] string tombAnimation;
        [SerializeField, SpineAnimation] string ghostAnimation;
        [SerializeField, SpineAnimation] string zombieAnimation;
        [SerializeField, SpineAnimation] string reindeerAnimation;
        [SerializeField, SpineAnimation] string snowmanAnimation;

        [SerializeField] AudioClip hitSound;
        [SerializeField] AudioClip crySound;
        [SerializeField] AudioClip walkSound;
        private ToiletRushLevel level;
    //    private TweenerCore<Vector3, Path, PathOptions> moveTween;
        private SkeletonAnimation anim;
        private Collider2D col;
        private Vector3[] movePath = null;

        public Transform BeetlePos;
        public Transform CrabPos;
        public Transform SkateBoardPos;
        public Transform CatBoardPos;
        public Transform SpderPos;
        public Transform GhostPos;
        public Transform ZombiePos;
        public ToiletRushToilet Toilet => toilet;
        public ToiletRushGuideLine GuideLine => guideLine;
        public bool HavePath => movePath != null;
        public Gender Gender => gender;

        private void Awake()
        {
            anim = GetComponentInChildren<SkeletonAnimation>();
            tag = ToiletRushManager.CharacterTag;
            col = GetComponentInChildren<Collider2D>();
        }

        private void Start()
        {
            anim.state.SetAnimation(0, idleAnimation, true);
        }
        /*
               // [BurstCompile]
                public struct DistanceJob : IJobParallelFor
                {
                    [ReadOnly] public NativeArray<Vector3> path;
                    public NativeArray<float> distances;

                    public void Execute(int index)
                    {
                        if (index < path.Length - 1)
                        {
                            distances[index] = Vector2.Distance(path[index], path[index + 1]);
                        }
                        else
                        {
                            distances[index] = 0f;
                        }
                    }
                }

                float CalculateTotalDistance()
                {
                    NativeArray<Vector3> pathArray = new NativeArray<Vector3>(movePath, Allocator.TempJob);
                    NativeArray<float> distances = new NativeArray<float>(movePath.Length, Allocator.TempJob);

                    DistanceJob distanceJob = new DistanceJob
                    {
                        path = pathArray,
                        distances = distances
                    };

                    JobHandle jobHandle = distanceJob.Schedule(movePath.Length - 1, 64);
                    jobHandle.Complete();

                    float totalDistance = 0;
                    for (int i = 0; i < distances.Length; i++)
                    {
                        totalDistance += distances[i];
                    }

                    pathArray.Dispose();
                    distances.Dispose();

                    return totalDistance;
                }*/

        public void SetToilet(ToiletRushToilet toilet)
        {
            this.toilet = toilet;
        }

        public void Move()
        {
            //float distance = CalculateTotalDistance();
            //float duration = distance / speed;
            anim.state.SetAnimation(0, moveAnimation, true).MixTime = .2f;
            /*moveTween = transform.DOPath(movePath, duration).SetEase(Ease.Linear).OnComplete(() =>
            {
                anim.state.SetAnimation(0, goToToiletAnimation, false).MixTime = .2f;
                anim.state.AddAnimation(0, winAnimation, true, 0).MixTime = .2f;
                toilet.GoToToilet(this);
            });*/

            MoveWithoutDOTween();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.CompareTag(ToiletRushManager.CharacterTag)) return;
            //moveTween?.Kill();

            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
                moveCoroutine = null;
            }
            //Instantiate(hitEff, transform.position, Quaternion.identity);
            ToiletRushManager.instance.showHitEff(transform.position);
            anim.gameObject.SetActive(false);
            ToiletRushManager.instance.LoseGameForHit();
            SoundManager_BabyGirl.Instance.PlaySoundEffectOneShot(hitSound);
        }

        public void TouchBanana()
        {
            //moveTween?.Kill();
            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
                moveCoroutine = null;
            }
            anim.state.SetAnimation(0, bananaAnimation, false).MixTime = .2f;
            col.enabled = false;
            ToiletRushManager.instance.LoseForTouchBanana(gender);
        }

        private void OnTouchMonster()
        {
            //  moveTween?.Kill();
            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
                moveCoroutine = null;
            }
            col.enabled = false;
            SoundManager_BabyGirl.Instance.PlaySoundEffectOneShot(crySound);
        }

        public void TouchGum()
        {
            OnTouchMonster();
            anim.state.SetAnimation(0, gumAnimation, false).MixTime = .2f;
            water.SetActive(true);
            ToiletRushManager.instance.LoseForTouchGum(gender);
        }

        public void TouchBeetle()
        {
            OnTouchMonster(); ;
            anim.state.SetAnimation(0, beetleAnimation, true).MixTime = .2f;
            water.SetActive(true);
            ToiletRushManager.instance.LoseForTouchBeetle(gender);
        }
        public void TouchCrab()
        {
            OnTouchMonster();
            anim.state.SetAnimation(0, beetleAnimation, true).MixTime = .2f;
            water.SetActive(true);
            ToiletRushManager.instance.LoseForTouchCrab(gender);
        }
        public void TouchGhost()
        {
            OnTouchMonster();
            anim.state.SetAnimation(0, ghostAnimation, true).MixTime = .2f;
            water.SetActive(true);
            shitEff.Play();
            ToiletRushManager.instance.LoseForTouchGhost(gender);
        }

        public void TouchSkateBoard()
        {
            OnTouchMonster();
            anim.state.SetAnimation(0, skateBoardAnimation, true).MixTime = .2f;
            shitEff.Play();
            ToiletRushManager.instance.LoseForTouchSkateBoard(gender);
           // transform.DOMoveX(transform.position.x + 1, .2f).SetLoops(-1, LoopType.Yoyo);
        }
        public void TouchSnowSkateBoard()
        {
            OnTouchMonster();
            anim.state.SetAnimation(0, skateBoardAnimation, true).MixTime = .2f;
            shitEff.Play();
            ToiletRushManager.instance.LoseForTouchSnowSkateBoard(gender);
          //  transform.DOMoveX(transform.position.x + 1, .2f).SetLoops(-1, LoopType.Yoyo);
        }

        public void TouchTomb()
        {
            OnTouchMonster();
            anim.state.SetAnimation(0, tombAnimation, true).MixTime = .2f;
            shitEff.Play();
            water.SetActive(true);
            ToiletRushManager.instance.LoseForTouchTomb(gender);
        }

        public void TouchCat()
        {
            OnTouchMonster();
            anim.state.SetAnimation(0, catAnimation, true).MixTime = .2f;
            shitEff.Play();
            water.SetActive(true);
            ToiletRushManager.instance.LoseForTouchCat(gender);
        }
        public void TouchPony()
        {
            OnTouchMonster();
            anim.state.SetAnimation(0, ponyAnimation, true).MixTime = .2f;
            shitEff.Play();
            ToiletRushManager.instance.LoseForTouchPony(gender);
        }
        public void TouchSpider()
        {
            OnTouchMonster();
            anim.state.SetAnimation(0, spiderAnimation, true).MixTime = .2f;
            water.SetActive(true);
            shitEff?.Play();
            ToiletRushManager.instance.LoseForSpider(gender);
        }
        public void TouchZombie()
        {
            OnTouchMonster();
            anim.state.SetAnimation(0, zombieAnimation, true).MixTime = .2f;
            water.SetActive(true);
            shitEff?.Play();
            ToiletRushManager.instance.LoseForTouchZombie(gender);
        }
        public void TouchReindeer()
        {
            OnTouchMonster();
            anim.state.SetAnimation(0, reindeerAnimation, true).MixTime = .2f;
            ToiletRushManager.instance.LoseForTouchReindeer(gender);
        }

        public void TouchSnowman()
        {
            OnTouchMonster();
            anim.state.SetAnimation(0, snowmanAnimation, true).MixTime = .2f;
            ToiletRushManager.instance.LoseForSnowman(gender);
        }

        public void SetLevel(ToiletRushLevel level)
        {
            this.level = level;
        }

        public void SetMovePath(List<Vector3> movePath)
        {
            movePath = FindPath(movePath);
            movePath = RemoveRedundantPoints(movePath);
            this.movePath = movePath.ToArray();
        }

        private List<Vector3> FindPath(List<Vector3> movePath)
        {
            if(movePath == null || movePath.Count == 0) return null;
            int startIndex = -1;
            Rect playerBoxRect = new Rect(
               drawZoneBoxCollider.bounds.min.x,
               drawZoneBoxCollider.bounds.min.y,
               drawZoneBoxCollider.bounds.size.x,
               drawZoneBoxCollider.bounds.size.y
            );
            Rect toiletBoxRect = new Rect(
               toilet.DrawZoneBoxCollider.bounds.min.x,
               toilet.DrawZoneBoxCollider.bounds.min.y,
               toilet.DrawZoneBoxCollider.bounds.size.x,
               toilet.DrawZoneBoxCollider.bounds.size.y
            );

            for (int i = 0; i < movePath.Count; i++)
            {
                if (playerBoxRect.Contains((Vector2)movePath[i]))
                {
                    startIndex = i;
                    break;
                }
            }

            if (startIndex == -1) return null;

            int toiletIndexLeft = -1, toiletIndexRight = -1;
            for (int i = startIndex; i < movePath.Count; i++)
            {
                if (toiletBoxRect.Contains((Vector2)movePath[i]))
                {
                    toiletIndexRight = i;
                    break;
                }
            }
            for (int i = startIndex; i >= 0; i--)
            {
                if (toiletBoxRect.Contains((Vector2)movePath[i]))
                {
                    toiletIndexLeft = i;
                    break;
                }
            }

            if(toiletIndexLeft != -1 && toiletIndexRight != -1)
            {
                if(Mathf.Abs(startIndex - toiletIndexLeft) < Mathf.Abs(startIndex - toiletIndexRight))
                {
                    return GetMovePath(startIndex, toiletIndexLeft);
                }
                else
                {
                    return GetMovePath(startIndex, toiletIndexRight);
                }
            } else if(toiletIndexLeft != -1)
            {
                return GetMovePath(startIndex, toiletIndexLeft);
            }
            else if(toiletIndexRight != -1)
            {
                return GetMovePath(startIndex, toiletIndexRight);
            }
            
            return null;

            List<Vector3> GetMovePath(int fromIndex, int toIndex)
            {
                bool needReverse = false;
                if (fromIndex > toIndex)
                {
                    needReverse = true;
                    int t = fromIndex;
                    fromIndex = toIndex;
                    toIndex = t;
                }

                var p = new List<Vector3>();
                for (int i = fromIndex; i < toIndex; i++)
                {
                    p.Add(movePath[i]);
                }
                if (needReverse) p.Reverse();
                return p;
            }
        }

        private List<Vector3> RemoveRedundantPoints(List<Vector3> movePath)
        {
            if (movePath == null || movePath.Count == 0) return null;
            movePath = RemoveRedundantStartPath(movePath, drawZoneBoxCollider);
            movePath = RemoveRedundantEndPath(movePath, toilet.DrawZoneBoxCollider);
            return movePath;
        }

        public List<Vector3> RemoveRedundantStartPath(List<Vector3> points, BoxCollider2D boxCollider)
        {
            // Convert the box collider's bounds to a Rect for easy comparison
            Rect boxRect = new Rect(
                boxCollider.bounds.min.x,
                boxCollider.bounds.min.y,
                boxCollider.bounds.size.x,
                boxCollider.bounds.size.y
            );
            int startIndex = -1;
            List<Vector3> newPath = new List<Vector3>();
            for (int i = 0; i < points.Count; i++)
            {
                if (boxRect.Contains((Vector2)points[i]))
                {
                    startIndex = i;
                }
            }
            if (startIndex == -1) return points;
            for (int i = startIndex + 1; i < points.Count; i++)
            {
                newPath.Add(points[i]);
            }
            return newPath;
        }
        public List<Vector3> RemoveRedundantEndPath(List<Vector3> points, BoxCollider2D boxCollider)
        {
            // Convert the box collider's bounds to a Rect for easy comparison
            Rect boxRect = new Rect(
                boxCollider.bounds.min.x,
                boxCollider.bounds.min.y,
                boxCollider.bounds.size.x,
                boxCollider.bounds.size.y
            );
            int endIndex = -1;

            for (int i = points.Count - 1; i >= 0; i--)
            {
                if (boxRect.Contains((Vector2)points[i]))
                {
                    endIndex = i;
                }
            }
            List<Vector3> newPath = new List<Vector3>();
            if (endIndex == -1) return points;
            for (int i = 0; i < endIndex; i++)
            {
                newPath.Add(points[i]);
            }
            return newPath;
        }

        private void OnDrawGizmos()
        {
            if (movePath != null)
            {
                for (int i = 1; i < movePath.Length; i++)
                {
                    Gizmos.DrawLine(movePath[i], movePath[i - 1]);
                }
            }
        }
        private Coroutine moveCoroutine;


        public void MoveWithoutDOTween()
        {
            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
            }
            moveCoroutine = StartCoroutine(MoveAlongPathCoroutine(movePath, duration));
        }

        private IEnumerator MoveAlongPathCoroutine(Vector3[] path, float duration)
        {
            if (path == null || path.Length < 2)
                yield break;

            float totalDistance = 0f;
            for (int i = 0; i < path.Length - 1; i++)
                totalDistance += Vector3.Distance(path[i], path[i + 1]);

            float elapsed = 0f;
            int currentSegment = 0;
            transform.position = path[0];
            SoundManager_BabyGirl.Instance.PlaySoundEffectOneShot(walkSound);
            while (currentSegment < path.Length - 1)
            {
                Vector3 start = path[currentSegment];
                Vector3 end = path[currentSegment + 1];
                float segmentDistance = Vector3.Distance(start, end);
                float segmentDuration = (segmentDistance / totalDistance) * duration;

                float t = 0f;
                while (t < 1f)
                {
                    t += Time.deltaTime / segmentDuration;
                    transform.position = Vector3.Lerp(start, end, t);
                    yield return null;
                }

                transform.position = end;
                currentSegment++;
            }

            // OnComplete logic
            anim.state.SetAnimation(0, goToToiletAnimation, false).MixTime = .2f;
            anim.state.AddAnimation(0, winAnimation, true, 0).MixTime = .2f;
            toilet.GoToToilet(this);
        }

    }
}

