using QuanUtilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HongQuan
{
    public class ParticleEffect : MonoBehaviour
    {
        public ParticleSystem effect;

        private void Awake()
        {
            effect = GetComponent<ParticleSystem>();
        }

        public void DespawnDelay(float delay)
        {
           // this.DelayFunction(delay, () => QuanUtilities.SimplePool.Despawn(gameObject));
        }

        public void DestroyDelay(float delay)
        {
            Destroy(gameObject, delay);
        }
    }

}