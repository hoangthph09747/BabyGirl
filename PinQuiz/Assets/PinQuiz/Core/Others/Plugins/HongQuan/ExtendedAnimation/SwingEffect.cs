using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using DG.Tweening.Core;

namespace ExtendedAnimation
{
    public class SwingEffect : MonoBehaviour
    {
        public bool randomDelayTime;
        public Vector2 delayTimeRandomRange;
        [Header("Position")]
        public Vector3 swingOffsetPosition;
        public float swingPositionDuration;
        public float swingPositionStartDelayTime;
        [Header("Rotation")]
        public Vector3 swingOffsetRotation;
        public float swingRotationDuration;
        public float swingRotationStartDelayTime;
        [Header("Scale")]
        public Vector3 swingOffsetScale;
        public float swingScaleDuration;
        public float swingScaleStartDelayTime;

        private Vector3 basePos;
        private Vector3 baseRot;
        private Vector3 baseScale;

        private Coroutine swingPosEffectCorotine;
        private Coroutine swingRotEffectCorotine;
        private Coroutine swingScaEffectCorotine;


        private void OnEnable()
        {
            if (randomDelayTime)
            {
                swingPositionStartDelayTime = Random.Range(delayTimeRandomRange.x, delayTimeRandomRange.y);
                swingRotationStartDelayTime = Random.Range(delayTimeRandomRange.x, delayTimeRandomRange.y);
                swingScaleStartDelayTime = Random.Range(delayTimeRandomRange.x, delayTimeRandomRange.y);
            }

            if (swingOffsetPosition != Vector3.zero && swingPositionDuration != 0)
            {
                basePos = transform.localPosition;
                Invoke("SwingEffectPosition", swingPositionStartDelayTime);
            }
            if (swingOffsetRotation != Vector3.zero && swingRotationDuration != 0)
            {
                baseRot = transform.localRotation.eulerAngles;
                Invoke("SwingEffectRotation", swingRotationStartDelayTime);
            }
            if (swingOffsetScale != Vector3.zero && swingScaleDuration != 0)
            {
                baseScale = transform.localScale;
                Invoke("SwingEffectScale", swingScaleStartDelayTime);
            }
        }


        public void SwingEffectPosition()
        {
            StopSwingPosition();
            swingPosEffectCorotine = StartCoroutine(StartSwingPosition());
        }
        IEnumerator StartSwingPosition()
        {
            while (true)
            {
                transform.DOLocalMove(swingOffsetPosition / 2f + basePos, swingPositionDuration / 2f);
                yield return new WaitForSeconds(swingPositionDuration / 2);
                transform.DOLocalMove(-swingOffsetPosition / 2f + basePos, swingPositionDuration / 2);
                yield return new WaitForSeconds(swingPositionDuration / 2);
            }
        }
        public void SwingEffectRotation()
        {
            StopSwingRotation();
            swingRotEffectCorotine = StartCoroutine(StartSwingRotation());
        }
        IEnumerator StartSwingRotation()
        {
            while (true)
            {
                transform.DOLocalRotate(swingOffsetRotation / 2 + baseRot, swingRotationDuration / 2f, RotateMode.Fast);
                yield return new WaitForSeconds(swingPositionDuration / 2);
                transform.DORotate(-swingOffsetRotation / 2 + baseRot, swingRotationDuration / 2, RotateMode.Fast).OnComplete(SwingEffectRotation);
                yield return new WaitForSeconds(swingPositionDuration / 2);
            }
        }
        public void SwingEffectScale()
        {
            StopSwingScale();
            swingScaEffectCorotine = StartCoroutine(StartSwingScale());
        }
        IEnumerator StartSwingScale()
        {
            while (true)
            {
                transform.DOScale(swingOffsetScale / 2f + baseScale, swingScaleDuration / 2f);
                yield return new WaitForSeconds(swingPositionDuration / 2);
                transform.DOScale(-swingOffsetScale / 2f + baseScale, swingScaleDuration / 2);
                yield return new WaitForSeconds(swingPositionDuration / 2);
            }
        }

        public void StopSwingPosition()
        {
            if (swingPosEffectCorotine != null)
                StopCoroutine(swingPosEffectCorotine);
            transform.DOLocalMove(basePos, swingPositionDuration / 2f);
        }
        public void StopSwingRotation()
        {
            if (swingRotEffectCorotine != null)
                StopCoroutine(swingRotEffectCorotine);
            transform.DOLocalRotate(baseRot, swingRotationDuration / 2f, RotateMode.Fast);
        }
        public void StopSwingScale()
        {
            if (swingScaEffectCorotine != null)
                StopCoroutine(swingScaEffectCorotine);
            transform.DOScale(baseScale, swingScaleDuration / 2f);
        }

        public void StopSwing()
        {
            StopSwingPosition();
            StopSwingRotation();
            StopSwingScale();
        }

    }

}
