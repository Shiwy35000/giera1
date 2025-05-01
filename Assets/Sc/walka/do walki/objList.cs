using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class objList
{
    public List<GameObject> obj = new List<GameObject>();

    public objList(List<GameObject> Obj)
    {
        obj = Obj;
    }
}
