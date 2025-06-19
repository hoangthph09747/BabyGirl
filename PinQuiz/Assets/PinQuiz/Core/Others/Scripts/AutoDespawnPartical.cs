using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HongQuan
{
    [RequireComponent(typeof(ParticleSystem))]
    public class AutoDespawnPartical : AutoDespawn
    {
        private ParticleSystem _particleSystem;
        private void Awake()
        {
            _particleSystem = GetComponent<ParticleSystem>();
        }

        protected override void OnEnable()
        {
            float delay = _particleSystem.main.duration;
            Despawn(delay);
        }
    }
}
