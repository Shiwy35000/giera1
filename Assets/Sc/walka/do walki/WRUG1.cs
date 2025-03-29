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

    //pozosta³e / przypisy
    [HideInInspector] public GameObject morInfo;
    private bazaEfektow BazaEfektow;
    [HideInInspector] public UnityEvent efektyWywo³anieOtrzyma³Cios;
    [HideInInspector] public UnityEvent efektyWywo³anieZada³Cios;
    [HideInInspector] public UnityEvent efektyWywo³anieKoniecTury;
    [HideInInspector] public float ilee;
    [HideInInspector] public bool nieUchronnee;
    [HideInInspector] public UnityEvent obecnaAkcja;

    private void Awake()
    {
        morInfo = this.gameObject.transform.GetChild(1).gameObject;
        morInfo.SetActive(false);
        hpAktualne = hpMax;
        this.gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = WrugGragika;
        BazaEfektow = this.GetComponent<bazaEfektow>();
        walkaStart.KoniecTury += Wywo³ajEfektyKoniecT;
        walkaStart.KoniecTury += PrzemijanieEfektuw;
    }
    private void OnDestroy()
    {
        walkaStart.KoniecTury -= Wywo³ajEfektyKoniecT;
        walkaStart.KoniecTury -= PrzemijanieEfektuw;
    }

    private void Update()
    {
        hpZasady();
    }

    private void hpZasady()
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

    public void PrzyjmijDmg(float ile , bool nieUchronne)
    {
        ilee = ile;
        nieUchronnee = nieUchronne;
        Wywo³ajEfektyOtrzyma³Cios();

        if (nieUchronnee)
        {
            hpAktualne -= Mathf.Round(ilee);
        }
        else
        {
            float z;
            aktualnyPancerz -= Mathf.Round(ilee);
            if (aktualnyPancerz < 0)
            {
                z = Mathf.Abs(aktualnyPancerz);
                hpAktualne -= z;
                aktualnyPancerz = 0;
            }
        }
    }

    public void PrzemijanieEfektuw(int numerTury)
    {
        for (int x = 0; x < na³orzoneEfekty.Count;)
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
    }

    public void Wywo³ajEfektyKoniecT(int numerTury)
    {
        if (efektyWywo³anieKoniecTury != null)
        {
            efektyWywo³anieKoniecTury.Invoke();
        }
    }
    public void Wywo³ajEfektyOtrzyma³Cios()
    {
        if (efektyWywo³anieOtrzyma³Cios != null)
        {
            efektyWywo³anieOtrzyma³Cios.Invoke();
        }
    }
    public void Wywo³ajEfektyZada³³Cios()
    {
        if (efektyWywo³anieZada³Cios != null)
        {
            efektyWywo³anieZada³Cios.Invoke();
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
