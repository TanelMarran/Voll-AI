using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RewardNotice : MonoBehaviour
{
    public float totalLifespan = 3f;
    public float heightOffsetAmount = .8f;
    private float _lifespan;

    public TextMeshProUGUI text;
    private float _originalY;

    public static void Create(RewardNotice prefab, Vector3 position, Transform parentTransform, float value)
    {
        RewardNotice notice = Instantiate(prefab, position, Quaternion.identity, parentTransform);
        TextMeshProUGUI text = notice.GetComponent<TextMeshProUGUI>();
        text.SetText((value >= 0 ? "+" : "") + value);
        text.color = value >= 0 ? Color.green : Color.red;
    }

    private void Start()
    {
        _lifespan = totalLifespan;
        text = GetComponent<TextMeshProUGUI>();

        _originalY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        _lifespan -= Time.deltaTime;
        var progress = 1 - _lifespan / totalLifespan;
        
        if (_lifespan <= 0)
        {
            Destroy(gameObject);
        }

        var _position = transform.position;
        transform.position = new Vector3(_position.x, _originalY + progress * heightOffsetAmount, _position.z);
        text.color = new Color(text.color.r, text.color.g, text.color.b, 1 - progress);
    }
}
