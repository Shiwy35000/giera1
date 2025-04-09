using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrolCards : MonoBehaviour
{
    private float pozycjaScroll;
    private float czu�o��Scroll = 0.05f;
    private float scrollSpeed = 6;
    private int ilo��KartOdKturychScroll = 19;
    private int ilo��KartWLini = 6;
    private Vector3 pozycjaPocz�tkowa;
    private Vector3 pozycjaMax;
    private float przesuniecieOlinijk�Y = 2.8f;

    [HideInInspector] public List<GameObject> obecnieWy�wietlanyZbiurKart;
    public GameObject pustySlotPrefab;

    void Update()
    {
        if(this.gameObject.transform.childCount >= ilo��KartOdKturychScroll)
        {
            ScrolujListe();
        }
    }

    public void Aktywuj(List<GameObject> zawarto��)
    {
        Czy��();
        Wype�nij(zawarto��);
        PozycjaMaxWylicz();

    }

    void PozycjaMaxWylicz()
    {
        if (this.gameObject.transform.childCount >= ilo��KartOdKturychScroll)
        {
            if (pozycjaPocz�tkowa!= Vector3.zero)
            {
                transform.position = new Vector3(transform.position.x, pozycjaPocz�tkowa.y, transform.position.z);
            }
            pozycjaPocz�tkowa = new Vector3 (transform.position.x, transform.position.y, transform.position.z);
            pozycjaScroll = pozycjaPocz�tkowa.y;
            int Y = this.gameObject.transform.childCount - (ilo��KartOdKturychScroll - 1);
            int reszta = Y % ilo��KartWLini;
            Y /= ilo��KartWLini;
            if(reszta > 0)
            {
                Y += 1;
            }
            float yFloat = Y * przesuniecieOlinijk�Y;
            pozycjaMax = new Vector3(pozycjaPocz�tkowa.x, pozycjaPocz�tkowa.y + yFloat, pozycjaPocz�tkowa.z);
        }
    }
    void Wype�nij(List<GameObject> zawarto��)
    {
        if (zawarto��.Count > 0)
        {
            obecnieWy�wietlanyZbiurKart = zawarto��;

            for (int x = 0; x < zawarto��.Count; x++)
            {
                GameObject nowySlot = Instantiate(pustySlotPrefab, this.transform);
                GameObject kartaa = Instantiate(zawarto��[x], nowySlot.transform);
                kartaa.name = zawarto��[x].name;
            }
        }
    }
    public void Czy��()
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
            pozycjaScroll -= Input.GetAxis("ScrollWheel") * czu�o��Scroll;

            if (pozycjaScroll > pozycjaMax.y)
            {
                pozycjaScroll = pozycjaMax.y;
            }
            else if (pozycjaScroll < pozycjaPocz�tkowa.y)
            {
                pozycjaScroll = pozycjaPocz�tkowa.y;
            }
        }

        transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, pozycjaScroll, transform.position.z), scrollSpeed * Time.deltaTime);
    }
}
