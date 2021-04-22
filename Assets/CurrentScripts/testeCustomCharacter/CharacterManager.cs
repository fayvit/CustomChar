using FayvitBasicTools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FayvitCommandReader;
using FayvitCam;
using FayvitMove;

namespace Criatures2021
{
    public class CharacterManager : MonoBehaviour,ICharacterManager
    {
        [SerializeField] private BasicMove mov;
        [SerializeField] private DadosDeJogador dados;

        private EstadoDePersonagem Estado = EstadoDePersonagem.naoIniciado;
        public CharacterState ThisState { get; private set; }
        public PetManager ActivePet { get; private set; }
        ICommandReader CurrentCommander { get => CommandReader.GetCR(/*AbstractGlobalController.Instance.Control*/Controlador.teclado); }

        // Start is called before the first frame update
        void Start()
        {
            mov = new BasicMove(new MoveFeatures() { jumpFeat = new JumpFeatures()});
            mov.StartFields(transform);

            if (Estado == EstadoDePersonagem.naoIniciado)
            {
                dados.InicializadorDosDados();

                if (ActivePet == null)
                    SeletaDeCriatures();

            }
        }

        void SeletaDeCriatures()
        { 
        
        }

        // Update is called once per frame
        void Update()
        {
            MoveControl();
            ControlCamera();
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
}
