using UnityEngine;
using System.Collections;

namespace FayvitSounds
{
    public interface ISoundEffectsManager
    {
        public float BaseVolume { get; set; }
        public void Instantiate3dSound(Transform T, AudioClip som, float spartial = 1);
        public void Instantiate3dSound(Transform T, SoundEffectID som, float spartial = 1);
        public void PlaySfx(SoundEffectID s);
        public void PlaySfx(string s);
        public void PlaySfx(AudioClip s);

    }

    public enum SoundEffectID
    {
        empty = 0,
        Evasion1 = 1,
        XP_Knock04 = 2,
        XP_Swing03 = 3,
        Wind1 = 4,
        XP_Swing04 = 5,
        rajadaDeAgua = 6,
        Shot1 = 7,
        Slash2 = 8,
        Slash1 = 9,
        Shot3 = 10
    }
}
