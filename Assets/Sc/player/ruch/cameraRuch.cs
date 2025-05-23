using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraRuch : MonoBehaviour
{
    private float rotacjaY;
    private float rotacjaAktualna;
    [HideInInspector] public bool walka = false;
    private bool czyEkwipunekOtwarty;
    private bool czySklepOtwarty;

    void Awake()
    {
        dialog.Walka += Wwalce;
        clickNieWalka.ekwipunekWidoczny += CzyEkwipunekOtwarty;
        dialog.sklepOn += CzySklepOtwarty;
    }
    private void OnDestroy()
    {
        dialog.Walka -= Wwalce;
        clickNieWalka.ekwipunekWidoczny -= CzyEkwipunekOtwarty;
        dialog.sklepOn -= CzySklepOtwarty;
    }

    void CzyEkwipunekOtwarty(bool czy)
    {
        czyEkwipunekOtwarty = czy;
    }
    void CzySklepOtwarty(List<aso> nic)
    {
        if (nic.Count == 0)
        {
            czySklepOtwarty = false;
        }
        else
        {
            czySklepOtwarty = true;
        }
    }
    void Wwalce(bool czy)
    {
        walka = czy;
    }

    void Start()
    {
        rotacjaY = transform.eulerAngles.y;
    }

    void Update()
    {
        rotacjaAktualna = transform.eulerAngles.y;

        if (walka == false && czyEkwipunekOtwarty == false && czySklepOtwarty == false)
        {
            if (Input.GetAxis("ScrollWheel") != 0f)
            {
                rotacjaY += Input.GetAxis("ScrollWheel");
            }
        }
        if (rotacjaY > 360 && 10 >= rotacjaAktualna && rotacjaAktualna > 0)
        {
            rotacjaY -= 360;
        }
        else if (0 > rotacjaY && 350 <= rotacjaAktualna && rotacjaAktualna < 360)
        {
            rotacjaY += 360;
        }
        else if (rotacjaY == 360 && rotacjaAktualna >= 360)
        {
            rotacjaY = 0;
        }
        else if (rotacjaY == 360 && rotacjaAktualna < 10)
        {
            rotacjaY = 0;
        }

        if (walka == false)
        {
            Vector3 newRotation = new Vector3(transform.eulerAngles.x, Mathf.Lerp(transform.eulerAngles.y, rotacjaY, 3f * Time.deltaTime), transform.eulerAngles.z);
            transform.eulerAngles = newRotation;
        }

    }
}
