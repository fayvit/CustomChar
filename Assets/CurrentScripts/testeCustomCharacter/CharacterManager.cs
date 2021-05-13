using FayvitBasicTools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FayvitCommandReader;
using FayvitCam;
using FayvitMove;
using FayvitMessageAgregator;
using Criatures2021Hud;
using TalkSpace;
using System;

namespace Criatures2021
{
    public class CharacterManager : MonoBehaviour, ICharacterManager
    {
        [SerializeField] private BasicMove mov;
        [SerializeField] private DadosDeJogador dados;

        public CharacterState ThisState { get; private set; } = CharacterState.notStarted;
        public PetManager ActivePet { get; private set; }
        private ICommandReader CurrentCommander { 
            get => CommandReader.GetCR(AbstractGlobalController.Instance.Control/*Controlador.teclado*/); 
        }

        public DadosDeJogador Dados { get => dados; }

        // Start is called before the first frame update
        void Start()
        {
            mov = new BasicMove(new MoveFeatures() { jumpFeat = new JumpFeatures()});
            mov.StartFields(transform);

            if (ThisState ==CharacterState.notStarted)
            {
                if (dados == null)
                    dados = new DadosDeJogador();

                dados.InicializadorDosDados();

                if (ActivePet == null)
                    //if(StaticInstanceExistence<IGameController>.SchelduleExistence(
                    //    SeletaDeCriatures,
                    //    this,()=> {
                    //        return AbstractGameController.Instance;
                    //    }))
                    SeletaDeCriatures();

                ThisState = CharacterState.onFree;

                MessageAgregator<MsgStartGameElementsHud>.Publish(new MsgStartGameElementsHud()
                {
                    petname = dados.CriaturesAtivos.Count>1?dados.CriaturesAtivos[dados.CriatureSai+1].NomeID:PetName.nulo
                });
            }

            MessageAgregator<MsgChangeToHero>.AddListener(OnChangeToHero);
            MessageAgregator<MsgRequestChangeSelectedPetWithPet>.AddListener(OnRequestChangePet);
            MessageAgregator<MsgStartTalk>.AddListener(OnStartTalk);
            MessageAgregator<MsgFinishTalk>.AddListener(OnFinishTalk);
            MessageAgregator<MsgChangeActivePet>.AddListener(OnChangeActivePet);
            MessageAgregator<MsgRequestReplacePet>.AddListener(OnRequestReplacePet);
            MessageAgregator<MsgRequestChangeToPetByReplace>.AddListener(OnRequestChangeToPetByReplace);
            MessageAgregator<MsgRequestChangeSelectedItemWithPet>.AddListener(OnRequestChangeSelectedItem);
            MessageAgregator<MsgRequestUseItem>.AddListener(OnRequestUseItem);

        }

        private void OnDestroy()
        {
            MessageAgregator<MsgChangeToHero>.RemoveListener(OnChangeToHero);
            MessageAgregator<MsgRequestChangeSelectedPetWithPet>.RemoveListener(OnRequestChangePet);
            MessageAgregator<MsgStartTalk>.RemoveListener(OnStartTalk);
            MessageAgregator<MsgFinishTalk>.RemoveListener(OnFinishTalk);
            MessageAgregator<MsgChangeActivePet>.RemoveListener(OnChangeActivePet);
            MessageAgregator<MsgRequestReplacePet>.RemoveListener(OnRequestReplacePet);
            MessageAgregator<MsgRequestChangeToPetByReplace>.RemoveListener(OnRequestChangeToPetByReplace);
            MessageAgregator<MsgRequestChangeSelectedItemWithPet>.RemoveListener(OnRequestChangeSelectedItem);
            MessageAgregator<MsgRequestUseItem>.RemoveListener(OnRequestUseItem);
        }

        private void OnRequestChangeSelectedItem(MsgRequestChangeSelectedItemWithPet obj)
        {
            if (obj.pet == ActivePet.gameObject)
            {
                ChangeSelectedItem(obj.change);
            }
        }

        private void OnRequestUseItem(MsgRequestUseItem obj)
        {
            if (obj.dono == gameObject)
            {
                StartUseItem(FluxoDeRetorno.criature);
                ThisState = CharacterState.stopedWithStoppedCam;
            }
        }

        private void OnRequestChangeToPetByReplace(MsgRequestChangeToPetByReplace obj)
        {
            if (obj.dono == gameObject && obj.fluxo==FluxoDeRetorno.criature)
            {
                PublishChangeToPet();
            }
        }

