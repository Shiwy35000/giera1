using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Linq;

public enum Cele { Gracz, Wrug, Wrogowie, Karta, KartyWD�oni,RandomKartaWD�oni , All, RandomWrug, Random, AlboWrugAlboGracz };
public enum CeleNieZagranej { Gracz, Wrogowie, TaKarta, KartyWD�oni, RandomKartaWD�oni, All, RandomWrug , Random };
public enum PoUrzyciu { Zniszcz, Zachowaj, wyklucz, cmentarz};
public enum Grywalno�� { Grywalna, Zablokowana, NieGrywalna };
[System.Flags]
public enum TypObrarzen : int { brak = 0x00, nieSkalowalne = 0x01, nieUchronne = 0x02 };
[System.Flags]
public enum kartaTag : int { normal = 0x00, atak = 0x01, czar = 0x02, test3 = 0x04}; //testowo bedzie si� to odnosi�a np do eket�w innych kart; (np. karta wzmacnia karty typu atak);
public enum rzadko�� { pospolita, scecjalna, unikalna, rzadka}; //bedzie potrzebe do systemu drop�w!!

public class taKarta : MonoBehaviour
{
    //przypisty
    private GameObject grafika, opis, koszt, ramka, nazwa; 
    private Vector3 pozEnd, pozEndRamka;
    private biblioteka Biblioteka;
    [HideInInspector] public GameObject podgl�dOpis;
    [HideInInspector] public GameObject dlon;
    [HideInInspector] public playerEq Eq;
    private efekty Efektu;
    [HideInInspector] public GameObject prefabTejKartyWdeck; //tylko potrzebne do usuwania jednorazuwek?

    [Header("Dane Karty")]
    public string Nazwa;
    public int Koszt;
    //public int Priorytet;
    public Sprite GrafikaKarty;
    public List<textKartaTyp> Opis;
    public List<textKartaTyp> SkruconyOpis;
    public kartaTag KartaTag;
    public rzadko�� Rzadko��;

    [Header("Dzia�anie")]
    public Grywalno�� grywalno��;
    public Cele cele;
    public PoUrzyciu poUrzyciu;
    public PoUrzyciu naKoniecTury;
    [HideInInspector] public UnityEvent<List<GameObject>> akcje;
    public float DmgGraczowi;
    public TypObrarzen specjalnyTypObrarze�Gracz;
    [Range(1, 20)]
    public int DmgGraczRazy = 1;
    [Range(1, 20)]
    public int RandomRazy = 1;
    public TypObrarzen specjalnyTypObrarze�;
    public float Dmg;
    [Range(1, 20)]
    public int DmgRazy = 1;
    public List<nalurzEfekt> efektyGracz;
    public List<nalurzEfekt> efektyWrug;
    public List<nalurzEfekt> efektyNaKarty;

    [Header("Dzia�ania na koniec tury")] //DOPISEK "T"
    public bool Dzia�anieNaKoniecTury = false;
    public CeleNieZagranej celeNieZagranej;
    [HideInInspector] public UnityEvent<List<GameObject>> akcjeKoniecTury;
    public float DmgGraczowiT;
    public TypObrarzen specjalnyTypObrarze�GraczT;
    [Range(1, 20)]
    public int DmgGraczRazyT = 1;
    [Range(1, 20)]
    public int RandomRazyT = 1;
    public TypObrarzen specjalnyTypObrarze�T;
    public float DmgT;
    [Range(1, 20)]
    public int DmgRazyT = 1;
    public List<nalurzEfekt> efektyGraczT;
    public List<nalurzEfekt> efektyWrugT;
    public List<nalurzEfekt> efektyNaKartyT;

    //pozosta�e
    private string finalnyOpis;
    [HideInInspector] public GameObject fizycznyDeck;

    void Awake()
    { 
        //WIZUALIA PRZYPISZ
        ramka = this.gameObject.transform.GetChild(0).gameObject;
        grafika = ramka.transform.GetChild(0).gameObject;
        koszt = ramka.transform.GetChild(1).gameObject;
        opis = ramka.transform.GetChild(2).gameObject;
        nazwa = ramka.transform.GetChild(3).gameObject;
        podgl�dOpis = this.gameObject.transform.GetChild(1).gameObject;
        podgl�dOpis.SetActive(false);
       
        Eq = GameObject.FindGameObjectWithTag("Player").GetComponent<playerEq>();
        Biblioteka = GameObject.FindGameObjectWithTag("saveGame").GetComponent<biblioteka>();
        fizycznyDeck = GameObject.FindGameObjectWithTag("fizycznyDeck").gameObject;

        Uzupelnij();
        PodpinajAkcje();
    }
   
    void Update()
    {
        if (transform.localPosition != pozEnd)
        {
            Move();
        }

        if (ramka.transform.localPosition != pozEndRamka)
        {
            MoveRamka();
        }
    }

