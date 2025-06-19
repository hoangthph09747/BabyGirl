using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using DG.Tweening.Plugins.Core.PathCore;

public class GuideHand : MonoBehaviour
{
    public static GuideHand instance;

    [SerializeField] private Transform hand;

    SpriteRenderer handSpriteRenderer;
    TweenerCore<Vector3, Vector3, VectorOptions> tweenMove;
    TweenerCore<Vector3, Path, PathOptions> tweenPath;

    Vector3 moveDelta = Vector3.down;

    private void Awake()
    {
        instance = this;
        handSpriteRenderer = hand.GetComponent<SpriteRenderer>();
    }

    public void Flip()
    {
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y * -1, transform.localScale.z);
        moveDelta.y *= -1;
    }

    public void Click(Vector3 postion)
    {
        tweenMove?.Kill();
        tweenPath?.Kill();
        hand.gameObject.SetActive(true);
        transform.position = postion;
        tweenMove = transform.DOMove(postion - moveDelta * 0.2f, 0.4f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
    }

    public void Drag(Vector3 from, Vector3 to)
    {
        tweenMove?.Kill();
        tweenPath?.Kill();
        hand.gameObject.SetActive(true);
        transform.position = from;
        tweenMove = transform.DOMove(to, 0.4f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);
    }

    public void LoopSwipe(Vector3 p1, Vector3 p2)
    {
        tweenMove?.Kill();
        tweenPath?.Kill();
        hand.gameObject.SetActive(true);
        transform.position = p1;
        tweenMove = transform.DOMove(p2, 0.4f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
    }

    public void MovePath(Vector3[] path, float speed, int loop = -1, LoopType loopType = LoopType.Restart)
    {
        tweenMove?.Kill();
        tweenPath?.Kill();
        transform.position = path[0];
        float duration = 0;
        for(int i = 1; i < path.Length; i++)
        {
            duration += Vector3.Distance(path[i], path[i - 1]);
        }
        duration /= speed;
        hand.gameObject.SetActive(true);
        tweenPath =  transform.DOPath(path,duration).SetEase(Ease.Linear).SetLoops(loop, LoopType.Restart);  
    }

    public void StopHand()
    {
        tweenMove?.Kill();
        tweenPath?.Kill();
        hand.gameObject?.SetActive(false);
    }

    public void SetSortingLayer(string layer, int order)
    {
        handSpriteRenderer.sortingLayerName = layer;
        handSpriteRenderer.sortingOrder = order;
    }

    public void ResetSortingLayer()
    {
        handSpriteRenderer.sortingLayerName = "Default";
        handSpriteRenderer.sortingOrder = 100;
    }

    private void OnDestroy()
    {
        instance = null;
    }
}
