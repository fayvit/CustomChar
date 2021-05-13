using UnityEngine;
using System.Collections.Generic;
using FayvitCam;
using FayvitCommandReader;
using FayvitBasicTools;
using FayvitMessageAgregator;
using FayvitSupportSingleton;

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

        protected override void OnCriatureDefeated(MsgCriatureDefeated obj)
        {
            base.OnCriatureDefeated(obj);

            if (obj.atacker == gameObject)
            { 
            
            }

            if (Controll.Mov.LockTarget!=null  && obj.defeated == Controll.Mov.LockTarget.gameObject)
            {
                SupportSingleton.Instance.InvokeInSeconds(() =>
                {
                    VerificaFocarInimigo();
                },1);
            }
        }

        private void OnChangeToPet(MsgChangeToPet obj)
        {
            if (obj.dono == tDono)
            {
                StaminaManager st = MeuCriatureBase.StManager;

                st.OnChangeStaminaPoints = null;
                st.OnRegenZeroedStamina = null;
                st.OnZeroedStamina = null;

                st.OnChangeStaminaPoints += () => {
                    MessageAgregator<MsgChangeST>.Publish(new MsgChangeST()
                    {
                        currentSt = st.StaminaPoints,
                        maxSt = st.MaxStaminaPoints,
                        gameObject = gameObject
                    });
                };

                st.OnRegenZeroedStamina += () => {
                    MessageAgregator<MsgRegenZeroedStamina>.Publish(new MsgRegenZeroedStamina()
                    {
                        gameObject = gameObject
                    });
                };

                st.OnZeroedStamina += () => {
                    MessageAgregator<MsgZeroedStamina>.Publish(new MsgZeroedStamina()
                    {
                        gameObject = gameObject
                    });
                };
                inControll = true;
                State = LocalState.onFree;

                Transform T = CameraAplicator.cam.transform;
                Vector3 pos = T.position;
                Quaternion Q = T.rotation;


                if (CameraAplicator.cam.Cdir.TargetIs(transform) && Mov.LockTarget == null)
                {
                    CameraAplicator.cam.FocusForDirectionalCam(transform, MeuCriatureBase.alturaCamera, MeuCriatureBase.distanciaCamera);

                    T.position = pos;
                    T.rotation = Q;

                    Vector3 V = transform.InverseTransformDirection(
                        Vector3.ProjectOnPlane(
                        T.forward, Vector3.up));


                    CameraAplicator.cam.RetornarParaCameraDirecional(V);
                }
                else if (CameraAplicator.cam.Cdir.TargetIs(transform) && Mov.LockTarget != null)
                {
                    CameraAplicator.cam.StartFightCam(transform, Mov.LockTarget);
                    MessageAgregator<MsgTargetEnemy>.Publish(new MsgTargetEnemy()
                    {
                        targetEnemy = Mov.LockTarget
                    });
                }
                else
                    CameraAplicator.cam.FocusForDirectionalCam(transform, MeuCriatureBase.alturaCamera, MeuCriatureBase.distanciaCamera);

                PetFeatures P = MeuCriatureBase.PetFeat;
            }
        }

        void MoreStatusCommands()
        {
            if (CurrentCommander.GetIntTriggerDown(CommandConverterString.selectAttack_selectCriature) < 0)
            {
                int x = MeuCriatureBase.GerenteDeGolpes.golpeEscolhido;
                x = ContadorCiclico.Contar(1, x, MeuCriatureBase.GerenteDeGolpes.meusGolpes.Count);
                MeuCriatureBase.GerenteDeGolpes.golpeEscolhido = x;

                MessageAgregator<MsgChangeSelectedAttack>.Publish(new MsgChangeSelectedAttack()
                {
                    attackName = MeuCriatureBase.GerenteDeGolpes.meusGolpes[x].Nome
                });
            }
            else if (CurrentCommander.GetIntTriggerDown(CommandConverterString.selectAttack_selectCriature) > 0)
            {
                MessageAgregator<MsgRequestChangeSelectedPetWithPet>.Publish(new MsgRequestChangeSelectedPetWithPet()
                {
                    pet = gameObject
                });
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

                    MeuCriatureBase.StManager.StaminaRegen(false);
                break;
                case LocalState.onFree:
                    Vector3 V = CameraAplicator.cam.SmoothCamDirectionalVector(
                    CurrentCommander.GetAxis(CommandConverterString.moveH),
                    CurrentCommander.GetAxis(CommandConverterString.moveV)
                    );
                    bool run = CurrentCommander.GetButton(CommandConverterInt.run) && MeuCriatureBase.StManager.VerifyStaminaAction();
                    bool startJump = CurrentCommander.GetButtonDown(CommandConverterInt.jump);
                    bool pressJump = CurrentCommander.GetButton(CommandConverterInt.jump);
                    Controll.Mov.MoveApplicator(V, run, startJump, pressJump);

                    MeuCriatureBase.StManager.StaminaRegen(run);

                    int itemChange = CurrentCommander.GetIntTriggerDown(CommandConverterString.itemChange);

                    if (CurrentCommander.GetIntTriggerDown(CommandConverterString.attack) > 0)
                        AplicaGolpe(Mov.LockTarget == null ? null : Mov.LockTarget.gameObject);
                    else if (CurrentCommander.GetIntTriggerDown(CommandConverterString.focusInTheEnemy) > 0)
                    {
                        VerificaFocarInimigo();
                    }
                    else if ( itemChange!= 0)
                    {
                        MessageAgregator<MsgRequestChangeSelectedItemWithPet>.Publish(new MsgRequestChangeSelectedItemWithPet()
                        {
                            pet = gameObject,
                            change=itemChange
                        });
                    }
                    else if (Controll.Mov.IsGrounded)
                    {
                        if (CurrentCommander.GetButtonDown(CommandConverterInt.heroToCriature, true))
                        {
                            if (Mov.LockTarget)
                            {
                                Mov.LockTarget = null;
                                CameraAplicator.cam.RemoveMira();
                            }
                            inControll = false;
                            State = LocalState.following;
                            Controll.Mov.MoveApplicator(Vector3.zero);
                            MessageAgregator<MsgChangeToHero>.Publish(new MsgChangeToHero() { myHero = tDono.gameObject });
                        }
                        else if (CurrentCommander.GetButtonDown(CommandConverterInt.dodge))
                        {
                            if (MeuCriatureBase.StManager.VerifyStaminaAction() && Roll.Start(V, gameObject))
                            {
                                gameObject.layer = 2;
                                Controll.Mov.ApplicableGravity = false;
                                GameObject G = Resources.Load<GameObject>("particles/" + GeneralParticles.rollParticles.ToString());
                                Destroy(Instantiate(G, transform.position, Quaternion.identity, transform), 3);
                                Controll.Mov.UseRollSpeed = true;
                                MeuCriatureBase.StManager.ConsumeStamina(20);
                                State = LocalState.inDodge;
                            }
                        }
                        else if (CurrentCommander.GetButtonDown(CommandConverterInt.criatureChange))
                        {
                            MessageAgregator<MsgRequestReplacePet>.Publish(new MsgRequestReplacePet()
                            {
                                dono = tDono.gameObject
                            });
                        }
                        else if (CurrentCommander.GetButtonDown(CommandConverterInt.itemUse))
                        {
                            MessageAgregator<MsgRequestUseItem>.Publish(new MsgRequestUseItem()
                            {
                                dono = tDono.gameObject
                            });
                        }
                    }

                    MoreStatusCommands();
                break;
                case LocalState.atk:
                    if (AtkApply.UpdateAttack())
                    {
                        State = inControll? LocalState.onFree:LocalState.following;
                    }

                    MoreStatusCommands();
                break;
                case LocalState.inDamage:
                    if (DamageState.Update())
                    {
                        EndDamageState();
                        State = inControll ? LocalState.onFree : LocalState.following;
                    }
                break;
                case LocalState.inDodge:
                    InRollState();
                break;
                case LocalState.returnOfRoll:
                    if (Roll.ReturnTime())
                        EndRollState();
                break;
            }
        }

        void EndRollState()
        {
            gameObject.layer = 0;
            Controll.Mov.ApplicableGravity = true;
            Controll.Mov.UseRollSpeed = false;
            Controll.Mov.MoveApplicator(Vector3.zero);
            CurrentCommander.DirectionalVector();
            State = LocalState.onFree;
        }

        void InRollState()
        {

            if (Roll.Update())
            {
                if (Roll.RequestAttack)
                {
                    if (MeuCriatureBase.StManager.VerifyStaminaAction())
                    {
                        //estado = MotionMoveState.inExternalAction;
                        //FayvitMoveEventAgregator.Publish(new FayvitMoveEvent(FayvitMoveEventKey.posRollAttack, gameObject));
                        MeuCriatureBase.StManager.ConsumeStamina(40);
                    }
                    else
                        State = LocalState.returnOfRoll;
                }
                else
                    State = LocalState.returnOfRoll;

                Controll.Mov.MoveApplicator(Vector3.zero);
            }
            else
            {
                //if (CurrentCommander.GetButtonDown(5))
                //    Roll.RequestAttack = true;

                Controll.Mov.MoveApplicator(Roll.DirOfRoll, true);
            }
        }

        void VerificaFocarInimigo()
        {
            if (Mov.LockTarget)
            {
                Mov.LockTarget = null;
                Vector3 V = transform.InverseTransformDirection(
                    Vector3.ProjectOnPlane(
                    CameraAplicator.cam.transform.forward,Vector3.up));
                CameraAplicator.cam.RetornarParaCameraDirecional(V);
            }
            else
            {
                string[] tags = new string[1] { "Criature" };
                List<GameObject> osPerto = FindBestTarget.ProximosDeMim(gameObject, tags);

                RemoveDefeated(osPerto);
                Mov.LockTarget = FindBestTarget.Procure(gameObject, osPerto,20,true);
                //CameraAplicator.cam.SetarInimigosProximosParaFoco(osPerto);

                if (Mov.LockTarget)
                {
                    CameraAplicator.cam.StartFightCam(transform, Mov.LockTarget);
                    MessageAgregator<MsgTargetEnemy>.Publish(new MsgTargetEnemy()
                    {
                        targetEnemy = Mov.LockTarget
                    });
                }
            }
        }

        private void RemoveDefeated(List<GameObject> osPerto)
        {
            int count = osPerto.Count;
            for (int i = count; i > 0; i--)
            {
                if (osPerto[i - 1].GetComponent<PetManager>().State == LocalState.defeated)
                {
                    osPerto.RemoveAt(i - 1);
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

        protected override void Update()
        {
            BaseAction();
            CamAction();

            base.Update();
        }

    }

    public struct MsgChangeToHero : IMessageBase {
        public GameObject myHero;
    }
    public struct MsgZeroedStamina : IMessageBase { 
        public GameObject gameObject; 
    }
    public struct MsgRegenZeroedStamina : IMessageBase { 
        public GameObject gameObject; 
    }
    public struct MsgRequestChangeSelectedPetWithPet : IMessageBase {
        public GameObject pet;
    }
    public struct MsgTargetEnemy : IMessageBase {
        public Transform targetEnemy;
    }
    public struct MsgRequestReplacePet : IMessageBase {
        public GameObject dono;
    }
    public struct MsgRequestChangeSelectedItemWithPet : IMessageBase
    {
        public int change;
        public GameObject pet;
    }
    public struct MsgRequestUseItem : IMessageBase
    {
        public GameObject dono;
    }
}