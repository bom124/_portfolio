using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Let : MonoBehaviour
{
    public float speed;
    void FixedUpdate()
    {
        transform.position = new Vector3(transform.position.x - speed, transform.position.y, 0);
    }
}
