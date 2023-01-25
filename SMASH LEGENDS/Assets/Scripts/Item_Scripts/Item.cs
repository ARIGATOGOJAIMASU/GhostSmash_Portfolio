using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace Juhyung
{
    public abstract class Item : MonoBehaviourPun
    {
        public abstract void DestroyItem();
        public abstract void RunItem();

        [SerializeField] protected GameObject ItemStation;

        void Start()
        {
            
        }

        private void Update()
        {
            DestroyItem();
        }

        [PunRPC]
        public void DestroyItemRPC()
        {
            ItemManager.Instance.s_Getitem = null;
            ItemManager.Instance.s_PlayerNum = null;
            ItemStation.GetComponent<CapsuleCollider>().enabled = false;
            gameObject.SetActive(false);
        }
    }
}
