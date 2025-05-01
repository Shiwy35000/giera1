using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum typWywo³ania {koniecTury, pocz¹tekTury, otrzymanieObrarzeñ, zadawanieObrarzeñ, atak, natychmiastowy_odrazuPrzemija_bezLicznika};
public enum typPrzemijania {koniecTury_domyœlny, wywo³anie, koniecTuryCa³kowity, wywo³aniemCa³kowity, niePrzemija}; //dopisek "ca³kowity" spawia ¿e niezalerznie od staków efekt znika gdy zadzia³a;
public enum odbiurEfektu { buff, debuff, nieUsuwalne_pasyw};
[System.Serializable]
public class efekty
{
    [Header("Dane opisowe")]
    public string nazwa;
    public odbiurEfektu odbiurEfektu; // do np (neutralizacji wszystkich negatywnych efektów itp.)
    public Sprite sprite;
    [TextArea]
    public string opis;

    [Header("Dzia³anie")]
    public typWywo³ania TypWywo³ania;
    public typPrzemijania TypPrzemijania;
    public int licznik;

    public efekty(string Nazwa, odbiurEfektu odbiurEfektuu, Sprite S, string Opis, typWywo³ania TypW, typPrzemijania typP, int Licznik)
    {
        nazwa = Nazwa;
        odbiurEfektu = odbiurEfektuu;
        sprite = S;
        opis = Opis;
        licznik = Licznik;
        TypWywo³ania = TypW;
        TypPrzemijania = typP;
    }
}
