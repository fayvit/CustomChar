using UnityEngine;
using System.Collections;
using FayvitMessageAgregator;
using Criatures2021;
using System;

namespace Criatures2021Hud
{
    public class NoveltyForLevel : MonoBehaviour
    {
        [SerializeField] private GameObject container;
        // Use this for initialization
        void Start()
        {
            MessageAgregator<MsgChangeLevel>.AddListener(OnChangeLevel);
            MessageAgregator<MsgStartGameElementsHud>.AddListener(OnStartElements);
            MessageAgregator<MsgRequestNewAttackHud>.AddListener(OnRequestNewAttackHud);
        }

        private void OnDestroy()
        {
            MessageAgregator<MsgChangeLevel>.RemoveListener(OnChangeLevel);
            MessageAgregator<MsgStartGameElementsHud>.RemoveListener(OnStartElements);
            MessageAgregator<MsgRequestNewAttackHud>.RemoveListener(OnRequestNewAttackHud);
        }

        private void OnRequestNewAttackHud(MsgRequestNewAttackHud obj)
        {
            container.SetActive(false);
        }

        private void OnStartElements(MsgStartGameElementsHud obj)
        {
            if(obj.temGolpePorAprender)
                container.SetActive(true);
        }

        private void OnChangeLevel(MsgChangeLevel obj)
        {
            if (obj.petAtkDb.Nome != AttackNameId.nulo)
            {
                container.SetActive(true);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}