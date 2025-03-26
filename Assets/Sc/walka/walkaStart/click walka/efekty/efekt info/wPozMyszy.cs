using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wPozMyszy : MonoBehaviour
{
    private Vector3 nowaPozycja = new Vector3(0, 0, 0);

    public GameObject rug, rugColider, infoWizualia;
    private bool kolizjaR, kolizjaD;
    private float poprawkaX = 30f;

    void LateUpdate()
    {
        this.gameObject.GetComponent<RectTransform>().position = Input.mousePosition;
    }

    public void ResetPoz()
    {
        //infoWizualia.GetComponent<RectTransform>().localPosition = Vector3.zero;
        rugColider.GetComponent<RectTransform>().position = rug.GetComponent<RectTransform>().position;
    }

    public void WyliczKorektePoz()
    {
        kolizjaR = rugColider.GetComponent<kolizja>().KolizjaR;
        kolizjaD = rugColider.GetComponent<kolizja>().KolizjaD;
        if (kolizjaR)
        {
            nowaPozycja.x = - ((rugColider.GetComponent<RectTransform>().localPosition.x / 2) + poprawkaX);
        }
        else
        {
            float n = rug.GetComponent<RectTransform>().position.x;
            Vector3 N = new Vector3(n, rugColider.GetComponent<RectTransform>().position.y, rugColider.GetComponent<RectTransform>().position.z);
            rugColider.GetComponent<RectTransform>().position = N;
            nowaPozycja.x = (rugColider.GetComponent<RectTransform>().localPosition.x / 2) + poprawkaX;
        }

        if(kolizjaD)
        {
            nowaPozycja.y = -rugColider.GetComponent<RectTransform>().localPosition.y;
        }
        else
        {
            float n = rug.GetComponent<RectTransform>().position.y;
            Vector3 N = new Vector3(rugColider.GetComponent<RectTransform>().position.x, n, rugColider.GetComponent<RectTransform>().position.z);
            rugColider.GetComponent<RectTransform>().position = N;
            nowaPozycja.y = 0;
        }

        infoWizualia.GetComponent<RectTransform>().localPosition = nowaPozycja;
    }
    
}
