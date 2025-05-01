using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Czegoo {zdrowie, waluta, karta, artefakt};
public enum Znak { wi�ksze_posiada,mniejsze_niePosida, r�wne, wi�kszeLubR�wne, mniejszeLubR�wne};
[System.Serializable]
public class warunekOdpowiedzi
{
    public Czegoo czego;
    public Znak IleZnak; //(w odnie�ieniu do Ile ">x","<x","=x",">=x","<=x")
    public int Ile_Id;
    [HideInInspector] public bool czySpelniony;

    public warunekOdpowiedzi(int ile, Znak MniejWiecejTyle, Czegoo c, bool CzySpelniony)
    {
        Ile_Id = ile;
        IleZnak = MniejWiecejTyle;
        czego = c;
        czySpelniony = CzySpelniony;
    }
}
