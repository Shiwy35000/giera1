using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum typReakcji { dalszyDialog, walka, sklep, otrzymanieStrataArtefaktu, otrzymanieStrataKarty, otrzymanieStrataZłota, otrzymanieStrataZdrowia, otrzymanieStrataMaxZdrowia};

[System.Serializable]
public class reakcjaTyp
{
    public typReakcji TypReakcji;
    public int reakcjaUzupełnienie; //numer dialogu, ilość otrzymanego/straconego zasobu lub jego indeks w bibliotece;

    public reakcjaTyp(int idIle_dodatek, typReakcji T)
    {
        TypReakcji = T;
        reakcjaUzupełnienie = idIle_dodatek;
    }
}
