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

public enum Cele { Gracz, Wrug, Wrogowie, Karta, KartyWD³oni,RandomKartaWD³oni , All, RandomWrug, Random, AlboWrugAlboGracz };
public enum CeleNieZagranej { Gracz, Wrogowie, TaKarta, KartyWD³oni, RandomKartaWD³oni, All, RandomWrug , Random };
public enum PoUrzyciu { Zniszcz, Zachowaj, wyklucz, cmentarz};
public enum Grywalnoœæ { Grywalna, Zablokowana, NieGrywalna };
[System.Flags]
public enum TypObrarzen : int { brak = 0x00, nieSkalowalne = 0x01, nieUchronne = 0x02 };
[System.Flags]
public enum kartaTag : int { normal = 0x00, atak = 0x01, czar = 0x02, test3 = 0x04}; //testowo bedzie siê to odnosi³a np do eketów innych kart; (np. karta wzmacnia karty typu atak);
public enum rzadkoñæ { pospolita, scecjalna, unikalna, rzadka}; //bedzie potrzebe do systemu dropów!!

public class taKarta : MonoBehaviour
{
    //przypisty
    private GameObject grafika, opis, koszt, ramka, nazwa; 
    private Vector3 pozEnd, pozEndRamka;
    private biblioteka Biblioteka;
    [HideInInspector] public GameObject podgl¹dOpis;
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
    public rzadkoñæ Rzadkoœæ;

    [Header("Dzia³anie")]
    public Grywalnoœæ grywalnoœæ;
    public Cele cele;
    public PoUrzyciu poUrzyciu;
    public PoUrzyciu naKoniecTury;
    [HideInInspector] public UnityEvent<List<GameObject>> akcje;
    public float DmgGraczowi;
    public TypObrarzen specjalnyTypObrarzeñGracz;
    [Range(1, 20)]
    public int DmgGraczRazy = 1;
    [Range(1, 20)]
    public int RandomRazy = 1;
    public TypObrarzen specjalnyTypObrarzeñ;
    public float Dmg;
    [Range(1, 20)]
    public int DmgRazy = 1;
    public List<nalurzEfekt> efektyGracz;
    public List<nalurzEfekt> efektyWrug;
    public List<nalurzEfekt> efektyNaKarty;

    [Header("Dzia³ania na koniec tury")] //DOPISEK "T"
    public bool Dzia³anieNaKoniecTury = false;
    public CeleNieZagranej celeNieZagranej;
    [HideInInspector] public UnityEvent<List<GameObject>> akcjeKoniecTury;
    public float DmgGraczowiT;
    public TypObrarzen specjalnyTypObrarzeñGraczT;
    [Range(1, 20)]
    public int DmgGraczRazyT = 1;
    [Range(1, 20)]
    public int RandomRazyT = 1;
    public TypObrarzen specjalnyTypObrarzeñT;
    public float DmgT;
    [Range(1, 20)]
    public int DmgRazyT = 1;
    public List<nalurzEfekt> efektyGraczT;
    public List<nalurzEfekt> efektyWrugT;
    public List<nalurzEfekt> efektyNaKartyT;

