//using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SpriteRenderer))]

public class FitSpriteRendererWithMainCamera : MonoBehaviour
{
    private Camera mainCam;
    [HideInInspector]
    public SpriteRenderer spriteRenderer;
    public UnityEvent onDone;

    private void Awake()
    {
        mainCam = Camera.main;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        FitWithMainCam();
        onDone?.Invoke();
    }

    public void FitWithMainCam()
    {
        transform.localScale= Vector3.one;
        Vector2 camSize = new Vector2(mainCam.orthographicSize * mainCam.aspect, mainCam.orthographicSize) * 2f;
        Vector2 rendererSize = spriteRenderer.bounds.size;

        transform.position = (Vector2)mainCam.transform.position;

        float multiScale = Mathf.Max(camSize.x / rendererSize.x, camSize.y / rendererSize.y);
        transform.localScale = Vector3.one * multiScale;    
    }

    public void FitWithMainCam(float orthoSize)
    {
        transform.localScale = Vector3.one;
        Vector2 camSize = new Vector2(orthoSize * mainCam.aspect, orthoSize) * 2f;
        Vector2 rendererSize = spriteRenderer.bounds.size;

        transform.position = (Vector2)mainCam.transform.position;

        float multiScale = Mathf.Max(camSize.x / rendererSize.x, camSize.y / rendererSize.y);
        transform.localScale = Vector3.one * multiScale;
    }

    public void FitWithMainCam(float orthoSize,Vector3 pos)
    {
        transform.localScale = Vector3.one;
        Vector2 camSize = new Vector2(orthoSize * mainCam.aspect, orthoSize) * 2f;
        Vector2 rendererSize = spriteRenderer.bounds.size;

        transform.position = pos;

        float multiScale = Mathf.Max(camSize.x / rendererSize.x, camSize.y / rendererSize.y);
        transform.localScale = Vector3.one * multiScale;
    }
}

