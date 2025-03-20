using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "nowaAkcjaWroga", menuName = "akcjaWroga")]
public class wrugAkcja : ScriptableObject
{
    public int indeksAkcji;
    public int nazwa;
    [SerializeField] private UnityEvent dzia³anie;
}
