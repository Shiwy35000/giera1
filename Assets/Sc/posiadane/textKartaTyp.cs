
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum rodzajTrescKarta { normalText, obrarzenia, obrarzeniaGracz, obrarzeniaRazy, obrarzeniaGraczRazy};

[System.Serializable]
public class textKartaTyp 
{
    [SerializeField] public rodzajTrescKarta RodzajTrescKarta;
    [TextArea]
    public string treœæ;


    public textKartaTyp(rodzajTrescKarta RodzajTrescKartaa, string Treœæ)
    {
        RodzajTrescKarta = RodzajTrescKartaa;
        treœæ = Treœæ;
    }

}
