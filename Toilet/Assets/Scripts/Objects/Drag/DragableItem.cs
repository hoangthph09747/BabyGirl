using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace HongQuan
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class DragableItem : MonoBehaviour
    {
        public bool canDrag = true;
        [HideInInspector] public SpriteRenderer spriteRenderer;
        [HideInInspector] public BoxCollider2D boxCollider;
        [HideInInspector] public Transform mTransform;

        public UnityEvent onStartDrag;
        public UnityEvent onDrag;
        public UnityEvent onEndDrag;

        protected virtual void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            boxCollider = GetComponent<BoxCollider2D>();
            mTransform = transform;
        }

        [ContextMenu("Reset Box Size")]
        public void ResetColliderSize()
        {
            if (transform.localScale.x == 0 || transform.localScale.y == 0)
            {
                boxCollider.size = Vector2.zero;
                Debug.LogWarning("Local scale equals zero, size will be zeros");
                return;
            }
            boxCollider.size = (Vector2)spriteRenderer.bounds.size / (Vector2)transform.localScale;
            boxCollider.size += boxCollider.size * 0.5f;
        }
        public void ResetColliderSizeNoOffset()
        {
            if (transform.localScale.x == 0 || transform.localScale.y == 0)
            {
                boxCollider.size = Vector2.zero;
                Debug.LogWarning("Local scale equals zero, size will be zeros");
                return;
            }
            boxCollider.size = (Vector2)spriteRenderer.bounds.size / (Vector2)transform.localScale;
        }

        public void ResetColliderOffset()
        {
            boxCollider.offset = spriteRenderer.bounds.center - transform.position;
        }

        public void ResetLocalTransform()
        {
            mTransform.localScale = Vector3.one;
            mTransform.localPosition = Vector3.zero;
            mTransform.localRotation = Quaternion.identity;
        }

        public virtual void OnStartDrag() { }
        public virtual void OnDrag() { }
        public virtual void OnEndDrag() { }
    }
}

