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

    void Awake()
    {
        maxEnergia = 3; // narazie?
        dialog.Walka += MaxEnergiaWalka;
        dialog.Walka += CzyœcimyListy;
        walkaStart.KoniecTury += MaxEnergiaTura;
        walkaStart.KoniecTury += Wywo³ajEfektyKoniecT;
        //walkaStart.KoniecTury += PrzemijanieEfektuw;
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
        //walkaStart.KoniecTury -= PrzemijanieEfektuw;
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

    public void PrzyjmijDmg(float ile, bool nieUchronne)
    {
        ilee = ile;
        nieUchronnee = nieUchronne;
        Wywo³ajEfektyOtrzyma³Cios();

        if (nieUchronnee)
        {
            hp -= Mathf.Round(ilee);
        }
        else
        {
            float z;
            aktualnyPancerz -= Mathf.Round(ilee);
            if (aktualnyPancerz < 0)
            {
                z = Mathf.Abs(aktualnyPancerz);
                hp -= z;
                aktualnyPancerz = 0;
            }
        }
    }

    public void PrzemijanieEfektuw(typWywo³ania typ)
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
            else if (na³orzoneEfekty[x].TypPrzemijania == typPrzemijania.wywo³anie && na³orzoneEfekty[x].TypWywo³ania == typ)
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
            else if (na³orzoneEfekty[x].TypPrzemijania == typPrzemijania.koniecTuryCa³kowity && na³orzoneEfekty[x].TypWywo³ania == typ)
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

    public void Wywo³ajEfektyKoniecT(int numerTury)
    {
        if(efektyWywo³anieKoniecTury != null)
        {
            efektyWywo³anieKoniecTury.Invoke();
            typWywo³ania t = typWywo³ania.koniecTury;
            PrzemijanieEfektuw(t);
        }
    }
    public void Wywo³ajEfektyPocz¹tekT(int numerTury)
    {
        if (efektyWywo³aniePocz¹tekTury != null)
        {
            efektyWywo³aniePocz¹tekTury.Invoke();
            typWywo³ania t = typWywo³ania.pocz¹tekTury;
            PrzemijanieEfektuw(t);
        }
    }
    public void Wywo³ajEfektyOtrzyma³Cios()
    {
        if (efektyWywo³anieOtrzyma³Cios != null)
        {
            efektyWywo³anieOtrzyma³Cios.Invoke();
            typWywo³ania t = typWywo³ania.otrzymanieObrarzeñ;
            PrzemijanieEfektuw(t);
        }
    }
    public void Wywo³ajEfektyZada³³Cios()
    {
        if (efektyWywo³anieZada³Cios != null)
        {
            efektyWywo³anieZada³Cios.Invoke();
            typWywo³ania t = typWywo³ania.zadawanieObrarzeñ;
            PrzemijanieEfektuw(t);
        }
    }
    
    private void Die()
    {
        //Destroy(this.gameObject);
    }
}
