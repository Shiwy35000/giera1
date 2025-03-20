using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "nowyZapis", menuName = "zapis")]
public class zapisSObj : ScriptableObject
{
    [Header("Gracz")]
    public Vector3 pozycja;
    public Vector3 rotacja;
    public int hp;
    public int sakiewka;
    public float rzar;

    [Header("Eq")]
    public List<GameObject> deck;
    public List<artefakt> posiadaneArtefakty;

    [Header("Eq")]
    public List<nowyDialogTyp> dialogiWybory;
}
