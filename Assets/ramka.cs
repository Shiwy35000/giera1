using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ramka : MonoBehaviour
{
    private Transform LD, PD, LG, PG; //lewyDu³ (rogi ramki itd)
    public float wymiarX, wymiarY;

    void Start()
    {
        TworzenieRamki();
        Pozycjonowanie();
    }

    void Update()
    {
        //Pozycjonowanie(); // WY£¥CZ GDY NIE DOBIERASZ WYMIARÓW!!
    }

    private void TworzenieRamki()
    {
        GameObject ld = this.gameObject.transform.GetChild(0).gameObject;
        LD = ld.transform;
        ld.GetComponent<SpriteRenderer>().flipY = true;
        GameObject pd = this.gameObject.transform.GetChild(1).gameObject;
        PD = pd.transform;
        pd.GetComponent<SpriteRenderer>().flipX = true;
        pd.GetComponent<SpriteRenderer>().flipY = true;
        GameObject lg = this.gameObject.transform.GetChild(2).gameObject;
        LG = lg.transform;
        GameObject pg = this.gameObject.transform.GetChild(3).gameObject;
        PG = pg.transform;
        pg.GetComponent<SpriteRenderer>().flipX = true;
    }

    private void Pozycjonowanie()
    {
        LD.localPosition = new Vector3(-wymiarX, -wymiarY, 0);
        PD.localPosition = new Vector3(wymiarX, -wymiarY, 0);
        LG.localPosition = new Vector3(-wymiarX, wymiarY, 0);
        PG.localPosition = new Vector3(wymiarX, wymiarY, 0);
    }
}
