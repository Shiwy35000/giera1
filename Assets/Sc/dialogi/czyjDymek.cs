using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class czyjDymek : MonoBehaviour
{
    public GameObject wlasciciel;
    [SerializeField] public Vector3 offset;
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    public void WstawText(string tresc)
    {
        this.transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = tresc;
    }

    public void KoniecDialogu()
    {
        Destroy(this.gameObject);
    }

    void Update()
    {
        Vector3 pos = wlasciciel.transform.position + offset;
        if (transform.position != pos)
        {
            transform.position = wlasciciel.transform.position + offset;
        }

        if (wlasciciel == null)
        {
            Destroy(this.gameObject);
        }

        Vector3 newRotation = wlasciciel.transform.eulerAngles;
        this.gameObject.GetComponent<RectTransform>().eulerAngles = newRotation;
    }
}
