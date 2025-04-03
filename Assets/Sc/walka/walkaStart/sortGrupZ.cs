using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sortGrupZ : MonoBehaviour
{
    public List<GameObject> kartyWD�oni = new List<GameObject>();
    [HideInInspector] public List<GameObject> sloty = new List<GameObject>();
    private GameObject player, uiWalki;
    private walkaStart WalkaStart;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").gameObject;
        uiWalki = GameObject.FindGameObjectWithTag("nadUiWalka").gameObject;
        WalkaStart = uiWalki.transform.parent.gameObject.GetComponent<walkaStart>();

        SlotyList();

        walkaStart.KoniecTury += NaKoniecTury;
    }
    private void OnDestroy()
    {
        walkaStart.KoniecTury -= NaKoniecTury;
    }

    private void PasywneEfektyKart(GameObject cel)
    {
        taKarta karta = cel.GetComponent<taKarta>();
        if (karta.Dzia�anieNaKoniecTury)
        {
            List<GameObject> ObiektyCele = new List<GameObject>();
            playerEq eq = WalkaStart.gracz.gameObject.GetComponent<playerEq>();

            if (karta.celeNieZagranej == CeleNieZagranej.Gracz)
            {
                ObiektyCele.Add(player);
                karta.akcjeKoniecTury.Invoke(ObiektyCele);
            }
            else if (karta.celeNieZagranej == CeleNieZagranej.Wrogowie || karta.celeNieZagranej == CeleNieZagranej.RandomWrug)
            {
                ObiektyCele.AddRange(WalkaStart.przeciwnicyWwalce);
                karta.akcjeKoniecTury.Invoke(ObiektyCele);
            }
            else if (karta.celeNieZagranej == CeleNieZagranej.All || karta.celeNieZagranej == CeleNieZagranej.Random)
            {
                ObiektyCele.AddRange(WalkaStart.przeciwnicyWwalce);
                ObiektyCele.Add(player);
                karta.akcjeKoniecTury.Invoke(ObiektyCele);
            }
            else if (karta.celeNieZagranej == CeleNieZagranej.TaKarta)
            {
                ObiektyCele.Add(cel);
                karta.akcjeKoniecTury.Invoke(ObiektyCele);
            }
            else if (karta.celeNieZagranej == CeleNieZagranej.KartyWD�oni) //nie wliczamy tej karty
            {
                ObiektyCele.AddRange(kartyWD�oni);
                ObiektyCele.Remove(cel);
                karta.akcjeKoniecTury.Invoke(ObiektyCele);
            }
            else if (karta.celeNieZagranej == CeleNieZagranej.RandomKartaWD�oni) //nie wliczamy tej karty
            {
                List<GameObject> kartyMinusTa = new List<GameObject>();
                kartyMinusTa.AddRange(kartyWD�oni);
                kartyMinusTa.Remove(cel);
                int z = Random.Range(0, kartyMinusTa.Count - 1);
                ObiektyCele.Add(kartyMinusTa[z]);
                karta.akcjeKoniecTury.Invoke(ObiektyCele);
            }
        }
    }

    private void NaKoniecTury(int nic)
    {
        for (int x = 0; x < kartyWD�oni.Count;)
        {
            taKarta karta = kartyWD�oni[x].GetComponent<taKarta>();

            PasywneEfektyKart(kartyWD�oni[x]);

            if (karta.naKoniecTury == PoUrzyciu.Zniszcz)
            {
                karta.Usu�TeKarte();
            }
            else if (karta.naKoniecTury == PoUrzyciu.cmentarz)
            {
                karta.NaCmentarzTaKarta();
            }
            else if (karta.naKoniecTury == PoUrzyciu.wyklucz)
            {
                karta.WykluczTeKarte();
            }
            else if(karta.naKoniecTury == PoUrzyciu.Zachowaj)
            {
                x++;
            }
        }
    }

    private void SlotyList()
    {
        for (int x = 0; x < transform.childCount; x++)
        {
            if (!sloty.Contains(transform.GetChild(x).gameObject))
            {
                sloty.Add(transform.GetChild(x).gameObject);
            }
        }
    }
   
    public void UsunKarteZdloni(GameObject kartaa)
    {
        kartyWD�oni.Remove(kartaa);
        Destroy(kartaa.gameObject);
        AktywujIPrzypiszSloty();
    }

    private void AktywujIPrzypiszSloty()
    {
        for (int x = 0; x < sloty.Count; x++)
        {
            if(x>= kartyWD�oni.Count)
            {
                sloty[x].SetActive(false);
            }
            else
            {
                sloty[x].SetActive(true);
                kartyWD�oni[x].transform.parent = sloty[x].transform;
            }
        }
    }

    public void DodajKarte(GameObject karta)
    {
        GameObject kartaa = Instantiate(karta, sloty[0].transform);
        kartaa.name = karta.name;
        Destroy(karta.gameObject);
        kartaa.GetComponent<taKarta>().dlon = this.gameObject;
        kartyWD�oni.Add(kartaa);

        AktywujIPrzypiszSloty();
    }

    public void CzyscWszystko()
    {
        for (int x = 0; x < sloty.Count; x++)
        {
            if(sloty[x].transform.childCount > 0)
            {
                foreach (Transform child in sloty[x].transform)
                {
                    Destroy(child.gameObject);
                }
            }
        }
    }
}
