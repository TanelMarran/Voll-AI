using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition : MonoBehaviour
{
    public static Transition instance;
    public static bool isCovering;

    private Animator _animator;
    private Action _callback = null;
    private float _lastProgress = 0;
    private float _endTimestamp = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _callback = null;
        instance = this;
    }

    public static void Play(Action callback)
    {
        instance._animator.Play("Transition_In", 0, 0);
        instance._callback = callback;
    }
    
    public static bool isPlaying()
    {
        float animationProgress = instance._animator.GetCurrentAnimatorStateInfo(0).normalizedTime;

        return !(instance._callback == null && animationProgress > .995f); 
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_callback != null)
        {
            AnimatorStateInfo info = _animator.GetCurrentAnimatorStateInfo(0);
            float time = info.normalizedTime;

            if (time > .995f)
            {
                _callback();
                _callback = null;
                instance._animator.Play("Transition_Out", 0, 0);
                instance._endTimestamp = _animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            }
        }
    }
}