    private void PodpinajAkcje()
    {
        akcje = new UnityEvent<List<GameObject>>();

        if (cele == Cele.Gracz)
        {
            if (DmgGraczowi != 0)
            {
                akcje.AddListener(DmgDiler);
            }
            if (efektyGracz.Count > 0)
            {
                akcje.AddListener(Na�urzEfekty);
            }
        }
        else if (cele == Cele.Wrug || cele == Cele.Wrogowie)
        {
            if (Dmg != 0)
            {
                akcje.AddListener(DmgDiler);
            }
            if (efektyWrug.Count > 0)
            {
                akcje.AddListener(Na�urzEfekty);
            }
        }
        else if (cele == Cele.RandomWrug)
        {
            if (Dmg != 0 || efektyWrug.Count > 0)
            {
                akcje.AddListener(DmgDiler);
            }
        }
        else if (cele == Cele.Random)
        {
            if (Dmg != 0 && DmgGraczowi != 0)
            {
                akcje.AddListener(DmgDiler);
            }
            else if (efektyWrug.Count > 0 && efektyGracz.Count > 0)
            {
                akcje.AddListener(DmgDiler);
            }
            else if (Dmg != 0 && efektyGracz.Count > 0)
            {
                akcje.AddListener(DmgDiler);
            }
            else if (DmgGraczowi != 0 && efektyWrug.Count > 0)
            {
                akcje.AddListener(DmgDiler);
            }
        }
        else if (cele == Cele.All)
        {
            if (DmgGraczowi != 0 || Dmg != 0)
            {
                akcje.AddListener(DmgDiler);
            }
            if (efektyGracz.Count > 0 || efektyWrug.Count > 0)
            {
                akcje.AddListener(Na�urzEfekty);
            }
        }
        else if (cele == Cele.Karta || cele == Cele.KartyWD�oni || cele == Cele.RandomKartaWD�oni)
        {
            if (efektyNaKarty.Count > 0)
            {
                akcje.AddListener(Na�urzEfekty);
            }
        }
        else if(cele == Cele.AlboWrugAlboGracz)
        {
            if (Dmg != 0 && DmgGraczowi != 0)
            {
                akcje.AddListener(DmgDiler);
            }
            else if (efektyWrug.Count > 0 && efektyGracz.Count > 0)
            {
                akcje.AddListener(Na�urzEfekty);
            }
            else if (Dmg != 0 && efektyGracz.Count > 0)
            {
                akcje.AddListener(DmgDiler);
                akcje.AddListener(Na�urzEfekty);
            }
            else if (DmgGraczowi != 0 && efektyWrug.Count > 0)
            {
                akcje.AddListener(DmgDiler);
                akcje.AddListener(Na�urzEfekty);
            }
        }

        if (Dzia�anieNaKoniecTury == true) //NO BEDZIE TROCH� UZUPE�NIANIA
        {
            if (celeNieZagranej == CeleNieZagranej.Gracz)
            {
                if (DmgGraczowiT != 0)
                {
                    akcjeKoniecTury.AddListener(DmgDilerT);
                }
                if (efektyGraczT.Count > 0)
                {
                    akcjeKoniecTury.AddListener(Na�urzEfektyT);
                }
            }
            else if (celeNieZagranej == CeleNieZagranej.Wrogowie)
            {
                if (DmgT != 0)
                {
                    akcjeKoniecTury.AddListener(DmgDilerT);
                }
                if (efektyWrugT.Count > 0)
                {
                    akcjeKoniecTury.AddListener(Na�urzEfektyT);
                }
            }
            else if (celeNieZagranej == CeleNieZagranej.TaKarta || celeNieZagranej == CeleNieZagranej.KartyWD�oni || celeNieZagranej == CeleNieZagranej.RandomKartaWD�oni)
            {
                if (efektyNaKartyT.Count > 0)
                {
                   akcjeKoniecTury.AddListener(Na�urzEfektyT);
                }
            }
            else if (celeNieZagranej == CeleNieZagranej.All)
            {
                if (DmgGraczowiT != 0 || DmgT != 0)
                {
                    akcjeKoniecTury.AddListener(DmgDilerT);
                }
                if (efektyGraczT.Count > 0 || efektyWrugT.Count > 0)
                {
                    akcjeKoniecTury.AddListener(Na�urzEfektyT);
                }
            }
            else if (celeNieZagranej == CeleNieZagranej.RandomWrug)
            {
                if (DmgT != 0 || efektyWrugT.Count > 0)
                {
                    akcjeKoniecTury.AddListener(DmgDilerT);
                }
            }
            else if (celeNieZagranej == CeleNieZagranej.Random)
            {
                if (DmgT != 0 && DmgGraczowiT != 0)
                {
                    akcjeKoniecTury.AddListener(DmgDilerT);
                }
                else if (efektyWrugT.Count > 0 && efektyGraczT.Count > 0)
                {
                    akcjeKoniecTury.AddListener(DmgDilerT);
                }
                else if (DmgT != 0 && efektyGraczT.Count > 0)
                {
                    akcjeKoniecTury.AddListener(DmgDilerT);
                }
                else if (DmgGraczowiT != 0 && efektyWrugT.Count > 0)
                {
                    akcjeKoniecTury.AddListener(DmgDilerT);
                }
            }
        }



        if (poUrzyciu == PoUrzyciu.Zniszcz)
        {
            akcje.AddListener(Usu�TeKarte);
        }
        else if(poUrzyciu == PoUrzyciu.Zachowaj)
        {
            akcje.AddListener(ZachowajTeKarte);
        }
        else if(poUrzyciu == PoUrzyciu.cmentarz)
        {
            akcje.AddListener(NaCmentarzTaKarta);
        }
        else if(poUrzyciu == PoUrzyciu.wyklucz)
        {
            akcje.AddListener(Usu�TeKarte);
        }
    }

