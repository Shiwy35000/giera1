using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class biblioteka : MonoBehaviour
{
    public List<ObjectType> wszystkieKarty;
    public List<ObjectType> istniej¹ceArtefakty; // gameObject chyba bedzie tu w przysz³oœci?
    public List<efekty> dostêpneEfekty;

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
        for (int x = 0; x < istniej¹ceArtefakty.Count; x++)
        {
            istniej¹ceArtefakty[x].Obj.GetComponent<artefakt>().Id = x;
        }
    }

    public void OdblokowanieZawartoœci(GameObject kartaLubArtefakt)
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
        else if(istniej¹ceArtefakty.Any(a => a.Obj == kartaLubArtefakt))
        {
            for (int x = 0; x < istniej¹ceArtefakty.Count; x++)
            {
                if (istniej¹ceArtefakty[x].Obj == kartaLubArtefakt && istniej¹ceArtefakty[x].Odblokowane == false)
                {
                    istniej¹ceArtefakty[x].Odblokowane = true;
                }
            }
        }
    }
}
