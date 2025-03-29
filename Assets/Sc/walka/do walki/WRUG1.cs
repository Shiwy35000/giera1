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

    //pozosta�e / przypisy
    [HideInInspector] public GameObject morInfo;
    private bazaEfektow BazaEfektow;
    [HideInInspector] public UnityEvent efektyWywo�anieOtrzyma�Cios;
    [HideInInspector] public UnityEvent efektyWywo�anieZada�Cios;
    [HideInInspector] public UnityEvent efektyWywo�anieKoniecTury;
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
        walkaStart.KoniecTury += Wywo�ajEfektyKoniecT;
        walkaStart.KoniecTury += PrzemijanieEfektuw;
    }
    private void OnDestroy()
    {
        walkaStart.KoniecTury -= Wywo�ajEfektyKoniecT;
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
        Wywo�ajEfektyOtrzyma�Cios();

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
        for (int x = 0; x < na�orzoneEfekty.Count;)
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
    }

    public void Wywo�ajEfektyKoniecT(int numerTury)
    {
        if (efektyWywo�anieKoniecTury != null)
        {
            efektyWywo�anieKoniecTury.Invoke();
        }
    }
    public void Wywo�ajEfektyOtrzyma�Cios()
    {
        if (efektyWywo�anieOtrzyma�Cios != null)
        {
            efektyWywo�anieOtrzyma�Cios.Invoke();
        }
    }
    public void Wywo�ajEfektyZada��Cios()
    {
        if (efektyWywo�anieZada�Cios != null)
        {
            efektyWywo�anieZada�Cios.Invoke();
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
