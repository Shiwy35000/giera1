using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class dialog : MonoBehaviour
{
    public int idRozmuwcy;
    public Vector3 poprawkaPozycjiDymku;
    private GameObject gracz;
    private GameObject obecnyDymek;
    [HideInInspector] public int poczatekDialogu = 0; //"0" domyœlnie;
    public List<typDialogowy> listaDialogowa = new List<typDialogowy>();
    public GameObject prefabDialogu;
    public GameObject dialogZnacznik;
    private GameObject WordCanvas;
    private GameObject camPoz;
    private GameObject walkaCanvasUi;
    private walkaStart WalkaStart;
    private playerEq playerEQ;
    private biblioteka Biblioteka;
    public static event System.Action<bool> Walka;
    public static event System.Action<bool> wDialogu;
    public static event System.Action<List<aso>> sklepOn;
    private bool czyEkwipunekOtwarty;

    [Header("Dodatkowe Akcje")]
    public List<objList> Przeciwnicy = new List<objList>();
    public List<grupaAso> SklepyAsortyment = new List<grupaAso>();
    private bool sklepOtwarty;
    // podawanie nowy dialog pocz¹tkowy ma tylko sens gdy koniczymy rozmowe lub wchodzimy do walki by odpali³o odpowiedni dialog po jej zakoñczeniu,
    // podonie co z walk¹ bedzie ze sklepem;
    // gdy koñczymy rozmowê dodatkowo nasze wybory w dialogach s¹ zapisywane by ruszy³o od podanego w kolejnej rozmowie;

    private void Awake()
    {
        gracz = GameObject.FindGameObjectWithTag("Player");
        playerEQ = gracz.GetComponent<playerEq>();
        WordCanvas = GameObject.FindGameObjectWithTag("WordCanvas");
        camPoz = GameObject.FindGameObjectWithTag("MainCamera");
        walkaCanvasUi = GameObject.FindGameObjectWithTag("nadUiWalka");
        Biblioteka = GameObject.FindGameObjectWithTag("saveGame").GetComponent<biblioteka>();
        WalkaStart = GameObject.FindGameObjectWithTag("camWalka").GetComponent<walkaStart>();
        dialogZnacznik.SetActive(false);

        clickNieWalka.ekwipunekWidoczny += CzyEkwipunekOtwarty;
        sklepOn += SklepOf;
    }
    private void OnDestroy()
    {
        clickNieWalka.ekwipunekWidoczny -= CzyEkwipunekOtwarty;
        sklepOn -= SklepOf;
    }
    void CzyEkwipunekOtwarty(bool czy)
    {
        czyEkwipunekOtwarty = czy;
    }

    void Update()
    {
        if (Input.GetButtonDown("LewyMysz") && dialogZnacznik.activeSelf == true && czyEkwipunekOtwarty == false && sklepOtwarty == false)
        {
            WczytajPoczatekDialogu();
            PodejmijDialog();
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if (other.gameObject.GetComponent<moveWASD>().wRozmowie == false)
            {
                dialogZnacznik.SetActive(true);
            }
            else
            {
                dialogZnacznik.SetActive(false);
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            dialogZnacznik.SetActive(false);
        }
    }

    void PodejmijDialog()
    {
        if (obecnyDymek == null)
        {
            wDialogu?.Invoke(true);
            obecnyDymek = Instantiate(prefabDialogu, WordCanvas.transform);
            obecnyDymek.GetComponent<czyjDymek>().wlasciciel = this.gameObject;
            obecnyDymek.GetComponent<czyjDymek>().WstawText(listaDialogowa[poczatekDialogu].tresc);

            if (poprawkaPozycjiDymku != Vector3.zero)
            {
                obecnyDymek.GetComponent<czyjDymek>().offset = poprawkaPozycjiDymku;
            }

            gracz.GetComponent<moveWASD>().wRozmowie = true;
            gracz.GetComponent<OpcjeDialogowe>().zKimPrzyjemnoscRozmawiac = this.gameObject;
            SprawdziWarunkiOpcjiDialogowych();
            gracz.GetComponent<OpcjeDialogowe>().WizualizujOdpowiedzi(listaDialogowa[poczatekDialogu].listaOdpowiedzi);
        }
    }

    public void OpdowiedziNaPytanie(int numerOdp)
    {
        if (listaDialogowa[poczatekDialogu].listaOdpowiedzi[numerOdp].reakcje.Count == 0) // koniec dialogu
        {
            obecnyDymek.GetComponent<czyjDymek>().KoniecDialogu();
            obecnyDymek = null;
            gracz.GetComponent<moveWASD>().wRozmowie = false;
            gracz.GetComponent<OpcjeDialogowe>().UsunOpcjeDialogowe();
            poczatekDialogu = listaDialogowa[poczatekDialogu].listaOdpowiedzi[numerOdp].nowyDialogPoczatkowy;


            if (playerEQ.dialogiWybory.Any(a => a.zKimDialogId == idRozmuwcy))
            {
                for (int x = 0; x < playerEQ.dialogiWybory.Count; x++)
                {
                    if(playerEQ.dialogiWybory[x].zKimDialogId == idRozmuwcy)
                    {
                        playerEQ.dialogiWybory[x].nowyStartDialogu = poczatekDialogu;
                    }
                }
            }
            else
            {
                playerEQ.dialogiWybory.Add(new nowyDialogTyp(idRozmuwcy, poczatekDialogu));
            }
            poczatekDialogu = 0;

            wDialogu?.Invoke(false);

        }
        else if(listaDialogowa[poczatekDialogu].listaOdpowiedzi[numerOdp].reakcje.Count >= 1)
        {
            List <reakcjaTyp> reakcje = listaDialogowa[poczatekDialogu].listaOdpowiedzi[numerOdp].reakcje;

            for (int x = 0; x < reakcje.Count; x++)
            {
                EfektOdp(reakcje[x], numerOdp);
            }
        }
    }

    private void EfektOdp(reakcjaTyp reakcja, int n)
    {
        if (reakcja.TypReakcji == typReakcji.dalszyDialog) //kolejny dialog
        {
            poczatekDialogu = reakcja.reakcjaUzupe³nienie;
            obecnyDymek.GetComponent<czyjDymek>().WstawText(listaDialogowa[poczatekDialogu].tresc);
            gracz.GetComponent<OpcjeDialogowe>().UsunOpcjeDialogowe();
            gracz.GetComponent<OpcjeDialogowe>().zKimPrzyjemnoscRozmawiac = this.gameObject;
            SprawdziWarunkiOpcjiDialogowych();
            gracz.GetComponent<OpcjeDialogowe>().WizualizujOdpowiedzi(listaDialogowa[poczatekDialogu].listaOdpowiedzi);
        }
        else if (reakcja.TypReakcji == typReakcji.walka) // walka!! 
        {
            if (listaDialogowa[poczatekDialogu].listaOdpowiedzi[n].nowyDialogPoczatkowy != 0) //zawsze podaj pocz¹tek dialogu po walce!!
            {
                poczatekDialogu = listaDialogowa[poczatekDialogu].listaOdpowiedzi[n].nowyDialogPoczatkowy;
            }
            PrzygotowaniaDoWalki(reakcja.reakcjaUzupe³nienie);
        }
        else if (reakcja.TypReakcji == typReakcji.sklep) //sklep
        {
            gracz.GetComponent<OpcjeDialogowe>().wizualizacjaWyboru.SetActive(false);
            wDialogu?.Invoke(false);
            sklepOn?.Invoke(SklepyAsortyment[reakcja.reakcjaUzupe³nienie].zawartoœæ);
            if (listaDialogowa[poczatekDialogu].listaOdpowiedzi[n].nowyDialogPoczatkowy != 0) //zawsze podaj pocz¹tek dialogu po walce!!
            {
                poczatekDialogu = listaDialogowa[poczatekDialogu].listaOdpowiedzi[n].nowyDialogPoczatkowy;
            }
        }
        else if (reakcja.TypReakcji == typReakcji.otrzymanieStrataZdrowia)
        {
            playerEQ.hp += reakcja.reakcjaUzupe³nienie;
            playerEQ.hpZasady();
        }
        else if (reakcja.TypReakcji == typReakcji.otrzymanieStrataMaxZdrowia)
        {
            playerEQ.hpMax += reakcja.reakcjaUzupe³nienie;
            playerEQ.hpZasady();
        }
        else if (reakcja.TypReakcji == typReakcji.otrzymanieStrataZ³ota)
        {
            playerEQ.sakiewka += reakcja.reakcjaUzupe³nienie;
            playerEQ.sakiewkaZasady();
        }
        else if (reakcja.TypReakcji == typReakcji.otrzymanieStrataKarty)
        {
            if(reakcja.reakcjaUzupe³nienie > 0)
            {
                playerEQ.deckPrefab.Add(Biblioteka.wszystkieKarty[reakcja.reakcjaUzupe³nienie]);
            }
            else if (reakcja.reakcjaUzupe³nienie < 0)
            {
                for (int x = 0; x < playerEQ.deckPrefab.Count; x++)
                {
                    if(playerEQ.deckPrefab[x].GetComponent<taKarta>().Id == Mathf.Abs(reakcja.reakcjaUzupe³nienie))
                    {
                        playerEQ.deckPrefab.Remove(playerEQ.deckPrefab[x]);
                        break;
                    }
                }
            }
        }
        else if (reakcja.TypReakcji == typReakcji.otrzymanieStrataArtefaktu)
        {
            if (reakcja.reakcjaUzupe³nienie > 0)
            {
                //playerEQ.posiadaneArtefakty.Add(Biblioteka.istniej¹ceArtefakty[reakcja.reakcjaUzupe³nienie]);
                playerEQ.ArtefaktPrzypisz(Biblioteka.istniej¹ceArtefakty[reakcja.reakcjaUzupe³nienie]);
            }
            else if (reakcja.reakcjaUzupe³nienie < 0)
            {
                playerEQ.UsuñArtefakt(Mathf.Abs(reakcja.reakcjaUzupe³nienie));
                /*for (int x = 0; x < playerEQ.posiadaneArtefakty.Count; x++)
                {
                    if (playerEQ.posiadaneArtefakty[x].GetComponent<artefakt>().Id == Mathf.Abs(reakcja.reakcjaUzupe³nienie))
                    {
                        playerEQ.posiadaneArtefakty.Remove(playerEQ.posiadaneArtefakty[x]);
                        break;
                    }
                }*/
            }
        }
    }

    public void PrzygotowaniaDoWalki(int zKim)
    {
        WalkaStart.SpawnPrzeciwinicy(Przeciwnicy[zKim].obj, this.gameObject);
        WalkaStart.CzyszczenieRêki();
        gracz.GetComponent<OpcjeDialogowe>().wizualizacjaWyboru.SetActive(false);
        walkaCanvasUi.SetActive(true);
        WalkaStart.turaGracza = true;
        Walka?.Invoke(true);
        wDialogu?.Invoke(false);
        WalkaStart.DodajKartyDoRêkiStart();
    }

    public void PoWalce()
    {
        obecnyDymek.GetComponent<czyjDymek>().WstawText(listaDialogowa[poczatekDialogu].tresc);
        gracz.GetComponent<OpcjeDialogowe>().UsunOpcjeDialogowe();
        gracz.GetComponent<OpcjeDialogowe>().zKimPrzyjemnoscRozmawiac = this.gameObject;
        SprawdziWarunkiOpcjiDialogowych();
        gracz.GetComponent<OpcjeDialogowe>().WizualizujOdpowiedzi(listaDialogowa[poczatekDialogu].listaOdpowiedzi);
        wDialogu?.Invoke(true);
        Walka?.Invoke(false);
        walkaCanvasUi.SetActive(false);
    }

    public void SklepOf(List<aso> nic)
    {
        if (nic.Count == 0)
        {
            sklepOtwarty = false;

            obecnyDymek.GetComponent<czyjDymek>().WstawText(listaDialogowa[poczatekDialogu].tresc);
            gracz.GetComponent<OpcjeDialogowe>().UsunOpcjeDialogowe();
            gracz.GetComponent<OpcjeDialogowe>().zKimPrzyjemnoscRozmawiac = this.gameObject;
            SprawdziWarunkiOpcjiDialogowych();
            gracz.GetComponent<OpcjeDialogowe>().WizualizujOdpowiedzi(listaDialogowa[poczatekDialogu].listaOdpowiedzi);
            wDialogu?.Invoke(true);
        }
        else
        {
            sklepOtwarty = true;
        }
    }

    private void WczytajPoczatekDialogu()
    {
        for (int x = 0; x < playerEQ.dialogiWybory.Count; x++)
        {
            if (playerEQ.dialogiWybory[x].zKimDialogId == idRozmuwcy)
            {
                poczatekDialogu = playerEQ.dialogiWybory[x].nowyStartDialogu;
            }
        }
    }

    private void SprawdziWarunkiOpcjiDialogowych()
    {
        for (int x = 0; x < listaDialogowa[poczatekDialogu].listaOdpowiedzi.Count; x++)
        {
            if(listaDialogowa[poczatekDialogu].listaOdpowiedzi[x].warunkiZaistnienia.Count == 0)
            {
                listaDialogowa[poczatekDialogu].listaOdpowiedzi[x].czyAktywna = true;
            }
            else
            {
                WarunkiKonkretnychOdp(x);
                if(listaDialogowa[poczatekDialogu].listaOdpowiedzi[x].warunkiZaistnienia.All(a => a.czySpelniony == true))
                {
                    listaDialogowa[poczatekDialogu].listaOdpowiedzi[x].czyAktywna = true;
                }
                else
                {
                    listaDialogowa[poczatekDialogu].listaOdpowiedzi[x].czyAktywna = false;
                }
            }
        }
    }

    void WarunkiKonkretnychOdp(int numerOdp)
    {
        List<warunekOdpowiedzi> warunki = listaDialogowa[poczatekDialogu].listaOdpowiedzi[numerOdp].warunkiZaistnienia;
        for (int y = 0; y < warunki.Count; y++)
        {
            if (warunki[y].czego == Czegoo.zdrowie)
            {
                if (warunki[y].IleZnak == Znak.wiêksze_posiada && warunki[y].Ile_Id < playerEQ.hp)
                {
                    warunki[y].czySpelniony = true;
                }
                else if(warunki[y].IleZnak == Znak.mniejsze_niePosida && warunki[y].Ile_Id > playerEQ.hp)
                {
                    warunki[y].czySpelniony = true;
                }
                else if (warunki[y].IleZnak == Znak.równe && warunki[y].Ile_Id == playerEQ.hp)
                {
                    warunki[y].czySpelniony = true;
                }
                else if (warunki[y].IleZnak == Znak.wiêkszeLubRówne && warunki[y].Ile_Id <= playerEQ.hp)
                {
                    warunki[y].czySpelniony = true;
                }
                else if (warunki[y].IleZnak == Znak.mniejszeLubRówne && warunki[y].Ile_Id >= playerEQ.hp)
                {
                    warunki[y].czySpelniony = true;
                }
                else
                {
                    warunki[y].czySpelniony = false;
                }
            }
            else if (warunki[y].czego == Czegoo.waluta)
            {
                if (warunki[y].IleZnak == Znak.wiêksze_posiada && warunki[y].Ile_Id < playerEQ.sakiewka)
                {
                    warunki[y].czySpelniony = true;
                }
                else if (warunki[y].IleZnak == Znak.mniejsze_niePosida && warunki[y].Ile_Id > playerEQ.sakiewka)
                {
                    warunki[y].czySpelniony = true;
                }
                else if (warunki[y].IleZnak == Znak.równe && warunki[y].Ile_Id == playerEQ.sakiewka)
                {
                    warunki[y].czySpelniony = true;
                }
                else if (warunki[y].IleZnak == Znak.wiêkszeLubRówne && warunki[y].Ile_Id <= playerEQ.sakiewka)
                {
                    warunki[y].czySpelniony = true;
                }
                else if (warunki[y].IleZnak == Znak.mniejszeLubRówne && warunki[y].Ile_Id >= playerEQ.sakiewka)
                {
                    warunki[y].czySpelniony = true;
                }
                else
                {
                    warunki[y].czySpelniony = false;
                }
            }
            else if(warunki[y].czego == Czegoo.karta)
            {
                if(warunki[y].IleZnak == Znak.wiêksze_posiada && playerEQ.deckPrefab.Any(x => x.GetComponent<taKarta>().Id == warunki[y].Ile_Id))
                {          
                    warunki[y].czySpelniony = true;           
                }
                else if(warunki[y].IleZnak == Znak.mniejsze_niePosida && playerEQ.deckPrefab.All(x => x.GetComponent<taKarta>().Id != warunki[y].Ile_Id))
                {
                    warunki[y].czySpelniony = true;
                }
                else
                {
                    warunki[y].czySpelniony = false;
                }
            }
            else if (warunki[y].czego == Czegoo.artefakt)
            {
                if (warunki[y].IleZnak == Znak.wiêksze_posiada && playerEQ.posiadaneArtefakty.Any(x => x.GetComponent<artefakt>().Id == warunki[y].Ile_Id))
                {
                    warunki[y].czySpelniony = true;
                }
                else if (warunki[y].IleZnak == Znak.mniejsze_niePosida && playerEQ.posiadaneArtefakty.All(x => x.GetComponent<artefakt>().Id != warunki[y].Ile_Id))
                {
                    warunki[y].czySpelniony = true;
                }
                else
                {
                    warunki[y].czySpelniony = false;
                }
            }
        }

    }
}
