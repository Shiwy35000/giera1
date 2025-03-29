using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class dialog : MonoBehaviour
{
    public int idRozmuwcy;
    public int indexDialogPoWalce;
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

    public static event System.Action<bool> Walka;

    void Start()
    {
        gracz = GameObject.FindGameObjectWithTag("Player");
        playerEQ = gracz.GetComponent<playerEq>();
        WordCanvas = GameObject.FindGameObjectWithTag("WordCanvas");
        camPoz = GameObject.FindGameObjectWithTag("MainCamera");
        dialogZnacznik.SetActive(false);
        walkaCanvasUi = GameObject.FindGameObjectWithTag("nadUiWalka");
        WalkaStart = walkaCanvasUi.transform.parent.gameObject.GetComponent<walkaStart>();
        walkaCanvasUi.SetActive(false);
    }

    void Update()
    {
        if (Input.GetButtonDown("LewyMysz") && dialogZnacznik.activeSelf == true)
        {
            WczytajPoczatekDialogu();
            PodejmijDialog();
        }
        if(WalkaStart.przeciwnicyWwalce.Count == 0 && walkaCanvasUi.activeSelf == true)
        {
            PoWalce();
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
        if (listaDialogowa[poczatekDialogu].listaOdpowiedzi[numerOdp].reakcje.Count == 0)
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
        if (reakcja.typReakcji > 0) //kolejny dialog
        {
            poczatekDialogu = reakcja.typReakcji;
            obecnyDymek.GetComponent<czyjDymek>().WstawText(listaDialogowa[poczatekDialogu].tresc);
            gracz.GetComponent<OpcjeDialogowe>().UsunOpcjeDialogowe();
            gracz.GetComponent<OpcjeDialogowe>().zKimPrzyjemnoscRozmawiac = this.gameObject;
            SprawdziWarunkiOpcjiDialogowych();
            gracz.GetComponent<OpcjeDialogowe>().WizualizujOdpowiedzi(listaDialogowa[poczatekDialogu].listaOdpowiedzi);
        }
        else if (reakcja.typReakcji == 0) //test
        {
            //Debug.Log("dzia³a!!");
        }
        else if (reakcja.typReakcji == -1) // walka!! 
        {
            poczatekDialogu = listaDialogowa[poczatekDialogu].listaOdpowiedzi[n].nowyDialogPoczatkowy;
            PrzygotowaniaDoWalki();
        }
    }

    public void PrzygotowaniaDoWalki()
    {
        WalkaStart.SpawnPrzeciwinicy(this.gameObject.GetComponent<zKimWalka>().przeciwnicy);
        WalkaStart.CzyszczenieRêki();
        gracz.GetComponent<OpcjeDialogowe>().wizualizacjaWyboru.SetActive(false);
        walkaCanvasUi.SetActive(true);
        WalkaStart.turaGracza = true;
        Walka?.Invoke(true);
        WalkaStart.DodajKartyDoRêkiStart();
    }

    public void PoWalce()
    {
        //jesli podamy inny indexDialogPoWalce to po walce bedzie ten dialog;
        //jeœli jednak wpiszemy jako nowy pocz¹tek dialogu zmieni nowy pocz¹tek dialogu (ma to sens);
        if (indexDialogPoWalce != 0)
        {
            poczatekDialogu = indexDialogPoWalce;
            obecnyDymek.GetComponent<czyjDymek>().WstawText(listaDialogowa[poczatekDialogu].tresc);
            gracz.GetComponent<OpcjeDialogowe>().UsunOpcjeDialogowe();
            gracz.GetComponent<OpcjeDialogowe>().zKimPrzyjemnoscRozmawiac = this.gameObject;
            SprawdziWarunkiOpcjiDialogowych();
            gracz.GetComponent<OpcjeDialogowe>().WizualizujOdpowiedzi(listaDialogowa[poczatekDialogu].listaOdpowiedzi);
        }
        else
        {
            obecnyDymek.GetComponent<czyjDymek>().KoniecDialogu();
            obecnyDymek = null;
            gracz.GetComponent<moveWASD>().wRozmowie = false;
            gracz.GetComponent<OpcjeDialogowe>().UsunOpcjeDialogowe();
            //poczatekDialogu = listaDialogowa[poczatekDialogu].listaOdpowiedzi[numerOdp].nowyDialogPoczatkowy;
        }
        Walka?.Invoke(false);
        walkaCanvasUi.SetActive(false);
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
        for (int y = 0; y < warunki.Count;)
        {
            if (warunki[y].Czego == "hp")
            {
                if (warunki[y].znak == ">x")
                {
                    //sprawdza czy hp > od warunki[y].Ile - nie napiszê bo narazie nie ma do czego siê odnieœæ;
                    //gdy powyrzysz warunek jest spe³niony wtedy y++; jesli nie break;!
                }
            }
            else if(warunki[y].Czego == "testTrue") //!
            {
                warunki[y].czySpelniony = true;
                y++;
            }
            else
            {
                warunki[y].czySpelniony = false;
                break;
            }
        }

    }
}
