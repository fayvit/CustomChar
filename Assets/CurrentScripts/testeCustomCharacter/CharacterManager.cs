using FayvitBasicTools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FayvitCommandReader;
using FayvitCam;
using FayvitMove;
using FayvitMessageAgregator;

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
                CurrentCommander.GetAxis("horizontal"),
                CurrentCommander.GetAxis("vertical")
                );

            bool run = CurrentCommander.GetButton(2);
            bool startJump = CurrentCommander.GetButtonDown(3);
            bool pressJump = CurrentCommander.GetButton(3);

            if(mov!=null)
                mov.MoveApplicator(V, run, startJump, pressJump);

            if (CurrentCommander.GetButtonDown(8))
            {
                Estado = EstadoDePersonagem.parado;
                mov.MoveApplicator(Vector3.zero);
                MessageAgregator<MsgChangeToPet>.Publish(new MsgChangeToPet() { dono = transform });
            }
        }

        void ControlCamera()
        {
            Vector2 V = new Vector3(
                CurrentCommander.GetAxis("Xcam"),
                CurrentCommander.GetAxis("Ycam")
                );

            bool focar = CurrentCommander.GetButtonDown(9);
            CameraAplicator.cam.ValoresDeCamera(V.x, V.y, focar, mov.Controller.velocity.sqrMagnitude > .1f);
        }
    }

    public struct MsgChangeToPet : IMessageBase {
        public Transform dono;
    }
}
