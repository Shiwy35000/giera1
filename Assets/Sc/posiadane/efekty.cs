using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum typWywo³ania { natychmiastowy, koniecTury, otrzymanieObrarzeñ, zadawanieObrarzeñ};
public enum odbiurEfektu { buff, debuff};
[System.Serializable]
public class efekty
{
    [Header("Dane opisowe")]
    public string nazwa;
    public odbiurEfektu odbiurEfektu; // do np (neutralizacji wszystkich egatywnych efektów itp.)
    public Sprite sprite;
    public string opis;

    [Header("Dzia³anie")]
    public typWywo³ania TypWywo³ania;
    public int licznik;

    public efekty(string Nazwa, odbiurEfektu odbiurEfektuu, Sprite S, string Opis, typWywo³ania TypW, int Licznik)
    {
        nazwa = Nazwa;
        odbiurEfektu = odbiurEfektuu;
        sprite = S;
        opis = Opis;
        licznik = Licznik;
        TypWywo³ania = TypW;
    }
}
