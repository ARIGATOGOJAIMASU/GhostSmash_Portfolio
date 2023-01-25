using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Junpyo;
using Photon.Pun;

namespace Wooseok
{ 
    public class Skill_Coffin : MonoBehaviourPunCallbacks
    {

        public float MaxHP;
        public float CurHP;
        public Juhyung.AudioManager myAudio;
        public bool destroyingsound = true;
        public int DestroyingSoundNumber;

        private void Awake()
        {
            CurHP = MaxHP;
            myAudio = this.GetComponent<Juhyung.AudioManager>();
        }

        // Update is called once per frame
        void Update()
        {
            if(CurHP <= 0)
            {
                DestroyingSequence();
            }
        }

        void DestroyingSequence()
        {
            if (destroyingsound)
            {
                Debug.Log("Engaging Destroying sound");
                destroyingsound = false;
                myAudio.Play(Juhyung.SoundManager.Instance.AttSound[DestroyingSoundNumber]);
            }
            else if(!myAudio.PlayCheck())
            {
                destroyingsound = true;
                this.gameObject.SetActive(false);
            }
        }
    }
}