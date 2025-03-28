using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class działanieW1test : MonoBehaviour
{
    //przypisy
    private WRUG1 tenWrug;
    private int cyklTurowy; //jeśli przeciwnika zachowanie jest zalerzne od aktualnie rozgrywanej tury;

    //WARZNE: wrug zaczyna ture od pierwszego przypisanego do niego działania,
    //jeśli nie walczy liniowo niech zacznie od funkcji podejmowania decyzji;

    private void Awake()
    {
        tenWrug = this.gameObject.GetComponent<WRUG1>();
    }

    public void DziałanieTestowe()
    {
        Debug.Log(this.gameObject.name);
    }
}
