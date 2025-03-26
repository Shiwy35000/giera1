using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class rysujEfekty : MonoBehaviour
{
    public GameObject w³aœciciel;
    public GameObject efektPrefab;
    private bool gracza;
    //private GameObject infoEfektu;


    void Awake()
    {
        dialog.Walka += ZerujEfektyWizualia;

        if(w³aœciciel.tag == "Player")
        {
            gracza = true;
        }
        else
        {
            gracza = false;
        }

        //infoEfektu = transform.parent.transform.GetChild(1).gameObject;
    }

    private void OnDestroy()
    {
        dialog.Walka -= ZerujEfektyWizualia;
    }

    private void ZerujEfektyWizualia(bool nic)
    {
        foreach (Transform child in this.transform)
        {
            Destroy(child.gameObject);
        }
    }

    void Update()
    {
        Wizualia();
    }

    void Wizualia()
    {
        if(gracza)
        {
            playerEq eq = w³aœciciel.GetComponent<playerEq>();
            if(eq.na³orzoneEfekty.Count == 0 && transform.childCount != 0)
            {
                ZerujEfektyWizualia(true);
            }
            else if(eq.na³orzoneEfekty.Count > 0)
            {
                if(transform.childCount > eq.na³orzoneEfekty.Count)
                {
                    Destroy(transform.GetChild(0).gameObject);
                }
                else if(transform.childCount < eq.na³orzoneEfekty.Count)
                {
                    Instantiate(efektPrefab, this.transform);
                }
                else if (transform.childCount == eq.na³orzoneEfekty.Count)
                {
                    for (int x = 0; x < transform.childCount; x++)
                    {
                        Uzupe³nijDaneEfektu(transform.GetChild(x).gameObject, x);
                    }
                }
            }
        }
        else
        {
            WRUG1 eq = w³aœciciel.GetComponent<WRUG1>();
            if (eq.na³orzoneEfekty.Count == 0 && transform.childCount != 0)
            {
                ZerujEfektyWizualia(true);
            }
            else if (eq.na³orzoneEfekty.Count > 0)
            {
                if (transform.childCount > eq.na³orzoneEfekty.Count)
                {
                    Destroy(transform.GetChild(0).gameObject);
                }
                else if (transform.childCount < eq.na³orzoneEfekty.Count)
                {
                    Instantiate(efektPrefab, this.transform);
                }
                else if (transform.childCount == eq.na³orzoneEfekty.Count)
                {
                    for (int x = 0; x < transform.childCount; x++)
                    {
                        Uzupe³nijDaneEfektu(transform.GetChild(x).gameObject, x);
                    }
                }
            }
        }
    }

    void Uzupe³nijDaneEfektu(GameObject p, int x)
    {
        efekty e;
        if (gracza)
        {
            e = w³aœciciel.GetComponent<playerEq>().na³orzoneEfekty[x];
        }
        else
        {
            e = w³aœciciel.GetComponent<WRUG1>().na³orzoneEfekty[x];
        }

        if(e.licznik > 1) //stali 
        {
            p.transform.GetChild(0).gameObject.SetActive(true);
            p.transform.GetChild(0).gameObject.GetComponent<TextMeshPro>().text = e.licznik.ToString();
        }
        else
        {
            p.transform.GetChild(0).gameObject.SetActive(false);
        }

        if(e.sprite != null) //grafika
        {
            p.GetComponent<SpriteRenderer>().sprite = e.sprite;
        }
    }
}
