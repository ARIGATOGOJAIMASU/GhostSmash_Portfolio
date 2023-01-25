using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Juhyung
{
    public class Sound3DManager : MonoBehaviour
    {
        #region SoundType
        public enum SOUNDTYPE
        {
            ONESHOT,
            REPEAT,

            SOUNDTYPE_END
        }
        #endregion SoundType

        [SerializeField] private List<AudioClip> SFX_Name;
        private AudioSource[] _audioSource = new AudioSource[(int)SOUNDTYPE.SOUNDTYPE_END];
        private void Awake()
        {
            Init();
        }

        private void Init()
        {
            for(int i = 0; i < (int)SOUNDTYPE.SOUNDTYPE_END; ++i)
            {
                _audioSource[i] = gameObject.AddComponent<AudioSource>();
            }

            _audioSource[(int)SOUNDTYPE.REPEAT].loop = true;
        }

        public void Play(AudioClip audioClip, SOUNDTYPE type = SOUNDTYPE.ONESHOT, float volume = 1.0f, float pitch = 1.0f)
        {
            if (audioClip == null) return;

            switch(type)
            {
                case SOUNDTYPE.ONESHOT:
                    {
                        AudioSource audioSource = _audioSource[(int)SOUNDTYPE.ONESHOT];
                        if(_audioSource[(int)SOUNDTYPE.REPEAT].isPlaying)
                        {
                            _audioSource[(int)SOUNDTYPE.REPEAT].Stop();
                        }

                        audioSource.pitch = pitch;
                        audioSource.clip = audioClip;
                        audioSource.volume = volume;
                        audioSource.Play();
                    }

                    break;

                case SOUNDTYPE.REPEAT:
                    {
                        AudioSource audioSource = _audioSource[(int)SOUNDTYPE.REPEAT];
                        if (audioSource.isPlaying)
                        {
                            audioSource.Stop();
                        }

                        audioSource.pitch = pitch;
                        audioSource.clip = audioClip;
                        audioSource.volume = volume;
                        audioSource.Play();
                    }

                    break;
            }
        }
        /*public void Play(int i_num, SOUNDTYPE type = SOUNDTYPE.ONESHOT, float volume = 1.0f, float pitch = 1.0f)
        {
            Play(SFX_Name[i_num], type, volume, pitch);
        }*/

        public bool PlayCheck()
        {
            for(int i = 0; i < (int)SOUNDTYPE.SOUNDTYPE_END; ++i)
            {
                if(_audioSource[i].isPlaying)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
