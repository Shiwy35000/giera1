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
    public GameObject podgl¹dKart, UiPozaWalk¹;

    //przypisy
    private Camera cam;
    private Image InfoObjT³o;
    private Animator infoEfektAnim;
    private string TreœæArtefaktu;

    public static event System.Action<bool> ekwipunekWidoczny;

    void Awake()
    {
        dialog.Walka += CzyWalkaSwitch;
        cam = this.gameObject.transform.parent.gameObject.GetComponent<Camera>();
        InfoObjT³o = textMorInfo.gameObject.transform.parent.gameObject.GetComponent<Image>();
        infoEfektAnim = InfoObj.GetComponent<Animator>();

        UiPozaWalk¹.SetActive(false);
        podgl¹dKart.SetActive(false);
    }
    private void OnDestroy()
    {
        dialog.Walka -= CzyWalkaSwitch;
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
        if(Input.GetButtonDown("Eq"))
        {
            if (UiPozaWalk¹.activeSelf)
            {
                UiPozaWalk¹.SetActive(false);
                ekwipunekWidoczny?.Invoke(false);
            }
            else
            {
                UiPozaWalk¹.SetActive(true);
                ekwipunekWidoczny?.Invoke(true);
            }
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
        textMorInfo.text = target.GetComponent<taKarta>().publicznyPrzekszta³conyOpis;
        InfoObj.GetComponent<wPozMyszy>().WyliczKorektePoz();
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
        else if (TreœæArtefaktu != null && target == null)
        {
            TreœæArtefaktu = null;
            InfoObj.GetComponent<wPozMyszy>().ResetPoz();
            infoEfektAnim.Play("nic");
        }
    }
}
