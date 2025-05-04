using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AsoTyp { karta, artefakt};
[System.Flags]
public enum SpecjalAso : int {normal = 0x00, nieSkoñczoneAso = 0x01, odblokuj = 0x02};

[System.Serializable]
public class aso
{
    public AsoTyp asoTyp;
    public int Id;
    public SpecjalAso specjalAso;
    public int cena;

    public aso(AsoTyp a, int i, SpecjalAso s, int c)
    {
        asoTyp = a;
        Id = i;
        specjalAso = s;
        cena = c;
    }
}
