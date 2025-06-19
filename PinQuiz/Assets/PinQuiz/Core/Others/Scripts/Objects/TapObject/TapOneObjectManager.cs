using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace HongQuan
{
    public class TapOneObjectManager : MonoBehaviour
    {
        public UnityEvent<TapObject> onTapAnObject;

        private Camera mainCam;

        private void Awake()
        {
            mainCam = Camera.main;
        }

        void Update()
        {

            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
                var hits = Physics2D.RaycastAll(ray.origin, ray.direction, Mathf.Infinity);
                TapObject target = null;
                foreach (var hit in hits)
                {
                    if (!hit.transform.TryGetComponent(out TapObject tapItem)) continue;
                    if (target == null)
                    {
                        target = tapItem;
                    }
                    else
                    {
                        if (tapItem.transform.GetSiblingIndex() > target.transform.GetSiblingIndex())
                        {
                            target = tapItem;
                        }

                    }
                }

                if(target!= null)
                {
                    target.OnTap();
                    onTapAnObject.Invoke(target);
                }

                return;
            }
        }
    }
}