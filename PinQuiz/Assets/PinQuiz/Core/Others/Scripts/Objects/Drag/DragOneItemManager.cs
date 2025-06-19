using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace HongQuan
{
    public class DragOneItemManager : MonoBehaviour
    {
        public bool canDrag;
        public Vector3 dragOffset;
        private Camera mainCam;
        DragableItem currentItem;

        private void Awake()
        {
            mainCam = Camera.main;
        }

        Vector3 offset;

        void Update()
        {
            if (!canDrag) return;

            if(Input.touchCount > 1)
            {
                if (currentItem == null) return;
                currentItem.OnEndDrag();
                currentItem.onEndDrag?.Invoke();
                currentItem = null;
                return;
            } 

            if (Input.GetMouseButtonDown(0))
            {
                if (IsPointerOverUIObject()) return;
                Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
                Vector3 mousePos = GetMousePos();
                var hits = Physics2D.RaycastAll(ray.origin, ray.direction, Mathf.Infinity);
                DragableItem target = null;
                foreach (var hit in hits)
                {
                    if (!hit.transform.TryGetComponent(out DragableItem dragItem)) continue;
                    if (target == null)
                    {
                        target = dragItem;
                    }
                    else
                    {
                        if (dragItem.canDrag)
                        {
                            if (dragItem.transform.GetSiblingIndex() > target.transform.GetSiblingIndex())
                            {
                                target = dragItem;
                            }
                        }
                    }

                }
                currentItem = target;
                if (currentItem != null)
                {
                    offset = mousePos - target.transform.position;
                    currentItem.OnStartDrag();
                    currentItem.onStartDrag?.Invoke();
                }

                return;
            }
            if (Input.GetMouseButton(0))
            {
                if (currentItem == null) return;
                currentItem.transform.position = GetMousePos() - offset + dragOffset;
                currentItem.OnDrag();
                currentItem.onDrag?.Invoke();
            }
            if(Input.GetMouseButtonUp(0))
            {
                if (currentItem == null) return;
                currentItem.OnEndDrag();
                currentItem.onEndDrag?.Invoke();
                currentItem = null;
            }
        }

        private Vector3 GetMousePos()
        {
            return mainCam.ScreenToWorldPoint(Input.mousePosition);
        }

        public void SetDrag(bool canDrag)
        {
            this.canDrag = canDrag;
        }

        private bool IsPointerOverUIObject()
        {
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            return results.Count > 0;
        }

        public void SetDragItem(DragableItem item,Vector3 offset)
        {
            currentItem = item;
            this.offset = offset;
        }
    }

}
