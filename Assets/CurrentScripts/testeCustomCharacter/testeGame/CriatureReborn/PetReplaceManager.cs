using UnityEngine;
using System.Collections;
using FayvitMessageAgregator;

namespace Criatures2021
{
    public class PetReplaceManager : MonoBehaviour
    {
        private AnimateArm animaB;
        private FluxoDeRetorno fluxo;
        private PetBase petToGoOut;
        private Transform hero;
        // Use this for initialization
        void Start()
        {

        }

        public void StartReplace(Transform hero, Transform alvo, FluxoDeRetorno fluxo,PetBase petToGoOut)
        {
            this.fluxo = fluxo;
            this.petToGoOut = petToGoOut;
            this.hero = hero;
            #region suprimido
            //List<CriatureBase> lista = manager.Dados.CriaturesAtivos;
            //CriatureBase temp = lista[0];
            //lista[0] = lista[manager.Dados.CriatureSai + 1];
            //lista[manager.Dados.CriatureSai + 1] = temp;

            //manager.Estado = EstadoDePersonagem.parado;
            //manager.CriatureAtivo.PararCriatureNoLocal();
            //manager.Mov.Animador.PararAnimacao();

            //GameController.g.HudM.ModoLimpo();
            //GameController.g.HudM.Painel.EsconderMensagem();

            //estouTrocandoDeCriature = true;
            #endregion
            animaB = new AnimateArm(hero, alvo);
            MessageAgregator<MsgStartReplacePet>.Publish(new MsgStartReplacePet()
            {
                dono = hero.gameObject
            });
        }

        public void Update()
        {

            if (!animaB.AnimaTroca())
            {
                if (!animaB.AnimaEnvia(petToGoOut))
                {
                    if (fluxo == FluxoDeRetorno.heroi || fluxo == FluxoDeRetorno.menuHeroi)
                    {
                        MessageAgregator<MsgChangeToHero>.Publish(new MsgChangeToHero() { myHero = hero.gameObject });
                        //manager.AoHeroi();
                    }
                    else if (fluxo == FluxoDeRetorno.criature || fluxo == FluxoDeRetorno.menuCriature)
                    {
                        MessageAgregator<MsgRequestChangeToPetByReplace>.Publish(new MsgRequestChangeToPetByReplace()
                        {
                            dono=hero.gameObject,
                            fluxo = fluxo
                        });

                    }

                    //manager.Mov.Animador.ResetaTroca();
                    //manager.Mov.Animador.ResetaEnvia();
                    //estouTrocandoDeCriature = false;
                    Destroy(this);
                }
            }
        }
    }

    public struct MsgRequestChangeToPetByReplace : IMessageBase
    {
        public GameObject dono;
        public FluxoDeRetorno fluxo;
    }

    public struct MsgStartReplacePet : IMessageBase
    {
        public GameObject dono;
    }
}