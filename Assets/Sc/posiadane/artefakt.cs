using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif

public enum MomentZerowania { brak, koniecTury, poWalce};
public enum TypDzia�ania { licznik, wywo�anie};
public enum Wywo�anie { pocz�tekTury, koniecTury, otrzymanieObrarze�, zadanieObrarze�, pocz�tekWalki, koniecWalki};

public class artefakt : MonoBehaviour
{
    public string nazwa;
    public int Id;
    public Sprite sprite;
    public string opis;
    [HideInInspector] public UnityEvent efekty; //trzeba zewn�trzny skrypt doda� do efekt�w kt�ry ma by� wywo�ywany!!
    [HideInInspector] public int licznik;

    public MomentZerowania momentZerowanial;
    public TypDzia�ania typDzia�ania;
    public Wywo�anie wywo�anie;
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

        if (typDzia�ania == TypDzia�ania.licznik)
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

        if(typDzia�ania == TypDzia�ania.wywo�anie)
        {
            if (wywo�anie == Wywo�anie.pocz�tekTury)
            {
                walkaStart.Pocz�tekTury += Wywo�ajEfektInt;
            }
            else if (wywo�anie == Wywo�anie.koniecTury)
            {
                walkaStart.KoniecTury += Wywo�ajEfektInt;
            }
            else if (wywo�anie == Wywo�anie.otrzymanieObrarze�)
            {
                player.GetComponent<playerEq>().efektyWywo�anieOtrzyma�Cios.AddListener(Wywo�ajEfekt);
            }
            else if (wywo�anie == Wywo�anie.zadanieObrarze�)
            {
                player.GetComponent<playerEq>().efektyWywo�anieZada�Cios.AddListener(Wywo�ajEfekt);
            }
            else if (wywo�anie == Wywo�anie.pocz�tekWalki)
            {
                dialog.Walka += Wywo�ajEfektPocz�tekWalki;
            }
            else if (wywo�anie == Wywo�anie.koniecWalki)
            {
                dialog.Walka += Wywo�ajEfektKoniecWalki;
            }

        }
    }

    private void OnDestroy()
    {
        if(typDzia�ania == TypDzia�ania.licznik)
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

        if (typDzia�ania == TypDzia�ania.wywo�anie)
        {
            if (wywo�anie == Wywo�anie.pocz�tekTury)
            {
                walkaStart.Pocz�tekTury -= Wywo�ajEfektInt;
            }
            else if (wywo�anie == Wywo�anie.koniecTury)
            {
                walkaStart.KoniecTury -= Wywo�ajEfektInt;
            }
            else if (wywo�anie == Wywo�anie.otrzymanieObrarze�)
            {
                player.GetComponent<playerEq>().efektyWywo�anieOtrzyma�Cios.RemoveListener(Wywo�ajEfekt);
            }
            else if (wywo�anie == Wywo�anie.zadanieObrarze�)
            {
                player.GetComponent<playerEq>().efektyWywo�anieZada�Cios.RemoveListener(Wywo�ajEfekt);
            }
            else if (wywo�anie == Wywo�anie.pocz�tekWalki)
            {
                dialog.Walka -= Wywo�ajEfektPocz�tekWalki;
            }
            else if (wywo�anie == Wywo�anie.koniecWalki)
            {
                dialog.Walka -= Wywo�ajEfektKoniecWalki;
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
    private void Wywo�ajEfektPocz�tekWalki(bool b)
    {
        if (b)
        {
            efekty.Invoke();
        }
    }
    private void Wywo�ajEfektKoniecWalki(bool b)
    {
        if (b == false)
        {
            efekty.Invoke();
        }
    }
    private void Wywo�ajEfekt()
    {
        efekty.Invoke();
    }
    private void Wywo�ajEfektInt(int nic)
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
            script.typDzia�ania = (TypDzia�ania)EditorGUILayout.EnumPopup(label: "Typ Dzia�ania", script.typDzia�ania);

            if(script.typDzia�ania == TypDzia�ania.licznik)
            {
                script.licznikMax = EditorGUILayout.IntField(label: "Licznik Max", script.licznikMax);
                script.momentZerowanial = (MomentZerowania)EditorGUILayout.EnumPopup(label: "Moment Zerowania", script.momentZerowanial);
            }
            else if (script.typDzia�ania == TypDzia�ania.wywo�anie)
            {
                script.wywo�anie = (Wywo�anie)EditorGUILayout.EnumPopup(label: "Wywo�anie", script.wywo�anie);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}
