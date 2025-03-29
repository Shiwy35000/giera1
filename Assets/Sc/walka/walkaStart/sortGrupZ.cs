using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sortGrupZ : MonoBehaviour
{
    public List<GameObject> kartyWD這ni = new List<GameObject>();
    [HideInInspector] public List<GameObject> sloty = new List<GameObject>();

    void Awake()
    {
        SlotyList();

        walkaStart.KoniecTury += NaKoniecTury;
    }
    private void OnDestroy()
    {
        walkaStart.KoniecTury -= NaKoniecTury;
    }

    private void NaKoniecTury(int nic)
    {
        for (int x = 0; x < kartyWD這ni.Count;)
        {
            taKarta karta = kartyWD這ni[x].GetComponent<taKarta>();

            if (karta.naKoniecTury == PoUrzyciu.Zniszcz)
            {
                if (karta.prefabTejKartyWdeck != null)
                {
                    karta.Eq.deckPrefab.Remove(karta.prefabTejKartyWdeck);
                }
                UsunKarteZdloni(karta.gameObject);
            }
            else if (karta.naKoniecTury == PoUrzyciu.cmentarz)
            {
                GameObject klon = GameObject.Instantiate(karta.gameObject, karta.fizycznyDeck.transform);
                karta.Eq.cmentarz.Add(klon);
                UsunKarteZdloni(karta.gameObject);
            }
            else if (karta.naKoniecTury == PoUrzyciu.wyklucz)
            {
                GameObject klon = GameObject.Instantiate(karta.gameObject, karta.fizycznyDeck.transform);
                karta.Eq.wykluczone.Add(klon);
                UsunKarteZdloni(karta.gameObject);
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
        kartyWD這ni.Remove(kartaa);
        Destroy(kartaa.gameObject);
        AktywujIPrzypiszSloty();
    }

    private void AktywujIPrzypiszSloty()
    {
        for (int x = 0; x < sloty.Count; x++)
        {
            if(x>= kartyWD這ni.Count)
            {
                sloty[x].SetActive(false);
            }
            else
            {
                sloty[x].SetActive(true);
                kartyWD這ni[x].transform.parent = sloty[x].transform;
            }
        }
    }

    public void DodajKarte(GameObject karta)
    {
        GameObject kartaa = Instantiate(karta, sloty[0].transform);
        Destroy(karta.gameObject);
        kartaa.GetComponent<taKarta>().dlon = this.gameObject;
        kartyWD這ni.Add(kartaa);

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
