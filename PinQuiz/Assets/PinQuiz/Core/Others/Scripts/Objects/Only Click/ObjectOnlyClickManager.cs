using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuanUtilities;

namespace HongQuan
{
    public class ObjectOnlyClickManager : MonoBehaviour
    {
        public bool canClick = true;

        public Camera cam;
        public LayerMask targetLayerMask;

        protected virtual void Update()
        {
            if (!canClick) return;

            if (!Input.GetMouseButtonDown(0)) return;
            if (this.IsPointerOverUIObject()) return;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            var hits = Physics2D.RaycastAll(ray.origin, ray.direction, Mathf.Infinity, targetLayerMask);
            ObjectOnlyClick obj = null;
            foreach (var hit in hits)
            {
                if (!hit.transform.TryGetComponent(out ObjectOnlyClick clkObj)) continue;
                if (!clkObj.canClick) continue;
                if (obj == null) obj = clkObj;
                else
                {
                    if (obj.prioity < clkObj.prioity)
                    {
                        obj = clkObj;
                    }
                }
            }
            obj?.Click();
        }

        public void CanClick(bool canClick)
        {
            this.canClick = canClick;
        }

        protected Vector3 GetMousePos()
        {
            return cam.ScreenToWorldPoint(Input.mousePosition);
        }
    }

}