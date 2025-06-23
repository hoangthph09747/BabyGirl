//using FluffyUnderware.Curvy.Generator.Modules;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace ToiletRush
{
    [RequireComponent(typeof(LineRenderer))]
    public class ToiletRushDrawLine : MonoBehaviour
    {
        public static ToiletRushDrawLine instance;

        [SerializeField] float resolution = .2f;
        [SerializeField] float startRadius = 1f;

        [SerializeField] LayerMask targetLayerMask;
        [SerializeField] LayerMask defaultLayerMask;

        [SerializeField] private Material nonGenderLineMat;
        [SerializeField] private Material girlLineMat;
        [SerializeField] private Material boyLineMat;

        private LineRenderer lineRenderer;
        private LineRenderer redLineRenderer;
        private Camera mainCam;
        private bool enableDraw = true;

        private ToiletRushCharacter character;
        private ToiletRushToilet toilet;
        private bool ignoreDefaultToTargetLayer;

        public List<Vector3> path = new List<Vector3>();
        public UnityEvent<ToiletRushDrawLine> onDrawSuccess;

        public ToiletRushCharacter Character => character;

        private void Awake()
        {
            instance = this;
            lineRenderer = GetComponent<LineRenderer>();
            redLineRenderer = transform.GetChild(0).GetComponent<LineRenderer>();
            mainCam = Camera.main;

            Physics2D.IgnoreLayerCollision((int)Mathf.Log(targetLayerMask, 2), (int)Mathf.Log(defaultLayerMask, 2), true);
            ignoreDefaultToTargetLayer = Physics2D.GetIgnoreLayerCollision((int)Mathf.Log(targetLayerMask, 2), (int)Mathf.Log(defaultLayerMask, 2));
        }

        private void OnDestroy()
        {
            Physics2D.IgnoreLayerCollision((int)Mathf.Log(targetLayerMask, 2), (int)Mathf.Log(defaultLayerMask, 2), ignoreDefaultToTargetLayer);
            instance = null;
        }
        Vector3 inputPos;
        private void Update()
        {
            if (!enableDraw) return;
            if (Input.touchCount > 0)
            {
                inputPos = mainCam.ScreenToWorldPoint(Input.mousePosition);
            }
            inputPos.z = 0;
            if (Input.GetMouseButtonDown(0))
            {
                if(!ContinePath(inputPos))
                {
                    ClearDraw();
                    path.Add(inputPos);
                }
                CheckInputPosition(inputPos, 0);

                return;
            }

            if (Input.GetMouseButton(0) && path.Count > 0)
            {
                Vector3 lastPos = path[path.Count - 1];
                float distance = Vector3.Distance(inputPos, lastPos);

                var hit = Physics2D.Raycast(lastPos, (inputPos - lastPos).normalized, distance);
                if (hit && hit.transform.CompareTag(ToiletRushManager.ObstacleTag))
                {
                    redLineRenderer.positionCount = 2;
                    redLineRenderer.SetPosition(0, lastPos);
                    redLineRenderer.SetPosition(1, inputPos);
                }
                else if (distance >= resolution)
                {
                    redLineRenderer.positionCount = 0;

                    Vector3 dir = (lastPos - inputPos).normalized;
                    int newPointsCount = (int)(distance / resolution);
                    List<Vector3> newPaths = new List<Vector3>();
                    for (int i = 0; i < newPointsCount; i++)
                    {
                        Vector3 positon = inputPos + dir * resolution * i;
                        newPaths.Add(positon);

                        CheckInputPosition(positon, path.Count + i);
                    }
                    newPaths.Reverse();
                    path.AddRange(newPaths);
                    if (path.Count > lineRenderer.positionCount)
                    {
                        lineRenderer.positionCount = path.Count;
                        lineRenderer.SetPositions(path.ToArray());
                    }
                }
            }

            if (Input.GetMouseButtonUp(0) && path.Count > 0)
            {
                if (toilet != null && character != null)
                {
                    character.SetMovePath(path);
                    toilet.SetDrawDone();
                    GameObject g = new GameObject("Character line");
                    g.transform.SetParent(character.transform.parent, true);
                    var line = g.AddComponent<LineRenderer>();
                    line.positionCount = lineRenderer.positionCount;
                    line.SetPositions(path.ToArray());
                    line.material = lineRenderer.material;
                    line.widthCurve = lineRenderer.widthCurve;
                    line.widthMultiplier = lineRenderer.widthMultiplier;
                    lineRenderer.positionCount = 0;
                    redLineRenderer.positionCount = 0;
                    path.Clear();

                    onDrawSuccess?.Invoke(this);
                }
            }
        }

        private void CheckInputPosition(Vector3 position, int index)
        {
            var h = Physics2D.OverlapPoint(position, targetLayerMask);
            if (h && h.attachedRigidbody && h.attachedRigidbody.TryGetComponent(out ToiletRushCharacter character) && !character.HavePath)
            {
                if (this.character == null && this.toilet == null)
                {
                    this.character = character;
                    ChangeLineColor(character.Gender);
                }
                else if (this.character == null && this.toilet != null && this.toilet.Gender == character.Gender)
                {
                    this.character = character;
                }
            }
            else if (h && h.attachedRigidbody && h.attachedRigidbody.TryGetComponent(out ToiletRushToilet toilet) && !toilet.isDoneDraw)
            {
                if (this.character == null && this.toilet == null)
                {
                    this.toilet = toilet;
                    ChangeLineColor(toilet.Gender);
                }
                else if (this.character != null && this.toilet == null && toilet.Gender == this.character.Gender)
                {
                    this.toilet = toilet;
                }
            }
        }

        private bool ContinePath(Vector3 inputPos)
        {
            if(path.Count < 2) return false;

            float distanceToStartPath = Vector2.Distance(inputPos, path[0]);
            float distanceToEndPath = Vector2.Distance(inputPos, path[path.Count - 1]);

            if (distanceToEndPath <= startRadius && distanceToStartPath <= startRadius)
            {
                if (distanceToEndPath < distanceToStartPath)
                {
                    ContinueAtLast();
                    return true;
                }
                else
                {
                    ContinueAtFirst();
                    return true;
                }
            }
            else if (distanceToEndPath <= startRadius)
            {
                ContinueAtLast();
                return true;
            }
            else if (distanceToStartPath <= startRadius)
            {
                ContinueAtFirst();
                return true;
            }

            void ContinueAtFirst()
            {
                path.Reverse();
                path.Add(inputPos);
                lineRenderer.positionCount = path.Count;
                lineRenderer.SetPositions(path.ToArray());
            }

            void ContinueAtLast()
            {
                path.Add(inputPos);
                lineRenderer.positionCount = path.Count;
                lineRenderer.SetPositions(path.ToArray());
            }

            return false;
        }

        private void ChangeLineColor(Gender gender)
        {
            switch (gender)
            {
                case Gender.Boy: lineRenderer.material = boyLineMat; break;
                case Gender.Girl: lineRenderer.material = girlLineMat; break;
                default: lineRenderer.material = nonGenderLineMat; break;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, startRadius);
        }

        public void EnableDraw()
        {
            enableDraw = true;
        }

        public void DisableDraw()
        {
            enableDraw = false;
        }

        public void ClearDraw()
        {
            path.Clear();
            lineRenderer.positionCount = 0;
            lineRenderer.material = nonGenderLineMat;

            toilet = null;
            character = null;
        }


    }
}


