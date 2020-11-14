using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public BoxCollider2D LeftWall;
    public BoxCollider2D RightWall;
    public BoxCollider2D Floor;

    // Start is called before the first frame update
    void Start()
    {
        /*float viewHalfWidth = Camera.main.orthographicSize * Camera.main.aspect;
        float viewHalfHeight = Camera.main.orthographicSize;

        LeftWall.transform.position = new Vector2(-viewHalfWidth + .5f, viewHalfHeight - .5f);*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
