using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif

public enum MomentZerowania { brak, koniecTury, poWalce};
public enum TypDzia³ania { licznik, wywo³anie};
public enum Wywo³anie { pocz¹tekTury, koniecTury, otrzymanieObrarzeñ, zadanieObrarzeñ, pocz¹tekWalki, koniecWalki};

public class artefakt : MonoBehaviour
{
    public string nazwa;
    public int Id;
    public Sprite sprite;
    public string opis;
    [HideInInspector] public UnityEvent efekty; //trzeba zewnêtrzny skrypt dodaæ do efektów który ma byæ wywo³ywany!!
    [HideInInspector] public int licznik;

    public MomentZerowania momentZerowanial;
    public TypDzia³ania typDzia³ania;
    public Wywo³anie wywo³anie;
    public int licznikMax;
    private GameObject player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").gameObject;

        Przypisy();
    }

    void Przypisy()
    {

        if (sprite != null)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
        }

        if (typDzia³ania == TypDzia³ania.licznik)
        {
            this.gameObject.transform.GetChild(0).gameObject.SetActive(true);
            this.gameObject.transform.GetChild(0).GetComponent<TextMeshPro>().text = licznik.ToString();

            if (momentZerowanial == MomentZerowania.koniecTury)
            {
                walkaStart.KoniecTury += ZerowanieKoniecTury;
            }
            else if (momentZerowanial == MomentZerowania.poWalce)
            {
                dialog.Walka += ZerowanieKoniecWalki;
            }
        }
        else
        {
            this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        }

        if(typDzia³ania == TypDzia³ania.wywo³anie)
        {
            if (wywo³anie == Wywo³anie.pocz¹tekTury)
            {
                walkaStart.Pocz¹tekTury += Wywo³ajEfektInt;
            }
            else if (wywo³anie == Wywo³anie.koniecTury)
            {
                walkaStart.KoniecTury += Wywo³ajEfektInt;
            }
            else if (wywo³anie == Wywo³anie.otrzymanieObrarzeñ)
            {
                player.GetComponent<playerEq>().efektyWywo³anieOtrzyma³Cios.AddListener(Wywo³ajEfekt);
            }
            else if (wywo³anie == Wywo³anie.zadanieObrarzeñ)
            {
                player.GetComponent<playerEq>().efektyWywo³anieZada³Cios.AddListener(Wywo³ajEfekt);
            }
            else if (wywo³anie == Wywo³anie.pocz¹tekWalki)
            {
                dialog.Walka += Wywo³ajEfektPocz¹tekWalki;
            }
            else if (wywo³anie == Wywo³anie.koniecWalki)
            {
                dialog.Walka += Wywo³ajEfektKoniecWalki;
            }

        }
    }

    private void OnDestroy()
    {
        if(typDzia³ania == TypDzia³ania.licznik)
        {
            if (momentZerowanial == MomentZerowania.koniecTury)
            {
                walkaStart.KoniecTury -= ZerowanieKoniecTury;
            }
            else if (momentZerowanial == MomentZerowania.poWalce)
            {
                dialog.Walka -= ZerowanieKoniecWalki;
            }
        }

        if (typDzia³ania == TypDzia³ania.wywo³anie)
        {
            if (wywo³anie == Wywo³anie.pocz¹tekTury)
            {
                walkaStart.Pocz¹tekTury -= Wywo³ajEfektInt;
            }
            else if (wywo³anie == Wywo³anie.koniecTury)
            {
                walkaStart.KoniecTury -= Wywo³ajEfektInt;
            }
            else if (wywo³anie == Wywo³anie.otrzymanieObrarzeñ)
            {
                player.GetComponent<playerEq>().efektyWywo³anieOtrzyma³Cios.RemoveListener(Wywo³ajEfekt);
            }
            else if (wywo³anie == Wywo³anie.zadanieObrarzeñ)
            {
                player.GetComponent<playerEq>().efektyWywo³anieZada³Cios.RemoveListener(Wywo³ajEfekt);
            }
            else if (wywo³anie == Wywo³anie.pocz¹tekWalki)
            {
                dialog.Walka -= Wywo³ajEfektPocz¹tekWalki;
            }
            else if (wywo³anie == Wywo³anie.koniecWalki)
            {
                dialog.Walka -= Wywo³ajEfektKoniecWalki;
            }
        }
    }

    private void ZerowanieKoniecTury(int nic)
    {
        licznik = 0;
    }
    private void ZerowanieKoniecWalki(bool b)
    {
        if (b == false)
        {
            licznik = 0;
        }
    }
    private void Wywo³ajEfektPocz¹tekWalki(bool b)
    {
        if (b)
        {
            efekty.Invoke();
        }
    }
    private void Wywo³ajEfektKoniecWalki(bool b)
    {
        if (b == false)
        {
            efekty.Invoke();
        }
    }
    private void Wywo³ajEfekt()
    {
        efekty.Invoke();
    }
    private void Wywo³ajEfektInt(int nic)
    {
        efekty.Invoke();
    }

    public void EfektLicznikPlus()
    {
        licznik += 1;
        this.gameObject.transform.GetChild(0).GetComponent<TextMeshPro>().text = licznik.ToString();

        if (licznik == licznikMax)
        {
            licznik = 0;
            efekty.Invoke();
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(artefakt))]
    public class artefakt_Editor : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            var script = (artefakt)target;

            //DEBUGOWANIE?!?!??
            if (GUILayout.Button("Zapisz Zmiany Artefakt"))
            {
                artefakt noise = ((artefakt)target);
                EditorUtility.SetDirty(noise);
            }
            EditorGUILayout.LabelField(" ");

            //zawszeWidoczne
            script.nazwa = EditorGUILayout.TextField(label: "Nazwa", script.nazwa);
            script.Id = EditorGUILayout.IntField(label: "ID w bibliotece", script.Id);
            script.sprite = (Sprite)EditorGUILayout.ObjectField(label: "Sprite", script.sprite, typeof(Sprite), true);
            script.opis = EditorGUILayout.TextField(label: "Opis", script.opis);
            script.typDzia³ania = (TypDzia³ania)EditorGUILayout.EnumPopup(label: "Typ Dzia³ania", script.typDzia³ania);

            if(script.typDzia³ania == TypDzia³ania.licznik)
            {
                script.licznikMax = EditorGUILayout.IntField(label: "Licznik Max", script.licznikMax);
                script.momentZerowanial = (MomentZerowania)EditorGUILayout.EnumPopup(label: "Moment Zerowania", script.momentZerowanial);
            }
            else if (script.typDzia³ania == TypDzia³ania.wywo³anie)
            {
                script.wywo³anie = (Wywo³anie)EditorGUILayout.EnumPopup(label: "Wywo³anie", script.wywo³anie);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}
