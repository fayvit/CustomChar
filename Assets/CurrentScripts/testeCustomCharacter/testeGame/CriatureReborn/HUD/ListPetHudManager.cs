using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Criatures2021;
using FayvitCommandReader;
using FayvitBasicTools;
using TextBankSpace;
using FayvitMessageAgregator;

namespace Criatures2021Hud
{

    public class ListPetHudManager : MonoBehaviour
    {
        [SerializeField] private ListPetHud lPet;

        private PetBase[] lista;
        private CharacterManager mDoDono;
        
        private bool armagedom = false;
        private LocalState thisState = LocalState.emEspera;

        private enum LocalState
        { 
            emEspera,
            ativado,
            confirmationOpened,
            singleMessageOpened
        }

        public ICommandReader CurrentCommander => CommandReader.GetCR(AbstractGlobalController.Instance.Control);

        public void StartFields(CharacterManager dono,bool armagedom=false)
        {
            this.mDoDono = dono;
            this.armagedom = armagedom;
            thisState = LocalState.ativado;

            if (!armagedom)
            {
                this.lista = mDoDono.Dados.CriaturesAtivos.ToArray();
                lPet.StartHud(lista, OnSelectPet);
            }
            if (armagedom)
            {
                this.lista = mDoDono.Dados.CriaturesArmagedados.ToArray();
            }

            
        }


        // Use this for initialization
        void Start()
        {
            MessageAgregator<MsgOpenPetList>.AddListener(OnRequestStatThisHud);
        }

        private void OnDestroy()
        {
            MessageAgregator<MsgOpenPetList>.RemoveListener(OnRequestStatThisHud);
        }

        private void OnRequestStatThisHud(MsgOpenPetList obj)
        {
            StartFields(obj.dono, obj.armagedom);
        }

        // Update is called once per frame
        void Update()
        {
            switch (thisState)
            {
                case LocalState.ativado:
                    bool confirmInput = CurrentCommander.GetButtonDown(CommandConverterInt.confirmButton, true);
                    int vert = -CurrentCommander.GetIntTriggerDown(CommandConverterString.moveV) +
                        CurrentCommander.GetIntTriggerDown(CommandConverterString.alternativeV_Change);

                    lPet.Update(vert, confirmInput);
                break;
                case LocalState.confirmationOpened:
                    bool confirmInput_b = CurrentCommander.GetButtonDown(CommandConverterInt.confirmButton, true);
                    bool returninput_b = CurrentCommander.GetButtonDown(CommandConverterInt.returnButton, true);
                    int horiz = -CurrentCommander.GetIntTriggerDown(CommandConverterString.moveH) +
                        CurrentCommander.GetIntTriggerDown(CommandConverterString.alternativeH_Change);

                    AbstractGlobalController.Instance.Confirmation.ThisUpdate(horiz!=0, confirmInput_b, returninput_b);
                break;
                case LocalState.singleMessageOpened:
                    bool confirmInput_c = CurrentCommander.GetButtonDown(CommandConverterInt.confirmButton, true);
                    bool returninput_c = CurrentCommander.GetButtonDown(CommandConverterInt.returnButton, true);

                    AbstractGlobalController.Instance.OneMessage.ThisUpdate(confirmInput_c||returninput_c);
                break;
            }
        }

        //void QueroColocarEsse()
        //{
        //    if (OnSelectPet != null)
        //        OnSelectPet(transform.GetSiblingIndex() - 1);
        //    else
        //        Debug.LogError("A função hedeira não foi setada corretamente");
        //}

        public void QueroColocarEsse(int qual)
        {
            MessageAgregator<MsgRequestReplacePet>.Publish(new MsgRequestReplacePet()
            {
                dono = mDoDono.gameObject,
                replaceIndex = true,
                newIndex = qual - 1
            });

            thisState = LocalState.emEspera;
            lPet.FinishHud();

            //manager.Dados.CriatureSai = qual - 1;
            //fase = FaseDaDerrota.entrandoUmNovo;
            //replace = new ReplaceManager(manager, manager.CriatureAtivo.transform, FluxoDeRetorno.criature);

            Debug.LogError("Elementos de Hud");
            //GameController.g.HudM.EntraCriatures.FinalizarHud();
            //GameController.g.HudM.Painel.EsconderMensagem();
        }

        void DeVoltaAoMenu()
        {
            //GameController.g.HudM.EntraCriatures.PodeMudar = true;

            //ActionManager.ModificarAcao(GameController.g.transform, GameController.g.HudM.EntraCriatures.AcaoDeOpcaoEscolhida);

            thisState = LocalState.ativado;
            lPet.PodeMudar = true;
        }

        void OnSelectPet(int qual)
        {
            lPet.PodeMudar = false;
            string nomeCriature = lista[qual].GetNomeEmLinguas;
            string txtNivelNum = lista[qual].PetFeat.mNivel.Nivel.ToString();
            if (lista[qual].PetFeat.meusAtributos.PV.Corrente > 0)
            {
                string texto =
                    !armagedom ? string.Format(TextBank.RetornaListaDeTextoDoIdioma(TextKey.criatureParaMostrador)[0], nomeCriature)
                    : string.Format(TextBank.RetornaListaDeTextoDoIdioma(TextKey.frasesDeArmagedom)[7], nomeCriature, txtNivelNum);


                AbstractGlobalController.Instance.Confirmation.StartConfirmationPanel(()=> { QueroColocarEsse(qual); }, DeVoltaAoMenu,
                    texto
                    );
                thisState = LocalState.confirmationOpened;
                //if (cliqueDoPersonagem != null)
                //    cliqueDoPersonagem(transform.GetSiblingIndex() - 1);
            }
            else
            {
                AbstractGlobalController.Instance.OneMessage.StartMessagePanel(DeVoltaAoMenu,
                    string.Format(TextBank.RetornaListaDeTextoDoIdioma(TextKey.criatureParaMostrador)[1], nomeCriature)
                    );

                thisState = LocalState.singleMessageOpened;
            }
        }
    }
}
