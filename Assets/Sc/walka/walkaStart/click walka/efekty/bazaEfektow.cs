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
    private taKarta karta;

    private void Awake()
    {
        //this.gameObject.SendMessage("wypiszTest"); //wywo³uje funkcjê po nazwie!!

        if (this.gameObject.tag == "Player")
        {
            graczEfekty = true;
            eq = this.GetComponent<playerEq>();
        }
        else if (this.gameObject.tag == "wrug")
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

    public void OtrzymanieEfektu(efekty nowyEfekt, int ile)
    {
        if (graczEfekty)
        {
            if (nowyEfekt.TypWywo³ania == typWywo³ania.otrzymanieObrarzeñ)
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
            else if (nowyEfekt.TypWywo³ania == typWywo³ania.pocz¹tekTury)
            {
                UnityAction action = stringFunctionToUnityAction(this, nowyEfekt.nazwa);
                eq.efektyWywo³aniePocz¹tekTury.AddListener(action);
            }
            else if (nowyEfekt.TypWywo³ania == typWywo³ania.atak)
            {
                UnityAction action = stringFunctionToUnityAction(this, nowyEfekt.nazwa);
                eq.efektyWywo³anieAtak.AddListener(action);
            }
            else if (nowyEfekt.TypWywo³ania == typWywo³ania.natychmiastowy_odrazuPrzemija_bezLicznika)
            {
                this.gameObject.SendMessage(nowyEfekt.nazwa ,ile);
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
            else if (nowyEfekt.TypWywo³ania == typWywo³ania.pocz¹tekTury)
            {
                UnityAction action = stringFunctionToUnityAction(this, nowyEfekt.nazwa);
                wrug.efektyWywo³aniePocz¹tekTury.AddListener(action);
            }
            else if (nowyEfekt.TypWywo³ania == typWywo³ania.atak)
            {
                UnityAction action = stringFunctionToUnityAction(this, nowyEfekt.nazwa);
                wrug.efektyWywo³anieAtak.AddListener(action);
            }
            else if (nowyEfekt.TypWywo³ania == typWywo³ania.natychmiastowy_odrazuPrzemija_bezLicznika)
            {
                this.gameObject.SendMessage(nowyEfekt.nazwa, ile);
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
            else if (Efekt.TypWywo³ania == typWywo³ania.pocz¹tekTury)
            {
                UnityAction action = stringFunctionToUnityAction(this, Efekt.nazwa);
                eq.efektyWywo³aniePocz¹tekTury.RemoveListener(action);
            }
            else if (Efekt.TypWywo³ania == typWywo³ania.atak)
            {
                UnityAction action = stringFunctionToUnityAction(this, Efekt.nazwa);
                eq.efektyWywo³anieAtak.RemoveListener(action);
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
            else if (Efekt.TypWywo³ania == typWywo³ania.pocz¹tekTury)
            {
                UnityAction action = stringFunctionToUnityAction(this, Efekt.nazwa);
                wrug.efektyWywo³aniePocz¹tekTury.RemoveListener(action);
            }
            else if (Efekt.TypWywo³ania == typWywo³ania.atak)
            {
                UnityAction action = stringFunctionToUnityAction(this, Efekt.nazwa);
                wrug.efektyWywo³anieAtak.RemoveListener(action);
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

    public void RozbiciePancerza(int nic)
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

    public void Wampiryzm()
    {
        if (graczEfekty)
        {
            for (int x = 0; x < eq.na³orzoneEfekty.Count; x++)
            {
                if (eq.na³orzoneEfekty[x].nazwa == "Wampiryzm")
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
                if (wrug.na³orzoneEfekty[x].nazwa == "Wampiryzm")
                {
                    float z = (float)wrug.na³orzoneEfekty[x].licznik;
                    wrug.hpAktualne += z;
                }
            }
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
                if (eq.na³orzoneEfekty[x].nazwa == "Trucizna")
                {
                    float z = (float)eq.na³orzoneEfekty[x].licznik;
                    eq.PrzyjmijDmg(z, true, this.gameObject);
                }
            }
        }
        else
        {
            for (int x = 0; x < wrug.na³orzoneEfekty.Count; x++)
            {
                if (wrug.na³orzoneEfekty[x].nazwa == "Trucizna")
                {
                    float z = (float)wrug.na³orzoneEfekty[x].licznik;
                    wrug.PrzyjmijDmg(z, true, this.gameObject);
                }
            }
        }
    }

    public void Na³urzPancerz(int ile)
    {
        if (graczEfekty)
        {
            eq.aktualnyPancerz += (float)ile;
        }
        else
        {
            wrug.aktualnyPancerz += (float)ile;
        }
    }

}
