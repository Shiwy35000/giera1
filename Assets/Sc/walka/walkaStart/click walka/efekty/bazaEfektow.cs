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
        //this.gameObject.SendMessage("wypiszTest"); //wywo�uje funkcj� po nazwie!!

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
            if (nowyEfekt.TypWywo�ania == typWywo�ania.otrzymanieObrarze�)
            {
                UnityAction action = stringFunctionToUnityAction(this, nowyEfekt.nazwa);
                eq.efektyWywo�anieOtrzyma�Cios.AddListener(action);
            }
            else if (nowyEfekt.TypWywo�ania == typWywo�ania.zadawanieObrarze�)
            {
                UnityAction action = stringFunctionToUnityAction(this, nowyEfekt.nazwa);
                eq.efektyWywo�anieZada�Cios.AddListener(action);
            }
            else if (nowyEfekt.TypWywo�ania == typWywo�ania.koniecTury)
            {
                UnityAction action = stringFunctionToUnityAction(this, nowyEfekt.nazwa);
                eq.efektyWywo�anieKoniecTury.AddListener(action);
            }
            else if (nowyEfekt.TypWywo�ania == typWywo�ania.pocz�tekTury)
            {
                UnityAction action = stringFunctionToUnityAction(this, nowyEfekt.nazwa);
                eq.efektyWywo�aniePocz�tekTury.AddListener(action);
            }
            else if (nowyEfekt.TypWywo�ania == typWywo�ania.atak)
            {
                UnityAction action = stringFunctionToUnityAction(this, nowyEfekt.nazwa);
                eq.efektyWywo�anieAtak.AddListener(action);
            }
            else if (nowyEfekt.TypWywo�ania == typWywo�ania.natychmiastowy_odrazuPrzemija_bezLicznika)
            {
                this.gameObject.SendMessage(nowyEfekt.nazwa ,ile);
            }
        }
        else
        {
            if (nowyEfekt.TypWywo�ania == typWywo�ania.otrzymanieObrarze�)
            {
                UnityAction action = stringFunctionToUnityAction(this, nowyEfekt.nazwa);
                wrug.efektyWywo�anieOtrzyma�Cios.AddListener(action);
            }
            else if (nowyEfekt.TypWywo�ania == typWywo�ania.zadawanieObrarze�)
            {
                UnityAction action = stringFunctionToUnityAction(this, nowyEfekt.nazwa);
                wrug.efektyWywo�anieZada�Cios.AddListener(action);
            }
            else if (nowyEfekt.TypWywo�ania == typWywo�ania.koniecTury)
            {
                UnityAction action = stringFunctionToUnityAction(this, nowyEfekt.nazwa);
                wrug.efektyWywo�anieKoniecTury.AddListener(action);
            }
            else if (nowyEfekt.TypWywo�ania == typWywo�ania.pocz�tekTury)
            {
                UnityAction action = stringFunctionToUnityAction(this, nowyEfekt.nazwa);
                wrug.efektyWywo�aniePocz�tekTury.AddListener(action);
            }
            else if (nowyEfekt.TypWywo�ania == typWywo�ania.atak)
            {
                UnityAction action = stringFunctionToUnityAction(this, nowyEfekt.nazwa);
                wrug.efektyWywo�anieAtak.AddListener(action);
            }
            else if (nowyEfekt.TypWywo�ania == typWywo�ania.natychmiastowy_odrazuPrzemija_bezLicznika)
            {
                this.gameObject.SendMessage(nowyEfekt.nazwa, ile);
            }
        }
    }

    public void UsunEfekt(efekty Efekt)
    {
        if (graczEfekty)
        {
            if (Efekt.TypWywo�ania == typWywo�ania.otrzymanieObrarze�)
            {
                UnityAction action = stringFunctionToUnityAction(this, Efekt.nazwa);
                eq.efektyWywo�anieOtrzyma�Cios.RemoveListener(action);
            }
            else if (Efekt.TypWywo�ania == typWywo�ania.zadawanieObrarze�)
            {
                UnityAction action = stringFunctionToUnityAction(this, Efekt.nazwa);
                eq.efektyWywo�anieZada�Cios.RemoveListener(action);
            }
            else if (Efekt.TypWywo�ania == typWywo�ania.koniecTury)
            {
                UnityAction action = stringFunctionToUnityAction(this, Efekt.nazwa);
                eq.efektyWywo�anieKoniecTury.RemoveListener(action);
            }
            else if (Efekt.TypWywo�ania == typWywo�ania.pocz�tekTury)
            {
                UnityAction action = stringFunctionToUnityAction(this, Efekt.nazwa);
                eq.efektyWywo�aniePocz�tekTury.RemoveListener(action);
            }
            else if (Efekt.TypWywo�ania == typWywo�ania.atak)
            {
                UnityAction action = stringFunctionToUnityAction(this, Efekt.nazwa);
                eq.efektyWywo�anieAtak.RemoveListener(action);
            }
        }
        else
        {
            if (Efekt.TypWywo�ania == typWywo�ania.otrzymanieObrarze�)
            {
                UnityAction action = stringFunctionToUnityAction(this, Efekt.nazwa);
                wrug.efektyWywo�anieOtrzyma�Cios.RemoveListener(action);
            }
            else if (Efekt.TypWywo�ania == typWywo�ania.zadawanieObrarze�)
            {
                UnityAction action = stringFunctionToUnityAction(this, Efekt.nazwa);
                wrug.efektyWywo�anieZada�Cios.RemoveListener(action);
            }
            else if (Efekt.TypWywo�ania == typWywo�ania.koniecTury)
            {
                UnityAction action = stringFunctionToUnityAction(this, Efekt.nazwa);
                wrug.efektyWywo�anieKoniecTury.RemoveListener(action);
            }
            else if (Efekt.TypWywo�ania == typWywo�ania.pocz�tekTury)
            {
                UnityAction action = stringFunctionToUnityAction(this, Efekt.nazwa);
                wrug.efektyWywo�aniePocz�tekTury.RemoveListener(action);
            }
            else if (Efekt.TypWywo�ania == typWywo�ania.atak)
            {
                UnityAction action = stringFunctionToUnityAction(this, Efekt.nazwa);
                wrug.efektyWywo�anieAtak.RemoveListener(action);
            }
        }
    }


    //////////////////////////!!!!!!!!BAZA!!!!!!!!!///////////////////////////
    //NAZWY VOID EFEKT�W MUSZ� BY� TAKIE SAME JAK ICH ODPOWIEDNIKI W BIBLIOTECE!!!
    //A NAZWY W BIBLIOTECE MUSZ� BY� == nalurzEfekt.nazwaEfektu!!!!!!!!!!!!


    
    public void Krucho��()
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
            for (int x = 0; x < eq.na�orzoneEfekty.Count; x++)
            {
                if (eq.na�orzoneEfekty[x].nazwa == "Wampiryzm")
                {
                    float z = (float)eq.na�orzoneEfekty[x].licznik;
                    eq.hp += z;
                }
            }
        }
        else
        {
            for (int x = 0; x < wrug.na�orzoneEfekty.Count; x++)
            {
                if (wrug.na�orzoneEfekty[x].nazwa == "Wampiryzm")
                {
                    float z = (float)wrug.na�orzoneEfekty[x].licznik;
                    wrug.hpAktualne += z;
                }
            }
        }
    }

    public void LeczenieWczasie()
    {
        if (graczEfekty)
        {
            for (int x = 0; x < eq.na�orzoneEfekty.Count; x++)
            {
                if (eq.na�orzoneEfekty[x].nazwa == "LeczenieWczasie")
                {
                    float z = (float)eq.na�orzoneEfekty[x].licznik;
                    eq.hp += z;
                }
            }
        }
        else
        {
            for (int x = 0; x < wrug.na�orzoneEfekty.Count; x++)
            {
                if (wrug.na�orzoneEfekty[x].nazwa == "LeczenieWczasie")
                {
                    float z = (float)wrug.na�orzoneEfekty[x].licznik;
                    wrug.hpAktualne += z;
                }
            }
        }
    }

    public void Trucizna()
    {
        if (graczEfekty)
        {
            for (int x = 0; x < eq.na�orzoneEfekty.Count; x++)
            {
                if (eq.na�orzoneEfekty[x].nazwa == "Trucizna")
                {
                    float z = (float)eq.na�orzoneEfekty[x].licznik;
                    eq.PrzyjmijDmg(z, true, this.gameObject);
                }
            }
        }
        else
        {
            for (int x = 0; x < wrug.na�orzoneEfekty.Count; x++)
            {
                if (wrug.na�orzoneEfekty[x].nazwa == "Trucizna")
                {
                    float z = (float)wrug.na�orzoneEfekty[x].licznik;
                    wrug.PrzyjmijDmg(z, true, this.gameObject);
                }
            }
        }
    }

    public void Na�urzPancerz(int ile)
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
