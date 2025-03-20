using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sortGrupZ : MonoBehaviour
{
    public List<GameObject> kartyWD這ni = new List<GameObject>();
    [HideInInspector] public List<GameObject> sloty = new List<GameObject>();

    void Awake()
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
        //Debug.Log(kartaa.name);
        kartyWD這ni.Remove(kartaa);
        Destroy(kartaa.gameObject);
        AktywujIPrzypiszSloty();
    }

    void Update()
    {
        //AktywujIPrzypiszSloty();
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

    public void DodajKartyStart(List<GameObject> karty)
    {
        kartyWD這ni = new List<GameObject>();
        for (int x = 0; x < karty.Count; x++)
        {
            GameObject karta = Instantiate(karty[x], sloty[x].transform);
            karta.GetComponent<taKarta>().dlon = this.gameObject;
            kartyWD這ni.Add(karta);
        }
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
