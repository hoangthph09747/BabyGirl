/*using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Core.PathCore;
using DG.Tweening.Plugins.Options;*/
using HongQuan;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
//using static System.IO.Enumeration.FileSystemEnumerable<TResult>;
using UnityEngine.XR;



#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ToiletRush
{
    public class ToiletRushGuideLine : MonoBehaviour
    {
        [SerializeField] Transform Hand;
        [SerializeField] float guideDuration = 2f;

        LineRenderer lineRenderer;
      //  TweenerCore<Vector3, Path, PathOptions> tween;
        private void Awake()
        {
            lineRenderer = GetComponent<LineRenderer>();
        }

        private void OnEnable()
        {
            Vector3[] path = new Vector3[lineRenderer.positionCount];
            lineRenderer.GetPositions(path);
            Hand.transform.position = path[0];
             //tween = Hand.DOPath(path, guideDuration).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);

           MoveWithoutDOTween(path);
        }

        private void OnDisable()
        {
            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
                moveCoroutine = null;
            }
            //tween?.Kill();
        }

        private Coroutine moveCoroutine;


        public void MoveWithoutDOTween(Vector3[] path)
        {
            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
            }
            moveCoroutine = StartCoroutine(MoveAlongPathCoroutine(path, guideDuration));
        }

        private IEnumerator MoveAlongPathCoroutine(Vector3[] path, float duration)
        {
            if (path == null || path.Length < 2)
                yield break;

            float totalDistance = 0f;
            for (int i = 0; i < path.Length - 1; i++)
                totalDistance += Vector3.Distance(path[i], path[i + 1]);

            while (true) // Lặp vô hạn
            {
                float elapsed = 0f;
                int currentSegment = 0;
                Hand.transform.position = path[0];

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
                        Hand.transform.position = Vector3.Lerp(start, end, t);
                        yield return null;
                    }

                    Hand.transform.position = end;
                    currentSegment++;
                }
            }
        }

    }

#if UNITY_EDITOR
    [CustomEditor(typeof(ToiletRushGuideLine))]
    public class ToiletRushGuideLineEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("Set Z to 0"))
            {
                LineRenderer lineRenderer = ((ToiletRushGuideLine)target).GetComponent<LineRenderer>();

                Undo.RecordObject(lineRenderer, "Set Z to 0");

                Vector3[] positions = new Vector3[lineRenderer.positionCount];

                lineRenderer.GetPositions(positions);

                for (int i = 0; i < positions.Length; i++)
                {
                    positions[i].z = 0;
                }

                lineRenderer.SetPositions(positions);
                EditorUtility.SetDirty(lineRenderer);
            }
        }
    }
#endif

}