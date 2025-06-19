using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace ExtendedAnimation
{
    public class VibarationEffect : MonoBehaviour
    {
        Vector3 basePos;

        [Range(0, 1)]
        public int xViberate;
        [Range(0, 1)]
        public int yViberate;
        [Range(0, 1)]
        public int zViberate;

        public void Vibrate(float numVibrations, float durationPerVibration, float strength)
        {
            StopViberate();
            basePos = transform.localPosition;
            if (strength != 0)
                StartCoroutine(StartVibration(numVibrations, durationPerVibration, strength));
        }

        public void InfVibrate(float strength)
        {
            StopViberate();
            basePos = transform.localPosition;
            if (strength != 0)
                StartCoroutine(StartInfVibration(strength));
        }

        private IEnumerator StartVibration(float numVibrations, float durationPerVibration, float strength)
        {
            basePos = transform.localPosition;
            while (numVibrations > 0)
            {
                numVibrations--;
                transform.DOLocalMove(basePos + strength *
                    new Vector3((1 * Random.Range(-1, 2)) * xViberate, (1f * Random.Range(-1, 2)) * yViberate, (1f * 1f * Random.Range(-1, 2)) * zViberate),
                    durationPerVibration);
                yield return new WaitForSeconds(durationPerVibration);
            }
        }
        private IEnumerator StartInfVibration(float strength)
        {
            basePos = transform.localPosition;
            while (true)
            {
                transform.DOLocalMove(basePos + strength *
                    new Vector3((1 * Random.Range(-1, 2)) * xViberate, (1f * Random.Range(-1, 2)) * yViberate, (1f * 1f * Random.Range(-1, 2)) * zViberate),
                    strength / 10f);
                yield return new WaitForSeconds(0.1f);
            }
        }

        public void InitViberate(Vector3Int viberateOn)
        {
            xViberate = viberateOn.x;
            yViberate = viberateOn.y;
            zViberate = viberateOn.z;
        }

        public void StopViberate()
        {
            StopAllCoroutines();
            transform.DOLocalMove(basePos, 0.1f);
        }
    }
}
