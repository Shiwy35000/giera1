using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]

public class typDialogowy 
{
    public string tresc;
    public List<odpowiedz> listaOdpowiedzi = new List<odpowiedz>();

    public typDialogowy(string Tresc, List<odpowiedz> ListaOdpowiedzi)
    {
        tresc = Tresc;
        listaOdpowiedzi = ListaOdpowiedzi;
    }
}
