using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HongQuan
{
    [RequireComponent(typeof(Collider2D))]
    public class ObjectOnlyClick : MonoBehaviour
    {
        public int prioity;
        public bool canClick = true;
        public UnityEvent<ObjectOnlyClick> onClick;

        public virtual void Click()
        {
            onClick?.Invoke(this);
        }
    }

}