        private void OnRequestReplacePet(MsgRequestReplacePet obj)
        {
            if (obj.dono == gameObject)
            {
                StartReplacePet(FluxoDeRetorno.criature);
            }
        }

        private void OnChangeActivePet(MsgChangeActivePet obj)
        {
            if (obj.dono == gameObject)
            {
                ActivePet = obj.pet.GetComponent<PetManagerCharacter>();
                Debug.Log("active pet changed");

                PetBase P = dados.CriaturesAtivos[0];
                dados.CriaturesAtivos[0] = dados.CriaturesAtivos[dados.CriatureSai + 1];
                dados.CriaturesAtivos[dados.CriatureSai+1] = P;

                MessageAgregator<MsgChangeSelectedPet>.Publish(
                new MsgChangeSelectedPet()
                {
                    petname = dados.CriaturesAtivos[dados.CriatureSai + 1].NomeID
                });
            }
        }

        private void OnFinishTalk(MsgFinishTalk obj)
        {
            ThisState = CharacterState.onFree;
        }

        private void OnStartTalk(MsgStartTalk obj)
        {
            mov.MoveApplicator(Vector3.zero);
            ThisState = CharacterState.externalPanelOpened;
        }

        private void OnRequestChangePet(MsgRequestChangeSelectedPetWithPet obj)
        {
            if (obj.pet == ActivePet.gameObject)
            {
                ChangeSelectedPet();
            }
        }

        private void OnChangeToHero(MsgChangeToHero obj)
        {
            if (obj.myHero == gameObject)
            {
                ThisState = CharacterState.onFree;
                SetHeroCamera.Set(transform);
            }
        }

        void ChangeSelectedItem(int change)
        { 
            dados.ItemSai = ContadorCiclico.Contar(change, dados.ItemSai, dados.Itens.Count);
            MessageAgregator<MsgChangeSelectedItem>.Publish(new MsgChangeSelectedItem()
            {
                nameItem = dados.Itens[dados.ItemSai].ID
            });
        }

        void ChangeSelectedPet()
        {
            dados.CriatureSai = ContadorCiclico.Contar(1, dados.CriatureSai, dados.CriaturesAtivos.Count-1);
            MessageAgregator<MsgChangeSelectedPet>.Publish(
                new MsgChangeSelectedPet()
                {
                    petname = dados.CriaturesAtivos[dados.CriatureSai+1].NomeID
                });
        }

        void SeletaDeCriatures()
        {
            if (!AbstractGameController.Instance.MyKeys.VerificaAutoShift(KeyShift.inTutorial))
            {
                if (dados.CriaturesAtivos.Count > 0 /*&& !eLoad*/)
                {
                    ActivePet =  PetInitialize.Initialize(transform, dados.CriaturesAtivos[0]).GetComponent<PetManagerCharacter>();
                }

                //Configurar Hud ...?
            }// aqui seria um sen�o para destruir o criature ativo caso exista
        }

        // Update is called once per frame
        void Update()
        {
            switch (ThisState)
            {
                case CharacterState.onFree:
                    MoveControl();
                    ControlCamera();
                    ActionCommand();
                break;
                case CharacterState.stopedWithStoppedCam:
                    mov.MoveApplicator(Vector3.zero);
                break;
                case CharacterState.externalPanelOpened:
                    CurrentCommander.DirectionalVector();
                    mov.MoveApplicator(Vector3.zero);
                    MessageAgregator<MsgSendExternaPanelCommand>.Publish(new MsgSendExternaPanelCommand()
                    {
                        confirmButton = CurrentCommander.GetButtonDown(CommandConverterInt.confirmButton),
                        returnButton = CurrentCommander.GetButtonDown(CommandConverterInt.returnButton),
                    });
                break;
            }
            
        }

        void ActionCommand()
        {
            if (mov.IsGrounded && CurrentCommander.GetButtonDown(CommandConverterInt.humanAction,true))
            {
                MessageAgregator<MsgInvokeActionFromHud>.Publish();
            }
        }

