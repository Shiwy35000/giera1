using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class walkaStart : MonoBehaviour
{
    public int obecnaTura;
    public Transform wrogowieSpawn, dlon, wskaünikEnergi;
    public List<GameObject> przeciwnicyWwalce = new List<GameObject>();
    public GameObject gracz;

    public static event System.Action<int> KoniecTury;

    public bool turaGracza; //[HideInInspector]

    void Update()
    {
        EnergiaWskazinik();
    }

    private void EnergiaWskazinik()
    {
        playerEq eq = gracz.gameObject.GetComponent<playerEq>();
        wskaünikEnergi.gameObject.transform.GetChild(1).GetComponent<TextMeshPro>().text = eq.aktualnaEnergia.ToString() + "/" + eq.maxEnergia.ToString();
    }

    public void SpawnPrzeciwinicy(List<GameObject> przeciwnicySpawn)
    {
        for (int x = 0; x < przeciwnicySpawn.Count; x++)
        {
            GameObject ten = Instantiate(przeciwnicySpawn[x], wrogowieSpawn);
            przeciwnicyWwalce.Add(ten);
        }
    }

    public void DodajKartyDoRÍki(List<GameObject> deck)
    {
        dlon.GetComponent<sortGrupZ>().DodajKartyStart(deck);
    }
    
    public void CzyszczenieRÍki()
    {
        dlon.GetComponent<sortGrupZ>().CzyscWszystko();
    }

    public void koniecTury()
    {
        obecnaTura += 1;
        KoniecTury?.Invoke(obecnaTura);
        turaGracza = true;
    }

    public void AkcjaWroga(int numerWroga)
    {
        przeciwnicyWwalce[numerWroga].GetComponent<WRUG1>().obecnaAkcja = przeciwnicyWwalce[numerWroga].GetComponent<WRUG1>().dzia≥anie[0];
        przeciwnicyWwalce[numerWroga].GetComponent<WRUG1>().dzia≥anie[0].Invoke();
    }
}
