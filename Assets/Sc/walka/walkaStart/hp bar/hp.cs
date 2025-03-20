using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class hp : MonoBehaviour
{
    public GameObject w³aœcicielZdrowia;
    private GameObject bar;
    private GameObject barText;
    void Awake()
    {
        bar = this.gameObject.transform.GetChild(1).gameObject; 
        barText = this.gameObject.transform.GetChild(2).gameObject;
    }
    void Update()
    {
        if(w³aœcicielZdrowia != null)
        {
            StanZdrowiaPokarz();
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
}
