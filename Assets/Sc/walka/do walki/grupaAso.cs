using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class grupaAso
{
    public List<aso> zawarto�� = new List<aso>();

    public grupaAso(List<aso> z)
    {
        zawarto�� = z;
    }
}
