using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AsoTyp { karta, artefakt};
public enum ilo��Aso {jednaSztuka, nieSko�czoneAso};

[System.Serializable]
public class aso
{
    public AsoTyp asoTyp;
    public int Id;
    public ilo��Aso ilo��;
    public int cena;

    public aso(AsoTyp a, int i, ilo��Aso s, int c)
    {
        asoTyp = a;
        Id = i;
        ilo�� = s;
        cena = c;
    }
}
