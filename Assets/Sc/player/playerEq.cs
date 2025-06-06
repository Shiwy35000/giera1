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
    public float bonusDoObrarzeń;
    public int ileKartDobiera = 5;
    public List<efekty> nałorzoneEfekty;

    [Header("karty w walce")] //noramlnie będą nie widoczne
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
    [HideInInspector] public UnityEvent efektyWywołanieOtrzymałCios;
    [HideInInspector] public UnityEvent efektyWywołanieZadałCios;
    [HideInInspector] public UnityEvent efektyWywołanieKoniecTury;
    [HideInInspector] public UnityEvent efektyWywołaniePoczątekTury;
    [HideInInspector] public UnityEvent efektyWywołanieAtak;
    [HideInInspector] public float ilee;
    [HideInInspector] public bool nieUchronnee;
    [HideInInspector] public GameObject atakującyy;
    [HideInInspector] public float otrzymywaneDMG;
    public GameObject listaArtefaktów;

    void Awake()
    {
        maxEnergia = 3; // narazie?
        dialog.Walka += MaxEnergiaWalka;
        dialog.Walka += CzyścimyListy;
        walkaStart.KoniecTury += MaxEnergiaTura;
        walkaStart.KoniecTury += WywołajEfektyKoniecT;
        walkaStart.PoczątekTury += WywołajEfektyPoczątekT;
        BazaEfektow = this.GetComponent<bazaEfektow>();
        sortZ = GameObject.FindGameObjectWithTag("dlon").gameObject.GetComponent<sortGrupZ>();

        ArtefaktyStart();
    }
    private void OnDestroy()
    {
        dialog.Walka -= MaxEnergiaWalka;
        dialog.Walka -= CzyścimyListy;
        walkaStart.KoniecTury -= MaxEnergiaTura;
        walkaStart.KoniecTury -= WywołajEfektyKoniecT;
        walkaStart.PoczątekTury -= WywołajEfektyPoczątekT;
    }

    private void MaxEnergiaWalka(bool nic)
    {
        aktualnaEnergia = maxEnergia;
    }

    private void MaxEnergiaTura(int nic)
    {
        aktualnaEnergia = maxEnergia;
    }

    void ArtefaktyStart() //tylko narazie potem będzei wszystko robione ArtefaktPrzypisz!!
    {
        for (int x = 0; x < posiadaneArtefakty.Count;x++)
        {
            GameObject it = Instantiate(posiadaneArtefakty[x], listaArtefaktów.transform);
            it.name = posiadaneArtefakty[x].name;
        }
    }

    public void ArtefaktPrzypisz(GameObject A)
    {
        GameObject it = Instantiate(A, listaArtefaktów.transform);
        it.name = A.name;
        posiadaneArtefakty.Add(it);
    }
    public void UsuńArtefakt(int Id)
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
    private void CzyścimyListy(bool walka)
    {
        if (walka == false)
        {
            cmentarz = new List<GameObject>();
            wykluczone = new List<GameObject>();
            deck = new List<GameObject>();
            sortZ.kartyWDłoni = new List<GameObject>();
        }
    }

    public void PrzyjmijDmg(float ile, bool nieUchronne, GameObject atakujący)
    {
        ilee = ile;
        nieUchronnee = nieUchronne;
        atakującyy = atakujący;

        if (nieUchronnee)
        {
            otrzymywaneDMG = Mathf.Round(ilee);
            hp -= otrzymywaneDMG;
            WywołajEfektyOtrzymałCios();
            if(atakujący.tag == "wrug")
            {
                atakujący.GetComponent<WRUG1>().WywołajEfektyZadałłCios();
            }
        }
        else
        {
            aktualnyPancerz -= Mathf.Round(ilee);
            if (aktualnyPancerz < 0)
            {
                otrzymywaneDMG = Mathf.Abs(aktualnyPancerz);
                hp -= otrzymywaneDMG;
                WywołajEfektyOtrzymałCios();
                if (atakujący.tag == "wrug")
                {
                    atakujący.GetComponent<WRUG1>().WywołajEfektyZadałłCios();
                }
                aktualnyPancerz = 0;
            }
        }
        if (atakujący.tag == "wrug")
        {
            atakujący.GetComponent<WRUG1>().WywołajEfektyAtak(); //niezalerznie od tego czy obrarzenia zostały zadane;
        }
        hpZasady();
    }
    public void PrzemijanieEfektówTura()
    {
        for (int x = 0; x < nałorzoneEfekty.Count;)
        {
            if (nałorzoneEfekty[x].TypPrzemijania == typPrzemijania.koniecTury_domyślny)
            {
                nałorzoneEfekty[x].licznik -= 1;
                if (nałorzoneEfekty[x].licznik <= 0)
                {
                    BazaEfektow.UsunEfekt(nałorzoneEfekty[x]);
                    nałorzoneEfekty.Remove(nałorzoneEfekty[x]);
                }
                else
                {
                    x++;
                }
            }
            else if (nałorzoneEfekty[x].TypPrzemijania == typPrzemijania.koniecTuryCałkowity)
            {
                BazaEfektow.UsunEfekt(nałorzoneEfekty[x]);
                nałorzoneEfekty.Remove(nałorzoneEfekty[x]);
            }
            else if(nałorzoneEfekty[x].TypPrzemijania == typPrzemijania.wywołanie && nałorzoneEfekty[x].TypWywołania == typWywołania.koniecTury)
            {
                nałorzoneEfekty[x].licznik -= 1;
                if (nałorzoneEfekty[x].licznik <= 0)
                {
                    BazaEfektow.UsunEfekt(nałorzoneEfekty[x]);
                    nałorzoneEfekty.Remove(nałorzoneEfekty[x]);
                }
                else
                {
                    x++;
                }
            }
            else if (nałorzoneEfekty[x].TypPrzemijania == typPrzemijania.wywołaniemCałkowity && nałorzoneEfekty[x].TypWywołania == typWywołania.koniecTury)
            {
                BazaEfektow.UsunEfekt(nałorzoneEfekty[x]);
                nałorzoneEfekty.Remove(nałorzoneEfekty[x]);
            }
            else
            {
                x++;
            }
        }
    }
    public void PrzemijanieEfektówWywołaniem(typWywołania typ)
    {
        for (int x = 0; x < nałorzoneEfekty.Count;)
        {
            if (nałorzoneEfekty[x].TypWywołania == typ)
            {
                if (nałorzoneEfekty[x].TypPrzemijania == typPrzemijania.wywołanie)
                {
                    nałorzoneEfekty[x].licznik -= 1;
                    if (nałorzoneEfekty[x].licznik <= 0)
                    {
                        BazaEfektow.UsunEfekt(nałorzoneEfekty[x]);
                        nałorzoneEfekty.Remove(nałorzoneEfekty[x]);
                    }
                    else
                    {
                        x++;
                    }
                }
                else if (nałorzoneEfekty[x].TypPrzemijania == typPrzemijania.wywołaniemCałkowity)
                {
                    BazaEfektow.UsunEfekt(nałorzoneEfekty[x]);
                    nałorzoneEfekty.Remove(nałorzoneEfekty[x]);
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
    public void WywołajEfektyKoniecT(int numerTury)
    {
        if(efektyWywołanieKoniecTury != null)
        {
            efektyWywołanieKoniecTury.Invoke();
            PrzemijanieEfektówTura();
        }
    }
    public void WywołajEfektyPoczątekT(int numerTury)
    {
        if (efektyWywołaniePoczątekTury != null)
        {
            efektyWywołaniePoczątekTury.Invoke();
            PrzemijanieEfektówWywołaniem(typWywołania.początekTury);
        }
    }
    public void WywołajEfektyOtrzymałCios()
    {
        if (efektyWywołanieOtrzymałCios != null)
        {
            efektyWywołanieOtrzymałCios.Invoke();
            PrzemijanieEfektówWywołaniem(typWywołania.otrzymanieObrarzeń);
        }
    }
    public void WywołajEfektyZadałłCios()
    {
        if (efektyWywołanieZadałCios != null)
        {
            efektyWywołanieZadałCios.Invoke();
            PrzemijanieEfektówWywołaniem(typWywołania.zadawanieObrarzeń);
        }
    }
    public void WywołajEfektyAtak()
    {
        if (efektyWywołanieAtak != null)
        {
            efektyWywołanieAtak.Invoke();
            PrzemijanieEfektówWywołaniem(typWywołania.atak);
        }
    }

    private void Die()
    {
        //Destroy(this.gameObject);
    }
}
