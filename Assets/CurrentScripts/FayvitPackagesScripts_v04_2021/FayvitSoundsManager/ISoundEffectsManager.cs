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
        empty = 0
    }
}
