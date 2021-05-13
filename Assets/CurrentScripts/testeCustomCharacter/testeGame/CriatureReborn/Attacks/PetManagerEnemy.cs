using UnityEngine;
using System.Collections;
using FayvitMove;
using FayvitSupportSingleton;
using System.Collections.Generic;
using FayvitMessageAgregator;
using System;

namespace Criatures2021
{
    public class PetManagerEnemy : PetManager
    {
        [SerializeField]private EnemyIaBase enemyIa = new EnemyIaBase();
        [SerializeField]
        private Dictionary<AttackResponse, float> atkResponseTax = new Dictionary<AttackResponse, float>()
        {
            { AttackResponse.aggresive,.95f},
            { AttackResponse.escapeTax,.05f}
        };

        private LocalState rememberedState;

        protected override void Start()
        {
            Controll = new ControlledMoveForCharacter(transform);
            Controll.SetCustomMove(MeuCriatureBase.MovFeat);

            State = LocalState.onFree;
            enemyIa.Start(transform,MeuCriatureBase,Controll);
            base.Start();

            MessageAgregator<MsgEnemyRequestAttack>.AddListener(OnRequestAttack);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            MessageAgregator<MsgEnemyRequestAttack>.RemoveListener(OnRequestAttack);
        }

        private void OnRequestAttack(MsgEnemyRequestAttack obj)
        {
            if (obj.gameObject == gameObject)
            {
                EfetiveApplyAttack(obj.atk,enemyIa.HeroPet);
            }
        }

        protected override void OnEnterInDamageState(MsgEnterInDamageState obj)
        {
            base.OnEnterInDamageState(obj);

            if (obj.oAtacado == gameObject)
            {
                enemyIa.OnStartDamageState(atkResponseTax,obj.atacante);
            }
        }

        protected override void OnCriatureDefeated(MsgCriatureDefeated obj)
        {
            base.OnCriatureDefeated(obj);

            if (obj.defeated == gameObject)
            {
                SupportSingleton.Instance.InvokeInSeconds(() => {
                    GameObject G = Resources.Load<GameObject>("particles/"+ImpactParticles.defeatedParticles);
                    Destroy(
                    Instantiate(G, transform.position, Quaternion.identity), 5);

                    Destroy(gameObject);
                },4);
            }
            //else if (obj.atacker == gameObject)
            //{ 
            
            //}
        }

        protected override void Update()
        {
            switch (State)
            {
                case LocalState.onFree:
                    enemyIa.Update();
                break;
                case LocalState.atk:
                    if (AtkApply.UpdateAttack())
                    {
                        State = LocalState.onFree;
                    }
                break;
                case LocalState.inDamage:
                    if (DamageState.Update())
                    {
                        EndDamageState();
                        enemyIa.OnEndDamageState();
                        State = LocalState.onFree;
                    }
                break;
            }

            base.Update();
        }

        public void StopWithRememberedState()
        {
            rememberedState = State;
            State = LocalState.stopped;
        }

        public void ReturnRememberedState()
        {
            State = rememberedState;
        }
    }
}