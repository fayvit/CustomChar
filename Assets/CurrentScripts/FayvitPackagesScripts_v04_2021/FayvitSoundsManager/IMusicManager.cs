using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FayvitSounds
{
    public interface IMusicManager
    {
        public MusicaComVolumeConfig ToRememberMusic { get; }

        public MusicaComVolumeConfig CurrentActiveMusic { get; }

        public float ActiveVel { get; set; }

        public float BaseVolume { get; set; }
        public void ResetActiveVel();
        public void StartRememberedMusic(float vel = -1);
        public void StartMusicRememberingCurrent(AudioClip esseClip, float volumeAlvo = 1, float vel = -1);
        public void StartMusicRememberingCurrent(NameMusicaComVolumeConfig n, float vel = -1);
        public void StartMusicRememberingCurrent(MusicaComVolumeConfig n, float vel = -1);
        public void StartMusicRememberingCurrent(NameMusic esseClip, float volumeAlvo = 1, float vel = -1);
        public void StartMusicRememberingCurrent(string esseClip, float volumeAlvo = 1, float vel = -1);
        public void StartMusic(NameMusicaComVolumeConfig esseClip, float vel = -1);
        public void StartMusic(NameMusic esseClip, float volumeAlvo = 1, float vel = -1);
        public void StartMusic(AudioClip esseClip, float volumeAlvo = 1, float vel = -1);
        public void StopMusic(float vel = -1);
        public void RestartMusic(bool doZero = false);
        public void Update();
    }

    public enum NameMusic
    {
        empty = 0,
        initialAdventureTheme = 1
    }

    [System.Serializable]
    public class MusicaComVolumeConfig
    {
        [SerializeField] private AudioClip musica;
        [SerializeField] private float volume = 1;

        public AudioClip Musica
        {
            get { return musica; }
            set { musica = value; }
        }

        public float Volume
        {
            get { return volume; }
            set { volume = value; }
        }
    }

    [System.Serializable]
    public class NameMusicaComVolumeConfig
    {
        [SerializeField] private NameMusic musica = NameMusic.initialAdventureTheme;
        [SerializeField] private float volume = 1;

        public NameMusic Musica
        {
            get { return musica; }
            set { musica = value; }
        }

        public float Volume
        {
            get { return volume; }
            set { volume = value; }
        }
    }
}