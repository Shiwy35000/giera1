using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]

public class odpowiedz
{
    [HideInInspector] public bool czyAktywna;
    public string odp;
    public List<warunekOdpowiedzi> warunkiZaistnienia = new List<warunekOdpowiedzi>();
    public List<reakcjaTyp> reakcje = new List<reakcjaTyp>();

    public int nowyDialogPoczatkowy;

    /* LISTA REAKCJI:
     * walka (-1)
     * sklep (-2)
     * dalszy dialog (numer dialogu == liczba > 0)
     * 
     * koniec dialogu (0) - NIE AKTUALNE!!
     * otrzymanie przedmiotu (-3)
     * otrzymanie karty (-4)
     * +/- hp (-5)
     * 
     * 
     * jeœli brak reakcji koniec dialogu!
     * 
     * NOWY DIALOG POCZ¥TKOWY:
     * ZMIANA KOLEJNEJ ROZMOWY Z T¥ POSTACI¥ NA OD WYBRANEGO DIALOGU;
     */

    public odpowiedz(bool CzyAktywna, List<warunekOdpowiedzi> WarunkiZaistnienia, string Odp, List<reakcjaTyp> Reakcje, int NowyDialogPoczatkowy)
    {
        czyAktywna = CzyAktywna;
        warunkiZaistnienia = WarunkiZaistnienia;
        odp = Odp;
        reakcje = Reakcje;
        nowyDialogPoczatkowy = NowyDialogPoczatkowy;
    }
}
