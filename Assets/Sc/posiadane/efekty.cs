using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum typWywo�ania { natychmiastowy, koniecTury, otrzymanieObrarze�, zadawanieObrarze�};
public enum odbiurEfektu { buff, debuff};
[System.Serializable]
public class efekty
{
    [Header("Dane opisowe")]
    public string nazwa;
    public odbiurEfektu odbiurEfektu; // do np (neutralizacji wszystkich egatywnych efekt�w itp.)
    public Sprite sprite;
    public string opis;

    [Header("Dzia�anie")]
    public typWywo�ania TypWywo�ania;
    public int licznik;

    public efekty(string Nazwa, odbiurEfektu odbiurEfektuu, Sprite S, string Opis, typWywo�ania TypW, int Licznik)
    {
        nazwa = Nazwa;
        odbiurEfektu = odbiurEfektuu;
        sprite = S;
        opis = Opis;
        licznik = Licznik;
        TypWywo�ania = TypW;
    }
}
