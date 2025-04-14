using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class click : MonoBehaviour
{
    private Camera cam;
    
    private GameObject poprzedniCel;

    //grab
    private GameObject podniesionaKarta;
    //wskazana karta
    private GameObject wskazana;

    //inne
    private walkaStart WalkaStart;
    private GameObject d³on;
    private GameObject player;
    private int kturyEfekt;
    public GameObject InfoObj;
    public TextMeshProUGUI textMorInfo;
    private Image InfoObjT³o;
    private Animator infoEfektAnim;
    private bool czyEkwipunekOtwarty;
    private bool clickLag;
    private GameObject punktLiniaGracz;
    public GameObject nadLinia;
    private celLinia CelLinia;

    void Awake()
    {
        WalkaStart = this.gameObject.GetComponent<walkaStart>();
        cam = this.gameObject.GetComponent<Camera>();
        d³on = GameObject.FindGameObjectWithTag("dlon").gameObject;
        player = GameObject.FindGameObjectWithTag("Player").gameObject;
        InfoObjT³o = textMorInfo.gameObject.transform.parent.gameObject.GetComponent<Image>();
        infoEfektAnim = InfoObj.GetComponent<Animator>();
        punktLiniaGracz = GameObject.FindGameObjectWithTag("gracz").gameObject.transform.GetChild(2).gameObject;
        CelLinia = nadLinia.GetComponent<celLinia>();
        nadLinia.SetActive(false);

        clickNieWalka.ekwipunekWidoczny += CzyEkwipunekOtwarty;
        clickNieWalka.clickLag += lagCorutineStart;
    }
    private void OnDestroy()
    {
        clickNieWalka.ekwipunekWidoczny -= CzyEkwipunekOtwarty;
        clickNieWalka.clickLag -= lagCorutineStart;
    }
    void CzyEkwipunekOtwarty(bool czy)
    {
        czyEkwipunekOtwarty = czy;
    }
    void lagCorutineStart(bool nic)
    {
        StartCoroutine(lagCorutine(0.1f));
    }
    IEnumerator lagCorutine(float PauzaColdown)
    {
        clickLag = true;
        yield return new WaitForSeconds(PauzaColdown);
        clickLag = false;
    }
    void LateUpdate()
    {
        if (czyEkwipunekOtwarty == false)
        {
            Cast();
        }
    }

    void Cast()
    {
        Vector3 mousePosition = Input.mousePosition;

        Ray myRay = cam.ScreenPointToRay(mousePosition);
        RaycastHit raycastHit;
        bool weHitSomthing = Physics.Raycast(myRay, out raycastHit);

        if(weHitSomthing)
        {
            if (raycastHit.transform.gameObject.tag == "karta")
            {
                if (podniesionaKarta == null)
                {
                    CardWskazana(raycastHit.transform.gameObject);
                }
                if (WalkaStart.turaGracza)
                {
                    if (Input.GetButtonDown("LewyMysz") && clickLag == false)
                    {
                        if (podniesionaKarta == null)
                        {
                            GrabCard(raycastHit.transform.gameObject);
                        }
                        else if (podniesionaKarta == raycastHit.transform.gameObject)
                        {
                            GrabCardOf();
                        }
                        else if (podniesionaKarta != raycastHit.transform.gameObject && podniesionaKarta != null && podniesionaKarta.GetComponent<taKarta>().cele != Cele.Karta && podniesionaKarta.GetComponent<taKarta>().cele != Cele.KartyWD³oni && podniesionaKarta.GetComponent<taKarta>().cele != Cele.RandomKartaWD³oni)
                        {
                            GrabCardOf();
                            GrabCard(raycastHit.transform.gameObject);
                        }
                    }
                }
            }

            if (WalkaStart.turaGracza)
            {
                if (raycastHit.transform.gameObject.name == "graczImg" || raycastHit.transform.gameObject.tag == "wrug" || raycastHit.transform.gameObject.tag == "karta")
                {
                    if (podniesionaKarta != null && podniesionaKarta != raycastHit.transform.gameObject && clickLag == false)
                    {
                        if (Input.GetButtonDown("LewyMysz") || Input.GetButtonUp("LewyMysz"))
                        {
                            CzyUrzytoKartePodniesiona(raycastHit.transform.gameObject);
                        }
                    }
                }

                if (raycastHit.transform.gameObject.name == "koniecTury" && Input.GetButtonDown("LewyMysz") && clickLag == false)
                {
                    GrabCardOf();
                    CzyœæWskazana();
                    KonieTury();
                }
            }

            if (podniesionaKarta == null)
            {
                if (raycastHit.transform.gameObject.tag == "efektImg")
                {
                    InfoOEfekcie(raycastHit.transform.gameObject);
                }
                else if(raycastHit.transform.gameObject.tag == "karta")
                {
                    InfoOKarcie(raycastHit.transform.gameObject);
                }
            }
        }
        else
        {
            CzyœæWskazana();

            if (InfoObjT³o.enabled == true)
            {
                InfoObj.GetComponent<wPozMyszy>().ResetPoz();
                infoEfektAnim.Play("nic");
            }

            if (WalkaStart.turaGracza)
            {
                if (Input.GetButtonDown("LewyMysz") && podniesionaKarta != null && clickLag == false)
                {
                    GrabCardOf();
                }
            }
        }

        if (podniesionaKarta != null)
        {
            if (weHitSomthing)
            {
                if(podniesionaKarta.GetComponent<taKarta>().cele == Cele.Wrug)
                {
                    nadLinia.SetActive(true);

                    if (raycastHit.transform.gameObject.tag == "wrug")
                    {
                        CelLinia.trybDzia³ania = TrybDzia³ania.celWrug;

                        taKarta k = podniesionaKarta.GetComponent<taKarta>();
                        CelLinia.pocz¹tek = k.punktLinia.transform.position;
                        WRUG1 k2 = raycastHit.transform.gameObject.GetComponent<WRUG1>();
                        CelLinia.koniec = k2.punktLinia.transform.position;
                    }
                    else
                    {
                        if (nadLinia.activeSelf == true)
                        {
                            nadLinia.SetActive(false);
                        }
                    }
                    /*else
                    {
                        LiniaDoMyszy();
                    }*/
                }
                else if (podniesionaKarta.GetComponent<taKarta>().cele == Cele.Gracz)
                {
                    nadLinia.SetActive(true);

                    if (raycastHit.transform.gameObject.tag == "gracz")
                    {
                        CelLinia.trybDzia³ania = TrybDzia³ania.celKarta;

                        taKarta k = podniesionaKarta.GetComponent<taKarta>();
                        CelLinia.pocz¹tek = k.punktLinia.transform.position;
                        CelLinia.koniec = punktLiniaGracz.transform.position;
                    }
                    else
                    {
                        if (nadLinia.activeSelf == true)
                        {
                            nadLinia.SetActive(false);
                        }
                    }
                    /*else
                    {
                        LiniaDoMyszy();
                    }*/
                }
                else if (podniesionaKarta.GetComponent<taKarta>().cele == Cele.Karta)
                {
                    nadLinia.SetActive(true);

                    if (raycastHit.transform.gameObject.tag == "karta" && raycastHit.transform.gameObject != podniesionaKarta)
                    {
                        CelLinia.trybDzia³ania = TrybDzia³ania.celKarta;

                        taKarta k = podniesionaKarta.GetComponent<taKarta>();
                        CelLinia.pocz¹tek = k.punktLinia.transform.position;
                        taKarta k2 = raycastHit.transform.gameObject.GetComponent<taKarta>();
                        CelLinia.koniec = k2.punktLinia.transform.position;
                    }
                    else
                    {
                        if (nadLinia.activeSelf == true)
                        {
                            nadLinia.SetActive(false);
                        }
                    }
                   /* else
                    {
                        LiniaDoMyszy();
                    }*/
                }
            }
            else
            {
                if(nadLinia.activeSelf == true)
                {
                    nadLinia.SetActive(false);
                }
                /*if(podniesionaKarta.GetComponent<taKarta>().cele == Cele.Wrug || podniesionaKarta.GetComponent<taKarta>().cele == Cele.Gracz || podniesionaKarta.GetComponent<taKarta>().cele == Cele.Karta)
                {
                    LiniaDoMyszy();
                }*/
            }
        }

        if (WalkaStart.turaGracza)
        {
            if (Input.GetButtonDown("PrawyMysz") && podniesionaKarta != null && clickLag == false)
            {
                GrabCardOf();
            }
        }
    }

    /*void LiniaDoMyszy()
    {
        /*if(podniesionaKarta.GetComponent<taKarta>().cele == Cele.Wrug)
        {
            CelLinia.trybDzia³ania = TrybDzia³ania.celWrug;
        }
        else if(podniesionaKarta.GetComponent<taKarta>().cele == Cele.Gracz || podniesionaKarta.GetComponent<taKarta>().cele == Cele.Karta)
        {
            CelLinia.trybDzia³ania = TrybDzia³ania.celKarta; 
        }

        taKarta k = podniesionaKarta.GetComponent<taKarta>();
        CelLinia.pocz¹tek = k.punktLinia.transform.position;
        Vector3 mousePos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z));    
        CelLinia.koniec = mousePos;*/
    //}

    void InfoOKarcie(GameObject target)
    {
        infoEfektAnim.Play("pojawia");
        textMorInfo.text = target.GetComponent<taKarta>().publicznyPrzekszta³conyOpis;
        InfoObj.GetComponent<wPozMyszy>().WyliczKorektePoz();
    }

    void InfoOEfekcie(GameObject target)
    {
        if (target.tag == "efektImg")
        {
            GameObject cel = target.transform.parent.transform.parent.transform.transform.parent.gameObject;
            GameObject targetParent = target.transform.parent.gameObject;
            for (int x = 0; x < targetParent.transform.childCount; x++)
            {
                if (targetParent.transform.GetChild(x).gameObject == target)
                {
                    kturyEfekt = x;
                }
            }

            if (cel.tag == "wrug")
            {
                string treœæ = cel.GetComponent<WRUG1>().na³orzoneEfekty[kturyEfekt].opis;
                infoEfektAnim.Play("pojawia");
                textMorInfo.text = treœæ;
            }
            else if (cel.tag == "gracz")
            {
                string treœæ = player.GetComponent<playerEq>().na³orzoneEfekty[kturyEfekt].opis;
                infoEfektAnim.Play("pojawia");
                textMorInfo.text = treœæ;
            }

            InfoObj.GetComponent<wPozMyszy>().WyliczKorektePoz();
        }
        else
        {
            if (InfoObjT³o.enabled == true)
            {
                InfoObj.GetComponent<wPozMyszy>().ResetPoz();
                infoEfektAnim.Play("nic");
            }
        }
    }

    public void CzyœæWskazana()
    {
        if (wskazana != null)
        {
            if (wskazana.tag == "karta")
            {
                if (wskazana != podniesionaKarta)
                {
                    wskazana.GetComponent<taKarta>().Wskazano(false);
                    wskazana = null;
                }
            }
        }
    }
  
    void CardWskazana(GameObject traf)
    {
        if (wskazana == null || wskazana != traf)
        {
            CzyœæWskazana();
            wskazana = traf;
            wskazana.GetComponent<taKarta>().Wskazano(true);
        }
    }

    void GrabCard(GameObject traf)
    {
        podniesionaKarta = traf;
        podniesionaKarta.GetComponent<taKarta>().PodniesionaPoz(true);
    }

    public void GrabCardOf()
    {
        if (podniesionaKarta != null)
        {
            podniesionaKarta.GetComponent<taKarta>().PodniesionaPoz(false);
            podniesionaKarta = null;
            nadLinia.SetActive(false);
        }
    }

    void CzyUrzytoKartePodniesiona(GameObject traf)
    {
        taKarta karta = podniesionaKarta.GetComponent<taKarta>();
        playerEq eq = WalkaStart.gracz.gameObject.GetComponent<playerEq>();
        List<GameObject> ObiektyCele = new List<GameObject>();

        if (karta.Koszt <= eq.aktualnaEnergia && karta.grywalnoœæ == Grywalnoœæ.Grywalna)
        {
            if(karta.cele == Cele.Gracz)
            {
                if(traf.tag == "gracz")
                {
                    eq.aktualnaEnergia -= karta.Koszt;
                    ObiektyCele.Add(WalkaStart.gracz.gameObject);
                    karta.akcje.Invoke(ObiektyCele);
                }
            }
            else if(karta.cele == Cele.Wrug)
            {
                if (traf.tag == "wrug")
                {
                    eq.aktualnaEnergia -= karta.Koszt;
                    ObiektyCele.Add(traf);
                    karta.akcje.Invoke(ObiektyCele);
                }
            }
            else if (karta.cele == Cele.Wrogowie || karta.cele == Cele.RandomWrug)
            {
                if (traf.tag == "wrug")
                {
                    eq.aktualnaEnergia -= karta.Koszt;
                    ObiektyCele.AddRange(WalkaStart.przeciwnicyWwalce);
                    karta.akcje.Invoke(ObiektyCele);
                }
            }
            else if (karta.cele == Cele.Karta)
            {
                if (traf.tag == "karta")
                {
                    eq.aktualnaEnergia -= karta.Koszt;
                    ObiektyCele.Add(traf);
                    karta.akcje.Invoke(ObiektyCele);
                }
            }
            else if (karta.cele == Cele.All || karta.cele == Cele.Random)
            {
                if (traf.tag == "wrug" || traf.tag == "gracz")
                {
                    eq.aktualnaEnergia -= karta.Koszt;
                    ObiektyCele.AddRange(WalkaStart.przeciwnicyWwalce);
                    ObiektyCele.Add(WalkaStart.gracz.gameObject);
                    karta.akcje.Invoke(ObiektyCele);
                }
            }
            else if(karta.cele == Cele.KartyWD³oni)
            {
                if (traf.tag == "karta")
                {
                    eq.aktualnaEnergia -= karta.Koszt;
                    ObiektyCele.AddRange(d³on.GetComponent<sortGrupZ>().kartyWD³oni);
                    karta.akcje.Invoke(ObiektyCele);
                }
            }
            else if(karta.cele == Cele.RandomKartaWD³oni)
            {
                if (traf.tag == "karta")
                {
                    eq.aktualnaEnergia -= karta.Koszt;
                    int z = Random.Range(0, d³on.GetComponent<sortGrupZ>().kartyWD³oni.Count -1);
                    ObiektyCele.Add(d³on.GetComponent<sortGrupZ>().kartyWD³oni[z]);
                    karta.akcje.Invoke(ObiektyCele);
                }
            }
            else if(karta.cele == Cele.AlboWrugAlboGracz)
            {
                if (traf.tag == "gracz")
                {
                    eq.aktualnaEnergia -= karta.Koszt;
                    ObiektyCele.Add(WalkaStart.gracz.gameObject);
                    karta.akcje.Invoke(ObiektyCele);
                }
                else if (traf.tag == "wrug")
                {
                    eq.aktualnaEnergia -= karta.Koszt;
                    ObiektyCele.Add(traf);
                    karta.akcje.Invoke(ObiektyCele);
                }
            }
        }

        if (podniesionaKarta != null)
        {
            GrabCardOf();
            CzyœæWskazana();
        }
    }

    public void KonieTury()
    {
        //Debug.Log("koniecTury!");
        WalkaStart.turaGracza = false;
        WalkaStart.AkcjaWroga(0);
    }
}
