using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class bazaEfektow : MonoBehaviour
{
    //przypisy
    private bool graczEfekty;
    private playerEq eq;
    private WRUG1 wrug;

    private void Awake()
    {
        //this.gameObject.SendMessage("wypiszTest"); //wywo³uje funkcjê po nazwie!!

        if (this.gameObject.tag == "Player")
        {
            graczEfekty = true;
            eq = this.GetComponent<playerEq>();
        }
        else
        {
            graczEfekty = false;
            wrug = this.GetComponent<WRUG1>();
        }
    }

    public void OtrzymanieEfektu(efekty nowyEfekt)
    {
        if(graczEfekty)
        { 
            if(nowyEfekt.TypWywo³ania == typWywo³ania.otrzymanieObrarzeñ)
            {
                eq.efektyWywo³anieOtrzyma³Cios.AddListener(wypiszTest);
            }
            else if (nowyEfekt.TypWywo³ania == typWywo³ania.zadawanieObrarzeñ)
            {
                eq.efektyWywo³anieZada³Cios.AddListener(wypiszTest);
            }
            else if (nowyEfekt.TypWywo³ania == typWywo³ania.koniecTury)
            {
                eq.efektyWywo³anieKoniecTury.AddListener(wypiszTest);
            }
            else if (nowyEfekt.TypWywo³ania == typWywo³ania.natychmiastowy)
            {
                wypiszTest();
            }
        }
        else
        {
            if (nowyEfekt.TypWywo³ania == typWywo³ania.otrzymanieObrarzeñ)
            {
                wrug.efektyWywo³anieOtrzyma³Cios.AddListener(wypiszTest);
            }
            else if (nowyEfekt.TypWywo³ania == typWywo³ania.zadawanieObrarzeñ)
            {
                wrug.efektyWywo³anieZada³Cios.AddListener(wypiszTest);
            }
            else if (nowyEfekt.TypWywo³ania == typWywo³ania.koniecTury)
            {
                wrug.efektyWywo³anieKoniecTury.AddListener(wypiszTest);
            }
            else if (nowyEfekt.TypWywo³ania == typWywo³ania.natychmiastowy)
            {
                wypiszTest();
            }
        }
    }

    public void UsunEfekt(efekty Efekt)
    {
        if (graczEfekty)
        {
            if (Efekt.TypWywo³ania == typWywo³ania.otrzymanieObrarzeñ)
            {
                eq.efektyWywo³anieOtrzyma³Cios.RemoveListener(wypiszTest);
            }
            else if (Efekt.TypWywo³ania == typWywo³ania.zadawanieObrarzeñ)
            {
                eq.efektyWywo³anieZada³Cios.RemoveListener(wypiszTest);
            }
            else if (Efekt.TypWywo³ania == typWywo³ania.koniecTury)
            {
                eq.efektyWywo³anieKoniecTury.RemoveListener(wypiszTest);
            }
        }
        else
        {
            if (Efekt.TypWywo³ania == typWywo³ania.otrzymanieObrarzeñ)
            {
                wrug.efektyWywo³anieOtrzyma³Cios.RemoveListener(wypiszTest);
            }
            else if (Efekt.TypWywo³ania == typWywo³ania.zadawanieObrarzeñ)
            {
                wrug.efektyWywo³anieZada³Cios.RemoveListener(wypiszTest);
            }
            else if (Efekt.TypWywo³ania == typWywo³ania.koniecTury)
            {
                wrug.efektyWywo³anieKoniecTury.RemoveListener(wypiszTest);
            }
        }
    }


    //////////////////////////!!!!!!!!BAZA!!!!!!!!!///////////////////////////
    ///
    public void wypiszTest()
    {
        Debug.Log(this.gameObject.name);
    }


}
