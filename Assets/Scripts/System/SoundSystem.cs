using UnityEngine;
using BS.GameObjects;
using System;

namespace BS.System
{
    public class SoundSystem : ISystem
    {
        private static SoundSystem _instance;
        public static SoundSystem Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = SystemGameObject.Instance.GetSystem<SoundSystem>();
                }
                return _instance;
            }
        }

        private AudioSource _uiAudioSource;

        public SoundSystem SetUIAudioSource(AudioSource uiAudioSource)
        {
            _uiAudioSource = uiAudioSource;

            return this;
        }

        public SoundSystem SetUISoundVolume(float volume)
        {
            if (_uiAudioSource != null)
            {
                _uiAudioSource.volume = volume;
            }

            return this;
        }

        public void PlayUISound(AudioClip uiSoundAsset)
        {
            _uiAudioSource.PlayOneShot(uiSoundAsset);
        }
    }
}