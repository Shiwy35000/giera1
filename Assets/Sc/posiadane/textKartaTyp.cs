
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum rodzajTrescKarta { normalText, obrarzenia, obrarzeniaGracz, obrarzeniaRazy, obrarzeniaGraczRazy};

[System.Serializable]
public class textKartaTyp 
{
    [SerializeField] public rodzajTrescKarta RodzajTrescKarta;
    [TextArea]
    public string tre��;


    public textKartaTyp(rodzajTrescKarta RodzajTrescKartaa, string Tre��)
    {
        RodzajTrescKarta = RodzajTrescKartaa;
        tre�� = Tre��;
    }

}
