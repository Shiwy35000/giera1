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
    public List<artefakt> posiadaneArtefakty;
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
    [HideInInspector] public float ilee;
    [HideInInspector] public bool nieUchronnee;

    void Awake()
    {
        maxEnergia = 3; // narazie?
        dialog.Walka += MaxEnergiaWalka;
        dialog.Walka += Czy�cimyListy;
        walkaStart.KoniecTury += MaxEnergiaTura;
        walkaStart.KoniecTury += Wywo�ajEfektyKoniecT;
        walkaStart.KoniecTury += PrzemijanieEfektuw;
        BazaEfektow = this.GetComponent<bazaEfektow>();
        sortZ = GameObject.FindGameObjectWithTag("dlon").gameObject.GetComponent<sortGrupZ>();
    }
    private void OnDestroy()
    {
        dialog.Walka -= MaxEnergiaWalka;
        dialog.Walka -= Czy�cimyListy;
        walkaStart.KoniecTury -= MaxEnergiaTura;
        walkaStart.KoniecTury -= Wywo�ajEfektyKoniecT;
        walkaStart.KoniecTury -= PrzemijanieEfektuw;
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

    public void PrzyjmijDmg(float ile, bool nieUchronne)
    {
        ilee = ile;
        nieUchronnee = nieUchronne;
        Wywo�ajEfektyOtrzyma�Cios();

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

    public void PrzemijanieEfektuw(int numerTury)
    {
        for (int x = 0; x < na�orzoneEfekty.Count; x++)
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
        if(efektyWywo�anieKoniecTury != null)
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
    
    private void Die()
    {
        //Destroy(this.gameObject);
    }
}
