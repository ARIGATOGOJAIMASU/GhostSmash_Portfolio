using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

namespace Junpyo
{
    public class PlayerParametors : MonoBehaviourPun, IPunObservable
    {
        public Canvas UI_Canvas;
        [SerializeField] GroundCheack GroundScript;
        [SerializeField] LineRenderer Line;
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                //레이어 동기화
                stream.SendNext(gameObject.layer);

                //Ui위치 동기화
                stream.SendNext(UI_Canvas.transform.localPosition);

                //Line 동기화
                stream.SendNext(Line.enabled);
            }
            else
            {
                //레이어 동기화
                gameObject.layer = (int)stream.ReceiveNext();

                //레이어 동기화
                UI_Canvas.transform.localPosition = (Vector3)stream.ReceiveNext();

                //Line 동기화
                Line.enabled = (bool)stream.ReceiveNext();
            }
        }
    }
}
