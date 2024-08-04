using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

namespace Devenant
{
    [RequireComponent(typeof(InitializableObject))]
    public class AudioManager : Singleton<AudioManager>, IInitializable
    {
        public class Channel
        {
            private readonly string id;
            private readonly AudioMixer audioMixer;

            public int volume
            {
                set
                {
                    _volume = value;

                    if(value > 0)
                    {
                        audioMixer?.SetFloat(id, Mathf.Log10((float)value / 100) * 20);
                    }
                    else
                    {
                        audioMixer?.SetFloat(id, -80);
                    }
                }
                get
                {
                    return _volume;
                }
            }
            private int _volume;

            public Channel(string id, AudioMixer audioMixer)
            {
                this.id = id;
                this.audioMixer = audioMixer;
            }
        }

        [SerializeField] private AudioMixer audioMixer;

        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource sfxSource;

        public Channel master { get { return _master; } private set { _master = value; } }
        private Channel _master;

        public Channel music { get { return _music; } private set { _music = value; } }
        private Channel _music;
            
        public Channel sfx { get { return _sfx; } private set { _sfx = value; } }
        private Channel _sfx;


        public void Initialize(Action<InitializationResponse> callback)
        {
            master = new Channel("Master", audioMixer);
            music = new Channel("Music", audioMixer);
            sfx = new Channel("Sfx", audioMixer);

            callback?.Invoke(new InitializationResponse(true));
        }

        public void PlayMusic(AudioClip audioClip, float time, Action callback = null)
        {
            float musicVolume = musicSource.volume;

            FadeVolume(musicSource, 0, time / 2, () =>
            {
                musicSource.Stop();

                if(audioClip != null)
                {
                    musicSource.clip = audioClip;

                    musicSource.Play();

                    FadeVolume(musicSource, musicVolume, time / 2, callback);
                }
                else
                {
                    musicSource.volume = musicVolume;

                    callback?.Invoke();
                }
            });
        }

        public void PlaySfx(AudioClip audioClip)
        {
            sfxSource.PlayOneShot(audioClip);
        }

        private void FadeVolume(AudioSource target, float value, float time, Action callback)
        {
            StartCoroutine(FadeCoroutine());

            IEnumerator FadeCoroutine()
            {
                float startVolume = target.volume;
                float elapsedTime = 0f;

                while(elapsedTime < time)
                {
                    target.volume = Mathf.Lerp(startVolume, value, elapsedTime / time);

                    elapsedTime += Time.deltaTime;

                    yield return null;
                }

                target.volume = value;

                callback?.Invoke();
            }
        }
    }
}