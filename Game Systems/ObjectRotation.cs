using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotation : MonoBehaviour
{
    public float rotationDegree;    //The amount object will be rotated each frame.

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, rotationDegree);
    }
}
