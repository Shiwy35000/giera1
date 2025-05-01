using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class WRUG1 : MonoBehaviour
{
    [Header("Dane Wroga")]
    public string nazwa;
    public Sprite WrugGragika;
    [SerializeField] public List<UnityEvent> dzia�anie;
   
    [Header("Dane w Walce")]
    public float hpAktualne;
    public float hpMax;
    public float aktualnyPancerz;
    public float bonusDoObrarze�;
    public List<efekty> na�orzoneEfekty;
    public GameObject ramkaCelu;
    //pozosta�e / przypisy
    //[HideInInspector] public GameObject morInfo;
    private bazaEfektow BazaEfektow;
    [HideInInspector] public UnityEvent efektyWywo�anieOtrzyma�Cios;
    [HideInInspector] public UnityEvent efektyWywo�anieZada�Cios;
    [HideInInspector] public UnityEvent efektyWywo�anieKoniecTury;
    [HideInInspector] public UnityEvent efektyWywo�aniePocz�tekTury;
    [HideInInspector] public UnityEvent efektyWywo�anieAtak;
    [HideInInspector] public float ilee;
    [HideInInspector] public bool nieUchronnee;
    [HideInInspector] public UnityEvent obecnaAkcja;
    [HideInInspector] public GameObject punktLinia;
    [HideInInspector] public GameObject atakuj�cyy;
    [HideInInspector] public playerEq Eq;
    [HideInInspector] public float otrzymywaneDMG;

    private void Awake()
    {
        //morInfo = this.gameObject.transform.GetChild(1).gameObject;
        //morInfo.SetActive(false);
        Eq = GameObject.FindGameObjectWithTag("Player").GetComponent<playerEq>();
        hpAktualne = hpMax;
        this.gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = WrugGragika;
        BazaEfektow = this.GetComponent<bazaEfektow>();
        punktLinia = this.gameObject.transform.GetChild(2).gameObject;
        ramkaCelu = this.gameObject.transform.GetChild(3).gameObject;
        ramkaCelu.SetActive(false);

        walkaStart.KoniecTury += Wywo�ajEfektyKoniecT;
        walkaStart.Pocz�tekTury += Wywo�ajEfektyPocz�tekT;
    }
    private void OnDestroy()
    {
        walkaStart.KoniecTury -= Wywo�ajEfektyKoniecT;
        walkaStart.Pocz�tekTury -= Wywo�ajEfektyPocz�tekT;
    }

    private void Update()
    {
        //hpZasady();
    }

    public void hpZasady()
    {
        if (hpAktualne > hpMax)
        {
            hpAktualne = hpMax;
        }
        else if (hpAktualne < 0)
        {
            hpAktualne = 0;
        }
        else if (hpAktualne == 0)
        {
            Die();
        }
    }

    private void Die()
    {
        GameObject walka = GameObject.FindGameObjectWithTag("nadUiWalka").transform.parent.gameObject;
        walka.GetComponent<walkaStart>().przeciwnicyWwalce.Remove(this.gameObject);
        Destroy(this.gameObject);
    }

    public void PrzyjmijDmg(float ile , bool nieUchronne, GameObject atakuj�cy)
    {
        ilee = ile;
        nieUchronnee = nieUchronne;
        atakuj�cyy = atakuj�cy;

        if (nieUchronnee)
        {
            otrzymywaneDMG = Mathf.Round(ilee);
            hpAktualne -= otrzymywaneDMG;
            Wywo�ajEfektyOtrzyma�Cios();
            if(atakuj�cy.tag == "karta")
            {
                Eq.Wywo�ajEfektyZada��Cios();
            }
        }
        else
        {
            aktualnyPancerz -= Mathf.Round(ilee);
            if (aktualnyPancerz < 0)
            {
                otrzymywaneDMG = Mathf.Abs(aktualnyPancerz);
                hpAktualne -= otrzymywaneDMG;
                Wywo�ajEfektyOtrzyma�Cios();
                if (atakuj�cy.tag == "karta")
                {
                    Eq.Wywo�ajEfektyZada��Cios();
                }
                aktualnyPancerz = 0;
            }
        }
        if (atakuj�cy.tag == "karta")
        {
            Eq.Wywo�ajEfektyAtak(); //niezalerznie od tego czy obrarzenia zosta�y zadane;
        }
        hpZasady();
    }

    public void PrzemijanieEfekt�wTura()
    {
        for (int x = 0; x < na�orzoneEfekty.Count;)
        {
            if (na�orzoneEfekty[x].TypPrzemijania == typPrzemijania.koniecTury_domy�lny)
            {
                na�orzoneEfekty[x].licznik -= 1;
                if (na�orzoneEfekty[x].licznik <= 0)
                {
                    BazaEfektow.UsunEfekt(na�orzoneEfekty[x]);
                    na�orzoneEfekty.Remove(na�orzoneEfekty[x]);
                }
                else
                {
                    x++;
                }
            }
            else if (na�orzoneEfekty[x].TypPrzemijania == typPrzemijania.koniecTuryCa�kowity)
            {
                BazaEfektow.UsunEfekt(na�orzoneEfekty[x]);
                na�orzoneEfekty.Remove(na�orzoneEfekty[x]);
            }
            else if (na�orzoneEfekty[x].TypPrzemijania == typPrzemijania.wywo�anie && na�orzoneEfekty[x].TypWywo�ania == typWywo�ania.koniecTury)
            {
                na�orzoneEfekty[x].licznik -= 1;
                if (na�orzoneEfekty[x].licznik <= 0)
                {
                    BazaEfektow.UsunEfekt(na�orzoneEfekty[x]);
                    na�orzoneEfekty.Remove(na�orzoneEfekty[x]);
                }
                else
                {
                    x++;
                }
            }
            else if (na�orzoneEfekty[x].TypPrzemijania == typPrzemijania.wywo�aniemCa�kowity && na�orzoneEfekty[x].TypWywo�ania == typWywo�ania.koniecTury)
            {
                BazaEfektow.UsunEfekt(na�orzoneEfekty[x]);
                na�orzoneEfekty.Remove(na�orzoneEfekty[x]);
            }
            else
            {
                x++;
            }
        }
    }
    public void PrzemijanieEfekt�wWywo�aniem(typWywo�ania typ)
    {
        for (int x = 0; x < na�orzoneEfekty.Count;)
        {
            if (na�orzoneEfekty[x].TypWywo�ania == typ)
            {
                if (na�orzoneEfekty[x].TypPrzemijania == typPrzemijania.wywo�anie)
                {
                    na�orzoneEfekty[x].licznik -= 1;
                    if (na�orzoneEfekty[x].licznik <= 0)
                    {
                        BazaEfektow.UsunEfekt(na�orzoneEfekty[x]);
                        na�orzoneEfekty.Remove(na�orzoneEfekty[x]);
                    }
                    else
                    {
                        x++;
                    }
                }
                else if (na�orzoneEfekty[x].TypPrzemijania == typPrzemijania.wywo�aniemCa�kowity)
                {
                    BazaEfektow.UsunEfekt(na�orzoneEfekty[x]);
                    na�orzoneEfekty.Remove(na�orzoneEfekty[x]);
                }
                else
                {
                    x++;
                }
            }
            else
            {
                x++;
            }
        }
    }


    public void Wywo�ajEfektyKoniecT(int numerTury)
    {
        if (efektyWywo�anieKoniecTury != null)
        {
            efektyWywo�anieKoniecTury.Invoke();
            PrzemijanieEfekt�wTura();
        }
    }
    public void Wywo�ajEfektyPocz�tekT(int numerTury)
    {
        if (efektyWywo�aniePocz�tekTury != null)
        {
            efektyWywo�aniePocz�tekTury.Invoke();
            PrzemijanieEfekt�wWywo�aniem(typWywo�ania.pocz�tekTury);
        }
    }
    public void Wywo�ajEfektyOtrzyma�Cios()
    {
        if (efektyWywo�anieOtrzyma�Cios != null)
        {
            efektyWywo�anieOtrzyma�Cios.Invoke();
            PrzemijanieEfekt�wWywo�aniem(typWywo�ania.otrzymanieObrarze�);
        }
    }
    public void Wywo�ajEfektyZada��Cios()
    {
        if (efektyWywo�anieZada�Cios != null)
        {
            efektyWywo�anieZada�Cios.Invoke();
            PrzemijanieEfekt�wWywo�aniem(typWywo�ania.zadawanieObrarze�);
        }
    }
    public void Wywo�ajEfektyAtak()
    {
        if (efektyWywo�anieAtak != null)
        {
            efektyWywo�anieAtak.Invoke();
            PrzemijanieEfekt�wWywo�aniem(typWywo�ania.atak);
        }
    }

    ////////////////////////////////////////////|||||| FUNKCJE PODSTAWOWE DO DZIA�A� PRZECIWNIKA |||||||///////////////////////

    public void KolejnaAkcjaWywo�aj() //wywo�uje kolejn� akcj� lub je�li jest ostatnia zaka�cza akcje tego wroga;
    {
        for (int x = 0; x < dzia�anie.Count; x++)
        {
            if (obecnaAkcja == dzia�anie[x])
            {
                if (x < dzia�anie.Count -1)
                {
                    AkcjaNumerWywo�aj(x + 1);
                }
                else if (x == dzia�anie.Count - 1)
                {
                    KoniecAkcjiWroga();
                }
            }
        }
    }
    public void AkcjaNumerWywo�aj(int numerAkcji) //wywo�uje kolejn� akcj� o podanym numerze;
    {
        obecnaAkcja = dzia�anie[numerAkcji];
        dzia�anie[numerAkcji].Invoke();
    }

    public void KoniecAkcjiWroga() //ko�czy akcje tego wroga, je�li jest ostatni to zaczyna tur� gracza;
    {
        GameObject walka = GameObject.FindGameObjectWithTag("nadUiWalka").transform.parent.gameObject;
        walkaStart WalkaStart = walka.GetComponent<walkaStart>();

        for (int x = 0; x < WalkaStart.przeciwnicyWwalce.Count; x++)
        {
            if (WalkaStart.przeciwnicyWwalce[x] == this.gameObject)
            {
                if (x < WalkaStart.przeciwnicyWwalce.Count - 1)
                {
                    WalkaStart.AkcjaWroga(x + 1);
                }
                else if (x == WalkaStart.przeciwnicyWwalce.Count - 1)
                {
                    WalkaStart.koniecTury();
                }
            }
        }
    }
}
