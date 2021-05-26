using UnityEngine;
using System.Collections;
using FayvitMessageAgregator;
using Criatures2021;
using FayvitSupportSingleton;
using System.Collections.Generic;

namespace Criatures2021Hud
{
    public class NewAttackHudManager : MonoBehaviour
    {
        [SerializeField] private ShowNewAttackHud showHud;
        [SerializeField] private TryLearnNewAttackHud tryLearn;

        private int hChange;
        private bool confirmButton;
        private bool returnButton;
        private LocalState lState = LocalState.inStand;

        List<PetAttackDb> learnings = new List<PetAttackDb>();

        private enum LocalState
        { 
            inStand,
            showOpened,
            tryLearnOpened
        }

        // Use this for initialization
        void Start()
        {
            MessageAgregator<MsgRequestNewAttackHud>.AddListener(OnRequestNewAttackHud);
        }

        private void OnDestroy()
        {
            MessageAgregator<MsgRequestNewAttackHud>.RemoveListener(OnRequestNewAttackHud);
        }

        private void OnRequestNewAttackHud(MsgRequestNewAttackHud obj)
        {
            lState = LocalState.showOpened;
            returnButton = false;
            confirmButton = false;

            learnings = obj.oAprendiz.GolpesPorAprender;
            
            showHud.Start(AttackFactory.GetAttack(learnings[0].Nome));

            SupportSingleton.Instance.InvokeOnCountFrame(() =>
            {
                MessageAgregator<MsgSendExternaPanelCommand>.AddListener(OnReceiveExternalCommands);
            },3);
        }

        private void OnReceiveExternalCommands(MsgSendExternaPanelCommand obj)
        {
            confirmButton= obj.confirmButton;
            returnButton = obj.returnButton;
        }

        private void EndNewAttackHud()
        {
            lState = LocalState.inStand;
            showHud.EndHud();
            MessageAgregator<MsgSendExternaPanelCommand>.RemoveListener(OnReceiveExternalCommands);
        }

        // Update is called once per frame
        void Update()
        {
            switch (lState)
            {
                case LocalState.showOpened:
                    if (confirmButton || returnButton)
                        EndNewAttackHud();
                break;
            }
        }
    }

    public struct MsgRequestNewAttackHud : IMessageBase
    {
        public PetBase oAprendiz;
        public FluxoDeRetorno fluxo;
    }
}