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
    public List<efekty> na³orzoneEfekty;

    [Header("Ekwipunek")]
    public List<GameObject> deck;
    public List<artefakt> posiadaneArtefakty;

    [Header("Inne")]
    public int sakiewka;
    public float rzar;
    public List<nowyDialogTyp> dialogiWybory;

    //przypisy
    private bazaEfektow BazaEfektow;
    [HideInInspector] public UnityEvent efektyWywo³anieOtrzyma³Cios;
    [HideInInspector] public UnityEvent efektyWywo³anieZada³Cios;
    [HideInInspector] public UnityEvent efektyWywo³anieKoniecTury;
    [HideInInspector] public float ilee;
    [HideInInspector] public bool nieUchronnee;
    //private GameObject graczMoreInfo, GraczZbiurEfektów;

    void Awake()
    {
        maxEnergia = 3; // narazie?
        dialog.Walka += MaxEnergiaWalka;
        walkaStart.KoniecTury += MaxEnergiaTura;
        walkaStart.KoniecTury += Wywo³ajEfektyKoniecT;
        walkaStart.KoniecTury += PrzemijanieEfektuw;
        BazaEfektow = this.GetComponent<bazaEfektow>();
        //graczMoreInfo = GameObject.FindGameObjectWithTag("gracz").gameObject;
        //GraczZbiurEfektów = graczMoreInfo.transform.GetChild(0).gameObject;
    }
    private void OnDestroy()
    {
        dialog.Walka -= MaxEnergiaWalka;
        walkaStart.KoniecTury -= MaxEnergiaTura;
        walkaStart.KoniecTury -= Wywo³ajEfektyKoniecT;
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
        Wywo³ajEfektyOtrzyma³Cios();

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
        for (int x = 0; x < na³orzoneEfekty.Count; x++)
        {
            if (na³orzoneEfekty[x].licznik > 0)
            {
                na³orzoneEfekty[x].licznik -= 1;
            }
            if (na³orzoneEfekty[x].licznik == 0)
            {
                BazaEfektow.UsunEfekt(na³orzoneEfekty[x]);
                na³orzoneEfekty.Remove(na³orzoneEfekty[x]);
            }
        }
    }

    public void Wywo³ajEfektyKoniecT(int numerTury)
    {
        if(efektyWywo³anieKoniecTury != null)
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
    
    private void Die()
    {
        //Destroy(this.gameObject);
    }
}
