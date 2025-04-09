using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrolCards : MonoBehaviour
{
    private float pozycjaScroll;
    private float czu³oœæScroll = 0.05f;
    private float scrollSpeed = 6;
    private int iloœæKartOdKturychScroll = 19;
    private int iloœæKartWLini = 6;
    private Vector3 pozycjaPocz¹tkowa;
    private Vector3 pozycjaMax;
    private float przesuniecieOlinijkêY = 2.8f;

    [HideInInspector] public List<GameObject> obecnieWyœwietlanyZbiurKart;
    public GameObject pustySlotPrefab;

    void Update()
    {
        if(this.gameObject.transform.childCount >= iloœæKartOdKturychScroll)
        {
            ScrolujListe();
        }
    }

    public void Aktywuj(List<GameObject> zawartoœæ)
    {
        Czyœæ();
        Wype³nij(zawartoœæ);
        PozycjaMaxWylicz();

    }

    void PozycjaMaxWylicz()
    {
        if (this.gameObject.transform.childCount >= iloœæKartOdKturychScroll)
        {
            if (pozycjaPocz¹tkowa!= Vector3.zero)
            {
                transform.position = new Vector3(transform.position.x, pozycjaPocz¹tkowa.y, transform.position.z);
            }
            pozycjaPocz¹tkowa = new Vector3 (transform.position.x, transform.position.y, transform.position.z);
            pozycjaScroll = pozycjaPocz¹tkowa.y;
            int Y = this.gameObject.transform.childCount - (iloœæKartOdKturychScroll - 1);
            int reszta = Y % iloœæKartWLini;
            Y /= iloœæKartWLini;
            if(reszta > 0)
            {
                Y += 1;
            }
            float yFloat = Y * przesuniecieOlinijkêY;
            pozycjaMax = new Vector3(pozycjaPocz¹tkowa.x, pozycjaPocz¹tkowa.y + yFloat, pozycjaPocz¹tkowa.z);
        }
    }
    void Wype³nij(List<GameObject> zawartoœæ)
    {
        if (zawartoœæ.Count > 0)
        {
            obecnieWyœwietlanyZbiurKart = zawartoœæ;

            for (int x = 0; x < zawartoœæ.Count; x++)
            {
                GameObject nowySlot = Instantiate(pustySlotPrefab, this.transform);
                GameObject kartaa = Instantiate(zawartoœæ[x], nowySlot.transform);
                kartaa.name = zawartoœæ[x].name;
            }
        }
    }
    public void Czyœæ()
    {
        foreach (Transform child in this.gameObject.transform)
        {
            Destroy(child.gameObject);
        }
    }

    void ScrolujListe()
    {
        if (Input.GetAxis("ScrollWheel") != 0f)
        {
            pozycjaScroll -= Input.GetAxis("ScrollWheel") * czu³oœæScroll;

            if (pozycjaScroll > pozycjaMax.y)
            {
                pozycjaScroll = pozycjaMax.y;
            }
            else if (pozycjaScroll < pozycjaPocz¹tkowa.y)
            {
                pozycjaScroll = pozycjaPocz¹tkowa.y;
            }
        }

        transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, pozycjaScroll, transform.position.z), scrollSpeed * Time.deltaTime);
    }
}
