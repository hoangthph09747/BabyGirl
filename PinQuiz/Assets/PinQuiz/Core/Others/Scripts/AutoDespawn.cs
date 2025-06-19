using QuanUtilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HongQuan
{
    public class AutoDespawn : MonoBehaviour
    {
        [SerializeField] protected float despawnDuration = 5f;
        protected Coroutine despawnCorotine;
        protected virtual void OnEnable()
        {
            Despawn(despawnDuration);
        }

        public virtual void Despawn(float duration)
        {
            if (despawnCorotine != null) StopCoroutine(despawnCorotine);
            this.DelayFunction(duration, () => QuanUtilities.SimplePool.Despawn(gameObject));
        }
    }
}