        void PublishChangeToPet()
        {
            PetBase P = dados.CriaturesAtivos[0];
            PetAtributes petA = P.PetFeat.meusAtributos;

            MessageAgregator<MsgChangeToPet>.Publish(new MsgChangeToPet()
            {
                dono = transform,
                petName = P.NomeID,
                petToGoOut = dados.CriaturesAtivos.Count > 1 ? dados.CriaturesAtivos[dados.CriatureSai + 1].NomeID : PetName.nulo,
                atkSelected = P.GerenteDeGolpes.meusGolpes[P.GerenteDeGolpes.golpeEscolhido].Nome,
                numCriatures = dados.CriaturesAtivos.Count,
                numItens = 12,
                currentHp = petA.PV.Corrente,
                currentMp = petA.PE.Corrente,
                currentSt = 1,
                maxSt = 1,
                level = P.PetFeat.mNivel.Nivel,
                maxHp = petA.PV.Maximo,
                maxMp = petA.PE.Maximo,
                name = P.GetNomeEmLinguas,
                oCriature = ActivePet.gameObject
            });
        }

        void MoveControl()
        {
            Vector3 V = CameraAplicator.cam.SmoothCamDirectionalVector(
                CurrentCommander.GetAxis(CommandConverterString.moveH),
                CurrentCommander.GetAxis(CommandConverterString.moveV)
                );

            bool run = CurrentCommander.GetButton(CommandConverterInt.run);
            bool startJump = CurrentCommander.GetButtonDown(CommandConverterInt.jump);
            bool pressJump = CurrentCommander.GetButton(CommandConverterInt.jump);

            if(mov!=null)
                mov.MoveApplicator(V, run, startJump, pressJump);

            int itemchange = CurrentCommander.GetIntTriggerDown(CommandConverterString.itemChange);

            if (CurrentCommander.GetIntTriggerDown(CommandConverterString.selectAttack_selectCriature) > 0)
            {
                ChangeSelectedPet();
            }
            else if (itemchange != 0)
            {
                ChangeSelectedItem(itemchange);
            }

            if (mov.IsGrounded)
            {
                if (CurrentCommander.GetButtonDown(CommandConverterInt.heroToCriature, true))
                {
                    ThisState = CharacterState.stopedWithStoppedCam;
                    mov.MoveApplicator(Vector3.zero);

                    PublishChangeToPet();

                }
                else if (CurrentCommander.GetButtonDown(CommandConverterInt.criatureChange))
                {
                    StartReplacePet(FluxoDeRetorno.heroi);
                }
                else if (CurrentCommander.GetButtonDown(CommandConverterInt.itemUse))
                {
                    //if (g.UsarTempoDeItem == UsarTempoDeItem.sempre || (g.UsarTempoDeItem == UsarTempoDeItem.emLuta && g.estaEmLuta))
                    //    gerente.Dados.TempoDoUltimoUsoDeItem = Time.time;
                    StartUseItem(FluxoDeRetorno.heroi);
                    ThisState = CharacterState.stopedWithStoppedCam;
                }
            }
        }

        void StartUseItem(FluxoDeRetorno fluxo)
        {
            UseItemManager useItem = gameObject.AddComponent<UseItemManager>();
            useItem.StartFields(gameObject, dados.Itens, dados.ItemSai,fluxo);
            CameraAplicator.cam.RemoveMira();
        }

        void StartReplacePet(FluxoDeRetorno fluxo)
        {
            PetReplaceManager prm = gameObject.AddComponent<PetReplaceManager>();
            prm.StartReplace(transform, ActivePet.transform, fluxo,
                dados.CriaturesAtivos[dados.CriatureSai + 1]
                );
            ThisState = CharacterState.stopedWithStoppedCam;
        }

        void ControlCamera()
        {
            if (ThisState != CharacterState.stopedWithStoppedCam)
            {
                Vector2 V = new Vector3(
                    CurrentCommander.GetAxis(CommandConverterString.camX),
                    CurrentCommander.GetAxis(CommandConverterString.camY)
                    );

                bool focar = CurrentCommander.GetButtonDown(CommandConverterInt.camFocus);
                CameraAplicator.cam.ValoresDeCamera(V.x, V.y, focar, mov.Controller.velocity.sqrMagnitude > .1f);
            }
        }
    }

    public struct MsgChangeToPet : IMessageBase {
        public int numCriatures;
        public int numItens;
        public int level;
        public int currentHp;
        public int maxHp;
        public int currentMp;
        public int maxMp;
        public int currentSt;
        public int maxSt;
        public string name;
        public GameObject oCriature;
        public AttackNameId atkSelected;
        public PetName petName;
        public PetName petToGoOut;
        public Transform dono;
    }
}
