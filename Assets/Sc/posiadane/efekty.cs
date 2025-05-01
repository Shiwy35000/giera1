using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum typWywo�ania {koniecTury, pocz�tekTury, otrzymanieObrarze�, zadawanieObrarze�, atak, natychmiastowy_odrazuPrzemija_bezLicznika};
public enum typPrzemijania {koniecTury_domy�lny, wywo�anie, koniecTuryCa�kowity, wywo�aniemCa�kowity, niePrzemija}; //dopisek "ca�kowity" spawia �e niezalerznie od stak�w efekt znika gdy zadzia�a;
public enum odbiurEfektu { buff, debuff, nieUsuwalne_pasyw};
[System.Serializable]
public class efekty
{
    [Header("Dane opisowe")]
    public string nazwa;
    public odbiurEfektu odbiurEfektu; // do np (neutralizacji wszystkich negatywnych efekt�w itp.)
    public Sprite sprite;
    [TextArea]
    public string opis;

    [Header("Dzia�anie")]
    public typWywo�ania TypWywo�ania;
    public typPrzemijania TypPrzemijania;
    public int licznik;

    public efekty(string Nazwa, odbiurEfektu odbiurEfektuu, Sprite S, string Opis, typWywo�ania TypW, typPrzemijania typP, int Licznik)
    {
        nazwa = Nazwa;
        odbiurEfektu = odbiurEfektuu;
        sprite = S;
        opis = Opis;
        licznik = Licznik;
        TypWywo�ania = TypW;
        TypPrzemijania = typP;
    }
}
