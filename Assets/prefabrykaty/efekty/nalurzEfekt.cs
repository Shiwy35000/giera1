using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//nazwy musz¹ byæ identyczne jak w BIBLIOTECE inaczej nie zadzia³a!!!
public enum nazwaEfektu { Kruchoœæ, Podpalenie, LeczenieWczasie, RozbiciePancerza, Trucizna}; 

[System.Serializable]
public class nalurzEfekt
{
    public nazwaEfektu NazwaEfektu;
    [Range(1, 99)]
    public int ile;

    public nalurzEfekt(nazwaEfektu NazwaEfektuu, int Ile)
    {
        NazwaEfektu = NazwaEfektuu;
        ile = Ile;
    }
}
