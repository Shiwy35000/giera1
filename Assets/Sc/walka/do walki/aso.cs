using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AsoTyp { karta, artefakt};
public enum iloœæAso {jednaSztuka, nieSkoñczoneAso};

[System.Serializable]
public class aso
{
    public AsoTyp asoTyp;
    public int Id;
    public iloœæAso iloœæ;
    public int cena;

    public aso(AsoTyp a, int i, iloœæAso s, int c)
    {
        asoTyp = a;
        Id = i;
        iloœæ = s;
        cena = c;
    }
}
