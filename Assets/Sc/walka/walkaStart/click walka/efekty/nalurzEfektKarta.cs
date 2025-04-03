using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum cel { koszt, kosztRandom, obrarzenia, obrarzeniaAll, obrzarzeniaNegatyw, grywalonœæ, poUrzyciu, KoniecTury, Wyklucz, Cmentarz, dobierz};
public enum zalerznoœæ { brak_teraz, nowaWartoœæ, PlusMinus, EnumZmiana};
public enum przemijanieEfektuKarty {brak_teraz, tura, opuszczenieD³oni};

[System.Serializable]
public class nalurzEfektKarta
{
    public cel Cel;
    public zalerznoœæ Zalerznoœæ;
    public int wartoœæ_enumPoz;
    public przemijanieEfektuKarty PrzemijanieEfektuKarty;

    public nalurzEfektKarta(cel c,zalerznoœæ z, int dL, przemijanieEfektuKarty p)
    {
        Cel = c;
        Zalerznoœæ = z;
        wartoœæ_enumPoz = dL;
        PrzemijanieEfektuKarty = p;
    }
}