    private void ZachowajTeKarte(List<GameObject> nieIstotne)
    {
        click c  = GameObject.FindGameObjectWithTag("nadUiWalka").gameObject.GetComponent<click>();
        c.GrabCardOf();
        c.Czy��CardMorInfo();
        c.Czy��Wskazana();
    }
    public void Usu�TeKarte(List<GameObject> nieIstotne)
    {
        if (prefabTejKartyWdeck != null)
        {
            Eq.deckPrefab.Remove(prefabTejKartyWdeck);
        }
        dlon.GetComponent<sortGrupZ>().UsunKarteZdloni(this.gameObject);
    }
    private void NaCmentarzTaKarta(List<GameObject> nieIstotne)
    {
        GameObject klon = GameObject.Instantiate(this.gameObject, fizycznyDeck.transform);
        klon.name = this.gameObject.name;
        Eq.cmentarz.Add(klon);
        dlon.GetComponent<sortGrupZ>().UsunKarteZdloni(this.gameObject);
    }
    private void WykluczTeKarte(List<GameObject> nieIstotne)
    {
        GameObject klon = GameObject.Instantiate(this.gameObject, fizycznyDeck.transform);
        klon.name = this.gameObject.name;
        Eq.wykluczone.Add(klon);
        dlon.GetComponent<sortGrupZ>().UsunKarteZdloni(this.gameObject);
    }

    private void Uzupelnij()
    {
        grafika.GetComponent<SpriteRenderer>().sprite = GrafikaKarty;
        koszt.GetComponent<TextMeshPro>().text = Koszt.ToString();
        nazwa.GetComponent<TextMeshPro>().text = Nazwa;
        Uzupe�nijOpis(SkruconyOpis);
        Uzupe�nijOpis(Opis);
    }

    private void Uzupe�nijOpis(List<textKartaTyp> tx) //AKTUALIZUJ PRZY ZMIANEI STATYSTYK!!!(bedzie podpi�te pewnie pod event!!)
    {
        finalnyOpis = null;

        if (tx.Count > 0)
        {
            for (int x = 0; x < tx.Count; x++)
            {
                if (tx[x].RodzajTrescKarta == rodzajTrescKarta.normalText)
                {
                    finalnyOpis += tx[x].tre�� + " ";
                }
                else if (tx[x].RodzajTrescKarta == rodzajTrescKarta.obrarzenia)
                {
                    if (specjalnyTypObrarze�.HasFlag(TypObrarzen.nieSkalowalne))
                    {
                        finalnyOpis += Dmg.ToString() + " ";
                    }
                    else
                    {
                        float suma = Dmg + Eq.bonusDoObrarze�;
                        finalnyOpis += suma.ToString() + " ";
                    }
                }
                else if (tx[x].RodzajTrescKarta == rodzajTrescKarta.obrarzeniaGracz)
                {
                    if (specjalnyTypObrarze�.HasFlag(TypObrarzen.nieSkalowalne))
                    {
                        finalnyOpis += DmgGraczowi.ToString() + " ";
                    }
                    else
                    {
                        float suma = DmgGraczowi + Eq.bonusDoObrarze�;
                        finalnyOpis += suma.ToString() + " ";
                    }
                }
            }
        }

        if (tx == SkruconyOpis)
        {
            opis.GetComponent<TextMeshPro>().text = finalnyOpis;
        }
        else if (tx == Opis)
        {
            podgl�dOpis.gameObject.transform.GetChild(1).gameObject.GetComponent<TextMeshPro>().text = finalnyOpis;
        }
    }

