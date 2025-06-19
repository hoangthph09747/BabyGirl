using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using static UnityEngine.GraphicsBuffer;

public class SimpleCollider : MonoBehaviour
{
    protected Transform m_Transform;
    public bool alwaysOnGizmos=false;

    public ColliderType colliderType { get; private set; } = ColliderType.Point;
    protected virtual void Awake()
    {
        m_Transform = transform;
    }

    protected void SetType(ColliderType type)
    {
        colliderType = type;
    }    

    public virtual bool CheckCollision(SimpleCollider target)
    {
        return transform.position.x == target.transform.position.x && transform.position.y == target.transform.position.y;
    }   

    public virtual bool CheckPoint(Vector3 point)
    {
        return transform.position.x == point.x && transform.position.y == point.y;
    }

    public virtual void OnDrawGizmosSelected()
    {
        Gizmos.DrawIcon(transform.position, "Point", true);
    }
    private void OnDrawGizmos()
    {
        if (!alwaysOnGizmos) return;
        OnDrawGizmosSelected();
    }
    public enum ColliderType
    {
        Circle,
        Box,
        Point,
    }

    public static float OverlapCircleBoxArea(SimpleCircleCollider circle,SimpleBoxCollider box)
    {
        // Define the dimensions of the rectangle AABB
        float rectTopLeftX = box.GetMinPoint().x; 
        float rectTopLeftY = box.GetMaxPoint().y;
        float rectWidth = box.GetMaxPoint().x - box.GetMinPoint().x;
        float rectHeight = box.GetMaxPoint().y - box.GetMinPoint().y;

        // Define the center and radius of the circle
        float circleCenterX = circle.GetCenter().x;
        float circleCenterY = circle.GetCenter().y;
        float circleRadius = circle.GetRadius();

        // Determine the closest point on the rectangle to the center of the circle
        float closestX = Mathf.Max(rectTopLeftX, Mathf.Min(circleCenterX, rectTopLeftX + rectWidth));
        float closestY = Mathf.Max(rectTopLeftY, Mathf.Min(circleCenterY, rectTopLeftY + rectHeight));

        // Calculate the distance between the closest point and the center of the circle
        float distance = Mathf.Sqrt(Mathf.Pow(circleCenterX - closestX, 2) + Mathf.Pow(circleCenterY - closestY, 2));

        // Determine if the circle and rectangle overlap
        bool overlap = distance <= circleRadius;

        // Calculate the area of overlap
        float overlapArea;
        if (overlap)
        {
            overlapArea = (circleRadius * circleRadius * Mathf.Acos(distance / circleRadius) - distance * Mathf.Sqrt(circleRadius * circleRadius - distance * distance));
        }
        else
        {
            overlapArea = 0;
        }

        return overlapArea;
    }

    public static float OverlapBoxsArea(SimpleBoxCollider box1, SimpleBoxCollider box2)
    {
        Vector2 l1=box1.GetMinPoint();
        Vector2 l2=box2.GetMinPoint();
        Vector2 r1=box1.GetMaxPoint();
        Vector2 r2=box2.GetMaxPoint();

        // Length of intersecting part i.e
        // start from max(l1.x, l2.x) of
        // x-coordinate and end at min(r1.x,
        // r2.x) x-coordinate by subtracting
        // start from end we get required
        // lengths
        float x_dist = (Mathf.Min(r1.x, r2.x) - Mathf.Max(l1.x, l2.x));
        float y_dist = (Mathf.Min(r1.y, r2.y) - Mathf.Max(l1.y, l2.y));
        float areaI = 0;
        if (x_dist > 0 && y_dist > 0)
        {
            areaI = x_dist * y_dist;
        }

        return areaI;
    }

    public static float OverlapCirlesArea(SimpleCircleCollider cirle1,SimpleCircleCollider cirle2)
    {
        // Define the center and radius of the first circle
        float circle1CenterX = cirle1.GetCenter().x;
        float circle1CenterY = cirle1.GetCenter().y;
        float circle1Radius = cirle1.GetRadius();

        // Define the center and radius of the second circle
        float circle2CenterX = cirle2.GetCenter().x;
        float circle2CenterY = cirle2.GetCenter().y;
        float circle2Radius = cirle2.GetRadius();

        // Calculate the distance between the centers of the two circles
        float distance = Mathf.Sqrt(Mathf.Pow(circle1CenterX - circle2CenterX, 2) + Mathf.Pow(circle1CenterY - circle2CenterY, 2));

        // Determine if the circles overlap
        bool overlap = distance <= circle1Radius + circle2Radius;

        // Calculate the area of overlap
        float overlapArea;
        if (overlap)
        {
            float r1Squared = circle1Radius * circle1Radius;
            float r2Squared = circle2Radius * circle2Radius;
            float cosTheta = (r1Squared + r2Squared - distance * distance) / (2.0f * circle1Radius * circle2Radius);
            float theta = Mathf.Acos(cosTheta);
            float d1 = circle1Radius * Mathf.Sin(theta);
            float d2 = circle2Radius * Mathf.Sin(theta);
            float sectorArea1 = 0.5f * theta * r1Squared;
            float sectorArea2 = 0.5f * theta * r2Squared;
            float triangleArea1 = d1 * Mathf.Sqrt(r1Squared - d1 * d1);
            float triangleArea2 = d2 * Mathf.Sqrt(r2Squared - d2 * d2);
            overlapArea = (sectorArea1 + sectorArea2 - triangleArea1 - triangleArea2);
        }
        else
        {
            overlapArea = 0;
        }

       return overlapArea;

    }

}
