﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using CriaturesLegado;

public class InitialSceneManager : MonoBehaviour
{
    public static InitialSceneManager i;

    [SerializeField]private GameObject btnNovoJogo;
    [SerializeField]private GameObject btnCarregarJogo;
    [SerializeField]private GameObject btnPrimeiroJogo;
    [SerializeField]private InputTextDoCriandoNovoJogo pDoInput;
    [SerializeField]private ContainerDeLoadSaves containerDeLoads;
    [SerializeField]private PainelUmaMensagem umaMensagem;
    [SerializeField]private PainelDeConfirmacao confirmacao;
    [SerializeField] private MenuBasico menuBasico;
   // [SerializeField]private SceneLoader loadScene;

    private LoadAndSaveGame salvador = new LoadAndSaveGame();
    private List<PropriedadesDeSave> lista;
    private CommandReader commandR = new CommandReader();
    private EstadosDoIniciar estado = EstadosDoIniciar.escolhaInicial;

    private enum EstadosDoIniciar
    {
        escolhaInicial,
        painelSuspenso,
        saveLoadsAberto
    }

    /*
    public SceneLoader LoadScene
    {
        get { return loadScene; }
    }*/
    
    public PainelDeConfirmacao Confirmacao
    {
        get { return confirmacao; }
    }

    public PainelUmaMensagem UmaMensagem
    {
        get { return umaMensagem; }
    }

    // Use this for initialization
    void Awake()
    {        
        i = this;
        GameController.g = null;      
        
        VerificaIdioma();
        AtualizaLista();
    }

    void VerificaIdioma()
    {
        
        // object o = (salvador.CarregarArquivo("chosenLanguage.ori"));

        //if (o != null)
        //{
            BancoDeTextos.linguaChave = SaveDatesForJolt.s.ChosenLanguage;//(idioma)o;

           

            InterfaceLanguageConverter[] ilc = FindObjectsOfType<InterfaceLanguageConverter>();

            foreach (InterfaceLanguageConverter I in ilc)
            {
                I.MudaTexto();
            }
        //}

        
    }

    public void AtualizaLista()
    {
        lista = SaveDatesForJolt.s.SaveProps;
        //lista = (List<PropriedadesDeSave>)(salvador.CarregarArquivo("criaturesGames.ori")); //SaveDatesForJolt.s.SaveProps;

        bool primeiro = true;

        if (lista != null)
            if (lista.Count > 0)
            {
                primeiro = false;
                lista.Sort();
            }

        /*
        btnNovoJogo.SetActive(!primeiro);
        btnCarregarJogo.SetActive(!primeiro);
        btnPrimeiroJogo.SetActive(primeiro);
        */

        menuBasico.FinalizarHud();
        if(primeiro)
            menuBasico.IniciarHud(EscolhaDoMenuInicial, new string[1] {
                BancoDeTextos.RetornaTextoDeInterface(InterfaceTextKey.iniciarJogo)
            });
        else
            menuBasico.IniciarHud(EscolhaDoMenuInicial, new string[2] {
                BancoDeTextos.RetornaTextoDeInterface(InterfaceTextKey.novoJogo),
            BancoDeTextos.RetornaTextoDeInterface(InterfaceTextKey.carregarJogo)
            });
    }

    void EscolhaDoMenuInicial(int e)
    {
        switch (e)
        {
            case 0:
                //BotaoNovoJogo();
                pDoInput.CriandoJogo();
                estado = EstadosDoIniciar.painelSuspenso; 
            break;
            case 1:
                BotaoCarregarJogo();
            break;
        }

        menuBasico.FinalizarHud();
    }



