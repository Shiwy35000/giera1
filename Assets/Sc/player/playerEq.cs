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
    public List<efekty> na�orzoneEfekty;

    [Header("Ekwipunek")]
    public List<GameObject> deck;
    public List<artefakt> posiadaneArtefakty;

    [Header("Inne")]
    public int sakiewka;
    public float rzar;
    public List<nowyDialogTyp> dialogiWybory;

    //przypisy
    private bazaEfektow BazaEfektow;
    [HideInInspector] public UnityEvent efektyWywo�anieOtrzyma�Cios;
    [HideInInspector] public UnityEvent efektyWywo�anieZada�Cios;
    [HideInInspector] public UnityEvent efektyWywo�anieKoniecTury;
    [HideInInspector] public float ilee;
    [HideInInspector] public bool nieUchronnee;
    //private GameObject graczMoreInfo, GraczZbiurEfekt�w;

    void Awake()
    {
        maxEnergia = 3; // narazie?
        dialog.Walka += MaxEnergiaWalka;
        walkaStart.KoniecTury += MaxEnergiaTura;
        walkaStart.KoniecTury += Wywo�ajEfektyKoniecT;
        walkaStart.KoniecTury += PrzemijanieEfektuw;
        BazaEfektow = this.GetComponent<bazaEfektow>();
        //graczMoreInfo = GameObject.FindGameObjectWithTag("gracz").gameObject;
        //GraczZbiurEfekt�w = graczMoreInfo.transform.GetChild(0).gameObject;
    }
    private void OnDestroy()
    {
        dialog.Walka -= MaxEnergiaWalka;
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

    public void PrzyjmijDmg(float ile, bool nieUchronne)
    {
        ilee = ile;
        nieUchronnee = nieUchronne;
        Wywo�ajEfektyOtrzyma�Cios();

        if (nieUchronnee)
        {
            hp -= ilee;
        }
        else
        {
            float z;
            aktualnyPancerz -= ilee;
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
            if (na�orzoneEfekty[x].licznik > 0)
            {
                na�orzoneEfekty[x].licznik -= 1;
            }
            if (na�orzoneEfekty[x].licznik == 0)
            {
                BazaEfektow.UsunEfekt(na�orzoneEfekty[x]);
                na�orzoneEfekty.Remove(na�orzoneEfekty[x]);
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
