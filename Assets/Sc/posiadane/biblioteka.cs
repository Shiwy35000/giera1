using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class biblioteka : MonoBehaviour
{
    public List<GameObject> wszystkieKarty;
    public List<GameObject> istniej�ceArtefakty; // gameObject chyba bedzie tu w przysz�o�ci?
    public List<efekty> dost�pneEfekty;

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
        for (int x = 0; x < istniej�ceArtefakty.Count; x++)
        {
            istniej�ceArtefakty[x].GetComponent<artefakt>().Id = x;
        }
    }
}
