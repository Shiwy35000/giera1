using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class playerEq : MonoBehaviour
{
    [Header("Dane w Walce")]
    public float hp;
    public float hpMax;
    public int aktualnaEnergia;
    public int maxEnergia;
    public float aktualnyPancerz;
    public float bonusDoObrarzeñ;
    public int ileKartDobiera = 5;
    public List<efekty> na³orzoneEfekty;

    [Header("karty w walce")] //noramlnie bêd¹ nie widoczne
    public List<GameObject> deck;
    public List<GameObject> cmentarz;
    public List<GameObject> wykluczone;

    [Header("Ekwipunek")]
    public List<artefakt> posiadaneArtefakty;
    public List<GameObject> deckPrefab;

    [Header("Inne")]
    public int sakiewka;
    public float rzar;
    public List<nowyDialogTyp> dialogiWybory;

    //przypisy
    private bazaEfektow BazaEfektow;
    private sortGrupZ sortZ;
    [HideInInspector] public UnityEvent efektyWywo³anieOtrzyma³Cios;
    [HideInInspector] public UnityEvent efektyWywo³anieZada³Cios;
    [HideInInspector] public UnityEvent efektyWywo³anieKoniecTury;
    [HideInInspector] public UnityEvent efektyWywo³aniePocz¹tekTury;
    [HideInInspector] public float ilee;
    [HideInInspector] public bool nieUchronnee;
    [HideInInspector] public GameObject atakuj¹cyy;
    [HideInInspector] public float otrzymywaneDMG;

    void Awake()
    {
        maxEnergia = 3; // narazie?
        dialog.Walka += MaxEnergiaWalka;
        dialog.Walka += CzyœcimyListy;
        walkaStart.KoniecTury += MaxEnergiaTura;
        walkaStart.KoniecTury += Wywo³ajEfektyKoniecT;
        walkaStart.Pocz¹tekTury += Wywo³ajEfektyPocz¹tekT;
        BazaEfektow = this.GetComponent<bazaEfektow>();
        sortZ = GameObject.FindGameObjectWithTag("dlon").gameObject.GetComponent<sortGrupZ>();
    }
    private void OnDestroy()
    {
        dialog.Walka -= MaxEnergiaWalka;
        dialog.Walka -= CzyœcimyListy;
        walkaStart.KoniecTury -= MaxEnergiaTura;
        walkaStart.KoniecTury -= Wywo³ajEfektyKoniecT;
        walkaStart.Pocz¹tekTury -= Wywo³ajEfektyPocz¹tekT;
    }

    private void MaxEnergiaWalka(bool nic)
    {
        aktualnaEnergia = maxEnergia;
    }

    private void MaxEnergiaTura(int nic)
    {
        aktualnaEnergia = maxEnergia;
    }

    private void Update()
    {
        hpZasady();
    }

    private void hpZasady()
    {
        if (hp > hpMax)
        {
            hp = hpMax;
        }
        else if (hp < 0)
        {
            hp = 0;
        }
        else if (hp == 0)
        {
            Die();
        }
    }
    private void CzyœcimyListy(bool walka)
    {
        if (walka == false)
        {
            cmentarz = new List<GameObject>();
            wykluczone = new List<GameObject>();
            deck = new List<GameObject>();
            sortZ.kartyWD³oni = new List<GameObject>();
        }
    }

    public void PrzyjmijDmg(float ile, bool nieUchronne, GameObject atakuj¹cy)
    {
        ilee = ile;
        nieUchronnee = nieUchronne;
        atakuj¹cyy = atakuj¹cy;

        if (nieUchronnee)
        {
            otrzymywaneDMG = Mathf.Round(ilee);
            hp -= otrzymywaneDMG;
            Wywo³ajEfektyOtrzyma³Cios();
            if(atakuj¹cy.tag == "wrug")
            {
                atakuj¹cy.GetComponent<WRUG1>().Wywo³ajEfektyZada³³Cios();
            }
        }
        else
        {
            aktualnyPancerz -= Mathf.Round(ilee);
            if (aktualnyPancerz < 0)
            {
                otrzymywaneDMG = Mathf.Abs(aktualnyPancerz);
                hp -= otrzymywaneDMG;
                Wywo³ajEfektyOtrzyma³Cios();
                if (atakuj¹cy.tag == "wrug")
                {
                    atakuj¹cy.GetComponent<WRUG1>().Wywo³ajEfektyZada³³Cios();
                }
                aktualnyPancerz = 0;
            }
        }
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
            else if(na³orzoneEfekty[x].TypPrzemijania == typPrzemijania.wywo³anie && na³orzoneEfekty[x].TypWywo³ania == typWywo³ania.koniecTury)
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
        if(efektyWywo³anieKoniecTury != null)
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
    
    private void Die()
    {
        //Destroy(this.gameObject);
    }
}
