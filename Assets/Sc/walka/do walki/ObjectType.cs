using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectType
{
    public GameObject Obj;
    public bool Odblokowane;

    public ObjectType(GameObject obj, bool odb)
    {
        Obj = obj;
        Odblokowane = odb;
    }
}
