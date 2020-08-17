using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRotator : MonoBehaviour
{
    private float angle = 45;
    // Update is called once per frame
    void Update()
    {
        gameObject.transform.Rotate(Vector3.up, angle);
    }
}
