using UnityEngine;
using FayvitSounds;
using FayvitMessageAgregator;
using FayvitBasicTools;
using Criatures2021Hud;

namespace TalkSpace
{
    public class TalkButtonActivate : ButtonActivate
    {
        
        [SerializeField]
        private NameMusicaComVolumeConfig nameMusic = new NameMusicaComVolumeConfig()
        {
            Musica = NameMusic.empty,
            Volume = 1
        };

        private bool inputNext = false;
        private bool inputReturn = false;

        protected TalkManagerBase NPC { get; set; } = new TalkManagerBase();

        // Use this for initialization
        protected void Start()
        {
            MessageAgregator<MsgSendExternaPanelCommand>.AddListener(OnReceiveCommands);
            NPC.Start();

            SempreEstaNoTrigger();
        }

        private void OnDestroy()
        {
            MessageAgregator<MsgSendExternaPanelCommand>.RemoveListener(OnReceiveCommands);
        }

        private void OnReceiveCommands(MsgSendExternaPanelCommand obj)
        {
            if (gameObject.activeSelf)
            {
                inputNext = obj.confirmButton;
                inputReturn = obj.returnButton;
            }
        }

        new protected void Update()
        {
            base.Update();

            if (NPC.Update(inputNext,inputReturn))
            {
                OnFinishTalk();
            }
        }

        protected virtual void OnFinishTalk()
        {
            MessageAgregator<MsgFinishExternalInteraction>.Publish();
            MessageAgregator<MsgReturnRememberedMusic>.Publish();
            inputNext = false;
            inputReturn = false;
        }

        void BotaoConversa()
        {
            if (gameObject.activeSelf)
            {
                if (nameMusic.Musica != NameMusic.empty)
                {
                    MessageAgregator<MsgStartMusicWithRecovery>.Publish(new MsgStartMusicWithRecovery()
                    {
                        nmcvc = nameMusic
                    });
                }

                FluxoDeBotao();

                NPC.IniciaConversa();

                OnStartTalk();
            }
        }

        protected virtual void OnStartTalk()
        {
            MessageAgregator<MsgStartExternalInteraction>.Publish();
        }

        public override void FuncaoDoBotao()
        {
            BotaoConversa();
        }
    }
    
}

public struct MsgSendExternaPanelCommand : IMessageBase
{
    public bool confirmButton;
    public bool returnButton;
    public int hChange;
    public int vChange;
}

public struct MsgStartExternalInteraction : IMessageBase { }
public struct MsgFinishExternalInteraction : IMessageBase { }