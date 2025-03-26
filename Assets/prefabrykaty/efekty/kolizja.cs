using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class kolizja : MonoBehaviour
{
    [HideInInspector] public bool KolizjaR, KolizjaD;

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "krawedzie")
        {
            KolizjaR = true;
        }
        if(other.tag == "krawedzieD")
        {
            KolizjaD = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "krawedzie")
        {
            KolizjaR = false;
        }
        if (other.tag == "krawedzieD")
        {
            KolizjaD = false;
        }
    }
}
