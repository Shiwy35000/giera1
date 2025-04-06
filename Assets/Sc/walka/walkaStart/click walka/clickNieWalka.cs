using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class clickNieWalka : MonoBehaviour
{

    public GameObject InfoObj;
    public TextMeshProUGUI textMorInfo;

    //przypisy
    private Camera cam;
    private Image InfoObjT³o;
    private Animator infoEfektAnim;
    private string TreœæArtefaktu;

    void Awake()
    {
        cam = this.gameObject.transform.parent.gameObject.GetComponent<Camera>();
        InfoObjT³o = textMorInfo.gameObject.transform.parent.gameObject.GetComponent<Image>();
        infoEfektAnim = InfoObj.GetComponent<Animator>();
    }
    void LateUpdate()
    {
        Cast();
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
        }
        else
        {
            ArtefaktInfo(null);
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
        else if (TreœæArtefaktu != null && target == null)
        {
            TreœæArtefaktu = null;
            InfoObj.GetComponent<wPozMyszy>().ResetPoz();
            infoEfektAnim.Play("nic");
        }
    }
}
