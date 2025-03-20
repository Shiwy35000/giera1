using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OpcjeDialogowe : MonoBehaviour
{
    public GameObject opleWyboruw;
    [HideInInspector] public GameObject wizualizacjaWyboru;
    private List<string> listaOdpowiedzi = new List<string>();
    public GameObject prefabOpcjiDialogowej, niedostêpnyDialogPrefab;
    [HideInInspector] public List<Button> clickButtons = new List<Button>();
    [HideInInspector] public GameObject zKimPrzyjemnoscRozmawiac;
    void Awake()
    {
        wizualizacjaWyboru = opleWyboruw.transform.GetChild(0).gameObject;
        wizualizacjaWyboru.SetActive(false);
    }

    public void WizualizujOdpowiedzi(List<odpowiedz> odpowiedzi)
    {
        wizualizacjaWyboru.SetActive(true);
        for (int x = 0; x < odpowiedzi.Count; x++)
        {
            listaOdpowiedzi.Add(odpowiedzi[x].odp);
        }

        for(int x = 0; x < listaOdpowiedzi.Count; x++)
        {   
            GameObject nowaOpcja;
            if (odpowiedzi[x].czyAktywna == true)
            {
                nowaOpcja = Instantiate(prefabOpcjiDialogowej, wizualizacjaWyboru.transform);
                Button nBT = nowaOpcja.GetComponent<Button>();
                clickButtons.Add(nBT);
                int n = x;
                nBT.onClick.AddListener(() => Click(n));
            }
            else
            {
                nowaOpcja = Instantiate(niedostêpnyDialogPrefab, wizualizacjaWyboru.transform);
            }
            nowaOpcja.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = listaOdpowiedzi[x];
        }
    }

    public void Click(int x)
    {
        zKimPrzyjemnoscRozmawiac.GetComponent<dialog>().OpdowiedziNaPytanie(x);
    }

    public void UsunOpcjeDialogowe()
    {
        foreach (Transform child in wizualizacjaWyboru.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        listaOdpowiedzi = new List<string>();
        clickButtons = new List<Button>();
        zKimPrzyjemnoscRozmawiac = null;
    }
}
