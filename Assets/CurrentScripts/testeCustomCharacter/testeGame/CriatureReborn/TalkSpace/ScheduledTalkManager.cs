using UnityEngine;
using System.Collections;
using FayvitBasicTools;
using TextBankSpace;

namespace TalkSpace
{
    [System.Serializable]
    public class ScheduledTalkManager : TalkManagerBase
    {
        #region inspector
        [SerializeField] private FalasAgendaveis[] falas = null;
        #endregion

        private int ultimoIndice = -1;

        [System.Serializable]
        private class FalasAgendaveis
        {
            [SerializeField] private KeyShift chaveCondicionalDaConversa;
            [SerializeField] private TextKey chaveDeTextoDaConversa;
            [SerializeField] private int repetir = 0;

            public KeyShift ChaveCondicionalDaConversa
            {
                get { return chaveCondicionalDaConversa; }
                set { chaveCondicionalDaConversa = value; }
            }

            public TextKey ChaveDeTextoDaConversa
            {
                get { return chaveDeTextoDaConversa; }
                set { chaveDeTextoDaConversa = value; }
            }

            public int Repetir { get { return repetir; } set { repetir = value; } }
        }

        void VerificaQualFala()
        {
            KeyVar myKeys = AbstractGameController.Instance.MyKeys;

            Debug.Log("ultimo indice no inicio: " + ultimoIndice);


            int indiceFinal = ultimoIndice > 0 ? Mathf.Min(ultimoIndice, falas.Length) : falas.Length;


            for (int i = 0; i < indiceFinal; i++)
            {
                if (myKeys.VerificaAutoShift(falas[i].ChaveCondicionalDaConversa))
                {

                    conversa = TextBank.RetornaListaDeTextoDoIdioma(falas[i].ChaveDeTextoDaConversa).ToArray();
                    ultimoIndice = i;
                }
            }

            Debug.Log(indiceFinal + " : " + ultimoIndice);

            if (falas[ultimoIndice].Repetir >= 0)
            {
                string kCont = falas[ultimoIndice].ChaveCondicionalDaConversa.ToString();

                myKeys.SomaAutoCont(kCont, 1);
                if (falas[ultimoIndice].Repetir < myKeys.VerificaAutoCont(kCont))
                    myKeys.MudaShift(falas[ultimoIndice].ChaveCondicionalDaConversa, false);

            }

        }

        override public void IniciaConversa()
        {
            VerificaQualFala();
            base.IniciaConversa();
        }
    }
}