using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum cel { kosztNowaWarto��, kosztPlusMinus, kosztRandom, obrarzeniaPlusMinus, grywalon��Zmiana, poUrzyciuZmiana, KoniecTuryZmiana, terazWyklucz, terazCmentarz, dobierz}; //if dopisek "Zmiana" , "deklaracjaLiczbowa" == pozycja w enum wybranej opcji;
public enum przemijanieEfektuKarty {brak_natychmiastowe, tura, opuszczenieD�oni, koniecWalki};

[System.Serializable]
public class nalurzEfektKarta
{
    public cel Cel;
    public int deklaracjaLiczbowa;
    public przemijanieEfektuKarty PrzemijanieEfektuKarty;

    public nalurzEfektKarta(cel c, int dL, przemijanieEfektuKarty p)
    {
        Cel = c;
        deklaracjaLiczbowa = dL;
        PrzemijanieEfektuKarty = p;
    }
}
