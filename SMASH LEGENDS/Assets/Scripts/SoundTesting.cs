using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wooseok
{
    public class SoundTesting : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SoundManager.Instance.PlayEffectSoundOnce(this.gameObject.name, SoundManager.Instance.EFFECTaudioclips[0]);
            }
        }
    }

}