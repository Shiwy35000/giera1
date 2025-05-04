using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class biblioteka : MonoBehaviour
{
    public List<GameObject> wszystkieKarty;
    public List<GameObject> istniej¹ceArtefakty; // gameObject chyba bedzie tu w przysz³oœci?
    public List<efekty> dostêpneEfekty;

    private void Awake()
    {
        PrzypiszId();
    }

    private void PrzypiszId()
    {
        for (int x = 0; x < wszystkieKarty.Count; x++)
        {
            wszystkieKarty[x].GetComponent<taKarta>().Id = x;
        }
        for (int x = 0; x < istniej¹ceArtefakty.Count; x++)
        {
            istniej¹ceArtefakty[x].GetComponent<artefakt>().Id = x;
        }
    }
}
