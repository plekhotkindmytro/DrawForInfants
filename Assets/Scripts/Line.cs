using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Line : MonoBehaviour
{
    public LineRenderer lineRenderer;
    List<Vector2> points;
   

    void SetPoint(Vector2 point)
    {
        Debug.Log("Line started");
        points.Add(point);

        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPosition(points.Count - 1, point);

    }

    public void UpdateLine(Vector2 position) 
    {
        if (points == null)
        {
            points = new List<Vector2>();
            SetPoint(position);
            return;
        }

        Vector2 smothedPosition = Bezier(points[points.Count - 1], position, .4f);
        SetPoint(smothedPosition);

        //SetPoint(position);
    }

    Vector2 Bezier(Vector2 a, Vector2 b, float t)
    {
        return Vector2.Lerp(a, b, t);
    }

    Vector2 Bezier(Vector2 a, Vector2 b, Vector2 c, float t)
    {
        return Vector2.Lerp(Bezier(a, b, t), Bezier(b, c, t), t);
    }

    Vector2 Bezier(Vector2 a, Vector2 b, Vector2 c, Vector2 d, float t)
    {
        return Vector2.Lerp(Bezier(a, b, c, t), Bezier(b, c, d, t), t);
    }
}
