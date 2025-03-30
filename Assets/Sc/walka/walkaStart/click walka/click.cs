using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class click : MonoBehaviour
{
    private Camera cam;
    
    //cardInfo;
    private GameObject cel;
    private GameObject poprzedniCel;

    //grab
    private GameObject podniesionaKarta;
    //wskazana karta
    private GameObject wskazana;

    //inne
    private GameObject graczImg, graczMorInfo;
    private walkaStart WalkaStart;
    private GameObject d³on;
    private GameObject player;
    private int kturyEfekt;
    public GameObject InfoObj;
    public TextMeshProUGUI textMorInfo;
    private Image InfoObjT³o;
    private Animator infoEfektAnim;

    void Awake()
    {
        WalkaStart = this.gameObject.transform.parent.gameObject.GetComponent<walkaStart>();
        graczImg = GameObject.FindGameObjectWithTag("gracz").gameObject;
        graczMorInfo = graczImg.transform.GetChild(1).gameObject;
        graczMorInfo.SetActive(false);
        cam = this.gameObject.transform.parent.gameObject.GetComponent<Camera>();
        d³on = GameObject.FindGameObjectWithTag("dlon").gameObject;
        player = GameObject.FindGameObjectWithTag("Player").gameObject;
        InfoObjT³o = textMorInfo.gameObject.transform.parent.gameObject.GetComponent<Image>();
        infoEfektAnim = InfoObj.GetComponent<Animator>();
    }

    void LateUpdate() //nie pewna, ale zmiana na LATE!!!
    {
        Cast();
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
                    MorInfo(raycastHit.transform.gameObject);
                    CardWskazana(raycastHit.transform.gameObject);
                }
                if (WalkaStart.turaGracza)
                {
                    if (Input.GetButtonDown("LewyMysz"))
                    {
                        if (podniesionaKarta == null)
                        {
                            GrabCard(raycastHit.transform.gameObject);
                            CzyœæCardMorInfo();
                        }
                        else if (podniesionaKarta == raycastHit.transform.gameObject)
                        {
                            GrabCardOf();
                        }
                        else if (podniesionaKarta != raycastHit.transform.gameObject && podniesionaKarta != null && podniesionaKarta.GetComponent<taKarta>().cele != Cele.Karta)
                        {
                            GrabCardOf();
                            GrabCard(raycastHit.transform.gameObject);
                            CzyœæCardMorInfo();
                        }
                    }
                }
            }
            if (WalkaStart.turaGracza)
            {
                if (raycastHit.transform.gameObject.name == "graczImg" || raycastHit.transform.gameObject.tag == "wrug" || raycastHit.transform.gameObject.tag == "karta")
                {
                    if (podniesionaKarta != null && podniesionaKarta != raycastHit.transform.gameObject)
                    {
                        if (Input.GetButtonDown("LewyMysz") || Input.GetButtonUp("LewyMysz"))
                        {
                            CzyUrzytoKartePodniesiona(raycastHit.transform.gameObject);
                        }
                    }
                }
            }

            if (podniesionaKarta == null)
            {
                if (raycastHit.transform.gameObject.tag == "wrug")
                {
                    MorInfo(raycastHit.transform.gameObject);
                    InfoOEfekcie(raycastHit.transform.gameObject);
                }
                else if(raycastHit.transform.gameObject.name == "graczImg")
                {
                    MorInfo(raycastHit.transform.gameObject);
                    InfoOEfekcie(raycastHit.transform.gameObject);
                }
                else if (raycastHit.transform.gameObject.tag == "moreInfo" || raycastHit.transform.gameObject.tag == "efektImg")
                {
                    MorInfo(cel);
                    InfoOEfekcie(raycastHit.transform.gameObject);
                }

            }
            if (WalkaStart.turaGracza)
            {
                if (raycastHit.transform.gameObject.name == "koniecTury" && Input.GetButtonDown("LewyMysz"))
                {
                    GrabCardOf();
                    CzyœæCardMorInfo();
                    CzyœæWskazana();
                    KonieTury();
                }
            }
          
        }
        else
        {
            CzyœæCardMorInfo();
            CzyœæWskazana();

            if(InfoObjT³o.enabled == true)
            {
                InfoObj.GetComponent<wPozMyszy>().ResetPoz();
                infoEfektAnim.Play("nic");
            }

            if (WalkaStart.turaGracza)
            {
                if (Input.GetButtonDown("LewyMysz") && podniesionaKarta != null)
                {
                    GrabCardOf();
                }
            }
        }

        if (WalkaStart.turaGracza)
        {
            if (Input.GetButtonDown("PrawyMysz") && podniesionaKarta != null)
            {
                GrabCardOf();
            }
        }
    }

    void InfoOEfekcie(GameObject target)
    {
        if (target.tag == "efektImg")
        {

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

    public void CzyœæCardMorInfo()
    {
        if (cel != null)
        {
            if (cel.tag == "karta")
            {
                cel.GetComponent<taKarta>().podgl¹dOpis.SetActive(false);
                cel = null;
            }
            else if (cel.tag == "wrug")
            {
                cel.GetComponent<WRUG1>().morInfo.SetActive(false);
                cel = null;
            }
            else if (cel.name == "graczImg")
            {
                graczMorInfo.SetActive(false);
                cel = null;
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
    void MorInfo(GameObject traf)
    {
        if (cel == null || cel != traf)
        {
            CzyœæCardMorInfo();
            cel = traf;
        }
        else
        {
            if (cel.tag == "karta")
            {
                cel.GetComponent<taKarta>().podgl¹dOpis.SetActive(true);
            }
            else if (cel.tag == "wrug")
            {
                cel.GetComponent<WRUG1>().morInfo.SetActive(true);
            }
            else if(cel == graczImg)
            {
                graczMorInfo.SetActive(true);
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
                if(traf == graczImg)
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
                if (traf.tag == "wrug" || traf == graczImg)
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
                if (traf == graczImg)
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

    void KonieTury()
    {
        //Debug.Log("koniecTury!");
        WalkaStart.turaGracza = false;
        WalkaStart.AkcjaWroga(0);
    }
}
