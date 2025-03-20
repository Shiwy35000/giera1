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

    void Update()
    {
        Energia();
    }

    private void Energia()
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

    }
}
