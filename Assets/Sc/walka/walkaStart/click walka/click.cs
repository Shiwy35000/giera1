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
    private GameObject d這n;
    private GameObject player;
    private int kturyEfekt;
    public GameObject InfoObj;
    public TextMeshProUGUI textMorInfo;
    private Image InfoObjT這;
    private Animator infoEfektAnim;
    private bool czyEkwipunekOtwarty;
    private bool clickLag;
    private GameObject punktLiniaGracz;
    public GameObject nadLinia;
    private celLinia CelLinia;
    private GameObject ramkaCeluGracz;
    private bool czySklep;

    void Awake()
    {
        WalkaStart = this.gameObject.GetComponent<walkaStart>();
        cam = this.gameObject.GetComponent<Camera>();
        d這n = GameObject.FindGameObjectWithTag("dlon").gameObject;
        player = GameObject.FindGameObjectWithTag("Player").gameObject;
        InfoObjT這 = textMorInfo.gameObject.transform.parent.gameObject.GetComponent<Image>();
        infoEfektAnim = InfoObj.GetComponent<Animator>();
        punktLiniaGracz = GameObject.FindGameObjectWithTag("gracz").gameObject.transform.GetChild(2).gameObject;
        CelLinia = nadLinia.GetComponent<celLinia>();
        nadLinia.SetActive(false);
        ramkaCeluGracz = GameObject.FindGameObjectWithTag("gracz").gameObject.transform.GetChild(3).gameObject;
        ramkaCeluGracz.SetActive(false);

        clickNieWalka.ekwipunekWidoczny += CzyEkwipunekOtwarty;
        clickNieWalka.clickLag += lagCorutineStart;
        dialog.sklepOn += czySklepAktywny;
    }
    private void OnDestroy()
    {
        clickNieWalka.ekwipunekWidoczny -= CzyEkwipunekOtwarty;
        clickNieWalka.clickLag -= lagCorutineStart;
        dialog.sklepOn -= czySklepAktywny;
    }
    void CzyEkwipunekOtwarty(bool czy)
    {
        czyEkwipunekOtwarty = czy;
    }
    void czySklepAktywny(List<aso> czy)
    {
        if(czy.Count == 0)
        {
            czySklep = false;
        }
        else
        {
            czySklep = true;
        }
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
        if (czyEkwipunekOtwarty == false && czySklep == false)
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
                        else if (podniesionaKarta != raycastHit.transform.gameObject && podniesionaKarta != null && podniesionaKarta.GetComponent<taKarta>().cele != Cele.Karta && podniesionaKarta.GetComponent<taKarta>().cele != Cele.KartyWD這ni && podniesionaKarta.GetComponent<taKarta>().cele != Cele.RandomKartaWD這ni)
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
                    Czy�潧skazana();
                    KonieTury();
                }
            }

            if (podniesionaKarta == null)
            {
                if (raycastHit.transform.gameObject.tag == "efektImg")
                {
                    InfoOEfekcie(raycastHit.transform.gameObject);
                }
                /*else if(raycastHit.transform.gameObject.tag == "karta")
                {
                    InfoOKarcie(raycastHit.transform.gameObject);
                }*/
            }
            CeleRamkiRysuj(raycastHit.transform.gameObject);
        }
        else
        {
            CeleRamkiRysuj(null);
            Czy�潧skazana();

            if (InfoObjT這.enabled == true)
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
                    if (raycastHit.transform.gameObject.tag == "wrug")
                    {
                        nadLinia.SetActive(true);
                        CelLinia.trybDzia豉nia = TrybDzia豉nia.celWrug;

                        taKarta k = podniesionaKarta.GetComponent<taKarta>();
                        CelLinia.pocz靖ek = k.punktLinia.transform.position;
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
                }
                else if (podniesionaKarta.GetComponent<taKarta>().cele == Cele.Gracz)
                {
                    if (raycastHit.transform.gameObject.tag == "gracz")
                    {
                        nadLinia.SetActive(true);
                        CelLinia.trybDzia豉nia = TrybDzia豉nia.celKarta;

                        taKarta k = podniesionaKarta.GetComponent<taKarta>();
                        CelLinia.pocz靖ek = k.punktLinia.transform.position;
                        CelLinia.koniec = punktLiniaGracz.transform.position;
                    }
                    else
                    {
                        if (nadLinia.activeSelf == true)
                        {
                            nadLinia.SetActive(false);
                        }
                    }
                }
                else if (podniesionaKarta.GetComponent<taKarta>().cele == Cele.Karta)
                {
                    if (raycastHit.transform.gameObject.tag == "karta" && raycastHit.transform.gameObject != podniesionaKarta)
                    {
                        nadLinia.SetActive(true);
                        CelLinia.trybDzia豉nia = TrybDzia豉nia.celKarta;

                        taKarta k = podniesionaKarta.GetComponent<taKarta>();
                        CelLinia.pocz靖ek = k.punktLinia.transform.position;
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
                }
                else if(podniesionaKarta.GetComponent<taKarta>().cele == Cele.AlboWrugAlboGracz)
                {
                    if (raycastHit.transform.gameObject.tag == "gracz")
                    {
                        nadLinia.SetActive(true);
                        CelLinia.trybDzia豉nia = TrybDzia豉nia.celKarta;

                        taKarta k = podniesionaKarta.GetComponent<taKarta>();
                        CelLinia.pocz靖ek = k.punktLinia.transform.position;
                        CelLinia.koniec = punktLiniaGracz.transform.position;
                    }
                    else if (raycastHit.transform.gameObject.tag == "wrug")
                    {
                        nadLinia.SetActive(true);
                        CelLinia.trybDzia豉nia = TrybDzia豉nia.celWrug;

                        taKarta k = podniesionaKarta.GetComponent<taKarta>();
                        CelLinia.pocz靖ek = k.punktLinia.transform.position;
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
                }
            }
            else
            {
                if(nadLinia.activeSelf == true)
                {
                    nadLinia.SetActive(false);
                }
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

    /*void InfoOKarcie(GameObject target)
    {
        infoEfektAnim.Play("pojawia");
        textMorInfo.text = target.GetComponent<taKarta>().publicznyPrzekszta販onyOpis;
        InfoObj.GetComponent<wPozMyszy>().WyliczKorektePoz();
    }*/

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
                string tre�� = cel.GetComponent<WRUG1>().na這rzoneEfekty[kturyEfekt].opis;
                infoEfektAnim.Play("pojawia");
                textMorInfo.text = tre��;
            }
            else if (cel.tag == "gracz")
            {
                string tre�� = player.GetComponent<playerEq>().na這rzoneEfekty[kturyEfekt].opis;
                infoEfektAnim.Play("pojawia");
                textMorInfo.text = tre��;
            }

            InfoObj.GetComponent<wPozMyszy>().WyliczKorektePoz();
        }
        else
        {
            if (InfoObjT這.enabled == true)
            {
                InfoObj.GetComponent<wPozMyszy>().ResetPoz();
                infoEfektAnim.Play("nic");
            }
        }
    }

    public void Czy�潧skazana()
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
            Czy�潧skazana();
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

    void CeleRamkiRysuj(GameObject traf)
    {
        if (podniesionaKarta != null)
        {
            taKarta karta = podniesionaKarta.GetComponent<taKarta>();
            if (karta.cele == Cele.Wrogowie || karta.cele == Cele.RandomWrug)
            {
                for (int x = 0; x < WalkaStart.przeciwnicyWwalce.Count; x++)
                {
                    if (WalkaStart.przeciwnicyWwalce[x].GetComponent<WRUG1>().ramkaCelu.activeSelf == false)
                    {
                        WalkaStart.przeciwnicyWwalce[x].GetComponent<WRUG1>().ramkaCelu.SetActive(true);
                    }
                }
            }
            else if (karta.cele == Cele.All || karta.cele == Cele.Random)
            {
                ramkaCeluGracz.SetActive(true);
                for (int x = 0; x < WalkaStart.przeciwnicyWwalce.Count; x++)
                {
                    if (WalkaStart.przeciwnicyWwalce[x].GetComponent<WRUG1>().ramkaCelu.activeSelf == false)
                    {
                        WalkaStart.przeciwnicyWwalce[x].GetComponent<WRUG1>().ramkaCelu.SetActive(true);
                    }
                }
            }
            else if (karta.cele == Cele.KartyWD這ni || karta.cele == Cele.RandomKartaWD這ni)
            {
                for (int x = 0; x < d這n.GetComponent<sortGrupZ>().kartyWD這ni.Count; x++)
                {
                    if (d這n.GetComponent<sortGrupZ>().kartyWD這ni[x].GetComponent<taKarta>().ramkaCelu.activeSelf == false)
                    {
                        d這n.GetComponent<sortGrupZ>().kartyWD這ni[x].GetComponent<taKarta>().ramkaCelu.SetActive(true);
                    }
                }
            }
            else if(karta.cele == Cele.Gracz && traf != null)
            {
                if (traf.tag == "gracz")
                {
                    ramkaCeluGracz.SetActive(true);
                }
                else
                {
                    ramkaCeluGracz.SetActive(false);
                }
            }
            else if(karta.cele == Cele.Wrug && traf != null)
            {
                for (int x = 0; x < WalkaStart.przeciwnicyWwalce.Count; x++)
                {
                    if(WalkaStart.przeciwnicyWwalce[x] == traf)
                    {
                        WalkaStart.przeciwnicyWwalce[x].GetComponent<WRUG1>().ramkaCelu.SetActive(true);
                    }
                    else
                    {
                        WalkaStart.przeciwnicyWwalce[x].GetComponent<WRUG1>().ramkaCelu.SetActive(false);
                    }
                }
            }
            else if(karta.cele == Cele.Karta && traf != null && traf != podniesionaKarta)
            {
                for (int x = 0; x < d這n.GetComponent<sortGrupZ>().kartyWD這ni.Count; x++)
                {
                    if (d這n.GetComponent<sortGrupZ>().kartyWD這ni[x] == traf)
                    {
                        d這n.GetComponent<sortGrupZ>().kartyWD這ni[x].GetComponent<taKarta>().ramkaCelu.SetActive(true);
                    }
                    else
                    {
                        d這n.GetComponent<sortGrupZ>().kartyWD這ni[x].GetComponent<taKarta>().ramkaCelu.SetActive(false);
                    }
                }
            }
            else if(karta.cele == Cele.AlboWrugAlboGracz && traf != null)
            {
                if(traf.tag == "wrug")
                {
                    ramkaCeluGracz.SetActive(false);
                    for (int x = 0; x < WalkaStart.przeciwnicyWwalce.Count; x++)
                    {
                        if (WalkaStart.przeciwnicyWwalce[x] == traf)
                        {
                            WalkaStart.przeciwnicyWwalce[x].GetComponent<WRUG1>().ramkaCelu.SetActive(true);
                        }
                        else
                        {
                            WalkaStart.przeciwnicyWwalce[x].GetComponent<WRUG1>().ramkaCelu.SetActive(false);
                        }
                    }
                }
                else if(traf.tag == "gracz")
                {
                    ramkaCeluGracz.SetActive(true);
                    for (int x = 0; x < WalkaStart.przeciwnicyWwalce.Count; x++)
                    {
                        if (WalkaStart.przeciwnicyWwalce[x].GetComponent<WRUG1>().ramkaCelu.activeSelf == true)
                        {
                            WalkaStart.przeciwnicyWwalce[x].GetComponent<WRUG1>().ramkaCelu.SetActive(false);
                        }
                    }
                }
            }
            else //czyszczenie
            {
                ramkaCeluGracz.SetActive(false);
                if (d這n.GetComponent<sortGrupZ>().kartyWD這ni.Count > 0)
                {
                    for (int x = 0; x < d這n.GetComponent<sortGrupZ>().kartyWD這ni.Count; x++)
                    {
                        if (d這n.GetComponent<sortGrupZ>().kartyWD這ni[x].GetComponent<taKarta>().ramkaCelu.activeSelf == true)
                        {
                            d這n.GetComponent<sortGrupZ>().kartyWD這ni[x].GetComponent<taKarta>().ramkaCelu.SetActive(false);
                        }
                    }
                }
                if (WalkaStart.przeciwnicyWwalce.Count > 0)
                {
                    for (int x = 0; x < WalkaStart.przeciwnicyWwalce.Count; x++)
                    {
                        if (WalkaStart.przeciwnicyWwalce[x].GetComponent<WRUG1>().ramkaCelu.activeSelf == true)
                        {
                            WalkaStart.przeciwnicyWwalce[x].GetComponent<WRUG1>().ramkaCelu.SetActive(false);
                        }
                    }
                }
            }
        }
        else //czyszczenie
        {
            ramkaCeluGracz.SetActive(false);
            if (d這n.GetComponent<sortGrupZ>().kartyWD這ni.Count > 0)
            {
                for (int x = 0; x < d這n.GetComponent<sortGrupZ>().kartyWD這ni.Count; x++)
            {
                if (d這n.GetComponent<sortGrupZ>().kartyWD這ni[x].GetComponent<taKarta>().ramkaCelu.activeSelf == true)
                {
                    d這n.GetComponent<sortGrupZ>().kartyWD這ni[x].GetComponent<taKarta>().ramkaCelu.SetActive(false);
                }
            }
            }
            if (WalkaStart.przeciwnicyWwalce.Count > 0)
            {
                for (int x = 0; x < WalkaStart.przeciwnicyWwalce.Count; x++)
                {
                    if (WalkaStart.przeciwnicyWwalce[x].GetComponent<WRUG1>().ramkaCelu.activeSelf == true)
                    {
                        WalkaStart.przeciwnicyWwalce[x].GetComponent<WRUG1>().ramkaCelu.SetActive(false);
                    }
                }
            }
        }
    }

    void CzyUrzytoKartePodniesiona(GameObject traf)
    {
        taKarta karta = podniesionaKarta.GetComponent<taKarta>();
        playerEq eq = WalkaStart.gracz.gameObject.GetComponent<playerEq>();
        List<GameObject> ObiektyCele = new List<GameObject>();

        if (karta.Koszt <= eq.aktualnaEnergia && karta.grywalno�� == Grywalno��.Grywalna)
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
            else if(karta.cele == Cele.KartyWD這ni)
            {
                if (traf.tag == "karta")
                {
                    eq.aktualnaEnergia -= karta.Koszt;
                    ObiektyCele.AddRange(d這n.GetComponent<sortGrupZ>().kartyWD這ni);
                    karta.akcje.Invoke(ObiektyCele);
                }
            }
            else if(karta.cele == Cele.RandomKartaWD這ni)
            {
                if (traf.tag == "karta")
                {
                    eq.aktualnaEnergia -= karta.Koszt;
                    int z = Random.Range(0, d這n.GetComponent<sortGrupZ>().kartyWD這ni.Count -1);
                    ObiektyCele.Add(d這n.GetComponent<sortGrupZ>().kartyWD這ni[z]);
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
            Czy�潧skazana();
        }
    }

    public void KonieTury()
    {
        //Debug.Log("koniecTury!");
        WalkaStart.turaGracza = false;
        WalkaStart.AkcjaWroga(0);
    }
}
