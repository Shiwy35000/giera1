using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]

public class warunekOdpowiedzi
{
    public string Czego;
    public int Ile;
    public string znak; //(w odnie�ieniu do Ile ">x","<x","=x",">=x","<=x")
    [HideInInspector] public bool czySpelniony;

    public warunekOdpowiedzi(int ile, string MniejWiecejTyle, string czego, bool CzySpelniony)
    {
        Ile = ile;
        znak = MniejWiecejTyle;
        Czego = czego;
        czySpelniony = CzySpelniony;
    }
}
