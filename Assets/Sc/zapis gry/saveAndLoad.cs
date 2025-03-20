using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class saveAndLoad : MonoBehaviour
{
    [HideInInspector] public zapisSObj zapis; //nie usuwamy rzyda siê do menu?
    //[HideInInspector] public List<GameObject> przeciwnicyWwalce = new List<GameObject>();

    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("saveGame");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
        NowaGra(); // finalnie ma byæ inaczej to tylko tu ze wzglêdu na to rze reszty nie ma!!
        //Wczytaj();
    }

    public void NowaGra() //resetuje ca³y zapis
    {
        zapis.pozycja = new Vector3();
        zapis.rotacja = new Vector3();
        zapis.hp = 0;
        zapis.sakiewka = 0;
        zapis.rzar = 0f;
        zapis.deck = new List<GameObject>();
        zapis.posiadaneArtefakty = new List<artefakt>();
        zapis.dialogiWybory = new List<nowyDialogTyp>();
    }

    /*public void Wczytaj()
    {
        if(SceneManager.GetActiveScene().name == "teren")
        {
            GameObject gracz = GameObject.FindGameObjectWithTag("Player").gameObject;
            gracz.transform.position = zapis.pozycja;
            GameObject cameraRot = GameObject.FindGameObjectWithTag("MainCamera").gameObject.transform.parent.gameObject;
            cameraRot.transform.eulerAngles = zapis.rotacja;
            //przeciwnicyWwalce = new List<GameObject>();
            //i reszta jak bedzie!!
        }
    }*/
}
