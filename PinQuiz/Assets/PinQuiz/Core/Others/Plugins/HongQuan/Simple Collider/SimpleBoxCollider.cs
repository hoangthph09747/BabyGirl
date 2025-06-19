using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SimpleBoxCollider : SimpleCollider
{
    [SerializeField, Tooltip("All value of maxPoint must greater than min point")] private Vector2 maxPoint = new Vector3(1, 1);
    [SerializeField, Tooltip("All value of maxPoint must greater than min point")] private Vector2 minPoint = new Vector3(-1, -1);

    public Vector2 GetMaxPoint() => maxPoint + (Vector2)transform.position;
    public Vector2 GetMinPoint() => minPoint + (Vector2)transform.position;


    protected override void Awake()
    {
        base.Awake();
        SetType(ColliderType.Box);

        if (maxPoint.x < minPoint.x || minPoint.y > maxPoint.y)
            Debug.LogError("All value of maxPoint must greater than min point");
    }

    public override bool CheckCollision(SimpleCollider target)
    {
        if (target.colliderType == ColliderType.Box)
        {
            SimpleBoxCollider targetBox = (SimpleBoxCollider)target;

            float d1x = targetBox.GetMinPoint().x - this.GetMaxPoint().x;
            float d1y = targetBox.GetMinPoint().y - this.GetMaxPoint().y;
            float d2x = this.GetMinPoint().x - targetBox.GetMaxPoint().x;
            float d2y = this.GetMinPoint().y - targetBox.GetMaxPoint().y;

            if (d1x > 0.0f || d1y > 0.0f)
                return false;

            if (d2x > 0.0f || d2y > 0.0f)
                return false;

            return true;
        }
        else if (target.colliderType == ColliderType.Circle)
        {
            SimpleCircleCollider cirle = (SimpleCircleCollider)target; ;
            float Xn = Mathf.Max(GetMinPoint().x, Mathf.Min(cirle.GetCenter().x, GetMaxPoint().x));
            float Yn = Mathf.Max(GetMinPoint().y, Mathf.Min(cirle.GetCenter().y, GetMaxPoint().y));
            float Dx = Xn - cirle.GetCenter().x;
            float Dy = Yn - cirle.GetCenter().y;
            return (Dx * Dx + Dy * Dy) <= cirle.GetRadius() * cirle.GetRadius();
        }
        else
        {
            return CheckPoint(target.transform.position);
        }

    }

    public override bool CheckPoint(Vector3 target)
    {
        return target.x < GetMaxPoint().x && target.y < GetMaxPoint().y && target.x > GetMinPoint().x && target.y > GetMinPoint().y;
    }

    public override void OnDrawGizmosSelected()
    {
        if (maxPoint.x < minPoint.x || minPoint.y > maxPoint.y)
            Gizmos.color = Color.red;
        else
            Gizmos.color = Color.green;

        Gizmos.DrawLine((Vector3)maxPoint + transform.position, new Vector3(maxPoint.x, minPoint.y) + transform.position);
        Gizmos.DrawLine((Vector3)minPoint + transform.position, new Vector3(maxPoint.x, minPoint.y) + transform.position);
        Gizmos.DrawLine((Vector3)minPoint + transform.position, new Vector3(minPoint.x, maxPoint.y) + transform.position);
        Gizmos.DrawLine((Vector3)maxPoint + transform.position, new Vector3(minPoint.x, maxPoint.y) + transform.position);

        Gizmos.DrawIcon((Vector3)maxPoint + transform.position, "Max Point", true);
        Gizmos.DrawIcon((Vector3)minPoint + transform.position, "Min Point", true);
    }

    public void SetMaxPoint(Vector2 value)
    {
        maxPoint = value;
    }

    public void SetMinPoint(Vector2 value)
    {
        minPoint = value;
    }

    public void SetBottonRight(Vector2 value)
    {
        maxPoint.x = value.x;
        minPoint.y = value.y;
    }

    public void SetTopLeft(Vector2 value)
    {
        maxPoint.y = value.y;
        minPoint.x = value.x;
    }

    public float GetArea() => (maxPoint.x - minPoint.x) * (maxPoint.y - minPoint.y);

    public void GetBoundFromRenderer(Renderer renderer)
    {
        SetMaxPoint(new Vector2(renderer.bounds.center.x + renderer.bounds.size.x / 2, renderer.bounds.center.y + renderer.bounds.size.y / 2) - (Vector2)transform.position);
        SetMinPoint(new Vector2(renderer.bounds.center.x - renderer.bounds.size.x / 2, renderer.bounds.center.y - renderer.bounds.size.y / 2) - (Vector2)transform.position);
    } 

    public Vector2 GetRandomPointInsideBox()
    {
        Vector2 res;
        Vector2 maxPoint = GetMaxPoint();
        Vector2 minPoint = GetMinPoint();
        res.x = Random.Range(minPoint.x, maxPoint.x);
        res.y = Random.Range(minPoint.y, maxPoint.y);
        return res;
    }
}
