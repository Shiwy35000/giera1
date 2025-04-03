using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum cel { koszt, kosztRandom, obrarzenia, obrarzeniaAll, obrzarzeniaNegatyw, grywalon��, poUrzyciu, KoniecTury, Wyklucz, Cmentarz, dobierz};
public enum zalerzno�� { brak_teraz, nowaWarto��, PlusMinus, EnumZmiana};
public enum przemijanieEfektuKarty {brak_teraz, tura, opuszczenieD�oni};

[System.Serializable]
public class nalurzEfektKarta
{
    public cel Cel;
    public zalerzno�� Zalerzno��;
    public int warto��_enumPoz;
    public przemijanieEfektuKarty PrzemijanieEfektuKarty;

    public nalurzEfektKarta(cel c,zalerzno�� z, int dL, przemijanieEfektuKarty p)
    {
        Cel = c;
        Zalerzno�� = z;
        warto��_enumPoz = dL;
        PrzemijanieEfektuKarty = p;
    }
}
