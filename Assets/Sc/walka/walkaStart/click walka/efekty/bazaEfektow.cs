using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

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

    UnityAction stringFunctionToUnityAction(object target, string functionName)
    {
        UnityAction action = (UnityAction)Delegate.CreateDelegate(typeof(UnityAction), target, functionName);
        return action;
    }

    public void OtrzymanieEfektu(efekty nowyEfekt)
    { 
        if (graczEfekty)
        { 
            if(nowyEfekt.TypWywo³ania == typWywo³ania.otrzymanieObrarzeñ)
            {
                UnityAction action = stringFunctionToUnityAction(this, nowyEfekt.nazwa);
                eq.efektyWywo³anieOtrzyma³Cios.AddListener(action);
            }
            else if (nowyEfekt.TypWywo³ania == typWywo³ania.zadawanieObrarzeñ)
            {
                UnityAction action = stringFunctionToUnityAction(this, nowyEfekt.nazwa);
                eq.efektyWywo³anieZada³Cios.AddListener(action);
            }
            else if (nowyEfekt.TypWywo³ania == typWywo³ania.koniecTury)
            {
                UnityAction action = stringFunctionToUnityAction(this, nowyEfekt.nazwa);
                eq.efektyWywo³anieKoniecTury.AddListener(action);
            }
            else if (nowyEfekt.TypWywo³ania == typWywo³ania.natychmiastowy)
            {
                this.gameObject.SendMessage(nowyEfekt.nazwa);
            }
        }
        else
        {
            if (nowyEfekt.TypWywo³ania == typWywo³ania.otrzymanieObrarzeñ)
            {
                UnityAction action = stringFunctionToUnityAction(this, nowyEfekt.nazwa);
                wrug.efektyWywo³anieOtrzyma³Cios.AddListener(action);
            }
            else if (nowyEfekt.TypWywo³ania == typWywo³ania.zadawanieObrarzeñ)
            {
                UnityAction action = stringFunctionToUnityAction(this, nowyEfekt.nazwa);
                wrug.efektyWywo³anieZada³Cios.AddListener(action);
            }
            else if (nowyEfekt.TypWywo³ania == typWywo³ania.koniecTury)
            {
                UnityAction action = stringFunctionToUnityAction(this, nowyEfekt.nazwa);
                wrug.efektyWywo³anieKoniecTury.AddListener(action);
            }
            else if (nowyEfekt.TypWywo³ania == typWywo³ania.natychmiastowy)
            {
                this.gameObject.SendMessage(nowyEfekt.nazwa);
            }
        }
    }

    public void UsunEfekt(efekty Efekt)
    {
        if (graczEfekty)
        {
            if (Efekt.TypWywo³ania == typWywo³ania.otrzymanieObrarzeñ)
            {
                UnityAction action = stringFunctionToUnityAction(this, Efekt.nazwa);
                eq.efektyWywo³anieOtrzyma³Cios.RemoveListener(action);
            }
            else if (Efekt.TypWywo³ania == typWywo³ania.zadawanieObrarzeñ)
            {
                UnityAction action = stringFunctionToUnityAction(this, Efekt.nazwa);
                eq.efektyWywo³anieZada³Cios.RemoveListener(action);
            }
            else if (Efekt.TypWywo³ania == typWywo³ania.koniecTury)
            {
                UnityAction action = stringFunctionToUnityAction(this, Efekt.nazwa);
                eq.efektyWywo³anieKoniecTury.RemoveListener(action);
            }
        }
        else
        {
            if (Efekt.TypWywo³ania == typWywo³ania.otrzymanieObrarzeñ)
            {
                UnityAction action = stringFunctionToUnityAction(this, Efekt.nazwa);
                wrug.efektyWywo³anieOtrzyma³Cios.RemoveListener(action);
            }
            else if (Efekt.TypWywo³ania == typWywo³ania.zadawanieObrarzeñ)
            {
                UnityAction action = stringFunctionToUnityAction(this, Efekt.nazwa);
                wrug.efektyWywo³anieZada³Cios.RemoveListener(action);
            }
            else if (Efekt.TypWywo³ania == typWywo³ania.koniecTury)
            {
                UnityAction action = stringFunctionToUnityAction(this, Efekt.nazwa);
                wrug.efektyWywo³anieKoniecTury.RemoveListener(action);
            }
        }
    }


    //////////////////////////!!!!!!!!BAZA!!!!!!!!!///////////////////////////
    //NAZWY VOID EFEKTÓW MUSZ¥ BYÆ TAKIE SAME JAK ICH ODPOWIEDNIKI W BIBLIOTECE!!!
    //A NAZWY W BIBLIOTECE MUSZ¥ BYÆ == nalurzEfekt.nazwaEfektu!!!!!!!!!!!!


    
    public void Kruchoœæ()
    {
        if(graczEfekty)
        {
            eq.ilee *= 1.5f;
        }
        else
        {
            wrug.ilee *= 1.5f;
        }
    }

    public void RozbiciePancerza()
    {
        if (graczEfekty)
        {
            eq.aktualnyPancerz = 0;
        }
        else
        {
            wrug.aktualnyPancerz = 0;
        }
    }

    public void Podpalenie()
    {
        if(graczEfekty)
        {
            eq.hp -= 3;
        }
        else
        {
            wrug.hpAktualne -= 3;
        }
    }

    public void LeczenieWczasie()
    {
        if (graczEfekty)
        {
            for (int x = 0; x < eq.na³orzoneEfekty.Count; x++)
            {
                if (eq.na³orzoneEfekty[x].nazwa == "LeczenieWczasie")
                {
                    float z = (float)eq.na³orzoneEfekty[x].licznik;
                    eq.hp += z;
                }
            }
        }
        else
        {
            for (int x = 0; x < wrug.na³orzoneEfekty.Count; x++)
            {
                if (wrug.na³orzoneEfekty[x].nazwa == "LeczenieWczasie")
                {
                    float z = (float)wrug.na³orzoneEfekty[x].licznik;
                    wrug.hpAktualne += z;
                }
            }
        }

    }

    public void Trucizna()
    {
        if (graczEfekty)
        {
            for (int x = 0; x < eq.na³orzoneEfekty.Count; x++)
            {
                if (eq.na³orzoneEfekty[x].nazwa == "Podpalenie")
                {
                    float z = (float)eq.na³orzoneEfekty[x].licznik;
                    eq.PrzyjmijDmg(z, true);
                }
            }
        }
        else
        {
            for (int x = 0; x < wrug.na³orzoneEfekty.Count; x++)
            {
                if (wrug.na³orzoneEfekty[x].nazwa == "Podpalenie")
                {
                    float z = (float)wrug.na³orzoneEfekty[x].licznik;
                    wrug.PrzyjmijDmg(z, true);
                }
            }
        }
    }
}
