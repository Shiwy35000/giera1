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
    [SerializeField] public List<UnityEvent> dzia³anie;
   
    [Header("Dane w Walce")]
    public float hpAktualne;
    public float hpMax;
    public float aktualnyPancerz;
    public float bonusDoObrarzeñ;
    public List<efekty> na³orzoneEfekty;
    public GameObject ramkaCelu;
    //pozosta³e / przypisy
    //[HideInInspector] public GameObject morInfo;
    private bazaEfektow BazaEfektow;
    [HideInInspector] public UnityEvent efektyWywo³anieOtrzyma³Cios;
    [HideInInspector] public UnityEvent efektyWywo³anieZada³Cios;
    [HideInInspector] public UnityEvent efektyWywo³anieKoniecTury;
    [HideInInspector] public UnityEvent efektyWywo³aniePocz¹tekTury;
    [HideInInspector] public UnityEvent efektyWywo³anieAtak;
    [HideInInspector] public float ilee;
    [HideInInspector] public bool nieUchronnee;
    [HideInInspector] public UnityEvent obecnaAkcja;
    [HideInInspector] public GameObject punktLinia;
    [HideInInspector] public GameObject atakuj¹cyy;
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

        walkaStart.KoniecTury += Wywo³ajEfektyKoniecT;
        walkaStart.Pocz¹tekTury += Wywo³ajEfektyPocz¹tekT;
    }
    private void OnDestroy()
    {
        walkaStart.KoniecTury -= Wywo³ajEfektyKoniecT;
        walkaStart.Pocz¹tekTury -= Wywo³ajEfektyPocz¹tekT;
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

    public void PrzyjmijDmg(float ile , bool nieUchronne, GameObject atakuj¹cy)
    {
        ilee = ile;
        nieUchronnee = nieUchronne;
        atakuj¹cyy = atakuj¹cy;

        if (nieUchronnee)
        {
            otrzymywaneDMG = Mathf.Round(ilee);
            hpAktualne -= otrzymywaneDMG;
            Wywo³ajEfektyOtrzyma³Cios();
            if(atakuj¹cy.tag == "karta")
            {
                Eq.Wywo³ajEfektyZada³³Cios();
            }
        }
        else
        {
            aktualnyPancerz -= Mathf.Round(ilee);
            if (aktualnyPancerz < 0)
            {
                otrzymywaneDMG = Mathf.Abs(aktualnyPancerz);
                hpAktualne -= otrzymywaneDMG;
                Wywo³ajEfektyOtrzyma³Cios();
                if (atakuj¹cy.tag == "karta")
                {
                    Eq.Wywo³ajEfektyZada³³Cios();
                }
                aktualnyPancerz = 0;
            }
        }
        if (atakuj¹cy.tag == "karta")
        {
            Eq.Wywo³ajEfektyAtak(); //niezalerznie od tego czy obrarzenia zosta³y zadane;
        }
        hpZasady();
    }

    public void PrzemijanieEfektówTura()
    {
        for (int x = 0; x < na³orzoneEfekty.Count;)
        {
            if (na³orzoneEfekty[x].TypPrzemijania == typPrzemijania.koniecTury_domyœlny)
            {
                na³orzoneEfekty[x].licznik -= 1;
                if (na³orzoneEfekty[x].licznik <= 0)
                {
                    BazaEfektow.UsunEfekt(na³orzoneEfekty[x]);
                    na³orzoneEfekty.Remove(na³orzoneEfekty[x]);
                }
                else
                {
                    x++;
                }
            }
            else if (na³orzoneEfekty[x].TypPrzemijania == typPrzemijania.koniecTuryCa³kowity)
            {
                BazaEfektow.UsunEfekt(na³orzoneEfekty[x]);
                na³orzoneEfekty.Remove(na³orzoneEfekty[x]);
            }
            else if (na³orzoneEfekty[x].TypPrzemijania == typPrzemijania.wywo³anie && na³orzoneEfekty[x].TypWywo³ania == typWywo³ania.koniecTury)
            {
                na³orzoneEfekty[x].licznik -= 1;
                if (na³orzoneEfekty[x].licznik <= 0)
                {
                    BazaEfektow.UsunEfekt(na³orzoneEfekty[x]);
                    na³orzoneEfekty.Remove(na³orzoneEfekty[x]);
                }
                else
                {
                    x++;
                }
            }
            else if (na³orzoneEfekty[x].TypPrzemijania == typPrzemijania.wywo³aniemCa³kowity && na³orzoneEfekty[x].TypWywo³ania == typWywo³ania.koniecTury)
            {
                BazaEfektow.UsunEfekt(na³orzoneEfekty[x]);
                na³orzoneEfekty.Remove(na³orzoneEfekty[x]);
            }
            else
            {
                x++;
            }
        }
    }
    public void PrzemijanieEfektówWywo³aniem(typWywo³ania typ)
    {
        for (int x = 0; x < na³orzoneEfekty.Count;)
        {
            if (na³orzoneEfekty[x].TypWywo³ania == typ)
            {
                if (na³orzoneEfekty[x].TypPrzemijania == typPrzemijania.wywo³anie)
                {
                    na³orzoneEfekty[x].licznik -= 1;
                    if (na³orzoneEfekty[x].licznik <= 0)
                    {
                        BazaEfektow.UsunEfekt(na³orzoneEfekty[x]);
                        na³orzoneEfekty.Remove(na³orzoneEfekty[x]);
                    }
                    else
                    {
                        x++;
                    }
                }
                else if (na³orzoneEfekty[x].TypPrzemijania == typPrzemijania.wywo³aniemCa³kowity)
                {
                    BazaEfektow.UsunEfekt(na³orzoneEfekty[x]);
                    na³orzoneEfekty.Remove(na³orzoneEfekty[x]);
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


    public void Wywo³ajEfektyKoniecT(int numerTury)
    {
        if (efektyWywo³anieKoniecTury != null)
        {
            efektyWywo³anieKoniecTury.Invoke();
            PrzemijanieEfektówTura();
        }
    }
    public void Wywo³ajEfektyPocz¹tekT(int numerTury)
    {
        if (efektyWywo³aniePocz¹tekTury != null)
        {
            efektyWywo³aniePocz¹tekTury.Invoke();
            PrzemijanieEfektówWywo³aniem(typWywo³ania.pocz¹tekTury);
        }
    }
    public void Wywo³ajEfektyOtrzyma³Cios()
    {
        if (efektyWywo³anieOtrzyma³Cios != null)
        {
            efektyWywo³anieOtrzyma³Cios.Invoke();
            PrzemijanieEfektówWywo³aniem(typWywo³ania.otrzymanieObrarzeñ);
        }
    }
    public void Wywo³ajEfektyZada³³Cios()
    {
        if (efektyWywo³anieZada³Cios != null)
        {
            efektyWywo³anieZada³Cios.Invoke();
            PrzemijanieEfektówWywo³aniem(typWywo³ania.zadawanieObrarzeñ);
        }
    }
    public void Wywo³ajEfektyAtak()
    {
        if (efektyWywo³anieAtak != null)
        {
            efektyWywo³anieAtak.Invoke();
            PrzemijanieEfektówWywo³aniem(typWywo³ania.atak);
        }
    }

    ////////////////////////////////////////////|||||| FUNKCJE PODSTAWOWE DO DZIA£AÑ PRZECIWNIKA |||||||///////////////////////

    public void KolejnaAkcjaWywo³aj() //wywo³uje kolejn¹ akcjê lub jeœli jest ostatnia zakañcza akcje tego wroga;
    {
        for (int x = 0; x < dzia³anie.Count; x++)
        {
            if (obecnaAkcja == dzia³anie[x])
            {
                if (x < dzia³anie.Count -1)
                {
                    AkcjaNumerWywo³aj(x + 1);
                }
                else if (x == dzia³anie.Count - 1)
                {
                    KoniecAkcjiWroga();
                }
            }
        }
    }
    public void AkcjaNumerWywo³aj(int numerAkcji) //wywo³uje kolejn¹ akcjê o podanym numerze;
    {
        obecnaAkcja = dzia³anie[numerAkcji];
        dzia³anie[numerAkcji].Invoke();
    }

    public void KoniecAkcjiWroga() //koñczy akcje tego wroga, jeœli jest ostatni to zaczyna turê gracza;
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
