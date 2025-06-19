using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using HongQuan;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace PinQuiz
{
    public class Pin: MonoBehaviour
    {
        public static bool canRemove = true;
        public bool isRemoved;
        public float removeDistance = 20f;
        public Pin[] conditionPins;
        public bool haveOutline;
        public bool haveNumber;
        public int number;
        public bool autoAdjustTouchCollider = true;

        [SerializeField] private Transform[] mainRender;
        [SerializeField] private Transform[] outlineRender;
        [SerializeField] private GameObject outLine;
        [SerializeField] TMP_Text numberText;
        [SerializeField] private SpriteRenderer[] rens;

        [SerializeField] private Transform keyBodyColRenderer;
        [SerializeField] private Transform keyBodyRenderer;

        private void Start()
        {
            PinQuizManager.instance.AddPin(this);
            if (autoAdjustTouchCollider) AdjustTouchCollider();
            if (haveOutline) TurnOnOutline();
            if (haveNumber) TurnOnNumber();
            InitKeyBody();
        }

        private void InitKeyBody()
        {
            keyBodyRenderer.localPosition = keyBodyColRenderer.localPosition;
            keyBodyRenderer.rotation = keyBodyColRenderer.rotation;
            keyBodyRenderer.localScale = keyBodyColRenderer.localScale;

            keyBodyRenderer.GetComponent<SpriteRenderer>().enabled = true;
            keyBodyColRenderer.GetComponent<SpriteRenderer>().enabled = false;
        }

        private void OnDestroy()
        {
            PinQuizManager.instance?.RemovePin(this);
        }

        private void TurnOnNumber()
        {
            numberText.gameObject.SetActive(true);
            numberText.transform.rotation = Quaternion.identity;
            numberText.text = number.ToString();
        }

        public void RemovePin()
        {
            if (!canRemove)
            {
                CantRemovePinAnimation();
                return;
            }
            foreach (var pin in conditionPins)
                if (!pin.isRemoved)
                {
                    CantRemovePinAnimation();
                    return;
                }

            isRemoved = true;

            GetComponent<ObjectOnlyClick>().canClick = false;
            transform.DOMove(transform.position + transform.right * removeDistance, 1);

            SoundManager_BabyGirl.Instance.PlayOneShot("Sounds/Effects/Shopping/itemClick");

            //Fading
            if (TryGetComponent(out SpriteRenderer baseRen))
            {
                baseRen.DOFade(0, 1);
            }

            outLine.SetActive(false);
            foreach (var r in rens)
            {
                if (r != null)
                    r.DOFade(0, 1);
            }
        }

        private void CantRemovePinAnimation()
        {
            foreach (var r in rens)
                if (r != null)
                    r.transform.DOShakePosition(.5f, .1f, 50, 90, false, true);
            SoundManager_BabyGirl.Instance.PlayOneShot("SoundSpotIt/touchFail");
        }

        private void AdjustTouchCollider()
        {
            SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
            var rot = transform.rotation;
            transform.rotation = Quaternion.identity;
            BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();

            if (boxCollider != null && spriteRenderers.Length > 0)
            {
                Bounds bounds = spriteRenderers[0].bounds;
                for (int i = 1; i < spriteRenderers.Length; i++)
                {
                    bounds.Encapsulate(spriteRenderers[i].bounds);
                }
                Vector2 size = new Vector2(bounds.size.x, bounds.size.y);
                Vector2 center = bounds.center;
                Vector3 localCenter = boxCollider.transform.InverseTransformPoint(center);
                boxCollider.size = size;
                boxCollider.offset = new Vector2(localCenter.x, localCenter.y);
                transform.rotation = rot;
            }
            else
            {
                Debug.LogWarning("BoxCollider2D or SpriteRenderer not found.");
            }
        }

        public void TurnOnOutline()
        {
            outLine.SetActive(true);
            outlineRender[0].localScale = mainRender[0].localScale + new Vector3(.2f, .8f, .1f);
            outlineRender[1].localScale = mainRender[1].localScale + new Vector3(.3f, .3f, .3f);
        }

    }

#if UNITY_EDITOR
    [CustomEditor(typeof(Pin)), CanEditMultipleObjects]
    public class PinEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Adjust Collider"))
            {
                Undo.RecordObjects(targets, "Adjust Colliders");
                foreach (var target in targets)
                {
                    var obj = target as Pin;
                    var rot = obj.transform.rotation;
                    obj.transform.rotation = Quaternion.identity;

                    SpriteRenderer[] spriteRenderers = obj.GetComponentsInChildren<SpriteRenderer>();

                    BoxCollider2D boxCollider = obj.GetComponent<BoxCollider2D>();

                    if (boxCollider != null && spriteRenderers.Length > 0)
                    {
                        EditorUtility.SetDirty(boxCollider);
                        Bounds bounds = spriteRenderers[0].bounds;
                        for (int i = 1; i < spriteRenderers.Length; i++)
                        {
                            bounds.Encapsulate(spriteRenderers[i].bounds);
                        }
                        Vector2 size = new Vector2(bounds.size.x, bounds.size.y);
                        Vector2 center = bounds.center;
                        Vector3 localCenter = boxCollider.transform.InverseTransformPoint(center);
                        boxCollider.size = size;
                        boxCollider.offset = new Vector2(localCenter.x, localCenter.y);
                        obj.transform.rotation = rot;
                    }
                    else
                    {
                        Debug.LogWarning("BoxCollider2D or SpriteRenderer not found.");
                    }
                }
            }
        }

    }

#endif
}