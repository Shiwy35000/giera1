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
        //this.gameObject.SendMessage("wypiszTest"); //wywo�uje funkcj� po nazwie!!

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
            if(nowyEfekt.TypWywo�ania == typWywo�ania.otrzymanieObrarze�)
            {
                eq.efektyWywo�anieOtrzyma�Cios.AddListener(wypiszTest);
            }
            else if (nowyEfekt.TypWywo�ania == typWywo�ania.zadawanieObrarze�)
            {
                eq.efektyWywo�anieZada�Cios.AddListener(wypiszTest);
            }
            else if (nowyEfekt.TypWywo�ania == typWywo�ania.koniecTury)
            {
                eq.efektyWywo�anieKoniecTury.AddListener(wypiszTest);
            }
            else if (nowyEfekt.TypWywo�ania == typWywo�ania.natychmiastowy)
            {
                wypiszTest();
            }
        }
        else
        {
            if (nowyEfekt.TypWywo�ania == typWywo�ania.otrzymanieObrarze�)
            {
                wrug.efektyWywo�anieOtrzyma�Cios.AddListener(wypiszTest);
            }
            else if (nowyEfekt.TypWywo�ania == typWywo�ania.zadawanieObrarze�)
            {
                wrug.efektyWywo�anieZada�Cios.AddListener(wypiszTest);
            }
            else if (nowyEfekt.TypWywo�ania == typWywo�ania.koniecTury)
            {
                wrug.efektyWywo�anieKoniecTury.AddListener(wypiszTest);
            }
            else if (nowyEfekt.TypWywo�ania == typWywo�ania.natychmiastowy)
            {
                wypiszTest();
            }
        }
    }

    public void UsunEfekt(efekty Efekt)
    {
        if (graczEfekty)
        {
            if (Efekt.TypWywo�ania == typWywo�ania.otrzymanieObrarze�)
            {
                eq.efektyWywo�anieOtrzyma�Cios.RemoveListener(wypiszTest);
            }
            else if (Efekt.TypWywo�ania == typWywo�ania.zadawanieObrarze�)
            {
                eq.efektyWywo�anieZada�Cios.RemoveListener(wypiszTest);
            }
            else if (Efekt.TypWywo�ania == typWywo�ania.koniecTury)
            {
                eq.efektyWywo�anieKoniecTury.RemoveListener(wypiszTest);
            }
        }
        else
        {
            if (Efekt.TypWywo�ania == typWywo�ania.otrzymanieObrarze�)
            {
                wrug.efektyWywo�anieOtrzyma�Cios.RemoveListener(wypiszTest);
            }
            else if (Efekt.TypWywo�ania == typWywo�ania.zadawanieObrarze�)
            {
                wrug.efektyWywo�anieZada�Cios.RemoveListener(wypiszTest);
            }
            else if (Efekt.TypWywo�ania == typWywo�ania.koniecTury)
            {
                wrug.efektyWywo�anieKoniecTury.RemoveListener(wypiszTest);
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
