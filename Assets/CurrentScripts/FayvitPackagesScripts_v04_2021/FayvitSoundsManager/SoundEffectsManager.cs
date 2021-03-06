using UnityEngine;
using System.Collections.Generic;

namespace FayvitSounds
{
    [System.Serializable]
    public class SoundEffectsManager : ISoundEffectsManager
    {

#pragma warning disable 0649
        [SerializeField] private AudioSource[] audios;
#pragma warning restore 0649
        private List<AudioSource> ativos = new List<AudioSource>();

        public float BaseVolume { get; set; } = 0.95f;

        public void Instantiate3dSound(Transform T, AudioClip som, float spartial = 1)
        {
            AudioSource a = (AudioSource)MonoBehaviour.Instantiate(audios[0], T.position, Quaternion.identity);
            a.transform.parent = T;
            a.clip = som;
            a.volume = BaseVolume;
            a.Play();
            a.spatialBlend = spartial;

            //Debug.Log(a.clip.length + " : " + a.clip.name+" : "+a.clip.length+" : "+a.gameObject.name);

            MonoBehaviour.Destroy(a.gameObject, 2 * a.clip.length);
        }

        public void Instantiate3dSound(Transform T, SoundEffectID som, float spartial = 1)
        {
            Instantiate3dSound(T, (AudioClip)Resources.Load(som.ToString()), spartial);
        }

        public void PlaySfx(SoundEffectID s)
        {
            PlaySfx(s.ToString());
        }

        public void PlaySfx(string s)
        {
            PlaySfx((AudioClip)Resources.Load(s));
        }

        public void PlaySfx(AudioClip s)
        {
            AudioSource a = RetornaMelhorCandidato();

            a.clip = s;
            a.volume = BaseVolume;
            a.Play();
        }

        AudioSource RetornaMelhorCandidato()
        {
            VerificaAudioAtivo();
            for (int i = 0; i < audios.Length; i++)
            {
                if (!ativos.Contains(audios[i]))
                {
                    ativos.Add(audios[i]);
                    return audios[i];
                }
            }

            return ativos[0];
        }

        void VerificaAudioAtivo()
        {
            for (int i = 0; i < audios.Length; i++)
            {
                if (!audios[i].isPlaying)
                {
                    ativos.Remove(audios[i]);
                }
            }
        }
    }
}