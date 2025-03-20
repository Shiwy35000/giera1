using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    private Transform camPoz;

    void Awake()
    {
        camPoz = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }

    void LateUpdate()
    {
        transform.LookAt(camPoz);
        transform.Rotate(0f, 180f, 0f);
    }
}
