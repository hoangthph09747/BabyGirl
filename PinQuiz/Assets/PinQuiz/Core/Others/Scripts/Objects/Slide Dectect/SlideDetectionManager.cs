using HongQuan;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HongQuan 
{
    public class SlideDetectionManager : MonoBehaviour
    {
        public UnityEvent<SlideDectionObject> onTapAnObject;

        private Camera mainCam;

        private void Awake()
        {
            mainCam = Camera.main;
        }

        void Update()
        {

            if (Input.GetMouseButton(0))
            {
                Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
                var hits = Physics2D.RaycastAll(ray.origin, ray.direction, Mathf.Infinity);
                SlideDectionObject target = null;
                foreach (var hit in hits)
                {
                    if (!hit.transform.TryGetComponent(out SlideDectionObject slideItem)) continue;
                    if (target == null)
                    {
                        target = slideItem;
                    }
                    else
                    {
                        if (slideItem.transform.GetSiblingIndex() > target.transform.GetSiblingIndex())
                        {
                            target = slideItem;
                        }

                    }
                }

                if (target != null)
                {
                    target.OnTouch();
                    onTapAnObject.Invoke(target);
                }

                return;
            }
        }
    }
}
