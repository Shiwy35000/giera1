using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wyłaczNaStarcie : MonoBehaviour
{
    void Start()
    {
        this.gameObject.SetActive(false);
    }
}
