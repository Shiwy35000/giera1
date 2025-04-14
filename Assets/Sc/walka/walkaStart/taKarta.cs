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
public enum PoUrzyciu { Zniszcz = 0, Zachowaj = 1, wyklucz = 2, cmentarz = 3};
public enum Grywalnoœæ { Grywalna = 0, NieGrywalna = 1};
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
    [HideInInspector] public GameObject dlon;
    [HideInInspector] public playerEq Eq;
    private efekty Efektu;
    [HideInInspector] public GameObject prefabTejKartyWdeck;
    [HideInInspector] public string publicznyPrzekszta³conyOpis;
    [Header("Dane Karty")]
    public string Nazwa;
    public int Id;
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
    public List<nalurzEfektKarta> efektyNaKarty;
    [Range(1, 20)]
    public int efektyNaKartyRandomRazy = 1;

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
    public List<nalurzEfektKarta> efektyNaKartyT;
    [Range(1, 20)]
    public int efektyNaKartyRandomRazyT = 1;

    //pozosta³e
    private string finalnyOpis;
    [HideInInspector] public GameObject fizycznyDeck;
    public List<nalurzEfektKarta> na³orzoneEfektyKartaD³on;
    public List<nalurzEfektKarta> na³orzoneEfektyKartaTura;
    [HideInInspector] public GameObject punktLinia;

    void Awake()
    {
        //WIZUALIA PRZYPISZ
        ramka = this.gameObject.transform.GetChild(0).gameObject;
        grafika = ramka.transform.GetChild(0).gameObject;
        koszt = ramka.transform.GetChild(1).gameObject;
        opis = ramka.transform.GetChild(2).gameObject;
        nazwa = ramka.transform.GetChild(3).gameObject;
        punktLinia = this.gameObject.transform.GetChild(1).gameObject;

        Eq = GameObject.FindGameObjectWithTag("Player").GetComponent<playerEq>();
        Biblioteka = GameObject.FindGameObjectWithTag("saveGame").GetComponent<biblioteka>();
        fizycznyDeck = GameObject.FindGameObjectWithTag("fizycznyDeck").gameObject;

        Uzupelnij();
        PodpinajAkcje();


        walkaStart.KoniecTury += UsuwanieEfektówKartyTura;
    }
    private void OnDestroy()
    {
        walkaStart.KoniecTury -= UsuwanieEfektówKartyTura;
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
        else if (cele == Cele.AlboWrugAlboGracz)
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

        if (Dzia³anieNaKoniecTury == true)
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


        akcje.AddListener(CoPoUrzyciu);
    }

    private void CoPoUrzyciu(List<GameObject> nieIstotne)
    {
        if (poUrzyciu == PoUrzyciu.Zniszcz)
        {
            UsuñTeKarte();
        }
        else if (poUrzyciu == PoUrzyciu.Zachowaj)
        {
            ZachowajTeKarte();
        }
        else if (poUrzyciu == PoUrzyciu.cmentarz)
        {
            NaCmentarzTaKarta();
        }
        else if (poUrzyciu == PoUrzyciu.wyklucz)
        {
            UsuñTeKarte();
        }
    }

    private void ZachowajTeKarte()
    {
        click c = GameObject.FindGameObjectWithTag("nadUiWalka").gameObject.GetComponent<click>();
        c.GrabCardOf();
        c.CzyœæWskazana();
    }
    public void UsuñTeKarte()
    {
        if (prefabTejKartyWdeck != null)
        {
            Eq.deckPrefab.Remove(prefabTejKartyWdeck);
        }
        dlon.GetComponent<sortGrupZ>().UsunKarteZdloni(this.gameObject);
    }
    public void NaCmentarzTaKarta()
    {
        UsuwanieEfektówKarty(na³orzoneEfektyKartaD³on);
        UsuwanieEfektówKarty(na³orzoneEfektyKartaTura);
        GameObject klon = GameObject.Instantiate(this.gameObject, fizycznyDeck.transform);
        klon.name = this.gameObject.name;
        Eq.cmentarz.Add(klon);
        dlon.GetComponent<sortGrupZ>().UsunKarteZdloni(this.gameObject);
    }
    public void WykluczTeKarte()
    {
        UsuwanieEfektówKarty(na³orzoneEfektyKartaD³on);
        UsuwanieEfektówKarty(na³orzoneEfektyKartaTura);
        GameObject klon = GameObject.Instantiate(this.gameObject, fizycznyDeck.transform);
        klon.name = this.gameObject.name;
        Eq.wykluczone.Add(klon);
        dlon.GetComponent<sortGrupZ>().UsunKarteZdloni(this.gameObject);
    }

    public void Uzupelnij()
    {
        grafika.GetComponent<SpriteRenderer>().sprite = GrafikaKarty;
        koszt.GetComponent<TextMeshPro>().text = Koszt.ToString();
        nazwa.GetComponent<TextMeshPro>().text = Nazwa;
        Uzupe³nijOpis(SkruconyOpis);
        Uzupe³nijOpis(Opis);
    }

    public void Uzupe³nijOpis(List<textKartaTyp> tx) //AKTUALIZUJ PRZY ZMIANEI STATYSTYK!!!(bedzie podpiête pewnie pod event!!)
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
            publicznyPrzekszta³conyOpis = finalnyOpis;
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
            pozEnd = new Vector3(transform.localPosition.x, transform.localPosition.y + 1.8f, transform.localPosition.z - 0.5f);
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
            pozEndRamka = new Vector3(ramka.transform.localPosition.x, ramka.transform.localPosition.y + 0.5f, transform.localPosition.z - 0.5f);
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

    public void Na³urzEfekty(List<GameObject> celee)
    {
        if (celee[0].tag != "karta")
        {
            for (int x = 0; x < celee.Count; x++)
            {
                if (celee[x].tag == "wrug")
                {
                    for (int y = 0; y < efektyWrug.Count; y++)
                    {
                        Na³urzEfekt(celee[x], efektyWrug[y]);
                    }
                }
                else if (celee[x].tag == "Player")
                {
                    for (int y = 0; y < efektyGracz.Count; y++)
                    {
                        Na³urzEfekt(Eq.gameObject, efektyGracz[y]);
                    }
                }
            }
        }
        else
        {
            if (cele == Cele.KartyWD³oni || cele == Cele.Karta)
            {
                for (int x = 0; x < celee.Count; x++)
                {
                    for (int y = 0; y < efektyNaKarty.Count; y++)
                    {
                        EfektKartyPodaj(celee[x], efektyNaKarty[y]);
                    }
                }
            }
            else if (cele == Cele.RandomKartaWD³oni)
            {
                for (int n = 0; n < efektyNaKartyRandomRazy; n++)
                {
                    int z = Random.Range(0, celee.Count - 1);
                    for (int y = 0; y < efektyNaKarty.Count; y++)
                    {
                        EfektKartyPodaj(celee[z], efektyNaKarty[y]);
                    }
                }
            }
        }
    }

    public void Na³urzEfektyT(List<GameObject> cele)
    {
        if (cele[0].tag != "karta")
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
            }
        }
        else
        {
            if (celeNieZagranej == CeleNieZagranej.KartyWD³oni || celeNieZagranej == CeleNieZagranej.TaKarta)
            {
                for (int x = 0; x < cele.Count; x++)
                {
                    for (int y = 0; y < efektyNaKartyT.Count; y++)
                    {
                        EfektKartyPodaj(cele[x], efektyNaKartyT[y]);
                    }
                }
            }
            else if (celeNieZagranej == CeleNieZagranej.RandomKartaWD³oni)
            {
                for (int n = 0; n < efektyNaKartyRandomRazyT; n++)
                {
                    int z = Random.Range(0, cele.Count - 1);
                    for (int y = 0; y < efektyNaKartyT.Count; y++)
                    {
                        EfektKartyPodaj(cele[z], efektyNaKartyT[y]);
                    }
                }
            }
        }
    }

    public void EfektKartyPodaj(GameObject cell, nalurzEfektKarta ef)
    {
        taKarta ta = cell.GetComponent<taKarta>();

        nalurzEfektKarta nowyEf = new nalurzEfektKarta(ef.Cel,ef.Zalerznoœæ,ef.wartoœæ_enumPoz,ef.PrzemijanieEfektuKarty);

        if (ef.Zalerznoœæ == zalerznoœæ.brak_teraz && ef.PrzemijanieEfektuKarty == przemijanieEfektuKarty.brak_teraz) //efekty natychmiastowe !!
        {
            if (ef.Cel == cel.Wyklucz)
            {
                ta.WykluczTeKarte();
            }
            else if (ef.Cel == cel.Cmentarz)
            {
                ta.NaCmentarzTaKarta();
            }
            else if (ef.Cel == cel.dobierz)
            {
                GameObject m = GameObject.FindGameObjectWithTag("nadUiWalka").gameObject;
                m.transform.parent.transform.gameObject.GetComponent<walkaStart>().DobierzKarteRandom();
            }
            else if (ef.Cel == cel.KoniecTury)
            {
                GameObject m = GameObject.FindGameObjectWithTag("nadUiWalka").gameObject;
                m.GetComponent<click>().KonieTury();
            }
        }
        else
        {
            if (ef.Zalerznoœæ == zalerznoœæ.EnumZmiana) //zmiany w enumeratorach
            {
                int x = ef.wartoœæ_enumPoz;

                if (ef.Cel == cel.grywalonœæ)
                {
                    ta.grywalnoœæ = (Grywalnoœæ)x;
                }
                else if (ef.Cel == cel.poUrzyciu)
                {
                    ta.poUrzyciu = (PoUrzyciu)x;
                }
                else if (ef.Cel == cel.KoniecTury)
                {
                    ta.naKoniecTury = (PoUrzyciu)x;
                }
            }
            else
            {
                if (ef.Cel == cel.koszt)
                {
                    if (ef.Zalerznoœæ == zalerznoœæ.nowaWartoœæ)
                    {
                        nowyEf.wartoœæ_enumPoz = ef.wartoœæ_enumPoz - ta.Koszt;
                        ta.Koszt = ef.wartoœæ_enumPoz;
                    }
                    else if (ef.Zalerznoœæ == zalerznoœæ.PlusMinus)
                    {
                        ta.Koszt += ef.wartoœæ_enumPoz;
                    }
                }
                else if (ef.Cel == cel.kosztRandom && ef.Zalerznoœæ == zalerznoœæ.nowaWartoœæ)
                {
                    int z = Random.Range(0, ef.wartoœæ_enumPoz);
                    nowyEf.wartoœæ_enumPoz = z - ta.Koszt;
                    ta.Koszt = z;
                }
                else if (ef.Cel == cel.obrarzenia)
                {
                    if (ef.Zalerznoœæ == zalerznoœæ.nowaWartoœæ)
                    {
                        nowyEf.wartoœæ_enumPoz = ef.wartoœæ_enumPoz - (int)ta.Dmg;
                        ta.Dmg = (float)ef.wartoœæ_enumPoz;
                    }
                    else if (ef.Zalerznoœæ == zalerznoœæ.PlusMinus)
                    {
                        ta.Dmg += (float)ef.wartoœæ_enumPoz;
                    }
                }
                else if (ef.Cel == cel.obrarzeniaAll && ef.Zalerznoœæ == zalerznoœæ.PlusMinus)
                {
                    ta.Dmg += (float)ef.wartoœæ_enumPoz;
                    ta.DmgT += (float)ef.wartoœæ_enumPoz;
                    ta.DmgGraczowi += (float)ef.wartoœæ_enumPoz;
                    ta.DmgGraczowiT += (float)ef.wartoœæ_enumPoz;
                }
                else if (ef.Cel == cel.obrzarzeniaNegatyw && ef.Zalerznoœæ == zalerznoœæ.PlusMinus)
                {
                    ta.DmgGraczowi += (float)ef.wartoœæ_enumPoz;
                    ta.DmgGraczowiT += (float)ef.wartoœæ_enumPoz;
                }
            }
        }


        if (ef.PrzemijanieEfektuKarty == przemijanieEfektuKarty.tura)
        {
            ta.na³orzoneEfektyKartaTura.Add(nowyEf);
        }
        else if (ef.PrzemijanieEfektuKarty == przemijanieEfektuKarty.opuszczenieD³oni)
        {
            ta.na³orzoneEfektyKartaD³on.Add(nowyEf);
        }

        ta.Uzupelnij();
    }

    public void UsuwanieEfektówKartyTura(int nic) //wywo³ywane na koniec tury
    {
        UsuwanieEfektówKarty(na³orzoneEfektyKartaTura);
    }

    public void UsuwanieEfektówKarty(List<nalurzEfektKarta> listaE)
    {
        if (listaE.Count != 0)
        {
            for (int x = listaE.Count - 1; x >= 0; x--)
            {
                if (listaE[x].Zalerznoœæ == zalerznoœæ.EnumZmiana)
                {
                    if (listaE[x].Cel == cel.grywalonœæ)
                    {
                        grywalnoœæ = prefabTejKartyWdeck.GetComponent<taKarta>().grywalnoœæ;
                    }
                    else if (listaE[x].Cel == cel.poUrzyciu)
                    {
                        poUrzyciu = prefabTejKartyWdeck.GetComponent<taKarta>().poUrzyciu;
                    }
                    else if (listaE[x].Cel == cel.KoniecTury)
                    {
                        naKoniecTury = prefabTejKartyWdeck.GetComponent<taKarta>().naKoniecTury;
                    }
                }
                else
                {
                    if (listaE[x].Cel == cel.koszt || listaE[x].Cel == cel.kosztRandom)
                    {
                        Koszt -= listaE[x].wartoœæ_enumPoz;
                    }
                    else if (listaE[x].Cel == cel.obrarzenia)
                    {
                        Dmg -= (float)listaE[x].wartoœæ_enumPoz;
                    }
                    else if (listaE[x].Cel == cel.obrarzeniaAll)
                    {
                        Dmg -= (float)listaE[x].wartoœæ_enumPoz;
                        DmgT -= (float)listaE[x].wartoœæ_enumPoz;
                        DmgGraczowi -= (float)listaE[x].wartoœæ_enumPoz;
                        DmgGraczowiT -= (float)listaE[x].wartoœæ_enumPoz;
                    }
                    else if (listaE[x].Cel == cel.obrzarzeniaNegatyw)
                    {
                        DmgGraczowi -= (float)listaE[x].wartoœæ_enumPoz;
                        DmgGraczowiT -= (float)listaE[x].wartoœæ_enumPoz;
                    }
                }

                if (x == 0)
                {
                    if (listaE == na³orzoneEfektyKartaD³on)
                    {
                        na³orzoneEfektyKartaD³on = new List<nalurzEfektKarta>();
                    }
                    else if (listaE == na³orzoneEfektyKartaTura)
                    {
                        na³orzoneEfektyKartaTura = new List<nalurzEfektKarta>();
                    }
                    Uzupelnij();
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
                Efektu = new efekty(Biblioteka.dostêpneEfekty[x].nazwa, Biblioteka.dostêpneEfekty[x].odbiurEfektu, Biblioteka.dostêpneEfekty[x].sprite, Biblioteka.dostêpneEfekty[x].opis, Biblioteka.dostêpneEfekty[x].TypWywo³ania, Biblioteka.dostêpneEfekty[x].TypPrzemijania, Efektuuu.ile);
            }
        }

        if (Efektu.TypWywo³ania != typWywo³ania.natychmiastowy_odrazuPrzemija_bezLicznika)
        {
            if (Cel.tag == "wrug")
            {
                if (Cel.GetComponent<WRUG1>().na³orzoneEfekty.Count == 0 || Cel.GetComponent<WRUG1>().na³orzoneEfekty.All(a => a.nazwa != Efektu.nazwa))
                {
                    Cel.GetComponent<WRUG1>().na³orzoneEfekty.Add(Efektu);
                    Cel.GetComponent<bazaEfektow>().OtrzymanieEfektu(Efektu, 0);
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
                    Cel.GetComponent<bazaEfektow>().OtrzymanieEfektu(Efektu, 0);
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
        }
        else
        {
            if (Cel.tag == "wrug")
            {
                Cel.GetComponent<bazaEfektow>().OtrzymanieEfektu(Efektu, Efektuuu.ile);
            }
            else if(Cel.tag == "Player")
            {
                Cel.GetComponent<bazaEfektow>().OtrzymanieEfektu(Efektu, Efektuuu.ile);
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
                    Eq.PrzyjmijDmg(DmgGraczowi, true, this.gameObject);
                }
                else
                {
                    Eq.PrzyjmijDmg(DmgGraczowi, false, this.gameObject);
                }
            }
            else
            {
                if (specjalnyTypObrarzeñGracz.HasFlag(TypObrarzen.nieUchronne))
                {
                    Eq.PrzyjmijDmg((DmgGraczowi + Eq.bonusDoObrarzeñ), true , this.gameObject);
                }
                else
                {
                    Eq.PrzyjmijDmg((DmgGraczowi + Eq.bonusDoObrarzeñ), false, this.gameObject);
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
                    Eq.PrzyjmijDmg(DmgGraczowiT, true, this.gameObject);
                }
                else
                {
                    Eq.PrzyjmijDmg(DmgGraczowiT, false, this.gameObject);
                }
            }
            else
            {
                if (specjalnyTypObrarzeñGraczT.HasFlag(TypObrarzen.nieUchronne))
                {
                    Eq.PrzyjmijDmg((DmgGraczowiT + Eq.bonusDoObrarzeñ), true, this.gameObject);
                }
                else
                {
                    Eq.PrzyjmijDmg((DmgGraczowiT + Eq.bonusDoObrarzeñ), false, this.gameObject);
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
            script.Id = EditorGUILayout.IntField(label: "ID w bibliotece", script.Id);
            script.Koszt = EditorGUILayout.IntField(label: "Koszt", script.Koszt);
            script.KartaTag = (kartaTag)EditorGUILayout.EnumFlagsField(label: "Karta Tag", script.KartaTag);
            script.Rzadkoœæ = (rzadkoñæ)EditorGUILayout.EnumPopup(label: "Rzadkoœæ", script.Rzadkoœæ);
            script.GrafikaKarty = (Sprite)EditorGUILayout.ObjectField(label: "Sprite", script.GrafikaKarty, typeof(Sprite), true);

            EditorGUILayout.LabelField(" ");
            EditorGUILayout.PropertyField(serializedObject.FindProperty("SkruconyOpis"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("Opis"), true);
            if(script.na³orzoneEfektyKartaD³on.Count > 0 || script.na³orzoneEfektyKartaTura.Count > 0)
            {
                EditorGUILayout.LabelField("   ", "Tymczasowe zmiany w karcie:");
                if (script.na³orzoneEfektyKartaD³on.Count > 0)
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("na³orzoneEfektyKartaD³on"), true);
                }
                else if(script.na³orzoneEfektyKartaTura.Count > 0)
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("na³orzoneEfektyKartaTura"), true);
                }
            }
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
                else if (script.cele == Cele.Karta || script.cele == Cele.KartyWD³oni)
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("efektyNaKarty"), true);
                }
                else if (script.cele == Cele.RandomKartaWD³oni)
                {
                    script.efektyNaKartyRandomRazy = EditorGUILayout.IntSlider(label: "mnorznik random", script.efektyNaKartyRandomRazy, 1, 20);
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
                else if (script.celeNieZagranej == CeleNieZagranej.KartyWD³oni || script.celeNieZagranej == CeleNieZagranej.TaKarta)
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("efektyNaKartyT"), true);
                }
                else if(script.celeNieZagranej == CeleNieZagranej.RandomKartaWD³oni)
                {
                    script.efektyNaKartyRandomRazyT = EditorGUILayout.IntSlider(label: "mnorznik random", script.efektyNaKartyRandomRazyT, 1, 20);
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
