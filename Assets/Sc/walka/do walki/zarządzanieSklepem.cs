using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class zarządzanieSklepem : MonoBehaviour
{
    private GameObject sklep;
    private GameObject asoPoz, asoPoz2;
    private biblioteka Biblioteka;
    private playerEq eq;

    private List<GameObject> asoObj = new List<GameObject>();
    private List<GameObject> sloty = new List<GameObject>();
    public float scalArtefaktu;
    public int nrDialoguGdyPułkiPuste = -1; //domyślnie liczba < 0 by nie podawał nowego początku dialogu;
    [HideInInspector] public GameObject wybraneAsoDoKupienia;
    [HideInInspector] public GameObject właścicielSklepu;

    public GameObject BuyButton;

    private void Awake()
    {
        sklep = this.transform.GetChild(0).gameObject;
        asoPoz = this.transform.GetChild(0).transform.GetChild(0).gameObject;
        asoPoz2 = this.transform.GetChild(0).transform.GetChild(1).gameObject;
        Biblioteka = GameObject.FindGameObjectWithTag("saveGame").GetComponent<biblioteka>();
        eq = GameObject.FindGameObjectWithTag("Player").GetComponent<playerEq>();

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
            wybraneAsoDoKupienia = null;
            BuyButton.SetActive(false);
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
                if (Biblioteka.wszystkieKarty[dostawa[x].Id].Odblokowane == true)
                {
                    Dodaj(Biblioteka.wszystkieKarty[dostawa[x].Id].Obj, dostawa[x]);
                }
            }
            else if (dostawa[x].asoTyp == AsoTyp.artefakt)
            {
                if (Biblioteka.istniejąceArtefakty[dostawa[x].Id].Odblokowane == true)
                {
                    Dodaj(Biblioteka.istniejąceArtefakty[dostawa[x].Id].Obj, dostawa[x]);
                }
            }

            if(x == dostawa.Count - 1)
            {
                CzyPustePułki();
            }
        }
    }

    public void CzyPustePułki()
    {
        if (asoObj.Count == 0 && właścicielSklepu != null) //jeśli dostawa nie jest dostępna do wyłorzenia(itemy są nie odblokowane w biblotece), to zamykamy sklep!;
        {
            if (nrDialoguGdyPułkiPuste >= 0)
            {
                właścicielSklepu.GetComponent<dialog>().poczatekDialogu = nrDialoguGdyPułkiPuste;
            }
            właścicielSklepu.GetComponent<dialog>().SklepOfOn(new List<aso>());
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

    public void Kup()
    {
        if(wybraneAsoDoKupienia != null && wybraneAsoDoKupienia.GetComponent<asoInfo>().Aso.cena <= eq.sakiewka)
        {
            eq.sakiewka -= wybraneAsoDoKupienia.GetComponent<asoInfo>().Aso.cena;

            if(wybraneAsoDoKupienia.GetComponent<asoInfo>().Aso.asoTyp == AsoTyp.karta)
            {
                eq.deckPrefab.Add(Biblioteka.wszystkieKarty[wybraneAsoDoKupienia.GetComponent<asoInfo>().Aso.Id].Obj);
            }
            else if(wybraneAsoDoKupienia.GetComponent<asoInfo>().Aso.asoTyp == AsoTyp.artefakt)
            {
                eq.ArtefaktPrzypisz(Biblioteka.istniejąceArtefakty[wybraneAsoDoKupienia.GetComponent<asoInfo>().Aso.Id].Obj);
            }

            if(wybraneAsoDoKupienia.GetComponent<asoInfo>().Aso.ilość == ilośćAso.jednaSztuka)
            {
                asoObj.Remove(wybraneAsoDoKupienia);
                Destroy(wybraneAsoDoKupienia.gameObject);
                wybraneAsoDoKupienia = null;
                AktywujIPrzypiszSloty();
                BuyButton.SetActive(false);
            }
        }
    }
}
