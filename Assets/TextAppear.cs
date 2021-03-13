using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextAppear : MonoBehaviour
{
    public Vector2 MovementDirection;
    public float Speed;
    public bool isVisible;

    private float _target;
    private float _current;

    private TextMeshProUGUI _text;
    private Vector2 _origin;
    
    // Start is called before the first frame update
    void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
        _origin = transform.localPosition;
        setTarget(isVisible ? 0 : 1, false);
    }

    public void In(bool animate)
    {
        setTarget(0, animate);
    }
    
    public void Out(bool animate)
    {
        setTarget(1, animate);
    }

    private void setTarget(float value, bool animate)
    {
        _target = value;
        _current = animate ? _current : value;
    }

    void Toggle(bool animate)
    {
        setTarget(_target == 0 ? 1 : 0, animate);
    }

    // Update is called once per frame
    void Update()
    {
        var _diff = (_target - _current);
        var _speed = Speed * Time.deltaTime;
        if (Mathf.Abs(_diff) < _speed * _speed)
        {
            _current += _diff;
        }
        else
        {
            _current += _diff * _speed;
        }
        
        _text.color = Color.Lerp(Color.white, Color.clear, _current);
        transform.localPosition = _origin + MovementDirection * _current;
    }
}
