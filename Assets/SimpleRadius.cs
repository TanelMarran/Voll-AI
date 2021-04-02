using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[ExecuteInEditMode]
public class SimpleRadius : MonoBehaviour
{
    public Color Color;
    public float Radius;
    
    [Range(3, 360)] public int pointCount = 10;
    private LineRenderer _line;

    // Start is called before the first frame update
    void Start()
    {
        _line = GetComponent<LineRenderer>();
        setColor();
    }
    
    public int PointCount
    {
        get => pointCount;
        set
        {
            pointCount = value;
            setPoints(Radius);
        }
    }

    void setPoints(float radius)
    {
        Vector3[] positions = new Vector3[pointCount + 1];
        float stepAngle = 360f / pointCount;
        _line.positionCount = pointCount + 1;
        
        for (int i = 0; i < pointCount + 1; i++)
        {
            var angle = Mathf.Deg2Rad * stepAngle * i;
            positions[i] = new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0);
        }
        
        _line.SetPositions(positions);
    }

    void setColor()
    {
        Color color = Color;

        _line.startColor = color;
        _line.endColor = color;
    }

    // Update is called once per frame
    void Update()
    {
        setPoints(Radius);
        setColor();
    }
}
