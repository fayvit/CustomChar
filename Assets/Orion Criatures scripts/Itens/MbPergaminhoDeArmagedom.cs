﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class MbPergaminhoDeArmagedom : MbItens
{
    private int opcaoEscolhida = -1;
    //private float contadorDeTempo = 0;
    private FluxoDeRetorno fluxo;
    public MbPergaminhoDeArmagedom(int estoque = 1) : base(new ContainerDeCaracteristicasDeItem(nomeIDitem.pergArmagedom)
    {
        valor = 500
    }
        )
    {
        Estoque = estoque;
    }

    public override void IniciaUsoDeMenu(GameObject dono)
    {
        if (!GameController.g.estaEmLuta)
        {
            fluxo = FluxoDeRetorno.menuCriature;
            GameController.g.HudM.Menu_Basico.IniciarHud(OpcaoDeArmagedomescolhida, OpcoesDeArmagedom());
            Estado = EstadoDeUsoDeItem.selecaoDeItem;
        }
        else
        {
            Estado = EstadoDeUsoDeItem.emEspera;
            GameController.g.HudM.UmaMensagem.ConstroiPainelUmaMensagem(FecharMensagem, BancoDeTextos.RetornaFraseDoIdioma(ChaveDeTexto.mensLuta));
        }
    }

    public override void IniciaUsoComCriature(GameObject dono)
    {
        fluxo = FluxoDeRetorno.heroi;
        if (GameController.g.estaEmLuta)
        {
            GameController.g.HudM.Painel.AtivarNovaMens(BancoDeTextos.RetornaListaDeTextoDoIdioma(ChaveDeTexto.mensLuta)[0], 25, 2);
            Estado = EstadoDeUsoDeItem.finalizaUsaItem;
        }
        else
        {
            Estado = EstadoDeUsoDeItem.selecaoDeItem;

            if(GameController.g.Manager.Estado==EstadoDePersonagem.comMeuCriature)
                GameController.g.Manager.CriatureAtivo.Estado = CreatureManager.CreatureState.parado;

            GameController.EntrarNoFluxoDeTexto();
            GameController.g.HudM.Menu_Basico.IniciarHud(OpcaoDeArmagedomescolhida, OpcoesDeArmagedom());
        }
    }

    public override void IniciaUsoDeHeroi(GameObject dono)
    {
        GameController.g.Manager.Estado = EstadoDePersonagem.parado;
        IniciaUsoComCriature(dono);
    }

    string[] OpcoesDeArmagedom()
    {
        List<string> retorno = new List<string>();
        IndiceDeArmagedoms[] Vs = GameController.g.MyKeys.LocalArmag.ToArray();
        for (int i = 0; i < Vs.Length; i++)
            retorno.Add(VisitasParaArmagedom.NomeEmLinguas(Vs[i]));
        

        retorno.Add(BancoDeTextos.RetornaFraseDoIdioma(ChaveDeTexto.Voltar));

        //GameController.g.HudM.PauseM.gameObject.SetActive(false);
        GameController.g.HudM.MenuDePause.EsconderPainelDeItens();
        GameController.g.HudM.Painel.AtivarNovaMens(BancoDeTextos.RetornaFraseDoIdioma(ChaveDeTexto.viajarParaArmagedom),25);
        return retorno.ToArray();
    }

    void OpcaoDeArmagedomescolhida(int escolha)
    {
        GameController.g.HudM.Menu_Basico.FinalizarHud();
        IndiceDeArmagedoms[] Vs = GameController.g.MyKeys.LocalArmag.ToArray();
        if (escolha >= Vs.Length)
        {
            if (fluxo == FluxoDeRetorno.heroi || fluxo == FluxoDeRetorno.criature)
            {
                Estado = EstadoDeUsoDeItem.finalizaUsaItem;
                //AndroidController.a.LigarControlador();
                //GameController.g.HudM.ligarControladores();
            }
            else
            {
                //GameController.g.HudM.PauseM.gameObject.SetActive(true);
                GameController.g.HudM.MenuDePause.BotaoItens();
                GameController.g.HudM.MenuDePause.ReligarBotoesDoPainelDeItens();
                Estado = EstadoDeUsoDeItem.finalizaUsaItem;
            }
        }
        else
        {
            Estado = EstadoDeUsoDeItem.aplicandoItem;
            opcaoEscolhida = escolha;
            Time.timeScale = 1;
            GeiserVermelho(GameController.g.Manager.CriatureAtivo.transform.position);
            RetirarUmItem(GameController.g.Manager, this,1);
            AplicadorDeCamera.cam.FocarBasica(GameController.g.Manager.CriatureAtivo.transform,10,10);
            TempoDecorrido = 0;
        }

        //GameController.g.HudM.Menu_Basico.FinalizarHud();
        GameController.g.HudM.Painel.EsconderMensagem();

    }

    void GeiserVermelho(Vector3 pos)
    {
        GameObject geiser = (GameObject)MonoBehaviour.Instantiate(
            GameController.g.El.retorna("geiserDeEnergia"),
            pos,
            GameController.g.El.retorna("geiserDeEnergia").transform.rotation);

        MonoBehaviour.Destroy(geiser, 3);

        
        ParticleSystem P = geiser.GetComponent<ParticleSystem>();
        var v = P.main;
        v.startColor = Color.red;
        //P.main = v;
        //P.startColor = Color.red;
    }

    public override bool AtualizaUsoDeMenu()
    {
        return AtualizaUsoDoPergaminho();
    }

    public override bool AtualizaUsoComCriature()
    {
        return AtualizaUsoDoPergaminho();
    }

    public override bool AtualizaUsoDeHeroi()
    {
        return AtualizaUsoDoPergaminho();
    }

    bool AtualizaUsoDoPergaminho()
    {
        switch (Estado)
        {
            case EstadoDeUsoDeItem.aplicandoItem:
                GameController.g.Manager.CriatureAtivo.transform.position += 0.4f * Time.deltaTime * Vector3.up;
                TempoDecorrido += Time.deltaTime;
                if (TempoDecorrido > 2)
                {
                    SkinnedMeshRenderer[] sKs = GameController.g.Manager.CriatureAtivo.GetComponentsInChildren<SkinnedMeshRenderer>();
                    foreach (SkinnedMeshRenderer sk in sKs)
                        sk.enabled = false;

                    GeiserVermelho(GameController.g.Manager.transform.position);
                    AplicadorDeCamera.cam.FocarBasica(GameController.g.Manager.transform, 10, 10);
                    Estado = EstadoDeUsoDeItem.animandoBraco;
                    TempoDecorrido = 0;
                }
            break;
            case EstadoDeUsoDeItem.animandoBraco://Gambiarra para não criar um novo estado
                TempoDecorrido += Time.deltaTime;
                GameController.g.Manager.transform.position += 0.4f * Time.deltaTime * Vector3.up;
                if (TempoDecorrido > 2)
                {
                    SkinnedMeshRenderer[] sKs = GameController.g.Manager.GetComponentsInChildren<SkinnedMeshRenderer>();
                    foreach (SkinnedMeshRenderer sk in sKs)
                        sk.enabled = false;
                    Estado = EstadoDeUsoDeItem.emEspera;
                    EntraFaseDoTransporte();
                }
            break;
            case EstadoDeUsoDeItem.selecaoDeItem:
                MenuBasico m = GameController.g.HudM.Menu_Basico;
                m.MudarOpcao();
                if (GameController.g.CommandR.DisparaCancel())
                {
                    if (fluxo == FluxoDeRetorno.criature || FluxoDeRetorno.heroi == fluxo)
                    {
                        ActionManager.ModificarAcao(GameController.g.transform, null);
                        GameController.g.HudM.Painel.EsconderMensagem();
                        
                        Estado = EstadoDeUsoDeItem.finalizaUsaItem;
                    }
                    else if (fluxo == FluxoDeRetorno.menuCriature || FluxoDeRetorno.menuHeroi == fluxo)
                    {
                        GameController.g.HudM.MenuDePause.ReligarBotoesDoPainelDeItens();
                        Estado = EstadoDeUsoDeItem.finalizaUsaItem;
                    }

                    m.FinalizarHud();
                }

                if (GameController.g.CommandR.DisparaAcao())
                {
                    OpcaoDeArmagedomescolhida(m.OpcaoEscolhida);
                }
            break;
            case EstadoDeUsoDeItem.finalizaUsaItem:
                return false;
        }

        return true;
    }

    void EntraFaseDoTransporte()
    {
        GameController.g.gameObject.AddComponent<FadeView>();
        GameController.g.StartCoroutine(VoltaArmagedom());
    }

    IEnumerator VoltaArmagedom()
    {
        yield return new WaitForSeconds(1);
        VisitasParaArmagedom V = LocalizacaoDeArmagedoms.L[GameController.g.MyKeys.LocalArmag[opcaoEscolhida]];

        CharacterManager manager = GameController.g.Manager;
        manager.transform.position = V.Endereco;
        GameController.g.Salvador.SalvarAgora(V.nomeDasCenas);
        GameObject G = new GameObject();
        SceneLoader loadScene = G.AddComponent<SceneLoader>();
        loadScene.CenaDoCarregamento(GameController.g.Salvador.IndiceDoJogoAtual);
    }
}
