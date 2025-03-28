using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class hp : MonoBehaviour
{
    public GameObject w³aœcicielZdrowia;
    private GameObject bar, barText, pancerzIcon, pancerzLicznik;

    void Awake()
    {
        bar = this.gameObject.transform.GetChild(1).gameObject; 
        barText = this.gameObject.transform.GetChild(2).gameObject;
        pancerzIcon = this.gameObject.transform.GetChild(3).gameObject;
        pancerzLicznik = pancerzIcon.transform.GetChild(0).gameObject;
    }
    void Update()
    {
        if(w³aœcicielZdrowia != null)
        {
            StanZdrowiaPokarz();
            PacerzPokasz();
        }
    }

    private void StanZdrowiaPokarz()
    {
        if(w³aœcicielZdrowia.tag == "Player")
        {
            float x = w³aœcicielZdrowia.GetComponent<playerEq>().hp / w³aœcicielZdrowia.GetComponent<playerEq>().hpMax;
            bar.transform.localScale = new Vector3(x, 1f, 1f);
            barText.GetComponent<TextMeshPro>().text = w³aœcicielZdrowia.GetComponent<playerEq>().hp.ToString() + "/" + w³aœcicielZdrowia.GetComponent<playerEq>().hpMax.ToString();
        }
        else
        {
            float x = w³aœcicielZdrowia.GetComponent<WRUG1>().hpAktualne / w³aœcicielZdrowia.GetComponent<WRUG1>().hpMax;
            bar.transform.localScale = new Vector3(x, 1f, 1f);
            barText.GetComponent<TextMeshPro>().text = w³aœcicielZdrowia.GetComponent<WRUG1>().hpAktualne.ToString() + "/" + w³aœcicielZdrowia.GetComponent<WRUG1>().hpMax.ToString();
        }
    }

    private void PacerzPokasz()
    {
        if(w³aœcicielZdrowia.tag == "Player")
        {
            playerEq eq = w³aœcicielZdrowia.GetComponent<playerEq>();
            if(eq.aktualnyPancerz > 0)
            {
                pancerzIcon.SetActive(true);
                pancerzLicznik.GetComponent<TextMeshPro>().text = eq.aktualnyPancerz.ToString();
            }
            else
            {
                pancerzIcon.SetActive(false);
            }
        }
        else
        {
            WRUG1 wr = w³aœcicielZdrowia.GetComponent<WRUG1>();
            if(wr.aktualnyPancerz > 0)
            {
                pancerzIcon.SetActive(true);
                pancerzLicznik.GetComponent<TextMeshPro>().text = wr.aktualnyPancerz.ToString();
            }
            else
            {
                pancerzIcon.SetActive(false);
            }
        }
    }
}
