using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum typReakcji { dalszyDialog, walka, sklep, otrzymanieStrataArtefaktu, otrzymanieStrataKarty, otrzymanieStrataZ³ota, otrzymanieStrataZdrowia, otrzymanieStrataMaxZdrowia};

[System.Serializable]
public class reakcjaTyp
{
    public typReakcji TypReakcji;
    public int reakcjaUzupe³nienie; //numer dialogu, iloœæ otrzymanego/straconego zasobu lub jego indeks w bibliotece;

    public reakcjaTyp(int idIle_dodatek, typReakcji T)
    {
        TypReakcji = T;
        reakcjaUzupe³nienie = idIle_dodatek;
    }
}
