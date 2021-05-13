using UnityEngine;
using System.Collections;
using FayvitMessageAgregator;
using TextBankSpace;

namespace TalkSpace
{
    [System.Serializable]
    public class TalkManagerBase
    {
        [SerializeField] protected Sprite fotoDoNPC;

        private TextKey chaveDaConversa = TextKey.bomDia;

        protected EstadoDoNPC estado = EstadoDoNPC.parado;
        protected string[] conversa;

        protected enum EstadoDoNPC
        {
            caminhando,
            parado,
            conversando,
            finalizadoComBotao
        }

        public void ChangeTalkKey(TextKey chave)
        {
            conversa = TextBank.RetornaListaDeTextoDoIdioma(chave).ToArray();
        }

        public void ChangeSpriteView(Sprite S)
        {
            fotoDoNPC = S;
        }

        public void Start()
        {
            conversa = TextBank.RetornaListaDeTextoDoIdioma(chaveDaConversa).ToArray();
        }

        // Update is called once per frame
        public virtual bool Update(bool inputNext, bool inputFinish)
        {
            switch (estado)
            {

                case EstadoDoNPC.conversando:

                    if (DisplayTextManager.instance.DisplayText.UpdateTexts(inputNext,inputFinish,conversa, fotoDoNPC))
                    {
                        FinalizaConversa();
                    }
                break;
                case EstadoDoNPC.finalizadoComBotao:
                    estado = EstadoDoNPC.parado;
                    return true;
            }

            return false;
        }

        protected virtual void FinalizaConversa()
        {
            estado = EstadoDoNPC.finalizadoComBotao;

            MessageAgregator<MsgFinishTextDisplay>.Publish();

        }

        public virtual void IniciaConversa()
        {
            MessageAgregator<MsgStartTextDisplay>.Publish();
            DisplayTextManager.instance.DisplayText.StartTextDisplay();

            estado = EstadoDoNPC.conversando;
        }
    }

    public struct MsgFinishTextDisplay : IMessageBase { }
    public struct MsgStartTextDisplay : IMessageBase { }
}