    // Update is called once per frame
    void Update()
    {
        switch (estado)
        {
            case EstadosDoIniciar.escolhaInicial:
                menuBasico.MudarOpcao();

                if (commandR.DisparaAcao())
                {
                    EscolhaDoMenuInicial(menuBasico.OpcaoEscolhida);
                }

                if (Input.GetButtonDown("trocaCriature"))
                    FindObjectOfType<LanguageSwitcher>().FuncaoDoBotao();

            break;
            case EstadosDoIniciar.saveLoadsAberto:
                containerDeLoads.MudarOpcao();
                if (commandR.DisparaAcao())
                {
                    EscolhiSave(containerDeLoads.OpcaoEscolhida);
                }
                else if (commandR.DisparaCancel())
                {
                    FecharLoadContainer();
                    estado = EstadosDoIniciar.escolhaInicial;
                }
                else if (Input.GetButtonDown("trocaCriature"))
                {
                    LoadButton[] btnsLoad = FindObjectsOfType<LoadButton>();
                    btnsLoad[btnsLoad.Length-1- containerDeLoads.OpcaoEscolhida].BotaoExcluir();

                    Debug.Log(containerDeLoads.OpcaoEscolhida+" : "+ btnsLoad[btnsLoad.Length-1- containerDeLoads.OpcaoEscolhida].name);

                    estado = EstadosDoIniciar.painelSuspenso;
                }


            break;
        }
    }

    public void BotaoNovoJogo()
    {
        BtnsManager.DesligarBotoes(btnCarregarJogo.transform.parent.gameObject);
        pDoInput.Iniciar();
    }

    public void BotaoCarregarJogo()
    {
        //BtnsManager.DesligarBotoes(btnCarregarJogo.transform.parent.gameObject);
        containerDeLoads.IniciarHud(EscolhiSave,EscolhiDelete, lista.ToArray());
        estado = EstadosDoIniciar.saveLoadsAberto;
    }

    void EscolhiDelete(int indice)
    {
        FecharLoadContainer();

        PropriedadesDeSave p = lista[indice];
        //lista = (List<PropriedadesDeSave>)(salvador.CarregarArquivo("criaturesGames.ori")); 
        lista = SaveDatesForJolt.s.SaveProps;

        //salvador.ExcluirArquivo("criatures.ori" + p.indiceDoSave);

        lista.Remove(p);


        //salvador.SalvarArquivo("criaturesGames.ori", lista);
        SaveAndLoadInJolt.Save();

        lista.Sort();

        if (lista.Count > 0)
            BotaoCarregarJogo();
        else
        {
            estado = EstadosDoIniciar.escolhaInicial;
            AtualizaLista();
        }

    }

    void EscolhiSave(int indice)
    {
        PropriedadesDeSave p = lista[indice];

        lista = SaveDatesForJolt.s.SaveProps;
        //lista = (List<PropriedadesDeSave>)(salvador.CarregarArquivo("criaturesGames.ori"));
        indice = lista.IndexOf(p);

        lista[indice] =new PropriedadesDeSave()
        {
            ultimaJogada = System.DateTime.Now,
            nome = lista[indice].nome,
            indiceDoSave = lista[indice].indiceDoSave
        };

        //salvador.SalvarArquivo("criaturesGames.ori", lista);
        SaveAndLoadInJolt.Save();

        containerDeLoads.FinalizarHud();
        GameObject G = new GameObject();
        SceneLoader loadScene = G.AddComponent<SceneLoader>();
        loadScene.CenaDoCarregamento(lista[indice].indiceDoSave);

    }

    public void FecharLoadContainer()
    {
        BtnsManager.ReligarBotoes(btnCarregarJogo.transform.parent.gameObject);
        containerDeLoads.FinalizarHud();
        menuBasico.FinalizarHud();
        estado = EstadosDoIniciar.escolhaInicial;
        AtualizaLista();
    }

    public void FecharInputText()
    {
        BtnsManager.ReligarBotoes(btnCarregarJogo.transform.parent.gameObject);
        pDoInput.gameObject.SetActive(false);
        AtualizaLista();
    }

    public void EstadoDeRetornandoAoSave()
    {
        estado = EstadosDoIniciar.saveLoadsAberto;
    }

    public void EstadoDePainelSuspenso()
    {
        estado = EstadosDoIniciar.painelSuspenso;
    }

    public void EstadoDeEscolhaInicial()
    {
        estado = EstadosDoIniciar.escolhaInicial;
    }
}

/*
[System.Serializable]
public struct PropriedadesDeSave:System.IComparable
{
    public string nome;
    public int indiceDoSave;
    public System.DateTime ultimaJogada;

    public int CompareTo(object obj)
    {
        return System.DateTime.Compare(((PropriedadesDeSave)obj).ultimaJogada,ultimaJogada);
    }
}*/
