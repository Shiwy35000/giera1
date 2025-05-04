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
    public float bonusDoObrarze�;
    public int ileKartDobiera = 5;
    public List<efekty> na�orzoneEfekty;

    [Header("karty w walce")] //noramlnie b�d� nie widoczne
    public List<GameObject> deck;
    public List<GameObject> cmentarz;
    public List<GameObject> wykluczone;

    [Header("Ekwipunek")]
    public List<GameObject> posiadaneArtefakty;
    public List<GameObject> deckPrefab;

    [Header("Inne")]
    public int sakiewka;
    public float rzar;
    public List<nowyDialogTyp> dialogiWybory;

    //przypisy
    private bazaEfektow BazaEfektow;
    private sortGrupZ sortZ;
    [HideInInspector] public UnityEvent efektyWywo�anieOtrzyma�Cios;
    [HideInInspector] public UnityEvent efektyWywo�anieZada�Cios;
    [HideInInspector] public UnityEvent efektyWywo�anieKoniecTury;
    [HideInInspector] public UnityEvent efektyWywo�aniePocz�tekTury;
    [HideInInspector] public UnityEvent efektyWywo�anieAtak;
    [HideInInspector] public float ilee;
    [HideInInspector] public bool nieUchronnee;
    [HideInInspector] public GameObject atakuj�cyy;
    [HideInInspector] public float otrzymywaneDMG;
    public GameObject listaArtefakt�w;

    void Awake()
    {
        maxEnergia = 3; // narazie?
        dialog.Walka += MaxEnergiaWalka;
        dialog.Walka += Czy�cimyListy;
        walkaStart.KoniecTury += MaxEnergiaTura;
        walkaStart.KoniecTury += Wywo�ajEfektyKoniecT;
        walkaStart.Pocz�tekTury += Wywo�ajEfektyPocz�tekT;
        BazaEfektow = this.GetComponent<bazaEfektow>();
        sortZ = GameObject.FindGameObjectWithTag("dlon").gameObject.GetComponent<sortGrupZ>();

        ArtefaktyStart();
    }
    private void OnDestroy()
    {
        dialog.Walka -= MaxEnergiaWalka;
        dialog.Walka -= Czy�cimyListy;
        walkaStart.KoniecTury -= MaxEnergiaTura;
        walkaStart.KoniecTury -= Wywo�ajEfektyKoniecT;
        walkaStart.Pocz�tekTury -= Wywo�ajEfektyPocz�tekT;
    }

    private void MaxEnergiaWalka(bool nic)
    {
        aktualnaEnergia = maxEnergia;
    }

    private void MaxEnergiaTura(int nic)
    {
        aktualnaEnergia = maxEnergia;
    }

    void ArtefaktyStart() //tylko narazie potem b�dzei wszystko robione ArtefaktPrzypisz!!
    {
        for (int x = 0; x < posiadaneArtefakty.Count;x++)
        {
            GameObject it = Instantiate(posiadaneArtefakty[x], listaArtefakt�w.transform);
            it.name = posiadaneArtefakty[x].name;
        }
    }

    public void ArtefaktPrzypisz(GameObject A)
    {
        GameObject it = Instantiate(A, listaArtefakt�w.transform);
        it.name = A.name;
        posiadaneArtefakty.Add(it);
    }
    public void Usu�Artefakt(int Id)
    {
        for (int x = 0; x < posiadaneArtefakty.Count; x++)
        {
            if(posiadaneArtefakty[x].GetComponent<artefakt>().Id == Id)
            {
                GameObject it = posiadaneArtefakty[x];
                posiadaneArtefakty.Remove(posiadaneArtefakty[x]);
                Destroy(it);
                break;
            }
        }
    }

    public void hpZasady()
    {
        if (hp > hpMax)
        {
            hp = hpMax;
        }
        else if (hp < 0)
        {
            hp = 0;
        }
        if (hp == 0)
        {
            Die();
        }
    }

    public void sakiewkaZasady()
    {
        if (sakiewka < 0)
        {
            sakiewka = 0;
        }
    }
    private void Czy�cimyListy(bool walka)
    {
        if (walka == false)
        {
            cmentarz = new List<GameObject>();
            wykluczone = new List<GameObject>();
            deck = new List<GameObject>();
            sortZ.kartyWD�oni = new List<GameObject>();
        }
    }

    public void PrzyjmijDmg(float ile, bool nieUchronne, GameObject atakuj�cy)
    {
        ilee = ile;
        nieUchronnee = nieUchronne;
        atakuj�cyy = atakuj�cy;

        if (nieUchronnee)
        {
            otrzymywaneDMG = Mathf.Round(ilee);
            hp -= otrzymywaneDMG;
            Wywo�ajEfektyOtrzyma�Cios();
            if(atakuj�cy.tag == "wrug")
            {
                atakuj�cy.GetComponent<WRUG1>().Wywo�ajEfektyZada��Cios();
            }
        }
        else
        {
            aktualnyPancerz -= Mathf.Round(ilee);
            if (aktualnyPancerz < 0)
            {
                otrzymywaneDMG = Mathf.Abs(aktualnyPancerz);
                hp -= otrzymywaneDMG;
                Wywo�ajEfektyOtrzyma�Cios();
                if (atakuj�cy.tag == "wrug")
                {
                    atakuj�cy.GetComponent<WRUG1>().Wywo�ajEfektyZada��Cios();
                }
                aktualnyPancerz = 0;
            }
        }
        if (atakuj�cy.tag == "wrug")
        {
            atakuj�cy.GetComponent<WRUG1>().Wywo�ajEfektyAtak(); //niezalerznie od tego czy obrarzenia zosta�y zadane;
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
            else if(na�orzoneEfekty[x].TypPrzemijania == typPrzemijania.wywo�anie && na�orzoneEfekty[x].TypWywo�ania == typWywo�ania.koniecTury)
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
        if(efektyWywo�anieKoniecTury != null)
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

    private void Die()
    {
        //Destroy(this.gameObject);
    }
}
