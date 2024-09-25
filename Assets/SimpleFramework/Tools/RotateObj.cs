using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObj : MonoBehaviour
{
    public float Speed = 10;
    public Vector3 rotateDir = Vector3.up;

    private void Start()
    {
        rotateDir.Normalize();
    }

    void Update()
    {
        transform.localEulerAngles += rotateDir * Speed * Time.deltaTime;
    }
}
