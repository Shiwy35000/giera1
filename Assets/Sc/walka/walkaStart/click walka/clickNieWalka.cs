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
    public GameObject podgl¹dKart, UiPozaWalk¹, przyciskiWWalce, przyciskiPozaWalk¹, sklep;
    public zarz¹dzanieSklepem Zarz¹dzanieSklepem;

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
    private Image InfoObjT³o;
    private Animator infoEfektAnim;
    private string TreœæArtefaktu;
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
        InfoObjT³o = textMorInfo.gameObject.transform.parent.gameObject.GetComponent<Image>();
        infoEfektAnim = InfoObj.GetComponent<Animator>();
        ScrolCards = podgl¹dKart.gameObject.transform.GetChild(0).GetComponent<scrolCards>();
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
        ScrolCards.Czyœæ();
        podgl¹dKart.SetActive(false);
        przyciskiWWalce.SetActive(false);
        przyciskiPozaWalk¹.SetActive(false);
        UiPozaWalk¹.SetActive(false);
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
            if (UiPozaWalk¹.activeSelf)
            {
                ZamknijEq();
            }
            else
            {
                UiPozaWalk¹.SetActive(true);
                ekwipunekWidoczny?.Invoke(true);
                podgl¹dKart.SetActive(false);
                if(czyWalka)
                {
                    przyciskiWWalce.SetActive(true);
                }
                else
                {
                    przyciskiPozaWalk¹.SetActive(true);
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
                if (sklep.activeSelf == true || UiPozaWalk¹.activeSelf == true)
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
                    UiPozaWalk¹.SetActive(true);
                    ekwipunekWidoczny?.Invoke(true);
                    lagCorutineStart();
                    if (czyWalka)
                    {
                        przyciskiWWalce.SetActive(true);
                    }
                    else
                    {
                        przyciskiPozaWalk¹.SetActive(true);
                    }
                }
                else if(raycastHit.transform.gameObject == deckButton)
                {
                    if (czyWalka)
                    {
                        if (podgl¹dKart.activeSelf)
                        {
                            if (ScrolCards.obecnieWyœwietlanyZbiurKart == eq.deck)
                            {
                                ScrolCards.Czyœæ();
                                podgl¹dKart.SetActive(false);
                            }
                            else
                            {
                                ScrolCards.Aktywuj(eq.deck);
                            }
                        }
                        else
                        {
                            podgl¹dKart.SetActive(true);
                            ScrolCards.Aktywuj(eq.deck);
                        }
                    }
                    else
                    {
                        if (podgl¹dKart.activeSelf)
                        {
                            if (ScrolCards.obecnieWyœwietlanyZbiurKart == eq.deckPrefab)
                            {
                                ScrolCards.Czyœæ();
                                podgl¹dKart.SetActive(false);
                            }
                            else
                            {
                                ScrolCards.Aktywuj(eq.deckPrefab);
                            }
                        }
                        else
                        {
                            podgl¹dKart.SetActive(true);
                            ScrolCards.Aktywuj(eq.deckPrefab);
                        }
                    }
                }
                else if (raycastHit.transform.gameObject == cmentarzButton)
                {
                    if (podgl¹dKart.activeSelf)
                    {
                        if (ScrolCards.obecnieWyœwietlanyZbiurKart == eq.cmentarz)
                        {
                            ScrolCards.Czyœæ();
                            podgl¹dKart.SetActive(false);
                        }
                        else
                        {
                            ScrolCards.Aktywuj(eq.cmentarz);
                        }
                    }
                    else
                    {
                        podgl¹dKart.SetActive(true);
                        ScrolCards.Aktywuj(eq.cmentarz);
                    }
                }
                else if (raycastHit.transform.gameObject == wykluczoneButton)
                {
                    if (podgl¹dKart.activeSelf)
                    {
                        if (ScrolCards.obecnieWyœwietlanyZbiurKart == eq.wykluczone)
                        {
                            ScrolCards.Czyœæ();
                            podgl¹dKart.SetActive(false);
                        }
                        else
                        {
                            ScrolCards.Aktywuj(eq.wykluczone);
                        }
                    }
                    else
                    {
                        podgl¹dKart.SetActive(true);
                        ScrolCards.Aktywuj(eq.wykluczone);
                    }
                }
                else if(raycastHit.transform.gameObject == sklepOffButton)
                {
                    Zarz¹dzanieSklepem.w³aœcicielSklepu.GetComponent<dialog>().SklepOfOn(new List<aso>());
                    lagCorutineStart();
                }
                else if (raycastHit.transform.gameObject == sklepBuyButton)
                {
                    Zarz¹dzanieSklepem.Kup();
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
                    Zarz¹dzanieSklepem.wybraneAsoDoKupienia = cel;
                    sklepBuyButton.SetActive(true);
                    lagCorutineStart();
                }
                else
                {
                    Zarz¹dzanieSklepem.wybraneAsoDoKupienia = null;
                    sklepBuyButton.SetActive(false);
                    lagCorutineStart();
                }
            }
            else if(Input.GetButtonDown("PrawyMysz") && ClickLag == false)
            {
                Zarz¹dzanieSklepem.wybraneAsoDoKupienia = null;
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
        textMorInfo.text = target.GetComponent<taKarta>().publicznyPrzekszta³conyOpis;
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
            TreœæArtefaktu = target.GetComponent<artefakt>().opis;
            if (TreœæArtefaktu != null)
            {
                infoEfektAnim.Play("pojawia");
                textMorInfo.text = TreœæArtefaktu;
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