    private void Move()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, pozEnd, 50f * Time.deltaTime);
    }
    private void MoveRamka()
    {
        ramka.transform.localPosition = Vector3.Lerp(ramka.transform.localPosition, pozEndRamka, 50f * Time.deltaTime);
    }

    public void PodniesionaPoz(bool czyPodniesiona)
    {
        if (czyPodniesiona)
        {
            pozEnd = new Vector3(transform.localPosition.x, transform.localPosition.y + 2, transform.localPosition.z - 6f);
        }
        else
        {
            pozEnd = Vector3.zero;
        }
    }

    public void Wskazano(bool naTo)
    {
        if (naTo)
        {
            pozEndRamka = new Vector3(ramka.transform.localPosition.x, ramka.transform.localPosition.y + 0.5f, transform.localPosition.z - 5f);
        }
        else
        {
            pozEndRamka = Vector3.zero;
        }
    }

    //////////////////////////!!!!AKCJE!!!!///////////////////////////////

    public void DmgDiler(List<GameObject> celee)
    {
        if (cele == Cele.Random || cele == Cele.RandomWrug)
        {
            for (int y = 0; y < RandomRazy; y++)
            {
                int z = Random.Range(0, celee.Count);
                if (celee[z].tag == "Player")
                {
                    PodajObrarzenia(celee[z]);
                    if (efektyGracz.Count > 0)
                    {
                        for (int c = 0; c < efektyGracz.Count; c++)
                        {
                            Na�urzEfekt(Eq.gameObject, efektyGracz[c]);
                        }
                    }
                }
                else if (celee[z].tag == "wrug")
                {
                    PodajObrarzenia(celee[z]);
                    if (efektyWrug.Count > 0)
                    {
                        for (int c = 0; c < efektyWrug.Count; c++)
                        {
                            Na�urzEfekt(celee[z], efektyWrug[c]);
                        }
                    }
                }
            }
        }
        else
        {
            for (int x = 0; x < celee.Count; x++)
            {
                if (celee[x].tag == "Player")
                {
                    for (int y = 0; y < DmgGraczRazy; y++)
                    {
                        PodajObrarzenia(celee[x]);
                    }
                }
                else if (celee[x].tag == "wrug")
                {
                    for (int y = 0; y < DmgRazy; y++)
                    {
                        PodajObrarzenia(celee[x]);
                    }
                }
            }
        }
    }

    public void DmgDilerT(List<GameObject> celee)
    {
        if (celeNieZagranej == CeleNieZagranej.Random || celeNieZagranej == CeleNieZagranej.RandomWrug)
        {
            for (int y = 0; y < RandomRazyT; y++)
            {
                int z = Random.Range(0, celee.Count);
                if (celee[z].tag == "Player")
                {
                    PodajObrarzeniaT(celee[z]);
                    if (efektyGraczT.Count > 0)
                    {
                        for (int c = 0; c < efektyGraczT.Count; c++)
                        {
                            Na�urzEfekt(Eq.gameObject, efektyGraczT[c]);
                        }
                    }
                }
                else if (celee[z].tag == "wrug")
                {
                    PodajObrarzeniaT(celee[z]);
                    if (efektyWrugT.Count > 0)
                    {
                        for (int c = 0; c < efektyWrugT.Count; c++)
                        {
                            Na�urzEfekt(celee[z], efektyWrugT[c]);
                        }
                    }
                }
            }
        }
        else
        {
            for (int x = 0; x < celee.Count; x++)
            {
                if (celee[x].tag == "Player")
                {
                    for (int y = 0; y < DmgGraczRazyT; y++)
                    {
                        PodajObrarzeniaT(celee[x]);
                    }
                }
                else if (celee[x].tag == "wrug")
                {
                    for (int y = 0; y < DmgRazyT; y++)
                    {
                        PodajObrarzeniaT(celee[x]);
                    }
                }
            }
        }
    }

    public void Na�urzEfekty(List<GameObject> cele)
    {
        for (int x = 0; x < cele.Count; x++)
        {
            if (cele[x].tag == "wrug")
            {
                for (int y = 0; y < efektyWrug.Count; y++)
                {
                    Na�urzEfekt(cele[x], efektyWrug[y]);
                }
            }
            else if (cele[x].tag == "Player")
            {
                for (int y = 0; y < efektyGracz.Count; y++)
                {
                    Na�urzEfekt(Eq.gameObject, efektyGracz[y]);
                }
            }
            else if (cele[x].tag == "karta")
            {
                for (int y = 0; y < efektyNaKarty.Count; y++)
                {
                    Na�urzEfekt(cele[x], efektyNaKarty[y]);
                }
            }

        }
    }

    public void Na�urzEfektyT(List<GameObject> cele)
    {
        for (int x = 0; x < cele.Count; x++)
        {
            if (cele[x].tag == "wrug")
            {
                for (int y = 0; y < efektyWrugT.Count; y++)
                {
                    Na�urzEfekt(cele[x], efektyWrugT[y]);
                }
            }
            else if (cele[x].tag == "Player")
            {
                for (int y = 0; y < efektyGraczT.Count; y++)
                {
                    Na�urzEfekt(Eq.gameObject, efektyGraczT[y]);
                }
            }
            else if (cele[x].tag == "karta")
            {
                for (int y = 0; y < efektyNaKartyT.Count; y++)
                {
                    Na�urzEfekt(cele[x], efektyNaKartyT[y]);
                }
            }
        }
    }

    //////////////////////////////////////////NARZ�DZIA//////////////////////////////////////////////
    private void Na�urzEfekt(GameObject Cel, nalurzEfekt Efektuuu)
    {
        for (int x = 0; x < Biblioteka.dost�pneEfekty.Count; x++)
        {
            if (Biblioteka.dost�pneEfekty[x].nazwa == Efektuuu.NazwaEfektu.ToString())
            {
                Efektu = new efekty(Biblioteka.dost�pneEfekty[x].nazwa, Biblioteka.dost�pneEfekty[x].odbiurEfektu, Biblioteka.dost�pneEfekty[x].sprite, Biblioteka.dost�pneEfekty[x].opis, Biblioteka.dost�pneEfekty[x].TypWywo�ania, Efektuuu.ile);
            }
        }

        if (Efektu.TypWywo�ania != typWywo�ania.natychmiastowy)
        {
            if (Cel.tag == "wrug")
            {
                if (Cel.GetComponent<WRUG1>().na�orzoneEfekty.Count == 0 || Cel.GetComponent<WRUG1>().na�orzoneEfekty.All(a => a.nazwa != Efektu.nazwa))
                {
                    Cel.GetComponent<WRUG1>().na�orzoneEfekty.Add(Efektu);
                    Cel.GetComponent<bazaEfektow>().OtrzymanieEfektu(Efektu);
                }
                else
                {
                    for (int x = 0; x < Cel.GetComponent<WRUG1>().na�orzoneEfekty.Count; x++)
                    {
                        if (Cel.GetComponent<WRUG1>().na�orzoneEfekty[x].nazwa == Efektu.nazwa)
                        {
                            Cel.GetComponent<WRUG1>().na�orzoneEfekty[x].licznik += Efektu.licznik;
                        }
                    }
                }
            }
            else if (Cel.tag == "Player")
            {
                if (Eq.na�orzoneEfekty.Count == 0 || Eq.na�orzoneEfekty.All(a => a.nazwa != Efektu.nazwa))
                {
                    Eq.na�orzoneEfekty.Add(Efektu);
                    Cel.GetComponent<bazaEfektow>().OtrzymanieEfektu(Efektu);
                }
                else
                {
                    for (int x = 0; x < Eq.na�orzoneEfekty.Count; x++)
                    {
                        if (Eq.na�orzoneEfekty[x].nazwa == Efektu.nazwa)
                        {
                            Eq.na�orzoneEfekty[x].licznik += Efektu.licznik;
                        }
                    }
                }
            }
            else if (Cel.tag == "karta")
            {

            }
        }
        else
        {
            if (Cel.tag == "wrug")
            {
                Cel.GetComponent<bazaEfektow>().OtrzymanieEfektu(Efektu);
            }
            else if(Cel.tag == "Player")
            {
                Cel.GetComponent<bazaEfektow>().OtrzymanieEfektu(Efektu);
            }
            else if(Cel.tag == "karta")
            {

            }

        }
    }

    private void PodajObrarzenia(GameObject trafiony)
    {
        if (trafiony.tag == "Player")
        {
            if (specjalnyTypObrarze�Gracz.HasFlag(TypObrarzen.nieSkalowalne))
            {
                if (specjalnyTypObrarze�Gracz.HasFlag(TypObrarzen.nieUchronne))
                {
                    Eq.PrzyjmijDmg(DmgGraczowi, true);
                }
                else
                {
                    Eq.PrzyjmijDmg(DmgGraczowi, false);
                }
            }
            else
            {
                if (specjalnyTypObrarze�Gracz.HasFlag(TypObrarzen.nieUchronne))
                {
                    Eq.PrzyjmijDmg((DmgGraczowi + Eq.bonusDoObrarze�), true);
                }
                else
                {
                    Eq.PrzyjmijDmg((DmgGraczowi + Eq.bonusDoObrarze�), false);
                }
            }
        }
        else if (trafiony.tag == "wrug")
        {
            if (specjalnyTypObrarze�.HasFlag(TypObrarzen.nieSkalowalne))
            {
                if (specjalnyTypObrarze�.HasFlag(TypObrarzen.nieUchronne))
                {
                    trafiony.GetComponent<WRUG1>().PrzyjmijDmg(Dmg, true);
                }
                else
                {
                    trafiony.GetComponent<WRUG1>().PrzyjmijDmg(Dmg, false);
                }
            }
            else
            {
                if (specjalnyTypObrarze�.HasFlag(TypObrarzen.nieUchronne))
                {
                    trafiony.GetComponent<WRUG1>().PrzyjmijDmg((Dmg + Eq.bonusDoObrarze�), true);
                }
                else
                {
                    trafiony.GetComponent<WRUG1>().PrzyjmijDmg((Dmg + Eq.bonusDoObrarze�), false);
                }
            }
        }
    }
    private void PodajObrarzeniaT(GameObject trafiony)
    {
        if (trafiony.tag == "Player")
        {
            if (specjalnyTypObrarze�GraczT.HasFlag(TypObrarzen.nieSkalowalne))
            {
                if (specjalnyTypObrarze�GraczT.HasFlag(TypObrarzen.nieUchronne))
                {
                    Eq.PrzyjmijDmg(DmgGraczowiT, true);
                }
                else
                {
                    Eq.PrzyjmijDmg(DmgGraczowiT, false);
                }
            }
            else
            {
                if (specjalnyTypObrarze�GraczT.HasFlag(TypObrarzen.nieUchronne))
                {
                    Eq.PrzyjmijDmg((DmgGraczowiT + Eq.bonusDoObrarze�), true);
                }
                else
                {
                    Eq.PrzyjmijDmg((DmgGraczowiT + Eq.bonusDoObrarze�), false);
                }
            }
        }
        else if (trafiony.tag == "wrug")
        {
            if (specjalnyTypObrarze�T.HasFlag(TypObrarzen.nieSkalowalne))
            {
                if (specjalnyTypObrarze�T.HasFlag(TypObrarzen.nieUchronne))
                {
                    trafiony.GetComponent<WRUG1>().PrzyjmijDmg(DmgT, true);
                }
                else
                {
                    trafiony.GetComponent<WRUG1>().PrzyjmijDmg(DmgT, false);
                }
            }
            else
            {
                if (specjalnyTypObrarze�T.HasFlag(TypObrarzen.nieUchronne))
                {
                    trafiony.GetComponent<WRUG1>().PrzyjmijDmg((DmgT + Eq.bonusDoObrarze�), true);
                }
                else
                {
                    trafiony.GetComponent<WRUG1>().PrzyjmijDmg((DmgT + Eq.bonusDoObrarze�), false);
                }
            }
        }
    }
    ////////////////////////////////////!!!!!!!!!!!!!!!!!UI!!!!!!!!!!!!!!!////////////////////////////////////
