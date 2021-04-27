using UnityEngine;
using System.Collections;
using UnityEngine.PlayerLoop;
using FayvitMove;

namespace Criatures2021
{
    public class PetManagerEnemy : PetManager
    {
        protected override void Start()
        {
            Controll = new ControlledMoveForCharacter(transform);
            Controll.SetCustomMove(MeuCriatureBase.MovFeat);

            base.Start();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

        void Update()
        {
            switch (State)
            {
                case LocalState.onFree:
                    Controll.Mov.MoveApplicator(Vector3.zero);
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
                        State = LocalState.onFree;
                    }
                break;
            }
        }
    }
}