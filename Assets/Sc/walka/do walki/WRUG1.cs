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
    [SerializeField] public List<UnityEvent> działanie;
   
    [Header("Dane w Walce")]
    public float hpAktualne;
    public float hpMax;
    public float aktualnyPancerz;
    public float bonusDoObrarzeń;
    public List<efekty> nałorzoneEfekty;
    public GameObject ramkaCelu;
    //pozostałe / przypisy
    //[HideInInspector] public GameObject morInfo;
    private bazaEfektow BazaEfektow;
    [HideInInspector] public UnityEvent efektyWywołanieOtrzymałCios;
    [HideInInspector] public UnityEvent efektyWywołanieZadałCios;
    [HideInInspector] public UnityEvent efektyWywołanieKoniecTury;
    [HideInInspector] public UnityEvent efektyWywołaniePoczątekTury;
    [HideInInspector] public UnityEvent efektyWywołanieAtak;
    [HideInInspector] public float ilee;
    [HideInInspector] public bool nieUchronnee;
    [HideInInspector] public UnityEvent obecnaAkcja;
    [HideInInspector] public GameObject punktLinia;
    [HideInInspector] public GameObject atakującyy;
    [HideInInspector] public playerEq Eq;
    [HideInInspector] public float otrzymywaneDMG;

    [HideInInspector] public GameObject zDialogu;

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
        //ramkaCelu.SetActive(false);

        walkaStart.KoniecTury += WywołajEfektyKoniecT;
        walkaStart.PoczątekTury += WywołajEfektyPoczątekT;
    }
    private void OnDestroy()
    {
        walkaStart.KoniecTury -= WywołajEfektyKoniecT;
        walkaStart.PoczątekTury -= WywołajEfektyPoczątekT;
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
        if (hpAktualne == 0)
        {
            Die();
        }
    }

    private void Die()
    {
        GameObject walka = GameObject.FindGameObjectWithTag("nadUiWalka").transform.parent.gameObject;
        walka.GetComponent<walkaStart>().przeciwnicyWwalce.Remove(this.gameObject);

        if (walka.GetComponent<walkaStart>().przeciwnicyWwalce.Count == 0)
        {
            zDialogu.GetComponent<dialog>().PoWalce();
        }

        Destroy(this.gameObject);
    }

    public void PrzyjmijDmg(float ile , bool nieUchronne, GameObject atakujący)
    {
        ilee = ile;
        nieUchronnee = nieUchronne;
        atakującyy = atakujący;

        if (nieUchronnee)
        {
            otrzymywaneDMG = Mathf.Round(ilee);
            hpAktualne -= otrzymywaneDMG;
            WywołajEfektyOtrzymałCios();
            if(atakujący.tag == "karta")
            {
                Eq.WywołajEfektyZadałłCios();
            }
        }
        else
        {
            aktualnyPancerz -= Mathf.Round(ilee);
            if (aktualnyPancerz < 0)
            {
                otrzymywaneDMG = Mathf.Abs(aktualnyPancerz);
                hpAktualne -= otrzymywaneDMG;
                WywołajEfektyOtrzymałCios();
                if (atakujący.tag == "karta")
                {
                    Eq.WywołajEfektyZadałłCios();
                }
                aktualnyPancerz = 0;
            }
        }
        if (atakujący.tag == "karta")
        {
            Eq.WywołajEfektyAtak(); //niezalerznie od tego czy obrarzenia zostały zadane;
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
            else if (nałorzoneEfekty[x].TypPrzemijania == typPrzemijania.wywołanie && nałorzoneEfekty[x].TypWywołania == typWywołania.koniecTury)
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
        if (efektyWywołanieKoniecTury != null)
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

    ////////////////////////////////////////////|||||| FUNKCJE PODSTAWOWE DO DZIAŁAŃ PRZECIWNIKA |||||||///////////////////////

    public void KolejnaAkcjaWywołaj() //wywołuje kolejną akcję lub jeśli jest ostatnia zakańcza akcje tego wroga;
    {
        for (int x = 0; x < działanie.Count; x++)
        {
            if (obecnaAkcja == działanie[x])
            {
                if (x < działanie.Count -1)
                {
                    AkcjaNumerWywołaj(x + 1);
                }
                else if (x == działanie.Count - 1)
                {
                    KoniecAkcjiWroga();
                }
            }
        }
    }
    public void AkcjaNumerWywołaj(int numerAkcji) //wywołuje kolejną akcję o podanym numerze;
    {
        obecnaAkcja = działanie[numerAkcji];
        działanie[numerAkcji].Invoke();
    }

    public void KoniecAkcjiWroga() //kończy akcje tego wroga, jeśli jest ostatni to zaczyna turę gracza;
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