#if UNITY_EDITOR
    [CustomEditor(typeof(taKarta))]
    public class taKarta_Editor : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            var script = (taKarta)target;
            
            //DEBUGOWANIE?!?!??
            if (GUILayout.Button("Zapisz Zmiany w Karcie"))
            {
                taKarta noise = ((taKarta)target);
                EditorUtility.SetDirty(noise);
            }
            EditorGUILayout.LabelField(" ");

            //zawsze widoczne
            script.Nazwa = EditorGUILayout.TextField(label: "Nazwa", script.Nazwa);
            script.Koszt = EditorGUILayout.IntField(label: "Koszt", script.Koszt);
            script.KartaTag = (kartaTag)EditorGUILayout.EnumFlagsField(label: "Karta Tag", script.KartaTag);
            script.Rzadko�� = (rzadko��)EditorGUILayout.EnumPopup(label: "Rzadko��", script.Rzadko��);
            script.GrafikaKarty = (Sprite)EditorGUILayout.ObjectField(label: "Sprite", script.GrafikaKarty, typeof(Sprite), true);

            EditorGUILayout.LabelField(" ");
            EditorGUILayout.PropertyField(serializedObject.FindProperty("SkruconyOpis"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("Opis"), true);
            EditorGUILayout.LabelField(" ");
            EditorGUILayout.LabelField("DZIA�ANIE KARTY: ");
            script.grywalno�� = (Grywalno��)EditorGUILayout.EnumPopup(label: "Grywalno��", script.grywalno��);
            if (script.grywalno�� != Grywalno��.NieGrywalna)
            {
                script.poUrzyciu = (PoUrzyciu)EditorGUILayout.EnumPopup(label: "Po Urzyciu", script.poUrzyciu);
            }
            script.naKoniecTury = (PoUrzyciu)EditorGUILayout.EnumPopup(label: "Koniec Tury", script.naKoniecTury);
            if (script.grywalno�� != Grywalno��.NieGrywalna)
            {
                script.cele = (Cele)EditorGUILayout.EnumPopup(label: "Cele", script.cele);
            }

            //aktywowane wyborem celu!
            if (script.grywalno�� != Grywalno��.NieGrywalna)
            {
                if (script.cele == Cele.Gracz)
                {
                    script.DmgGraczowi = EditorGUILayout.FloatField(label: "obrarzenia gracz", script.DmgGraczowi);
                    if (script.DmgGraczowi != 0)
                    {
                        script.DmgGraczRazy = EditorGUILayout.IntSlider(label: "obrarzenia gracz mnorznik", script.DmgGraczRazy, 1, 20);
                        script.specjalnyTypObrarze�Gracz = (TypObrarzen)EditorGUILayout.EnumFlagsField(label: "specjalny typ obrarze� gracz", script.specjalnyTypObrarze�Gracz);
                    }
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("efektyGracz"), true);
                }
                if (script.cele == Cele.Wrug || script.cele == Cele.Wrogowie)
                {
                    script.Dmg = EditorGUILayout.FloatField(label: "obrarzenia", script.Dmg);
                    if (script.Dmg != 0)
                    {
                        script.DmgRazy = EditorGUILayout.IntSlider(label: "obrarzenia mnorznik", script.DmgRazy, 1, 20);
                        script.specjalnyTypObrarze� = (TypObrarzen)EditorGUILayout.EnumFlagsField(label: "specjalny typ obrarze�", script.specjalnyTypObrarze�);
                    }
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("efektyWrug"), true);
                }
                else if (script.cele == Cele.RandomWrug)
                {
                    script.RandomRazy = EditorGUILayout.IntSlider(label: "mnorznik random", script.RandomRazy, 1, 20);
                    script.Dmg = EditorGUILayout.FloatField(label: "obrarzenia", script.Dmg);
                    if (script.Dmg != 0)
                    {
                        script.specjalnyTypObrarze� = (TypObrarzen)EditorGUILayout.EnumFlagsField(label: "specjalny typ obrarze�", script.specjalnyTypObrarze�);
                    }
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("efektyWrug"), true);
                }
                else if (script.cele == Cele.All || script.cele == Cele.AlboWrugAlboGracz)
                {
                    if (script.cele == Cele.AlboWrugAlboGracz)
                    {
                        if (script.DmgGraczowi == 0 && script.efektyGracz.Count == 0)
                        {
                            EditorGUILayout.LabelField("    ", "Podaj dzia�ania dla obu stron by wywo�a�!");
                        }
                        else if (script.Dmg == 0 && script.efektyWrug.Count == 0)
                        {
                            EditorGUILayout.LabelField("    ", "Podaj dzia�ania dla obu stron by wywo�a�!");
                        }
                    }
                    script.DmgGraczowi = EditorGUILayout.FloatField(label: "obrarzenia gracz", script.DmgGraczowi);
                    if (script.DmgGraczowi != 0)
                    {
                        script.DmgGraczRazy = EditorGUILayout.IntSlider(label: "obrarzenia gracz mnorznik", script.DmgGraczRazy, 1, 20);
                        script.specjalnyTypObrarze�Gracz = (TypObrarzen)EditorGUILayout.EnumFlagsField(label: "specjalny typ obrarze� gracz", script.specjalnyTypObrarze�Gracz);
                    }
                    script.Dmg = EditorGUILayout.FloatField(label: "obrarzenia", script.Dmg);
                    if (script.Dmg != 0)
                    {
                        script.DmgRazy = EditorGUILayout.IntSlider(label: "obrarzenia mnorznik", script.DmgRazy, 1, 20);
                        script.specjalnyTypObrarze� = (TypObrarzen)EditorGUILayout.EnumFlagsField(label: "specjalny typ obrarze�", script.specjalnyTypObrarze�);
                    }
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("efektyGracz"), true);
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("efektyWrug"), true);
                }
                else if (script.cele == Cele.Random)
                {
                    script.RandomRazy = EditorGUILayout.IntSlider(label: "mnorznik random", script.RandomRazy, 1, 20);
                    if (script.DmgGraczowi == 0 && script.efektyGracz.Count == 0)
                    {
                        EditorGUILayout.LabelField("    ", "Podaj dzia�ania dla obu stron by wywo�a�!");
                    }
                    else if (script.Dmg == 0 && script.efektyWrug.Count == 0)
                    {
                        EditorGUILayout.LabelField("    ", "Podaj dzia�ania dla obu stron by wywo�a�!");
                    }
                    script.DmgGraczowi = EditorGUILayout.FloatField(label: "obrarzenia gracz", script.DmgGraczowi);
                    if (script.DmgGraczowi != 0)
                    {
                        script.specjalnyTypObrarze�Gracz = (TypObrarzen)EditorGUILayout.EnumFlagsField(label: "specjalny typ obrarze� gracz", script.specjalnyTypObrarze�Gracz);
                    }
                    script.Dmg = EditorGUILayout.FloatField(label: "obrarzenia wrug", script.Dmg);
                    if (script.Dmg != 0)
                    {
                        script.specjalnyTypObrarze� = (TypObrarzen)EditorGUILayout.EnumFlagsField(label: "specjalny typ obrarze�", script.specjalnyTypObrarze�);
                    }
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("efektyGracz"), true);
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("efektyWrug"), true);
                }
                else if (script.cele == Cele.Karta || script.cele == Cele.KartyWD�oni || script.cele == Cele.RandomKartaWD�oni)
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("efektyNaKarty"), true);
                }
            }
            /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// ///
            EditorGUILayout.LabelField("DZIA�ANIA KART NIEZAGRANYCH:");
            script.Dzia�anieNaKoniecTury = EditorGUILayout.Toggle(label: "akcje niezagrane?", script.Dzia�anieNaKoniecTury);
            if (script.Dzia�anieNaKoniecTury == true)
            {
                script.celeNieZagranej = (CeleNieZagranej)EditorGUILayout.EnumPopup(label: "Cele", script.celeNieZagranej);

                if (script.celeNieZagranej == CeleNieZagranej.Gracz)
                {
                    script.DmgGraczowiT = EditorGUILayout.FloatField(label: "obrarzenia gracz", script.DmgGraczowiT);
                    if (script.DmgGraczowiT != 0)
                    {
                        script.DmgGraczRazyT = EditorGUILayout.IntSlider(label: "obrarzenia gracz mnorznik", script.DmgGraczRazyT, 1, 20);
                        script.specjalnyTypObrarze�GraczT = (TypObrarzen)EditorGUILayout.EnumFlagsField(label: "specjalny typ obrarze� gracz", script.specjalnyTypObrarze�GraczT);
                    }
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("efektyGraczT"), true);
                }
                else if (script.celeNieZagranej == CeleNieZagranej.All)
                {
                    script.DmgGraczowiT = EditorGUILayout.FloatField(label: "obrarzenia gracz", script.DmgGraczowiT);
                    if (script.DmgGraczowiT != 0)
                    {
                        script.DmgGraczRazyT = EditorGUILayout.IntSlider(label: "obrarzenia gracz mnorznik", script.DmgGraczRazyT, 1, 20);
                        script.specjalnyTypObrarze�GraczT = (TypObrarzen)EditorGUILayout.EnumFlagsField(label: "specjalny typ obrarze� gracz", script.specjalnyTypObrarze�GraczT);
                    }
                    script.DmgT = EditorGUILayout.FloatField(label: "obrarzenia", script.DmgT);
                    if (script.DmgT != 0)
                    {
                        script.DmgRazyT = EditorGUILayout.IntSlider(label: "obrarzenia mnorznik", script.DmgRazyT, 1, 20);
                        script.specjalnyTypObrarze�T = (TypObrarzen)EditorGUILayout.EnumFlagsField(label: "specjalny typ obrarze�", script.specjalnyTypObrarze�T);
                    }
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("efektyGraczT"), true);
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("efektyWrugT"), true);
                }
                else if (script.celeNieZagranej == CeleNieZagranej.KartyWD�oni || script.celeNieZagranej == CeleNieZagranej.TaKarta || script.celeNieZagranej == CeleNieZagranej.RandomKartaWD�oni)
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("efektyNaKartyT"), true);
                }
                else if (script.celeNieZagranej == CeleNieZagranej.Random)
                {
                    script.RandomRazyT = EditorGUILayout.IntSlider(label: "mnorznik random", script.RandomRazyT, 1, 20);
                    if (script.DmgGraczowiT == 0 && script.efektyGraczT.Count == 0)
                    {
                        EditorGUILayout.LabelField("    ", "Podaj dzia�ania dla obu stron by wywo�a�!");
                    }
                    else if (script.DmgT == 0 && script.efektyWrugT.Count == 0)
                    {
                        EditorGUILayout.LabelField("    ", "Podaj dzia�ania dla obu stron by wywo�a�!");
                    }
                    script.DmgGraczowiT = EditorGUILayout.FloatField(label: "obrarzenia gracz", script.DmgGraczowiT);
                    if (script.DmgGraczowiT != 0)
                    {
                        script.specjalnyTypObrarze�GraczT = (TypObrarzen)EditorGUILayout.EnumFlagsField(label: "specjalny typ obrarze� gracz", script.specjalnyTypObrarze�GraczT);
                    }
                    script.DmgT = EditorGUILayout.FloatField(label: "obrarzenia wrug", script.DmgT);
                    if (script.DmgT != 0)
                    {
                        script.specjalnyTypObrarze�T = (TypObrarzen)EditorGUILayout.EnumFlagsField(label: "specjalny typ obrarze�", script.specjalnyTypObrarze�T);
                    }
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("efektyGraczT"), true);
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("efektyWrugT"), true);
                }
                else if (script.celeNieZagranej == CeleNieZagranej.RandomWrug)
                {
                    script.RandomRazyT = EditorGUILayout.IntSlider(label: "mnorznik random", script.RandomRazyT, 1, 20);
                    script.DmgT = EditorGUILayout.FloatField(label: "obrarzenia", script.DmgT);
                    if (script.DmgT != 0)
                    {
                        script.specjalnyTypObrarze�T = (TypObrarzen)EditorGUILayout.EnumFlagsField(label: "specjalny typ obrarze�", script.specjalnyTypObrarze�T);
                    }
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("efektyWrugT"), true);
                }
                else if (script.celeNieZagranej == CeleNieZagranej.Wrogowie)
                {
                    script.DmgT = EditorGUILayout.FloatField(label: "obrarzenia", script.DmgT);
                    if (script.DmgT != 0)
                    {
                        script.DmgRazyT = EditorGUILayout.IntSlider(label: "obrarzenia mnorznik", script.DmgRazyT, 1, 20);
                        script.specjalnyTypObrarze�T = (TypObrarzen)EditorGUILayout.EnumFlagsField(label: "specjalny typ obrarze�", script.specjalnyTypObrarze�T);
                    }
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("efektyWrugT"), true);
                }

            }
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif

}
