using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class clickNieWalka : MonoBehaviour
{
    [HideInInspector] public bool czyWalka;
    public GameObject InfoObj;
    public TextMeshProUGUI textMorInfo;
    public GameObject podgl�dKart, UiPozaWalk�, przyciskiWWalce, przyciskiPozaWalk�, sklep;
    public zarz�dzanieSklepem Zarz�dzanieSklepem;

    [Header("Przyciski:")]
    [Header("ZAWSZE W EQ")]
    public GameObject eqOffButton;
    public GameObject deckButton;
    [Header("NIE W EQ")]
    public GameObject eqOnButton;
    [Header("EQ W WALCE")]
    public GameObject cmentarzButton;
    public GameObject wykluczoneButton;
    [Header("EQ SKLEP")]
    public GameObject sklepOffButton;
    public GameObject sklepBuyButton;

    //przypisy
    private Camera cam;
    private Image InfoObjT�o;
    private Animator infoEfektAnim;
    private string Tre��Artefaktu;
    private scrolCards ScrolCards;
    private playerEq eq;
    
    public static event System.Action<bool> ekwipunekWidoczny;
    public static event System.Action<bool> clickLag;

    //
    private bool ClickLag;
    private bool wDialogu;
    private bool czyWSklepie;
    private bool czyOtwartyEwkipunek;

    void Awake()
    {
        dialog.Walka += CzyWalkaSwitch;
        dialog.wDialogu += wDialoguSwitch;
        dialog.sklepOn += CzyWsklepie;
        ekwipunekWidoczny += CzyOtwartyEwkipunek;

        cam = this.gameObject.transform.parent.gameObject.GetComponent<Camera>();
        InfoObjT�o = textMorInfo.gameObject.transform.parent.gameObject.GetComponent<Image>();
        infoEfektAnim = InfoObj.GetComponent<Animator>();
        ScrolCards = podgl�dKart.gameObject.transform.GetChild(0).GetComponent<scrolCards>();
        eq = GameObject.FindGameObjectWithTag("Player").gameObject.GetComponent<playerEq>();

        ZamknijEq();
    }
    private void OnDestroy()
    {
        dialog.Walka -= CzyWalkaSwitch;
        dialog.wDialogu -= wDialoguSwitch;
        dialog.sklepOn -= CzyWsklepie;
        ekwipunekWidoczny -= CzyOtwartyEwkipunek;
    }
    private void wDialoguSwitch(bool czy)
    {
        wDialogu = czy;
    }
    private void CzyOtwartyEwkipunek(bool czy)
    {
        czyOtwartyEwkipunek = czy;
    }
    private void CzyWsklepie(List<aso> czy)
    {
        if(czy.Count == 0)
        {
            czyWSklepie = false;
        }
        else
        {
            czyWSklepie = true;
        }
    }
    void ZamknijEq()
    {
        ScrolCards.Czy��();
        podgl�dKart.SetActive(false);
        przyciskiWWalce.SetActive(false);
        przyciskiPozaWalk�.SetActive(false);
        UiPozaWalk�.SetActive(false);
        ekwipunekWidoczny?.Invoke(false);
    }
    void LateUpdate()
    {
        if (Cursor.visible)
        {
            Cast();
        }

        AkcjeUiNieWalka();
    }

    void AkcjeUiNieWalka()
    {
        if(Input.GetButtonDown("Eq") && wDialogu == false)
        {
            if (UiPozaWalk�.activeSelf)
            {
                ZamknijEq();
            }
            else
            {
                UiPozaWalk�.SetActive(true);
                ekwipunekWidoczny?.Invoke(true);
                podgl�dKart.SetActive(false);
                if(czyWalka)
                {
                    przyciskiWWalce.SetActive(true);
                }
                else
                {
                    przyciskiPozaWalk�.SetActive(true);
                }
            }
            InfoObj.GetComponent<wPozMyszy>().ResetPoz();
            infoEfektAnim.Play("nic");
        }
    }

    void Cast()
    {
        Vector3 mousePosition = Input.mousePosition;

        Ray myRay = cam.ScreenPointToRay(mousePosition);
        RaycastHit raycastHit;
        bool weHitSomthing = Physics.Raycast(myRay, out raycastHit);

        if (weHitSomthing)
        {
            if (raycastHit.transform.gameObject.tag == "artefakt")
            {
                ArtefaktInfo(raycastHit.transform.gameObject);
            }
            else if(raycastHit.transform.gameObject.tag == "karta")
            {
                if (sklep.activeSelf == true || UiPozaWalk�.activeSelf == true)
                {
                    InfoOKarcie(raycastHit.transform.gameObject);
                }
                else
                {
                    if(Input.GetButton("Tab"))
                    {
                        InfoOKarcie(raycastHit.transform.gameObject);
                    }
                    else
                    {
                        InfoPlusOf();
                    }
                }
            }
            else if(raycastHit.transform.gameObject.tag == "efektImg")
            {
                //celowo puste!
            }
            else
            {
                InfoPlusOf();
            }

            if (Input.GetButtonDown("LewyMysz") && ClickLag == false) //przyciski
            {
                if (raycastHit.transform.gameObject == eqOffButton)
                {
                    ZamknijEq();
                    clickLag?.Invoke(false);
                }
                else if(raycastHit.transform.gameObject == eqOnButton)
                {
                    UiPozaWalk�.SetActive(true);
                    ekwipunekWidoczny?.Invoke(true);
                    lagCorutineStart();
                    if (czyWalka)
                    {
                        przyciskiWWalce.SetActive(true);
                    }
                    else
                    {
                        przyciskiPozaWalk�.SetActive(true);
                    }
                }
                else if(raycastHit.transform.gameObject == deckButton)
                {
                    if (czyWalka)
                    {
                        if (podgl�dKart.activeSelf)
                        {
                            if (ScrolCards.obecnieWy�wietlanyZbiurKart == eq.deck)
                            {
                                ScrolCards.Czy��();
                                podgl�dKart.SetActive(false);
                            }
                            else
                            {
                                ScrolCards.Aktywuj(eq.deck);
                            }
                        }
                        else
                        {
                            podgl�dKart.SetActive(true);
                            ScrolCards.Aktywuj(eq.deck);
                        }
                    }
                    else
                    {
                        if (podgl�dKart.activeSelf)
                        {
                            if (ScrolCards.obecnieWy�wietlanyZbiurKart == eq.deckPrefab)
                            {
                                ScrolCards.Czy��();
                                podgl�dKart.SetActive(false);
                            }
                            else
                            {
                                ScrolCards.Aktywuj(eq.deckPrefab);
                            }
                        }
                        else
                        {
                            podgl�dKart.SetActive(true);
                            ScrolCards.Aktywuj(eq.deckPrefab);
                        }
                    }
                }
                else if (raycastHit.transform.gameObject == cmentarzButton)
                {
                    if (podgl�dKart.activeSelf)
                    {
                        if (ScrolCards.obecnieWy�wietlanyZbiurKart == eq.cmentarz)
                        {
                            ScrolCards.Czy��();
                            podgl�dKart.SetActive(false);
                        }
                        else
                        {
                            ScrolCards.Aktywuj(eq.cmentarz);
                        }
                    }
                    else
                    {
                        podgl�dKart.SetActive(true);
                        ScrolCards.Aktywuj(eq.cmentarz);
                    }
                }
                else if (raycastHit.transform.gameObject == wykluczoneButton)
                {
                    if (podgl�dKart.activeSelf)
                    {
                        if (ScrolCards.obecnieWy�wietlanyZbiurKart == eq.wykluczone)
                        {
                            ScrolCards.Czy��();
                            podgl�dKart.SetActive(false);
                        }
                        else
                        {
                            ScrolCards.Aktywuj(eq.wykluczone);
                        }
                    }
                    else
                    {
                        podgl�dKart.SetActive(true);
                        ScrolCards.Aktywuj(eq.wykluczone);
                    }
                }
                else if(raycastHit.transform.gameObject == sklepOffButton)
                {
                    Zarz�dzanieSklepem.w�a�cicielSklepu.GetComponent<dialog>().SklepOfOn(new List<aso>());
                    lagCorutineStart();
                }
                else if (raycastHit.transform.gameObject == sklepBuyButton)
                {
                    Zarz�dzanieSklepem.Kup();
                    lagCorutineStart();
                }
            }

            if (czyWSklepie == true && czyOtwartyEwkipunek == false)
            {
                Zakupy(raycastHit.transform.gameObject);
            }
        }
        else
        {
            InfoPlusOf();
        }
    }


    void Zakupy(GameObject cel)
    {
        if (cel.tag == "karta" || cel.tag == "artefakt")
        {
            if (Input.GetButtonDown("LewyMysz") && ClickLag == false)
            {
                if(cel.GetComponent<asoInfo>().Aso.cena <= eq.sakiewka)
                {
                    Zarz�dzanieSklepem.wybraneAsoDoKupienia = cel;
                    sklepBuyButton.SetActive(true);
                    lagCorutineStart();
                }
                else
                {
                    Zarz�dzanieSklepem.wybraneAsoDoKupienia = null;
                    sklepBuyButton.SetActive(false);
                    lagCorutineStart();
                }
            }
            else if(Input.GetButtonDown("PrawyMysz") && ClickLag == false)
            {
                Zarz�dzanieSklepem.wybraneAsoDoKupienia = null;
                sklepBuyButton.SetActive(false);
            }
        }
    }

    void CzyWalkaSwitch(bool walka)
    {
        czyWalka = walka;
    }
    void InfoOKarcie(GameObject target)
    {
        infoEfektAnim.Play("pojawia");
        textMorInfo.text = target.GetComponent<taKarta>().publicznyPrzekszta�conyOpis;
        InfoObj.GetComponent<wPozMyszy>().WyliczKorektePoz();
    }

    void InfoPlusOf()
    {
        if(InfoObj.GetComponent<wPozMyszy>().infoWizualia.transform.GetChild(0).GetComponent<Image>().enabled == true)
        {
            InfoObj.GetComponent<wPozMyszy>().ResetPoz();
            infoEfektAnim.Play("nic");
        }
    }

    void ArtefaktInfo(GameObject target)
    {
        if (target != null)
        {
            Tre��Artefaktu = target.GetComponent<artefakt>().opis;
            if (Tre��Artefaktu != null)
            {
                infoEfektAnim.Play("pojawia");
                textMorInfo.text = Tre��Artefaktu;
                InfoObj.GetComponent<wPozMyszy>().WyliczKorektePoz();
            }
        }
    }

    void lagCorutineStart()
    {
        StartCoroutine(lagCorutine(0.1f));
    }
    IEnumerator lagCorutine(float PauzaColdown)
    {
        ClickLag = true;
        yield return new WaitForSeconds(PauzaColdown);
        ClickLag = false;
    }
}
