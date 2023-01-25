using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Juhyung
{
    public class AudioManager : MonoBehaviour
    {
        public enum DEFINE
        {
            ONESHOT,
            REPEAT,

            DEFINE_END
        }

        private AudioSource[] _audioSources = new AudioSource[10];

        private void Awake()
        {
            Init();
        }

        private void Init()
        {
            for(int i = 0; i < 10; ++i)
            {
                _audioSources[i] = gameObject.AddComponent<AudioSource>();
            }
        }

        public void Play(AudioClip audioClip, DEFINE type = DEFINE.ONESHOT, float volume = 1.0f, float pitch = 1.0f)
        {
            if (audioClip == null) return;

            switch(type)
            {
                case DEFINE.ONESHOT:
                    {
                        for(int i = 0; i < 10; ++i)
                        {
                            if(!_audioSources[i].isPlaying)
                            {
                                if(_audioSources[i].loop)
                                {
                                    _audioSources[i].loop = false;
                                }

                                _audioSources[i].pitch = pitch;
                                _audioSources[i].volume = volume;
                                _audioSources[i].PlayOneShot(audioClip);

                                break;
                            }
                        }
                    }

                    break;

                case DEFINE.REPEAT:
                    {
                        for(int i = 0; i < 10; ++i)
                        {
                            if (!_audioSources[i].isPlaying)
                            {
                                if (!_audioSources[i].loop)
                                {
                                    _audioSources[i].loop = true;
                                }

                                _audioSources[i].pitch = pitch;
                                _audioSources[i].clip = audioClip;
                                _audioSources[i].volume = volume;
                                _audioSources[i].Play();

                                break;
                            }
                        }
                    }

                    break;
            }
        }

        public bool PlayCheck()
        {
            for(int i = 0; i < 10; ++i)
            {
                if(_audioSources[i].isPlaying)
                {
                    if(_audioSources[i].loop)
                    {
                        continue;
                    }
                    else
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
