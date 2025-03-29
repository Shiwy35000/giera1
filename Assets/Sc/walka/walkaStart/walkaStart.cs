using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class walkaStart : MonoBehaviour
{
    public int obecnaTura;
    public Transform wrogowieSpawn, dlon, wskaŸnikEnergi;
    public List<GameObject> przeciwnicyWwalce = new List<GameObject>();
    public GameObject gracz;

    public static event System.Action<int> KoniecTury;

    [HideInInspector] public bool turaGracza;
    private GameObject fizycznyDeck;

    void Awake()
    {
        fizycznyDeck = GameObject.FindGameObjectWithTag("fizycznyDeck").gameObject;

        dialog.Walka += DeckKolekcjaWalka;
    }
    private void OnDestroy()
    {
        dialog.Walka -= DeckKolekcjaWalka;
    }

    private void DeckKolekcjaWalka(bool walka)
    {
        if (walka)
        {
            DeckFizycznyCzyœæ();

            playerEq eq = gracz.gameObject.GetComponent<playerEq>();
            eq.deck = new List<GameObject>();

            for (int x = 0; x < eq.deckPrefab.Count; x++)
            {
                GameObject karta = Instantiate(eq.deckPrefab[x], fizycznyDeck.transform);
                karta.GetComponent<taKarta>().prefabTejKartyWdeck = eq.deckPrefab[x];
                eq.deck.Add(karta);
            }
        }
    }

    private void DeckFizycznyCzyœæ()
    {
        
        foreach (Transform child in fizycznyDeck.transform)
        {
            Destroy(child.gameObject);
        }
    }

    void Update()
    {
        EnergiaWskazinik();
    }

    private void EnergiaWskazinik()
    {
        playerEq eq = gracz.gameObject.GetComponent<playerEq>();
        wskaŸnikEnergi.gameObject.transform.GetChild(1).GetComponent<TextMeshPro>().text = eq.aktualnaEnergia.ToString() + "/" + eq.maxEnergia.ToString();
    }

    public void SpawnPrzeciwinicy(List<GameObject> przeciwnicySpawn)
    {
        for (int x = 0; x < przeciwnicySpawn.Count; x++)
        {
            GameObject ten = Instantiate(przeciwnicySpawn[x], wrogowieSpawn);
            przeciwnicyWwalce.Add(ten);
        }
    }

    public void DodajKartyDoRêkiStart()
    {
        playerEq eq = gracz.gameObject.GetComponent<playerEq>();
        for (int x = 0; x < eq.ileKartDobiera; x++)
        {
            DobierzKarteRandom();
        }
    }

    public void DobierzKarteRandom()
    {
        EwentualniePrzetasuj();
        playerEq eq = gracz.gameObject.GetComponent<playerEq>();
        if (eq.deck.Count > 0 && dlon.GetComponent<sortGrupZ>().sloty.Count > dlon.GetComponent<sortGrupZ>().kartyWD³oni.Count)
        {
            int randomInt = Random.Range(0, eq.deck.Count - 1);
            dlon.GetComponent<sortGrupZ>().DodajKarte(eq.deck[randomInt]);
            eq.deck.Remove(eq.deck[randomInt]);
        }
    }
    
    public void CzyszczenieRêki()
    {
        dlon.GetComponent<sortGrupZ>().CzyscWszystko();
    }

    public void koniecTury()
    {
        obecnaTura += 1;
        KoniecTury?.Invoke(obecnaTura);
        turaGracza = true;
        DodajKartyDoRêkiStart();
    }

    public void EwentualniePrzetasuj()
    {
        playerEq eq = gracz.gameObject.GetComponent<playerEq>();
        if (eq.deck.Count == 0)
        {
            eq.deck.AddRange(eq.cmentarz);
            eq.cmentarz = new List<GameObject>();
        }
    }

    public void AkcjaWroga(int numerWroga)
    {
        przeciwnicyWwalce[numerWroga].GetComponent<WRUG1>().obecnaAkcja = przeciwnicyWwalce[numerWroga].GetComponent<WRUG1>().dzia³anie[0];
        przeciwnicyWwalce[numerWroga].GetComponent<WRUG1>().dzia³anie[0].Invoke();
    }
}