    //pozosta³e
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
        podgl¹dOpis = this.gameObject.transform.GetChild(1).gameObject;
        podgl¹dOpis.SetActive(false);
       
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
                akcje.AddListener(Na³urzEfekty);
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
                akcje.AddListener(Na³urzEfekty);
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
                akcje.AddListener(Na³urzEfekty);
            }
        }
        else if (cele == Cele.Karta || cele == Cele.KartyWD³oni || cele == Cele.RandomKartaWD³oni)
        {
            if (efektyNaKarty.Count > 0)
            {
                akcje.AddListener(Na³urzEfekty);
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
                akcje.AddListener(Na³urzEfekty);
            }
            else if (Dmg != 0 && efektyGracz.Count > 0)
            {
                akcje.AddListener(DmgDiler);
                akcje.AddListener(Na³urzEfekty);
            }
            else if (DmgGraczowi != 0 && efektyWrug.Count > 0)
            {
                akcje.AddListener(DmgDiler);
                akcje.AddListener(Na³urzEfekty);
            }
        }

        if (Dzia³anieNaKoniecTury == true) //NO BEDZIE TROCHÊ UZUPE£NIANIA
        {
            if (celeNieZagranej == CeleNieZagranej.Gracz)
            {
                if (DmgGraczowiT != 0)
                {
                    akcjeKoniecTury.AddListener(DmgDilerT);
                }
                if (efektyGraczT.Count > 0)
                {
                    akcjeKoniecTury.AddListener(Na³urzEfektyT);
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
                    akcjeKoniecTury.AddListener(Na³urzEfektyT);
                }
            }
            else if (celeNieZagranej == CeleNieZagranej.TaKarta || celeNieZagranej == CeleNieZagranej.KartyWD³oni || celeNieZagranej == CeleNieZagranej.RandomKartaWD³oni)
            {
                if (efektyNaKartyT.Count > 0)
                {
                   akcjeKoniecTury.AddListener(Na³urzEfektyT);
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
                    akcjeKoniecTury.AddListener(Na³urzEfektyT);
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
            akcje.AddListener(UsuñTeKarte);
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
            akcje.AddListener(UsuñTeKarte);
        }
    }

    private void ZachowajTeKarte(List<GameObject> nieIstotne)
    {
        click c  = GameObject.FindGameObjectWithTag("nadUiWalka").gameObject.GetComponent<click>();
        c.GrabCardOf();
        c.CzyœæCardMorInfo();
        c.CzyœæWskazana();
    }
    public void UsuñTeKarte(List<GameObject> nieIstotne)
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
        Uzupe³nijOpis(SkruconyOpis);
        Uzupe³nijOpis(Opis);
    }

    private void Uzupe³nijOpis(List<textKartaTyp> tx) //AKTUALIZUJ PRZY ZMIANEI STATYSTYK!!!(bedzie podpiête pewnie pod event!!)
    {
        finalnyOpis = null;

        if (tx.Count > 0)
        {
            for (int x = 0; x < tx.Count; x++)
            {
                if (tx[x].RodzajTrescKarta == rodzajTrescKarta.normalText)
                {
                    finalnyOpis += tx[x].treœæ + " ";
                }
                else if (tx[x].RodzajTrescKarta == rodzajTrescKarta.obrarzenia)
                {
                    if (specjalnyTypObrarzeñ.HasFlag(TypObrarzen.nieSkalowalne))
                    {
                        finalnyOpis += Dmg.ToString() + " ";
                    }
                    else
                    {
                        float suma = Dmg + Eq.bonusDoObrarzeñ;
                        finalnyOpis += suma.ToString() + " ";
                    }
                }
                else if (tx[x].RodzajTrescKarta == rodzajTrescKarta.obrarzeniaGracz)
                {
                    if (specjalnyTypObrarzeñ.HasFlag(TypObrarzen.nieSkalowalne))
                    {
                        finalnyOpis += DmgGraczowi.ToString() + " ";
                    }
                    else
                    {
                        float suma = DmgGraczowi + Eq.bonusDoObrarzeñ;
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
            podgl¹dOpis.gameObject.transform.GetChild(1).gameObject.GetComponent<TextMeshPro>().text = finalnyOpis;
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
                            Na³urzEfekt(Eq.gameObject, efektyGracz[c]);
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
                            Na³urzEfekt(celee[z], efektyWrug[c]);
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
                            Na³urzEfekt(Eq.gameObject, efektyGraczT[c]);
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
                            Na³urzEfekt(celee[z], efektyWrugT[c]);
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

    public void Na³urzEfekty(List<GameObject> cele)
    {
        for (int x = 0; x < cele.Count; x++)
        {
            if (cele[x].tag == "wrug")
            {
                for (int y = 0; y < efektyWrug.Count; y++)
                {
                    Na³urzEfekt(cele[x], efektyWrug[y]);
                }
            }
            else if (cele[x].tag == "Player")
            {
                for (int y = 0; y < efektyGracz.Count; y++)
                {
                    Na³urzEfekt(Eq.gameObject, efektyGracz[y]);
                }
            }
            else if (cele[x].tag == "karta")
            {
                for (int y = 0; y < efektyNaKarty.Count; y++)
                {
                    Na³urzEfekt(cele[x], efektyNaKarty[y]);
                }
            }

        }
    }

    public void Na³urzEfektyT(List<GameObject> cele)
    {
        for (int x = 0; x < cele.Count; x++)
        {
            if (cele[x].tag == "wrug")
            {
                for (int y = 0; y < efektyWrugT.Count; y++)
                {
                    Na³urzEfekt(cele[x], efektyWrugT[y]);
                }
            }
            else if (cele[x].tag == "Player")
            {
                for (int y = 0; y < efektyGraczT.Count; y++)
                {
                    Na³urzEfekt(Eq.gameObject, efektyGraczT[y]);
                }
            }
            else if (cele[x].tag == "karta")
            {
                for (int y = 0; y < efektyNaKartyT.Count; y++)
                {
                    Na³urzEfekt(cele[x], efektyNaKartyT[y]);
                }
            }
        }
    }

    //////////////////////////////////////////NARZÊDZIA//////////////////////////////////////////////
    private void Na³urzEfekt(GameObject Cel, nalurzEfekt Efektuuu)
    {
        for (int x = 0; x < Biblioteka.dostêpneEfekty.Count; x++)
        {
            if (Biblioteka.dostêpneEfekty[x].nazwa == Efektuuu.NazwaEfektu.ToString())
            {
                Efektu = new efekty(Biblioteka.dostêpneEfekty[x].nazwa, Biblioteka.dostêpneEfekty[x].odbiurEfektu, Biblioteka.dostêpneEfekty[x].sprite, Biblioteka.dostêpneEfekty[x].opis, Biblioteka.dostêpneEfekty[x].TypWywo³ania, Efektuuu.ile);
            }
        }

        if (Efektu.TypWywo³ania != typWywo³ania.natychmiastowy)
        {
            if (Cel.tag == "wrug")
            {
                if (Cel.GetComponent<WRUG1>().na³orzoneEfekty.Count == 0 || Cel.GetComponent<WRUG1>().na³orzoneEfekty.All(a => a.nazwa != Efektu.nazwa))
                {
                    Cel.GetComponent<WRUG1>().na³orzoneEfekty.Add(Efektu);
                    Cel.GetComponent<bazaEfektow>().OtrzymanieEfektu(Efektu);
                }
                else
                {
                    for (int x = 0; x < Cel.GetComponent<WRUG1>().na³orzoneEfekty.Count; x++)
                    {
                        if (Cel.GetComponent<WRUG1>().na³orzoneEfekty[x].nazwa == Efektu.nazwa)
                        {
                            Cel.GetComponent<WRUG1>().na³orzoneEfekty[x].licznik += Efektu.licznik;
                        }
                    }
                }
            }
            else if (Cel.tag == "Player")
            {
                if (Eq.na³orzoneEfekty.Count == 0 || Eq.na³orzoneEfekty.All(a => a.nazwa != Efektu.nazwa))
                {
                    Eq.na³orzoneEfekty.Add(Efektu);
                    Cel.GetComponent<bazaEfektow>().OtrzymanieEfektu(Efektu);
                }
                else
                {
                    for (int x = 0; x < Eq.na³orzoneEfekty.Count; x++)
                    {
                        if (Eq.na³orzoneEfekty[x].nazwa == Efektu.nazwa)
                        {
                            Eq.na³orzoneEfekty[x].licznik += Efektu.licznik;
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
            if (specjalnyTypObrarzeñGracz.HasFlag(TypObrarzen.nieSkalowalne))
            {
                if (specjalnyTypObrarzeñGracz.HasFlag(TypObrarzen.nieUchronne))
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
                if (specjalnyTypObrarzeñGracz.HasFlag(TypObrarzen.nieUchronne))
                {
                    Eq.PrzyjmijDmg((DmgGraczowi + Eq.bonusDoObrarzeñ), true);
                }
                else
                {
                    Eq.PrzyjmijDmg((DmgGraczowi + Eq.bonusDoObrarzeñ), false);
                }
            }
        }
        else if (trafiony.tag == "wrug")
        {
            if (specjalnyTypObrarzeñ.HasFlag(TypObrarzen.nieSkalowalne))
            {
                if (specjalnyTypObrarzeñ.HasFlag(TypObrarzen.nieUchronne))
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
                if (specjalnyTypObrarzeñ.HasFlag(TypObrarzen.nieUchronne))
                {
                    trafiony.GetComponent<WRUG1>().PrzyjmijDmg((Dmg + Eq.bonusDoObrarzeñ), true);
                }
                else
                {
                    trafiony.GetComponent<WRUG1>().PrzyjmijDmg((Dmg + Eq.bonusDoObrarzeñ), false);
                }
            }
        }
    }
    private void PodajObrarzeniaT(GameObject trafiony)
    {
        if (trafiony.tag == "Player")
        {
            if (specjalnyTypObrarzeñGraczT.HasFlag(TypObrarzen.nieSkalowalne))
            {
                if (specjalnyTypObrarzeñGraczT.HasFlag(TypObrarzen.nieUchronne))
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
                if (specjalnyTypObrarzeñGraczT.HasFlag(TypObrarzen.nieUchronne))
                {
                    Eq.PrzyjmijDmg((DmgGraczowiT + Eq.bonusDoObrarzeñ), true);
                }
                else
                {
                    Eq.PrzyjmijDmg((DmgGraczowiT + Eq.bonusDoObrarzeñ), false);
                }
            }
        }
        else if (trafiony.tag == "wrug")
        {
            if (specjalnyTypObrarzeñT.HasFlag(TypObrarzen.nieSkalowalne))
            {
                if (specjalnyTypObrarzeñT.HasFlag(TypObrarzen.nieUchronne))
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
                if (specjalnyTypObrarzeñT.HasFlag(TypObrarzen.nieUchronne))
                {
                    trafiony.GetComponent<WRUG1>().PrzyjmijDmg((DmgT + Eq.bonusDoObrarzeñ), true);
                }
                else
                {
                    trafiony.GetComponent<WRUG1>().PrzyjmijDmg((DmgT + Eq.bonusDoObrarzeñ), false);
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
            script.Rzadkoœæ = (rzadkoñæ)EditorGUILayout.EnumPopup(label: "Rzadkoœæ", script.Rzadkoœæ);
            script.GrafikaKarty = (Sprite)EditorGUILayout.ObjectField(label: "Sprite", script.GrafikaKarty, typeof(Sprite), true);

            EditorGUILayout.LabelField(" ");
            EditorGUILayout.PropertyField(serializedObject.FindProperty("SkruconyOpis"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("Opis"), true);
            EditorGUILayout.LabelField(" ");
            EditorGUILayout.LabelField("DZIA£ANIE KARTY: ");
            script.grywalnoœæ = (Grywalnoœæ)EditorGUILayout.EnumPopup(label: "Grywalnoœæ", script.grywalnoœæ);
            if (script.grywalnoœæ != Grywalnoœæ.NieGrywalna)
            {
                script.poUrzyciu = (PoUrzyciu)EditorGUILayout.EnumPopup(label: "Po Urzyciu", script.poUrzyciu);
            }
            script.naKoniecTury = (PoUrzyciu)EditorGUILayout.EnumPopup(label: "Koniec Tury", script.naKoniecTury);
            if (script.grywalnoœæ != Grywalnoœæ.NieGrywalna)
            {
                script.cele = (Cele)EditorGUILayout.EnumPopup(label: "Cele", script.cele);
            }

            //aktywowane wyborem celu!
            if (script.grywalnoœæ != Grywalnoœæ.NieGrywalna)
            {
                if (script.cele == Cele.Gracz)
                {
                    script.DmgGraczowi = EditorGUILayout.FloatField(label: "obrarzenia gracz", script.DmgGraczowi);
                    if (script.DmgGraczowi != 0)
                    {
                        script.DmgGraczRazy = EditorGUILayout.IntSlider(label: "obrarzenia gracz mnorznik", script.DmgGraczRazy, 1, 20);
                        script.specjalnyTypObrarzeñGracz = (TypObrarzen)EditorGUILayout.EnumFlagsField(label: "specjalny typ obrarzeñ gracz", script.specjalnyTypObrarzeñGracz);
                    }
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("efektyGracz"), true);
                }
                if (script.cele == Cele.Wrug || script.cele == Cele.Wrogowie)
                {
                    script.Dmg = EditorGUILayout.FloatField(label: "obrarzenia", script.Dmg);
                    if (script.Dmg != 0)
                    {
                        script.DmgRazy = EditorGUILayout.IntSlider(label: "obrarzenia mnorznik", script.DmgRazy, 1, 20);
                        script.specjalnyTypObrarzeñ = (TypObrarzen)EditorGUILayout.EnumFlagsField(label: "specjalny typ obrarzeñ", script.specjalnyTypObrarzeñ);
                    }
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("efektyWrug"), true);
                }
                else if (script.cele == Cele.RandomWrug)
                {
                    script.RandomRazy = EditorGUILayout.IntSlider(label: "mnorznik random", script.RandomRazy, 1, 20);
                    script.Dmg = EditorGUILayout.FloatField(label: "obrarzenia", script.Dmg);
                    if (script.Dmg != 0)
                    {
                        script.specjalnyTypObrarzeñ = (TypObrarzen)EditorGUILayout.EnumFlagsField(label: "specjalny typ obrarzeñ", script.specjalnyTypObrarzeñ);
                    }
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("efektyWrug"), true);
                }
                else if (script.cele == Cele.All || script.cele == Cele.AlboWrugAlboGracz)
                {
                    if (script.cele == Cele.AlboWrugAlboGracz)
                    {
                        if (script.DmgGraczowi == 0 && script.efektyGracz.Count == 0)
                        {
                            EditorGUILayout.LabelField("    ", "Podaj dzia³ania dla obu stron by wywo³aæ!");
                        }
                        else if (script.Dmg == 0 && script.efektyWrug.Count == 0)
                        {
                            EditorGUILayout.LabelField("    ", "Podaj dzia³ania dla obu stron by wywo³aæ!");
                        }
                    }
                    script.DmgGraczowi = EditorGUILayout.FloatField(label: "obrarzenia gracz", script.DmgGraczowi);
                    if (script.DmgGraczowi != 0)
                    {
                        script.DmgGraczRazy = EditorGUILayout.IntSlider(label: "obrarzenia gracz mnorznik", script.DmgGraczRazy, 1, 20);
                        script.specjalnyTypObrarzeñGracz = (TypObrarzen)EditorGUILayout.EnumFlagsField(label: "specjalny typ obrarzeñ gracz", script.specjalnyTypObrarzeñGracz);
                    }
                    script.Dmg = EditorGUILayout.FloatField(label: "obrarzenia", script.Dmg);
                    if (script.Dmg != 0)
                    {
                        script.DmgRazy = EditorGUILayout.IntSlider(label: "obrarzenia mnorznik", script.DmgRazy, 1, 20);
                        script.specjalnyTypObrarzeñ = (TypObrarzen)EditorGUILayout.EnumFlagsField(label: "specjalny typ obrarzeñ", script.specjalnyTypObrarzeñ);
                    }
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("efektyGracz"), true);
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("efektyWrug"), true);
                }
                else if (script.cele == Cele.Random)
                {
                    script.RandomRazy = EditorGUILayout.IntSlider(label: "mnorznik random", script.RandomRazy, 1, 20);
                    if (script.DmgGraczowi == 0 && script.efektyGracz.Count == 0)
                    {
                        EditorGUILayout.LabelField("    ", "Podaj dzia³ania dla obu stron by wywo³aæ!");
                    }
                    else if (script.Dmg == 0 && script.efektyWrug.Count == 0)
                    {
                        EditorGUILayout.LabelField("    ", "Podaj dzia³ania dla obu stron by wywo³aæ!");
                    }
                    script.DmgGraczowi = EditorGUILayout.FloatField(label: "obrarzenia gracz", script.DmgGraczowi);
                    if (script.DmgGraczowi != 0)
                    {
                        script.specjalnyTypObrarzeñGracz = (TypObrarzen)EditorGUILayout.EnumFlagsField(label: "specjalny typ obrarzeñ gracz", script.specjalnyTypObrarzeñGracz);
                    }
                    script.Dmg = EditorGUILayout.FloatField(label: "obrarzenia wrug", script.Dmg);
                    if (script.Dmg != 0)
                    {
                        script.specjalnyTypObrarzeñ = (TypObrarzen)EditorGUILayout.EnumFlagsField(label: "specjalny typ obrarzeñ", script.specjalnyTypObrarzeñ);
                    }
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("efektyGracz"), true);
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("efektyWrug"), true);
                }
                else if (script.cele == Cele.Karta || script.cele == Cele.KartyWD³oni || script.cele == Cele.RandomKartaWD³oni)
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("efektyNaKarty"), true);
                }
            }
            /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// ///
            EditorGUILayout.LabelField("DZIA£ANIA KART NIEZAGRANYCH:");
            script.Dzia³anieNaKoniecTury = EditorGUILayout.Toggle(label: "akcje niezagrane?", script.Dzia³anieNaKoniecTury);
            if (script.Dzia³anieNaKoniecTury == true)
            {
                script.celeNieZagranej = (CeleNieZagranej)EditorGUILayout.EnumPopup(label: "Cele", script.celeNieZagranej);

                if (script.celeNieZagranej == CeleNieZagranej.Gracz)
                {
                    script.DmgGraczowiT = EditorGUILayout.FloatField(label: "obrarzenia gracz", script.DmgGraczowiT);
                    if (script.DmgGraczowiT != 0)
                    {
                        script.DmgGraczRazyT = EditorGUILayout.IntSlider(label: "obrarzenia gracz mnorznik", script.DmgGraczRazyT, 1, 20);
                        script.specjalnyTypObrarzeñGraczT = (TypObrarzen)EditorGUILayout.EnumFlagsField(label: "specjalny typ obrarzeñ gracz", script.specjalnyTypObrarzeñGraczT);
                    }
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("efektyGraczT"), true);
                }
                else if (script.celeNieZagranej == CeleNieZagranej.All)
                {
                    script.DmgGraczowiT = EditorGUILayout.FloatField(label: "obrarzenia gracz", script.DmgGraczowiT);
                    if (script.DmgGraczowiT != 0)
                    {
                        script.DmgGraczRazyT = EditorGUILayout.IntSlider(label: "obrarzenia gracz mnorznik", script.DmgGraczRazyT, 1, 20);
                        script.specjalnyTypObrarzeñGraczT = (TypObrarzen)EditorGUILayout.EnumFlagsField(label: "specjalny typ obrarzeñ gracz", script.specjalnyTypObrarzeñGraczT);
                    }
                    script.DmgT = EditorGUILayout.FloatField(label: "obrarzenia", script.DmgT);
                    if (script.DmgT != 0)
                    {
                        script.DmgRazyT = EditorGUILayout.IntSlider(label: "obrarzenia mnorznik", script.DmgRazyT, 1, 20);
                        script.specjalnyTypObrarzeñT = (TypObrarzen)EditorGUILayout.EnumFlagsField(label: "specjalny typ obrarzeñ", script.specjalnyTypObrarzeñT);
                    }
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("efektyGraczT"), true);
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("efektyWrugT"), true);
                }
                else if (script.celeNieZagranej == CeleNieZagranej.KartyWD³oni || script.celeNieZagranej == CeleNieZagranej.TaKarta || script.celeNieZagranej == CeleNieZagranej.RandomKartaWD³oni)
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("efektyNaKartyT"), true);
                }
                else if (script.celeNieZagranej == CeleNieZagranej.Random)
                {
                    script.RandomRazyT = EditorGUILayout.IntSlider(label: "mnorznik random", script.RandomRazyT, 1, 20);
                    if (script.DmgGraczowiT == 0 && script.efektyGraczT.Count == 0)
                    {
                        EditorGUILayout.LabelField("    ", "Podaj dzia³ania dla obu stron by wywo³aæ!");
                    }
                    else if (script.DmgT == 0 && script.efektyWrugT.Count == 0)
                    {
                        EditorGUILayout.LabelField("    ", "Podaj dzia³ania dla obu stron by wywo³aæ!");
                    }
                    script.DmgGraczowiT = EditorGUILayout.FloatField(label: "obrarzenia gracz", script.DmgGraczowiT);
                    if (script.DmgGraczowiT != 0)
                    {
                        script.specjalnyTypObrarzeñGraczT = (TypObrarzen)EditorGUILayout.EnumFlagsField(label: "specjalny typ obrarzeñ gracz", script.specjalnyTypObrarzeñGraczT);
                    }
                    script.DmgT = EditorGUILayout.FloatField(label: "obrarzenia wrug", script.DmgT);
                    if (script.DmgT != 0)
                    {
                        script.specjalnyTypObrarzeñT = (TypObrarzen)EditorGUILayout.EnumFlagsField(label: "specjalny typ obrarzeñ", script.specjalnyTypObrarzeñT);
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
                        script.specjalnyTypObrarzeñT = (TypObrarzen)EditorGUILayout.EnumFlagsField(label: "specjalny typ obrarzeñ", script.specjalnyTypObrarzeñT);
                    }
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("efektyWrugT"), true);
                }
                else if (script.celeNieZagranej == CeleNieZagranej.Wrogowie)
                {
                    script.DmgT = EditorGUILayout.FloatField(label: "obrarzenia", script.DmgT);
                    if (script.DmgT != 0)
                    {
                        script.DmgRazyT = EditorGUILayout.IntSlider(label: "obrarzenia mnorznik", script.DmgRazyT, 1, 20);
                        script.specjalnyTypObrarzeñT = (TypObrarzen)EditorGUILayout.EnumFlagsField(label: "specjalny typ obrarzeñ", script.specjalnyTypObrarzeñT);
                    }
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("efektyWrugT"), true);
                }

            }
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif

}
