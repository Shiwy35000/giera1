using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class biblioteka : MonoBehaviour
{
    public List<ObjectType> wszystkieKarty;
    public List<ObjectType> istniejąceArtefakty; // gameObject chyba bedzie tu w przyszłości?
    public List<efekty> dostępneEfekty;

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
        for (int x = 0; x < istniejąceArtefakty.Count; x++)
        {
            istniejąceArtefakty[x].Obj.GetComponent<artefakt>().Id = x;
        }
    }

    public void OdblokowanieZawartości(GameObject kartaLubArtefakt)
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
        else if(istniejąceArtefakty.Any(a => a.Obj == kartaLubArtefakt))
        {
            for (int x = 0; x < istniejąceArtefakty.Count; x++)
            {
                if (istniejąceArtefakty[x].Obj == kartaLubArtefakt && istniejąceArtefakty[x].Odblokowane == false)
                {
                    istniejąceArtefakty[x].Odblokowane = true;
                }
            }
        }
    }
}
