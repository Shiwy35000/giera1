using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class rysujEfekty : MonoBehaviour
{
    public GameObject właściciel;
    public GameObject efektPrefab;
    private bool gracza;
    //private GameObject infoEfektu;


    void Awake()
    {
        dialog.Walka += ZerujEfektyWizualia;

        if(właściciel.tag == "Player")
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
            playerEq eq = właściciel.GetComponent<playerEq>();
            if(eq.nałorzoneEfekty.Count == 0 && transform.childCount != 0)
            {
                ZerujEfektyWizualia(true);
            }
            else if(eq.nałorzoneEfekty.Count > 0)
            {
                if(transform.childCount > eq.nałorzoneEfekty.Count)
                {
                    Destroy(transform.GetChild(0).gameObject);
                }
                else if(transform.childCount < eq.nałorzoneEfekty.Count)
                {
                    Instantiate(efektPrefab, this.transform);
                }
                else if (transform.childCount == eq.nałorzoneEfekty.Count)
                {
                    for (int x = 0; x < transform.childCount; x++)
                    {
                        UzupełnijDaneEfektu(transform.GetChild(x).gameObject, x);
                    }
                }
            }
        }
        else
        {
            WRUG1 eq = właściciel.GetComponent<WRUG1>();
            if (eq.nałorzoneEfekty.Count == 0 && transform.childCount != 0)
            {
                ZerujEfektyWizualia(true);
            }
            else if (eq.nałorzoneEfekty.Count > 0)
            {
                if (transform.childCount > eq.nałorzoneEfekty.Count)
                {
                    Destroy(transform.GetChild(0).gameObject);
                }
                else if (transform.childCount < eq.nałorzoneEfekty.Count)
                {
                    Instantiate(efektPrefab, this.transform);
                }
                else if (transform.childCount == eq.nałorzoneEfekty.Count)
                {
                    for (int x = 0; x < transform.childCount; x++)
                    {
                        UzupełnijDaneEfektu(transform.GetChild(x).gameObject, x);
                    }
                }
            }
        }
    }

    void UzupełnijDaneEfektu(GameObject p, int x)
    {
        efekty e;
        if (gracza)
        {
            e = właściciel.GetComponent<playerEq>().nałorzoneEfekty[x];
        }
        else
        {
            e = właściciel.GetComponent<WRUG1>().nałorzoneEfekty[x];
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
