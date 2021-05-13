using FayvitCommandReader;
using FayvitMessageAgregator;
using FayvitSounds;
using FayvitUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FayvitBasicTools
{
    public abstract class AbstractGlobalController : MonoBehaviour,IGlobalController
    {
        [SerializeField] private ConfirmationPanel confirmP;
        [SerializeField] private SingleMessagePanel singleP;
        [SerializeField] private MyFadeView myFade;
        [SerializeField] private SoundEffectsManager sfx;
        [SerializeField] private MusicManager music;

        private static AbstractGlobalController instance;

        public static IGlobalController Instance => instance;

        public List<IPlayersInGameDb> Players { get; set; }

        public Controlador Control { get; private set; } = Controlador.teclado;

        public ConfirmationPanel Confirmation => confirmP;

        public SingleMessagePanel OneMessage => singleP;

        public IFadeView FadeV => myFade;

        public float MusicVolume { get => music.BaseVolume; set => music.BaseVolume=value; }
        public float SfxVolume { get => sfx.BaseVolume; set => sfx.BaseVolume=value; }
        public bool EmTeste { get; set; }

        public Vector3 InitialGamePosition => new Vector3(0, 0, 0);

        protected virtual void Awake()
        {
            AbstractGlobalController[] gg = FindObjectsOfType<AbstractGlobalController>();

            if (gg.Length > 1)
            {
                Destroy(gameObject);
                return;
            }


            transform.parent = null;

            DontDestroyOnLoad(gameObject);

            instance = this;

        }

        // Start is called before the first frame update
        protected virtual void Start()
        {
            if (Players == null)
                Players = new List<IPlayersInGameDb>();

            MessageAgregator<MsgRequestSfx>.AddListener(OnRequestSfx);
            MessageAgregator<MsgStartMusic>.AddListener(OnRequestStartMusic);
            MessageAgregator<MsgStopMusic>.AddListener(OnRequestStopMusic);
            MessageAgregator<MsgStartMusicWithRecovery>.AddListener(OnRequestStartMusicWithRecovery);
            MessageAgregator<MsgReturnRememberedMusic>.AddListener(OnRequestReturnRememberedMusic);
            MessageAgregator<MsgRequest3dSound>.AddListener(OnRequest3dSound);


            //EventAgregator.AddListener(EventKey.changeActiveScene, OnChangeActiveScene);
            //EventAgregator.AddListener(EventKey.startCheckPoint, OnStartCheckPoint);
            //EventAgregator.AddListener(EventKey.requestToFillDates, OnChangeActiveScene);
            //EventAgregator.AddListener(EventKey.checkPointLoad, OnCheckPointLoad);
            //EventAgregator.AddListener(EventKey.checkPointExit, OnCheckPointExit);
            //EventAgregator.AddListener(EventKey.getUpdateGeometry, OnGetUpdateGeometry);
            //EventAgregator.AddListener(EventKey.startSceneMusic, OnChangeActiveScene);
            //EventAgregator.AddListener(EventKey.allAbilityOn, OnRequestAllAbility);

            //EventAgregator.AddListener(EventKey.disparaSom, OnRequestSoundEffects);
            //EventAgregator.AddListener(EventKey.startMusic, OnRequestStartMusic);
            //EventAgregator.AddListener(EventKey.stopMusic, OnRequestStopMusic);
            //EventAgregator.AddListener(EventKey.restartMusic, OnRequestRestartMusic);
            //EventAgregator.AddListener(EventKey.request3dSound, OnRequest3dSound);
            //EventAgregator.AddListener(EventKey.changeMusicWithRecovery, OnRequestMusicWithRecovery);
            //EventAgregator.AddListener(EventKey.returnRememberedMusic, OnRequestRememberedMusic);
        }

        protected virtual void OnDestroy()
        {
            MessageAgregator<MsgRequestSfx>.RemoveListener(OnRequestSfx);
            MessageAgregator<MsgStartMusic>.RemoveListener(OnRequestStartMusic);
            MessageAgregator<MsgStopMusic>.RemoveListener(OnRequestStopMusic);
            MessageAgregator<MsgStartMusicWithRecovery>.RemoveListener(OnRequestStartMusicWithRecovery);
            MessageAgregator<MsgReturnRememberedMusic>.RemoveListener(OnRequestReturnRememberedMusic);
            MessageAgregator<MsgRequest3dSound>.RemoveListener(OnRequest3dSound);
        }

        private void OnRequest3dSound(MsgRequest3dSound obj)
        {
            float spartial = -1;
            if (obj.changeSpartial)
                spartial = obj.spartialVal;

            if (obj.clip != null)
                sfx.Instantiate3dSound(obj.sender, obj.clip, spartial);
            else if (obj.sfxId!=SoundEffectID.empty)
                sfx.Instantiate3dSound(obj.sender,obj.sfxId,spartial);
        }

        private void OnRequestReturnRememberedMusic(MsgReturnRememberedMusic obj)
        {
            if (!obj.changeVel)
                music.StartRememberedMusic();
            else
                music.StartRememberedMusic(obj.velVal);
        }

        private void OnRequestStartMusicWithRecovery(MsgStartMusicWithRecovery obj)
        {
            if (obj.changeVel)
                music.StartMusicRememberingCurrent(obj.nmcvc, obj.velVal);
            else
                music.StartMusicRememberingCurrent(obj.nmcvc);
        }

        private void OnRequestStopMusic(MsgStopMusic obj)
        {
            float vel = 0.25f;

            if (obj.velOfStopMusic > 0)
                vel = obj.velOfStopMusic;

            music.StopMusic(vel);
        }

        private void OnRequestStartMusic(MsgStartMusic obj)
        {
            float vol = 1;
            if (obj.changeVolume)
                vol = obj.volumeVal;

            if (obj.nmcvc != null)
                music.StartMusic(obj.nmcvc);
            else if(obj.clip!=null)
                music.StartMusic(obj.clip, vol);
            else if (obj.nameMusic != NameMusic.empty)
                music.StartMusic(obj.nameMusic, vol);

        }

        private void OnRequestSfx(MsgRequestSfx obj)
        {
            if (obj.clip!=null)
            {
                sfx.PlaySfx(obj.clip);
            }
            else if (!string.IsNullOrEmpty(obj.sfxName))
            {
                sfx.PlaySfx(obj.sfxName);
            }
            else
            if (obj.sfxId!=SoundEffectID.empty)
            {
                sfx.PlaySfx(obj.sfxId);
            }
        }

        // Update is called once per frame
        protected virtual void Update()
        {
            music.Update();
        }
    }

    public struct MsgRequestSfx : IMessageBase
    {
        public AudioClip clip;
        public SoundEffectID sfxId;
        public string sfxName;
    }
    public struct MsgStartMusic:IMessageBase
    {
        public bool changeVolume;
        public float volumeVal;
        public NameMusic nameMusic;
        public AudioClip clip;
        public NameMusicaComVolumeConfig nmcvc;
    }
    public struct MsgStopMusic : IMessageBase
    {
        public float velOfStopMusic;
    }
    public struct MsgStartMusicWithRecovery : IMessageBase
    {
        public bool changeVel;
        public float velVal;
        public NameMusicaComVolumeConfig nmcvc;
    }
    public struct MsgReturnRememberedMusic : IMessageBase 
    {
        public bool changeVel;
        public float velVal;
    }
    public struct MsgRequest3dSound : IMessageBase
    {
        public AudioClip clip;
        public SoundEffectID sfxId;
        public Transform sender;     
        public bool changeSpartial;
        public float spartialVal;
    }
}