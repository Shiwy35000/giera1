using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class zarządzanieSklepem : MonoBehaviour
{
    private GameObject sklep;
    private GameObject asoPoz, asoPoz2;
    private biblioteka Biblioteka;

    private List<GameObject> asoObj = new List<GameObject>();
    private List<GameObject> sloty = new List<GameObject>();
    public float scalArtefaktu;
    [HideInInspector] public GameObject właścicielSklepu;

    private void Awake()
    {
        sklep = this.transform.GetChild(0).gameObject;
        asoPoz = this.transform.GetChild(0).transform.GetChild(0).gameObject;
        asoPoz2 = this.transform.GetChild(0).transform.GetChild(1).gameObject;
        Biblioteka = GameObject.FindGameObjectWithTag("saveGame").GetComponent<biblioteka>();
        SlotyList();
        sklep.SetActive(false);

        dialog.sklepOn += otwarcieZamknięcieSklep;
        dialog.straganiarz += ktoSprzedaje;
    }

    private void OnDestroy()
    {
        dialog.sklepOn -= otwarcieZamknięcieSklep;
        dialog.straganiarz -= ktoSprzedaje;
    }

    private void ktoSprzedaje(GameObject kto)
    {
        właścicielSklepu = kto;
    }

    private void otwarcieZamknięcieSklep(List<aso> dostawa)
    {
        if(dostawa.Count == 0)
        {
            CzyszczenieAso();
            sklep.SetActive(false);
        }
        else
        {
            sklep.SetActive(true);
            Zaopatrzenie(dostawa);
        }
    }

    private void CzyszczenieAso()
    {
        for (int x = 0; x < sloty.Count; x++)
        {
            if (sloty[x].transform.GetChild(0).transform.childCount > 0)
            {
                foreach (Transform child in sloty[x].transform.GetChild(0).transform)
                {
                    Destroy(child.gameObject);
                }
            }
        }
    }
    private void SlotyList()
    {
        for (int x = 0; x < asoPoz.transform.childCount; x++)
        {
            if (!sloty.Contains(asoPoz.transform.GetChild(x).gameObject))
            {
                sloty.Add(asoPoz.transform.GetChild(x).gameObject);
            }
        }
        for (int x = 0; x < asoPoz2.transform.childCount; x++)
        {
            if (!sloty.Contains(asoPoz2.transform.GetChild(x).gameObject))
            {
                sloty.Add(asoPoz2.transform.GetChild(x).gameObject);
            }
        }
    }

    private void AktywujIPrzypiszSloty()
    {
        if (asoObj.Count > 5)
        {
            asoPoz2.SetActive(true);
        }
        else
        {
            asoPoz2.SetActive(false);
        }

        for (int x = 0; x < sloty.Count; x++)
        {
            if (x >= asoObj.Count)
            {
                sloty[x].SetActive(false);
            }
            else
            {
                sloty[x].SetActive(true);
                asoObj[x].transform.parent = sloty[x].transform.GetChild(0).transform;
                asoObj[x].transform.localPosition = new Vector3(0, 0, 0);
                sloty[x].transform.GetChild(1).GetComponent<TextMeshPro>().text = asoObj[x].GetComponent<asoInfo>().Aso.cena.ToString() + "$";
            }
        }
    }

    private void Zaopatrzenie(List<aso> dostawa)
    {
        for (int x = 0; x < dostawa.Count; x++)
        {
            if(dostawa[x].asoTyp == AsoTyp.karta)
            {
                Dodaj(Biblioteka.wszystkieKarty[dostawa[x].Id], dostawa[x]);
            }
            else if (dostawa[x].asoTyp == AsoTyp.artefakt)
            {
                Dodaj(Biblioteka.istniejąceArtefakty[dostawa[x].Id], dostawa[x]);
            }
        }
    }

    public void Dodaj(GameObject item, aso A)
    {
        GameObject it = Instantiate(item, sloty[0].transform.GetChild(0).transform);
        it.name = item.name;
        it.AddComponent<asoInfo>();
        it.GetComponent<asoInfo>().Aso = A;
        asoObj.Add(it);

        if(it.tag == "artefakt")
        {
            it.transform.localScale = new Vector3(scalArtefaktu, scalArtefaktu, scalArtefaktu);
            it.gameObject.transform.GetChild(0).gameObject.SetActive(false);
            it.GetComponent<SpriteRenderer>().sortingOrder = 2;
        }

        AktywujIPrzypiszSloty();
    }
}
