using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;

namespace HongQuan
{
    public class FloatingObject : MonoBehaviour
    {
        public float amplitude = 0.5f;
        public float frequency = 1f;
        public float phase;
        public bool randomPhase = true;

        private Vector3 basePos;

        private void Start()
        {
            basePos = transform.position;
            if(randomPhase)
            {
                phase = Random.Range(0, Mathf.PI * 2f);
            }
        }

        private void Update()
        {
            var pos = new NativeArray<Vector3>(1, Allocator.TempJob);
            pos[0] = basePos;
            new FloatingJob
            {
                amplitude = amplitude,
                frequency = frequency,
                time = Time.fixedTime,
                phase = phase,
                pos = pos
            }.Schedule().Complete();

            transform.position = pos[0];

            pos.Dispose();
        }

        public struct FloatingJob : IJob
        {
            public float amplitude;
            public float frequency;
            public float time;
            public float phase;
            public NativeArray<Vector3> pos;
            public void Execute()
            {
                Vector3 t = pos[0];
                t.y += Mathf.Sin(time * Mathf.PI * frequency + phase) * amplitude;
                pos[0] = t;
            }
        }
    }

}