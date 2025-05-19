using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class biblioteka : MonoBehaviour
{
    public List<ObjectType> wszystkieKarty;
    public List<ObjectType> istniej�ceArtefakty; // gameObject chyba bedzie tu w przysz�o�ci?
    public List<efekty> dost�pneEfekty;

    private void Awake()
    {
        PrzypiszId();
    }

    private void PrzypiszId()
    {
        for (int x = 0; x < wszystkieKarty.Count; x++)
        {
            wszystkieKarty[x].Obj.GetComponent<taKarta>().Id = x;
        }
        for (int x = 0; x < istniej�ceArtefakty.Count; x++)
        {
            istniej�ceArtefakty[x].Obj.GetComponent<artefakt>().Id = x;
        }
    }

    public void OdblokowanieZawarto�ci(GameObject kartaLubArtefakt)
    {
        if (wszystkieKarty.Any(a => a.Obj == kartaLubArtefakt))
        {
            for (int x = 0; x < wszystkieKarty.Count; x++)
            {
                if(wszystkieKarty[x].Obj == kartaLubArtefakt && wszystkieKarty[x].Odblokowane == false)
                {
                    wszystkieKarty[x].Odblokowane = true;
                }
            }
        }
        else if(istniej�ceArtefakty.Any(a => a.Obj == kartaLubArtefakt))
        {
            for (int x = 0; x < istniej�ceArtefakty.Count; x++)
            {
                if (istniej�ceArtefakty[x].Obj == kartaLubArtefakt && istniej�ceArtefakty[x].Odblokowane == false)
                {
                    istniej�ceArtefakty[x].Odblokowane = true;
                }
            }
        }
    }
}
