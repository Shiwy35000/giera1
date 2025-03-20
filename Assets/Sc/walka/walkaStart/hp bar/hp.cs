using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class hp : MonoBehaviour
{
    public GameObject w�a�cicielZdrowia;
    private GameObject bar;
    private GameObject barText;
    void Awake()
    {
        bar = this.gameObject.transform.GetChild(1).gameObject; 
        barText = this.gameObject.transform.GetChild(2).gameObject;
    }
    void Update()
    {
        if(w�a�cicielZdrowia != null)
        {
            StanZdrowiaPokarz();
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
}
