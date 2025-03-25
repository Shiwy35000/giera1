using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class rysujEfekty : MonoBehaviour
{
    public GameObject w�a�ciciel;
    public GameObject efektPrefab;
    private bool gracza;
    //private GameObject infoEfektu;


    void Awake()
    {
        dialog.Walka += ZerujEfektyWizualia;

        if(w�a�ciciel.tag == "Player")
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
            playerEq eq = w�a�ciciel.GetComponent<playerEq>();
            if(eq.na�orzoneEfekty.Count == 0 && transform.childCount != 0)
            {
                ZerujEfektyWizualia(true);
            }
            else if(eq.na�orzoneEfekty.Count > 0)
            {
                if(transform.childCount > eq.na�orzoneEfekty.Count)
                {
                    Destroy(transform.GetChild(0).gameObject);
                }
                else if(transform.childCount < eq.na�orzoneEfekty.Count)
                {
                    Instantiate(efektPrefab, this.transform);
                }
                else if (transform.childCount == eq.na�orzoneEfekty.Count)
                {
                    for (int x = 0; x < transform.childCount; x++)
                    {
                        Uzupe�nijDaneEfektu(transform.GetChild(x).gameObject, x);
                    }
                }
            }
        }
        else
        {
            WRUG1 eq = w�a�ciciel.GetComponent<WRUG1>();
            if (eq.na�orzoneEfekty.Count == 0 && transform.childCount != 0)
            {
                ZerujEfektyWizualia(true);
            }
            else if (eq.na�orzoneEfekty.Count > 0)
            {
                if (transform.childCount > eq.na�orzoneEfekty.Count)
                {
                    Destroy(transform.GetChild(0).gameObject);
                }
                else if (transform.childCount < eq.na�orzoneEfekty.Count)
                {
                    Instantiate(efektPrefab, this.transform);
                }
                else if (transform.childCount == eq.na�orzoneEfekty.Count)
                {
                    for (int x = 0; x < transform.childCount; x++)
                    {
                        Uzupe�nijDaneEfektu(transform.GetChild(x).gameObject, x);
                    }
                }
            }
        }
    }

    void Uzupe�nijDaneEfektu(GameObject p, int x)
    {
        efekty e;
        if (gracza)
        {
            e = w�a�ciciel.GetComponent<playerEq>().na�orzoneEfekty[x];
        }
        else
        {
            e = w�a�ciciel.GetComponent<WRUG1>().na�orzoneEfekty[x];
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
