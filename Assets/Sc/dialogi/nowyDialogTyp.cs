using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]

public class nowyDialogTyp
{
    public int zKimDialogId;
    public int nowyStartDialogu;

    public nowyDialogTyp(int ZKimDialog, int NowyStartDialogu)
    {
        zKimDialogId = ZKimDialog;
        nowyStartDialogu = NowyStartDialogu;
    }
}
