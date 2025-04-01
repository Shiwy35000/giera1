using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//nazwy musz� by� identyczne jak w BIBLIOTECE inaczej nie zadzia�a!!!
public enum nazwaEfektu { Krucho��, Podpalenie, LeczenieWczasie, RozbiciePancerza, Trucizna, Na�urzPancerz}; 

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
