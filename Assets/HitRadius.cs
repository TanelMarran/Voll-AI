using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[ExecuteInEditMode]
public class HitRadius : MonoBehaviour
{
    public Color ReadyColor;
    public Color ActiveColor;
    public Color CooldownColor;
    
    [Range(3, 360)] public int pointCount = 10;
    private LineRenderer _line;
    private CircleCollider2D _circle;
    private Player _player;

    private bool isLeft;
    
    // Start is called before the first frame update
    void Start()
    {
        _line = GetComponent<LineRenderer>();
        _circle = GetComponentInParent<CircleCollider2D>();
        _player = GetComponentInParent<Player>();
        setPoints(_circle.radius);
        isLeft = _player.transform.position.x < 0;
    }

    public int PointCount
    {
        get => pointCount;
        set
        {
            pointCount = value;
            setPoints(_circle.radius);
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
            var x = Mathf.Cos(angle) * radius;
            if (x + _player.transform.localPosition.x > 0 && isLeft)
            {
                x =  -_player.transform.localPosition.x;
            }
            if (x + _player.transform.localPosition.x < 0 && !isLeft)
            {
                x = -_player.transform.localPosition.x;
            }
            positions[i] = new Vector3(x, Mathf.Sin(angle) * radius, 0);
        }
        
        _line.SetPositions(positions);
    }

    void setColor()
    {
        Color color = ReadyColor;
        
        if (_player.isHitting)
        {
            color = ActiveColor;
        }
        else if (_player.HitStateTimestamp > Time.time)
        {
            color = CooldownColor;
        }

        _line.startColor = color;
        _line.endColor = color;
    }

    // Update is called once per frame
    void Update()
    {
        setPoints(_circle.radius);
        setColor();
    }
}
