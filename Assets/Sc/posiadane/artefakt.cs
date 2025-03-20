using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "nowyArtefakt", menuName = "artefakt")]
public class artefakt : ScriptableObject
{
    public string nazwa;
    public int Id;
    public Sprite sprite;
    public string opis;
    [SerializeField] private UnityEvent efekty;
    public int licznikDodatkowy;
}
