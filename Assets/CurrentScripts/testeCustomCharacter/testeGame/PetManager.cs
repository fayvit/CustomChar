using FayvitBasicTools;
using FayvitMessageAgregator;
using FayvitMove;
using FayvitSounds;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Criatures2021
{
    public class PetManager : MonoBehaviour
    {

        [SerializeField] private LocalState state = LocalState.following;
        [SerializeField] private PetBase meuCriatureBase;
        [SerializeField] private ControlledMoveForCharacter controll;
        [SerializeField] private DamageState damageState;
        [SerializeField] private float targetUpdateTax = 1;

        protected float timeCount = 0;

        protected float TargetUpdateTax => targetUpdateTax;

        protected LocalState State { get=>state; set=>state=value; }

        protected enum LocalState
        {
            following,
            onFree,
            stopped,
            atk,
            inDamage
        }

        public PetBase MeuCriatureBase
        {
            get => meuCriatureBase;
            set
            {
                meuCriatureBase = value;
                Controll = new ControlledMoveForCharacter(transform);
                Controll.SetCustomMove(meuCriatureBase.MovFeat);
            }
        }

        public BasicMove Mov
        {
            get
            {
                if (Controll == null)
                    SetaMov();
                return Controll.Mov;
            }
        }

        protected ControlledMoveForCharacter Controll { get => controll; set => controll = value; }
        protected AttackApplyManager AtkApply { get; set; }
        protected DamageState DamageState { get => damageState; set => damageState = value; }

        // Start is called before the first frame update
        protected virtual void Start()
        {
            AtkApply = new AttackApplyManager(gameObject);
            DamageState = new DamageState(transform);
            Controll = new ControlledMoveForCharacter(transform);
            Controll.SetCustomMove(meuCriatureBase.MovFeat);

            MessageAgregator<MsgEnterInDamageState>.AddListener(OnEnterInDamageState);
            MessageAgregator<AnimateStartJumpMessage>.AddListener(OnStartJump);
        }

        protected virtual void OnDestroy()
        {
            MessageAgregator<MsgEnterInDamageState>.RemoveListener(OnEnterInDamageState);
            MessageAgregator<AnimateStartJumpMessage>.RemoveListener(OnStartJump);
        }

        private void OnStartJump(AnimateStartJumpMessage obj)
        {
            if (obj.gameObject == gameObject)
            {
                MessageAgregator<MsgRequest3dSound>.Publish(new MsgRequest3dSound()
                {
                    sender = transform,
                    sfxId = SoundEffectID.Evasion1
                }) ;
            }
        }

        private void OnEnterInDamageState(MsgEnterInDamageState obj)
        {
            if (obj.oAtacado == gameObject)
            {
                DamageState.StartDamageState(obj.golpe);
                state = LocalState.inDamage;
            }
        }

        void SetaMov()
        {
            Controll = new ControlledMoveForCharacter(transform);
            Controll.SetCustomMove(meuCriatureBase.MovFeat);
          
        }

        protected void AplicaGolpe()
        {
            
            PetAttackBase gg = meuCriatureBase.GerenteDeGolpes.meusGolpes[meuCriatureBase.GerenteDeGolpes.golpeEscolhido];

            Debug.Log("no chão: " + Controll.Mov.IsGrounded);

            if (Controll.Mov.IsGrounded || gg.PodeNoAr)
            {
                if (AttackApplyManager.CanStartAttack(MeuCriatureBase))
                {
                    PetAttackDb petDb = MeuCriatureBase.GerenteDeGolpes.ProcuraGolpeNaLista(MeuCriatureBase.NomeID, gg.Nome);
                    AtkApply.StartAttack(gg, petDb.TempoDeInstancia);
                    state = LocalState.atk;
                }
            }
        }
        

        // Update is called once per frame
        void Update() { }

        // eram comandos para o android mas já há comandos melhores para isso
        //public void ComandoDeAtacar()
        //{
        //    if (state == LocalState.onFree || state == LocalState.inFight)
        //        AplicaGolpe();
        //}

        //public void IniciaPulo()
        //{
        //    if (!meuCriatureBase.Mov.caracPulo.estouPulando && (state == LocalState.onFree || state == LocalState.inFight))
        //        mov._Pulo.IniciaAplicaPulo();
        //}

        public void PararCriatureNoLocal()
        {
            state = LocalState.stopped;
        }

        protected virtual void EndDamageState()
        {
            MessageAgregator<MsgEndDamageState>.Publish(new MsgEndDamageState() { gameObject = gameObject });
        }
    }

    public struct MsgEndDamageState : IMessageBase {
        public GameObject gameObject;
    }
}