using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Trajectory : MonoBehaviour
{
    public Player leftPlayer;
    public Player rightPlayer;

    public float maxDistance;
    public int pointCount;
    public float length;
    
    private LineRenderer _line;

    // Start is called before the first frame update
    void Start()
    {
        _line = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Player activePlayer = transform.localPosition.x < 0 ? leftPlayer : rightPlayer;

        DrawLine(transform.position - activePlayer.transform.position);
    }

    void DrawLine(Vector3 distance)
    {
        Vector2 direction = Vector3.Normalize(distance);

        Vector2 point = new Vector2(transform.position.x, transform.position.y) + direction * 1f;

        float factor = (1 - (distance.magnitude / maxDistance));
        Vector3[] positions = new Vector3[pointCount + 1];
        _line.positionCount = pointCount + 1;
        var step = length / pointCount * factor;
        
        for (int i = 0; i < pointCount + 1; i++)
        {
            Vector2 newpoint = point + i * step * direction;
            newpoint.x = Mathf.Clamp(newpoint.x, -9.5f, 9.5f);
            newpoint.y = Mathf.Max(newpoint.y, 0);
            
            positions[i] = newpoint;
        }
        
        _line.SetPositions(positions);
        _line.startColor = new Color(1f, 1f, 1f, factor * .5f);
        _line.endColor = new Color(1f, 1f, 1f, factor * .5f);
    }
}
