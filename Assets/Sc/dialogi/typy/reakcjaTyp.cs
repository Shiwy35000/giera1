using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]

public class reakcjaTyp
{
    public int typReakcji;
    public int IdIle_dodatek;

    public reakcjaTyp(int idIle_dodatek, int TypReakcji)
    {
        typReakcji = TypReakcji;
        IdIle_dodatek = idIle_dodatek;
    }
}
