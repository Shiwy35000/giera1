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
    public GameObject podgl�dKart, UiPozaWalk�;

    [Header("Przyciski")]
    public GameObject eqOffButton;
    public GameObject eqOnButton;
    public GameObject deckButton;

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

    void Awake()
    {
        dialog.Walka += CzyWalkaSwitch;
        dialog.wDialogu += wDialoguSwitch;

        cam = this.gameObject.transform.parent.gameObject.GetComponent<Camera>();
        InfoObjT�o = textMorInfo.gameObject.transform.parent.gameObject.GetComponent<Image>();
        infoEfektAnim = InfoObj.GetComponent<Animator>();
        ScrolCards = podgl�dKart.gameObject.transform.GetChild(0).GetComponent<scrolCards>();
        eq = GameObject.FindGameObjectWithTag("Player").gameObject.GetComponent<playerEq>();

        UiPozaWalk�.SetActive(false);
        podgl�dKart.SetActive(false);
    }
    private void OnDestroy()
    {
        dialog.Walka -= CzyWalkaSwitch;
        dialog.wDialogu -= wDialoguSwitch;
    }
    private void wDialoguSwitch(bool czy)
    {
        wDialogu = czy;
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
                UiPozaWalk�.SetActive(false);
                ekwipunekWidoczny?.Invoke(false);
            }
            else
            {
                UiPozaWalk�.SetActive(true);
                ekwipunekWidoczny?.Invoke(true);
                podgl�dKart.SetActive(false);
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
            else if(raycastHit.transform.gameObject.tag == "karta" && czyWalka == false)
            {
                InfoOKarcie(raycastHit.transform.gameObject);
            }

            if (Input.GetButtonDown("LewyMysz") && ClickLag == false) //przyciski
            {
                if (raycastHit.transform.gameObject == eqOffButton)
                {
                    UiPozaWalk�.SetActive(false);
                    ekwipunekWidoczny?.Invoke(false);
                    clickLag?.Invoke(false);
                }
                else if(raycastHit.transform.gameObject == eqOnButton)
                {
                    UiPozaWalk�.SetActive(true);
                    ekwipunekWidoczny?.Invoke(true);
                    lagCorutineStart();
                    podgl�dKart.SetActive(false);
                }
                else if(raycastHit.transform.gameObject == deckButton)
                {
                    if(podgl�dKart.activeSelf)
                    {
                        if(ScrolCards.obecnieWy�wietlanyZbiurKart == eq.deckPrefab)
                        {
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
        }
        else
        {
            ArtefaktInfo(null);
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
        else if (Tre��Artefaktu != null && target == null)
        {
            Tre��Artefaktu = null;
            InfoObj.GetComponent<wPozMyszy>().ResetPoz();
            infoEfektAnim.Play("nic");
        }
    }

    void lagCorutineStart() //narazie nie ma zastosowania?
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
