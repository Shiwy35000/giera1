//using EditorAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor;


public enum rodzajTrescKarta { normalText, obrarzenia, obrarzeniaGracz, obrarzeniaRazy, obrarzeniaGraczRazy };

[System.Serializable]
public class textKartaTyp 
{
    [SerializeField] public rodzajTrescKarta RodzajTrescKarta;

    //[EnableField(nameof(RodzajTrescKarta), rodzajTrescKarta.normalText)]
    public string tre��;


    public textKartaTyp(rodzajTrescKarta RodzajTrescKartaa, string Tre��)
    {
        RodzajTrescKarta = RodzajTrescKartaa;
        tre�� = Tre��;
    }

    /*[CustomPropertyDrawer(typeof(textKartaTyp))]
    public class textKartaTyp_Editor : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            label = EditorGUI.BeginProperty(position, label, property);
            Rect contentPosition = EditorGUI.PrefixLabel(position, label);
            contentPosition.width *= 0.30f;
            EditorGUI.indentLevel = 0;
            EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("RodzajTrescKarta"), GUIContent.none);
            //contentPosition.y += contentPosition.height;
            contentPosition.x += contentPosition.width;
            contentPosition.width *= 3f;
            EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("tre��"), GUIContent.none);

            EditorGUI.EndProperty();
        }
    }*/
}
