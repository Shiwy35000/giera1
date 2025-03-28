using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class hp : MonoBehaviour
{
    public GameObject w�a�cicielZdrowia;
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
        if(w�a�cicielZdrowia != null)
        {
            StanZdrowiaPokarz();
            PacerzPokasz();
        }
    }

    private void StanZdrowiaPokarz()
    {
        if(w�a�cicielZdrowia.tag == "Player")
        {
            float x = w�a�cicielZdrowia.GetComponent<playerEq>().hp / w�a�cicielZdrowia.GetComponent<playerEq>().hpMax;
            bar.transform.localScale = new Vector3(x, 1f, 1f);
            barText.GetComponent<TextMeshPro>().text = w�a�cicielZdrowia.GetComponent<playerEq>().hp.ToString() + "/" + w�a�cicielZdrowia.GetComponent<playerEq>().hpMax.ToString();
        }
        else
        {
            float x = w�a�cicielZdrowia.GetComponent<WRUG1>().hpAktualne / w�a�cicielZdrowia.GetComponent<WRUG1>().hpMax;
            bar.transform.localScale = new Vector3(x, 1f, 1f);
            barText.GetComponent<TextMeshPro>().text = w�a�cicielZdrowia.GetComponent<WRUG1>().hpAktualne.ToString() + "/" + w�a�cicielZdrowia.GetComponent<WRUG1>().hpMax.ToString();
        }
    }

    private void PacerzPokasz()
    {
        if(w�a�cicielZdrowia.tag == "Player")
        {
            playerEq eq = w�a�cicielZdrowia.GetComponent<playerEq>();
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
            WRUG1 wr = w�a�cicielZdrowia.GetComponent<WRUG1>();
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
