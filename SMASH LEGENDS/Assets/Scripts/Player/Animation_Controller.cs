using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Junpyo
{
    public class Animation_Controller : MonoBehaviour
    {
        [SerializeField] Transform playerTransform;
        [SerializeField] Animator playerAnimator;
        [SerializeField] Rigidbody playerRigidbody;
        [SerializeField] PlayerController_FSM playerController;

        public void FowardMove(float speed)
        {
            playerRigidbody.velocity = Vector3.zero;
            playerRigidbody.velocity = playerTransform.forward * speed;
        }

        public void UpMove(float speed)
        {
            playerRigidbody.velocity = Vector3.zero;
            playerRigidbody.velocity = playerTransform.up * speed;
        }

        public void PlayAnimation()
        {
            if (PhotonNetwork.IsConnected)
            {
                GameManager.Instance.AnimationPlay(playerController.ID,true);
            }
            else
            {
                playerAnimator.StopPlayback();
            }
        }

        public void StopAnimation()
        {
            if (PhotonNetwork.IsConnected)
            {
                GameManager.Instance.AnimationPlay(playerController.ID, false);
            }
            else
            {
                playerAnimator.StartPlayback();
            }
        }

        public void UseGravity()
        {
            playerRigidbody.velocity = Vector3.zero;
            playerRigidbody.useGravity = !playerRigidbody.useGravity;
        }

        public void LookChange()
        {
            playerTransform.LookAt(playerTransform.position + playerController.PlayerLook);
        }

        public virtual void HangAttackRolling()
        {
            playerTransform.position = playerTransform.position + (playerTransform.forward * 0.5f) + (playerTransform.up * 1.0f);
        }
    }
}
