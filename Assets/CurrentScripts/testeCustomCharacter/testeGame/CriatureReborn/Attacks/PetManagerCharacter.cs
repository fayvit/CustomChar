using UnityEngine;
using System.Collections.Generic;
using FayvitCam;
using FayvitCommandReader;
using FayvitBasicTools;
using FayvitMessageAgregator;

namespace Criatures2021
{
    public class PetManagerCharacter : PetManager
    {

        [SerializeField] private Transform tDono;

        private bool inControll;

        public Transform T_Dono { get => tDono; set => tDono = value; }

        private ICommandReader CurrentCommander
        {
            get => CommandReader.GetCR(AbstractGlobalController.Instance.Control);
        }

        protected override void Start()
        {
            base.Start();
            MessageAgregator<MsgChangeToPet>.AddListener(OnChangeToPet);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            MessageAgregator<MsgChangeToPet>.RemoveListener(OnChangeToPet);
        }

        private void OnChangeToPet(MsgChangeToPet obj)
        {
            if (obj.dono == tDono)
            {
                inControll = true;
                State = LocalState.onFree;
                CameraAplicator.cam.FocusForDirectionalCam(transform, MeuCriatureBase.alturaCamera, MeuCriatureBase.distanciaCamera);

            }
        }

        void BaseAction()
        {
            switch (State)
            {
                case LocalState.following:
                    timeCount += Time.deltaTime;
                    if (timeCount > TargetUpdateTax && tDono != null)
                    {
                        timeCount = 0;
                        Controll.ModificarOndeChegar(tDono.position);
                    }

                    if (Controll.UpdatePosition(2.5f))
                    {

                    }
                break;
                case LocalState.onFree:
                    Vector3 V = CameraAplicator.cam.SmoothCamDirectionalVector(
                    CurrentCommander.GetAxis(CommandConverterString.moveH),
                    CurrentCommander.GetAxis(CommandConverterString.moveV)
                    );
                    bool run = CurrentCommander.GetButton(CommandConverterInt.run);
                    bool startJump = CurrentCommander.GetButtonDown(CommandConverterInt.jump);
                    bool pressJump = CurrentCommander.GetButton(CommandConverterInt.jump);
                    Controll.Mov.MoveApplicator(V, run, startJump, pressJump);

                    if (CurrentCommander.GetIntTriggerDown(CommandConverterString.attack) > 0)
                        AplicaGolpe();
                    else if (CurrentCommander.GetIntTriggerDown(CommandConverterString.focusInTheEnemy) > 0)
                    {
                        VerificaFocarInimigo();
                    }
                    else if (Controll.Mov.IsGrounded)
                    {
                        if (CurrentCommander.GetButtonDown(CommandConverterInt.heroToCriature, true))
                        {
                            State = LocalState.following;
                            Controll.Mov.MoveApplicator(Vector3.zero);
                            MessageAgregator<MsgChangeToHero>.Publish(new MsgChangeToHero() { myHero = tDono.gameObject });
                        }
                    }
                break;
                case LocalState.atk:
                    if (AtkApply.UpdateAttack())
                    {
                        State = inControll? LocalState.onFree:LocalState.following;
                    }
                break;
                case LocalState.inDamage:
                    if (DamageState.Update())
                    {
                        EndDamageState();
                        State = inControll ? LocalState.onFree : LocalState.following;
                    }
                break;
            }
        }

        void VerificaFocarInimigo()
        {
            if (Mov.LockTarget)
            {
                Mov.LockTarget = null;

                CameraAplicator.cam.RetornarParaCameraDirecional();
            }
            else
            {
                string[] tags = new string[1] { "Criature" };
                List<GameObject> osPerto = FindBestTarget.ProximosDeMim(gameObject, tags);

                Mov.LockTarget = FindBestTarget.Procure(gameObject, osPerto);
                //CameraAplicator.cam.SetarInimigosProximosParaFoco(osPerto);

                if (Mov.LockTarget)
                {
                    CameraAplicator.cam.StartFightCam(transform, Mov.LockTarget);
                }
            }
        }

        void CamAction()
        {
            if (State != LocalState.following)
            {
                Vector2 V = new Vector3(
                    CurrentCommander.GetAxis(CommandConverterString.camX),
                    CurrentCommander.GetAxis(CommandConverterString.camY)
                    );

                bool focar = CurrentCommander.GetButtonDown(CommandConverterInt.camFocus);
                CameraAplicator.cam.ValoresDeCamera(V.x, V.y, focar, Controll.Mov.Controller.velocity.sqrMagnitude > .1f);
            }
        }

        void Update()
        {
            BaseAction();
            CamAction();
        }

    }

    public struct MsgChangeToHero : IMessageBase {
        public GameObject myHero;
    }
}