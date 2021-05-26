using UnityEngine;
using System.Collections;
using FayvitBasicTools;
using FayvitMessageAgregator;
using FayvitUI;
using Criatures2021Hud;
using System;
using TalkSpace;

namespace Criatures2021
{
    public class MyGlobalController : AbstractGlobalController
    {

        protected override void Start()
        {
            MessageAgregator<MsgGetChestItem>.AddListener(OnGetChestItem);
            MessageAgregator<MsgChangeOptionUI>.AddListener(OnChangeOptionUI);
            MessageAgregator<MsgPositiveUiInput>.AddListener(OnPositiveUiInput);
            MessageAgregator<MsgNegativeUiInput>.AddListener(OnNegativeUiInput);
            MessageAgregator<FillTextDisplayMessage>.AddListener(OnBoxGoingOut);
            MessageAgregator<MsgHideShowItem>.AddListener(OnHideUpperMessage);

            base.Start();
        }

        protected override void OnDestroy()
        {
            MessageAgregator<MsgGetChestItem>.RemoveListener(OnGetChestItem);
            MessageAgregator<MsgChangeOptionUI>.RemoveListener(OnChangeOptionUI);
            MessageAgregator<MsgPositiveUiInput>.RemoveListener(OnPositiveUiInput);
            MessageAgregator<MsgNegativeUiInput>.RemoveListener(OnNegativeUiInput);
            MessageAgregator<FillTextDisplayMessage>.RemoveListener(OnBoxGoingOut);
            MessageAgregator<MsgHideShowItem>.RemoveListener(OnHideUpperMessage);

            base.OnDestroy();
        }

        private void OnHideUpperMessage(MsgHideShowItem obj)
        {
            Sfx.PlaySfx(FayvitSounds.SoundEffectID.Book1);
        }

        private void OnBoxGoingOut(FillTextDisplayMessage obj)
        {
            Sfx.PlaySfx(FayvitSounds.SoundEffectID.Book1);
        }

        private void OnNegativeUiInput(MsgNegativeUiInput obj)
        {
            Sfx.PlaySfx(FayvitSounds.SoundEffectID.Book1);
        }

        private void OnPositiveUiInput(MsgPositiveUiInput obj)
        {
            Sfx.PlaySfx(FayvitSounds.SoundEffectID.Decision2);
        }

        private void OnChangeOptionUI(MsgChangeOptionUI obj)
        {
            Sfx.PlaySfx(FayvitSounds.SoundEffectID.Cursor1);
        }

        private void OnGetChestItem(MsgGetChestItem obj)
        {
            Sfx.PlaySfx(obj.getSfx);
        }

        protected override void Update()
        {
            base.Update();
        }
    }
}