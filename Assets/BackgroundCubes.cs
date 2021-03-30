using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundCubes : MonoBehaviour
{
    public Transform cube1;
    public Transform cube2;
    public Transform cube3;

    private Vector3 _transform1;
    private Vector3 _transform2;
    private Vector3 _transform3;
    
    // Start is called before the first frame update
    void Start()
    {
        var transform1 = cube1.transform;
        _transform1 = transform1.position;
        var transform2 = cube2.transform;
        _transform2 = transform2.position;
        var transform3 = cube3.transform;
        _transform3 = transform3.position;
    }

    // Update is called once per frame
    void Update()
    {
        float mover = Time.time * .5f;

        cube1.transform.position = _transform1 + Vector3.up * (Mathf.Sin(mover) * 2f);
        cube2.transform.position = _transform2 + Vector3.up * (Mathf.Sin(mover + 30f) * 5f);
        cube3.transform.position = _transform3 + Vector3.up * (6f * Mathf.Sin(mover + 60f));
        
        cube1.transform.Rotate(Vector3.up, 1f * Time.deltaTime);
        cube2.transform.Rotate(Vector3.up, -1f * Time.deltaTime);
        cube3.transform.Rotate(Vector3.up, 1f * Time.deltaTime);
    }
}
