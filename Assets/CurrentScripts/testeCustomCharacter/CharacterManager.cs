using FayvitBasicTools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FayvitCommandReader;
using FayvitCam;
using FayvitMove;
using FayvitMessageAgregator;
using System;

namespace Criatures2021
{
    public class CharacterManager : MonoBehaviour, ICharacterManager
    {
        [SerializeField] private BasicMove mov;
        [SerializeField] private DadosDeJogador dados;

        private EstadoDePersonagem Estado = EstadoDePersonagem.naoIniciado;
        public CharacterState ThisState { get; private set; }
        public PetManager ActivePet { get; private set; }
        private ICommandReader CurrentCommander { 
            get => CommandReader.GetCR(AbstractGlobalController.Instance.Control/*Controlador.teclado*/); 
        }

        // Start is called before the first frame update
        void Start()
        {
            mov = new BasicMove(new MoveFeatures() { jumpFeat = new JumpFeatures()});
            mov.StartFields(transform);

            if (Estado == EstadoDePersonagem.naoIniciado)
            {
                if (dados == null)
                    dados = new DadosDeJogador();

                dados.InicializadorDosDados();

                if (ActivePet == null)
                    if(StaticInstanceExistence<IGameController>.SchelduleExistence(
                        SeletaDeCriatures,
                        this,()=> {
                            return AbstractGameController.Instance;
                        }))
                    SeletaDeCriatures();

                Estado = EstadoDePersonagem.aPasseio;
            }

            MessageAgregator<MsgChangeToHero>.AddListener(OnChangeToHero);
        }

        private void OnDestroy()
        {
            MessageAgregator<MsgChangeToHero>.RemoveListener(OnChangeToHero);
        }

        private void OnChangeToHero(MsgChangeToHero obj)
        {
            if (obj.myHero == gameObject)
            {
                Estado = EstadoDePersonagem.aPasseio;
                CameraAplicator.cam.FocusForDirectionalCam(transform, .1f, 3);
                CameraAplicator.cam.Cdir.VarVerticalHeightPoint = 1;
            }
        }

        void SeletaDeCriatures()
        {
            if (!AbstractGameController.Instance.MyKeys.VerificaAutoShift(KeyShift.inTutorial))
            {
                if (dados.CriaturesAtivos.Count > 0 /*&& !eLoad*/)
                {
                    PetInitialize.Initialize(transform, dados.CriaturesAtivos[0]);
                }

                //Configurar Hud ...?
            }// aqui seria um senão para destruir o criature ativo caso exista
        }

        // Update is called once per frame
        void Update()
        {
            switch (Estado)
            {
                case EstadoDePersonagem.aPasseio:
                    MoveControl();
                    ControlCamera();
                break;
                case EstadoDePersonagem.parado:

                break;
            }
            
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

            if (mov.IsGrounded)
            {
                if (CurrentCommander.GetButtonDown(CommandConverterInt.heroToCriature, true))
                {
                    Estado = EstadoDePersonagem.parado;
                    mov.MoveApplicator(Vector3.zero);

                    MessageAgregator<MsgChangeToPet>.Publish(new MsgChangeToPet() { dono = transform });
                }
            }
        }

        void ControlCamera()
        {
            Vector2 V = new Vector3(
                CurrentCommander.GetAxis(CommandConverterString.camX),
                CurrentCommander.GetAxis(CommandConverterString.camY)
                );

            bool focar = CurrentCommander.GetButtonDown(CommandConverterInt.camFocus);
            CameraAplicator.cam.ValoresDeCamera(V.x, V.y, focar, mov.Controller.velocity.sqrMagnitude > .1f);
        }
    }

    public struct MsgChangeToPet : IMessageBase {
        public Transform dono;
    }
}
