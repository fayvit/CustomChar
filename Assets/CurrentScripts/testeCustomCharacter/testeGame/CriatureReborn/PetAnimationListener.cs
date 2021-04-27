using UnityEngine;
using System.Collections;
using FayvitMessageAgregator;
using FayvitMove;
using System;

namespace Criatures2021
{
    public class PetAnimationListener : MonoBehaviour
    {
        [SerializeField] private string velAnimatorString = "velocidade";
        [SerializeField] private string jumpNameBool = "pulo";
        [SerializeField] private string groundedNameBool = "noChao";
        [SerializeField] private string jumpAnimationStateName = "pulando";
        [SerializeField] private string atkBool = "atacando";
        [SerializeField] private string emDanoBool = "dano1";
        [SerializeField] private string emDano_2Bool = "dano2";
        [SerializeField] private string defeatedBool = "cair";


        [SerializeField] private Animator A;


        // Use this for initialization
        void Start()
        {
            A = GetComponent<Animator>();
            MessageAgregator<ChangeMoveSpeedMessage>.AddListener(OnChangeSpeed);
            MessageAgregator<AnimateStartJumpMessage>.AddListener(OnStartJump);
            MessageAgregator<AnimateDownJumpMessage>.AddListener(OnDownJump);
            MessageAgregator<MsgRequestAtkAnimation>.AddListener(OnStartAtk);
            MessageAgregator<MsgFreedonAfterAttack>.AddListener(OnFinishAtk);
            MessageAgregator<MsgEnterInDamageState>.AddListener(OnEnterInDamageState);
            MessageAgregator<MsgEndDamageState>.AddListener(OnEndDamageState);
            MessageAgregator<AnimateFallMessage>.AddListener(OnStartFall);
            MessageAgregator<MsgCriatureDefeated>.AddListener(OnCriatureDefeated);
        }

        private void OnDestroy()
        {
            MessageAgregator<ChangeMoveSpeedMessage>.RemoveListener(OnChangeSpeed);
            MessageAgregator<AnimateStartJumpMessage>.RemoveListener(OnStartJump);
            MessageAgregator<AnimateDownJumpMessage>.RemoveListener(OnDownJump);
            MessageAgregator<MsgRequestAtkAnimation>.RemoveListener(OnStartAtk);
            MessageAgregator<MsgFreedonAfterAttack>.RemoveListener(OnFinishAtk);
            MessageAgregator<MsgEnterInDamageState>.RemoveListener(OnEnterInDamageState);
            MessageAgregator<MsgEndDamageState>.RemoveListener(OnEndDamageState);
            MessageAgregator<AnimateFallMessage>.RemoveListener(OnStartFall);
            MessageAgregator<MsgCriatureDefeated>.RemoveListener(OnCriatureDefeated);
        }

        private void OnCriatureDefeated(MsgCriatureDefeated obj)
        {
            if (obj.defeated == gameObject)
            {
                A.SetBool(defeatedBool,true);
            }
        }

        private void OnStartFall(AnimateFallMessage obj)
        {
            if (obj.gameObject == gameObject)
            {
                A.SetBool(jumpNameBool, true);
                A.SetBool(groundedNameBool, false);
                A.Play(jumpNameBool);
            }
        }

        private void OnEndDamageState(MsgEndDamageState obj)
        {
            if (obj.gameObject == gameObject)
            {
                A.SetBool(emDanoBool, false);
                A.SetBool(emDano_2Bool, false);
            }
        }

        private void OnEnterInDamageState(MsgEnterInDamageState obj)
        {
            if (obj.oAtacado == gameObject)
            {
                bool b = UnityEngine.Random.Range(0, 2)==0?true:false;
                string animationName = b?"dano1": "dano1";
                A.Play(animationName);
                A.SetBool(emDanoBool, true);
                A.SetBool(emDano_2Bool, true);
            }
        }

        private void OnFinishAtk(MsgFreedonAfterAttack obj)
        {
            if (obj.gameObject == gameObject)
            {
                A.SetBool(atkBool, false);
            }
        }

        private void OnStartAtk(MsgRequestAtkAnimation obj)
        {
            if (obj.gameObject == gameObject)
            {
                A.SetBool(atkBool, true);
                A.Play(obj.nomeAnima);
            }
        }

        private void OnDownJump(AnimateDownJumpMessage obj)
        {
            if (obj.gameObject == gameObject)
            {
                A.SetBool(jumpNameBool, false);
                A.SetBool(groundedNameBool, true);
            }
        }

        private void OnStartJump(AnimateStartJumpMessage obj)
        {
            if (obj.gameObject == gameObject)
            {
                A.Play(jumpAnimationStateName);
                A.SetBool(jumpNameBool, true);
                A.SetBool(groundedNameBool, false);
            }
        }

        private void OnChangeSpeed(ChangeMoveSpeedMessage obj)
        {
            if (obj.gameObject == gameObject)
            {
                A.SetFloat(velAnimatorString, obj.velocity.sqrMagnitude);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}