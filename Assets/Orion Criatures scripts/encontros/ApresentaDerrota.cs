﻿using UnityEngine;
using System.Collections;

public class ApresentaDerrota
{
    private float contadorDeTempo = 0;
    private string[] textos;

    private FaseDaDerrota fase = FaseDaDerrota.emEspera;
    private CharacterManager manager;
    private CreatureManager inimigoDerrotado;
    private ReplaceManager replace;

    private const float TEMPO_PARA_MOSTRAR_MENSAGEM_INICIAL = 0.25F;
    private const float TEMPO_PARA_ESCURECER = 2;

    private enum FaseDaDerrota
    {
        emEspera,
        abreMensagem,
        esperandoFecharMensagemDeDerrota,
        entrandoUmNovo,
        mensDoArmagedom,
        tempoParaCarregarCena,
        hudEntraCriatureAberta
    }

    public enum RetornoDaDerrota
    {
        atualizando,
        voltarParaPasseio,
        deVoltaAoArmagedom
    }

    public ApresentaDerrota(CharacterManager manager, CreatureManager inimigoDerrotado)
    {
        this.manager = manager;
        this.inimigoDerrotado = inimigoDerrotado;
        textos = BancoDeTextos.RetornaListaDeTextoDoIdioma(ChaveDeTexto.apresentaDerrota).ToArray();
        fase = FaseDaDerrota.abreMensagem;
    }

    void AcaoDaMensagemDerrota()
    {
        fase = FaseDaDerrota.esperandoFecharMensagemDeDerrota;
    }

    public RetornoDaDerrota Update()
    {
        switch (fase)
        {
            case FaseDaDerrota.abreMensagem:
                contadorDeTempo += Time.deltaTime;
                if (contadorDeTempo > TEMPO_PARA_MOSTRAR_MENSAGEM_INICIAL)
                {
                    GameController.g.HudM.Painel.AtivarNovaMens(string.Format(textos[0],
                        manager.CriatureAtivo.MeuCriatureBase.NomeEmLinguas),30);
                    fase = FaseDaDerrota.esperandoFecharMensagemDeDerrota;
                }
            break;
            case FaseDaDerrota.esperandoFecharMensagemDeDerrota:
                 if(GameController.g.CommandR.DisparaAcao())
                {
                    GameController.g.HudM.Painel.EsconderMensagem();
                    if (manager.Dados.TemCriatureVivo())
                    {
                        GameController.g.HudM.Painel.AtivarNovaMens(textos[1], 20);
                        GameController.g.HudM.EntraCriatures.IniciarEssaHUD(manager.Dados.CriaturesAtivos.ToArray(),AoEscolherUmCriature);
                        fase = FaseDaDerrota.hudEntraCriatureAberta;
                    }
                    else
                    {
                        GameController.g.HudM.Painel.AtivarNovaMens(textos[2],20);
                        fase = FaseDaDerrota.mensDoArmagedom;
                        // Aqui vamos de volta para o armagedom
                        //return RetornoDaDerrota.deVoltaAoArmagedom;
                    }
                }
            break;
            case FaseDaDerrota.mensDoArmagedom:
                if (GameController.g.CommandR.DisparaAcao())
                {
                    GameController.g.HudM.Painel.EsconderMensagem();
                    GameController.g.gameObject.AddComponent<FadeView>();
                    contadorDeTempo = 0;
                    fase = FaseDaDerrota.tempoParaCarregarCena;
                }
            break;
            case FaseDaDerrota.tempoParaCarregarCena:
                contadorDeTempo += Time.deltaTime;
                if (contadorDeTempo > TEMPO_PARA_ESCURECER)
                {
                    CharacterManager manager = GameController.g.Manager;
                    VisitasParaArmagedom V = LocalizacaoDeArmagedoms.L[manager.Dados.UltimoArmagedom];
                    manager.transform.position = V.Endereco;//manager.Dados.UltimoArmagedom.posHeroi;
                    manager.transform.rotation = Quaternion.identity;
                    manager.Dados.TodosCriaturesPerfeitos();
                    GameController.g.Salvador.SalvarAgora(V.nomeDasCenas);
                    GameObject G = new GameObject();
                    SceneLoader loadScene = G.AddComponent<SceneLoader>();
                    loadScene.CenaDoCarregamento(GameController.g.Salvador.IndiceDoJogoAtual);
                }
            break;
            case FaseDaDerrota.entrandoUmNovo:
                if (replace.Update())
                {
                    if (GameController.g.InimigoAtivo)
                    {
                        manager.AoCriature(inimigoDerrotado);
                        GameController.g.HudM.AtualizaDadosDaHudVida(true);
                    }

                    fase = FaseDaDerrota.emEspera;
                    return RetornoDaDerrota.voltarParaPasseio;
                }
            break;
            case FaseDaDerrota.hudEntraCriatureAberta:
                GameController.g.HudM.EntraCriatures.Update();
            break;
        }

        return RetornoDaDerrota.atualizando;
    }

    public void AoEscolherUmCriature(int qual)
    {
        manager.Dados.CriatureSai = qual-1;
        fase = FaseDaDerrota.entrandoUmNovo;        
        replace = new ReplaceManager(manager,manager.CriatureAtivo.transform,FluxoDeRetorno.criature);
        GameController.g.HudM.EntraCriatures.FinalizarHud();
        GameController.g.HudM.Painel.EsconderMensagem();
    }
}
