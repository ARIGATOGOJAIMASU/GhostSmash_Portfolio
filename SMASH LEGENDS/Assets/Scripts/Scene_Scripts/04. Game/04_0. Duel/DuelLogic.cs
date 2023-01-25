using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace Juhyung
{
    public enum Team { RED, BLUE}

    public class DuelLogic : MonoBehaviourPun
    {
        [SerializeField] private Image RedTeamHP;
        [SerializeField] private Image BlueTeamHP;

        [SerializeField] private List<Image> RedTeamScore;
        [SerializeField] private List<Image> BlueTeamScore;

        [SerializeField] public Sprite RedTeamScoreFill;
        [SerializeField] public Sprite BlueTeamScoreFill;

        private bool FirstRed = true;
        private bool FirstBlue = true;

        void Update()
        {
            Duel_Logic();
        }

        void Duel_Logic()
        {
            if (RedTeamHP.fillAmount <= 0.0f && DuelManager.Instance.BlueScore != 3)
            {
                if(FirstRed)
                {
                    if(PhotonNetwork.IsMasterClient)
                    {
                        photonView.RPC(nameof(AddScore), RpcTarget.All, Team.RED);
                    }
                }
            }
            else if (BlueTeamHP.fillAmount <= 0.0f && DuelManager.Instance.RedScore != 3)
            {
                if(FirstBlue)
                {
                    if(PhotonNetwork.IsMasterClient)
                    {
                        photonView.RPC(nameof(AddScore), RpcTarget.All, Team.BLUE);
                    }
                }
            }
            
            if (RedTeamHP.fillAmount >= 1.0f && !FirstRed)
            {
                FirstRed = true;
            }
            
            if (BlueTeamHP.fillAmount >= 1.0f && !FirstBlue)
            {
                FirstBlue = true;
            }
        }

        [PunRPC]
        void AddScore(Team team)
        {
            if (team == Team.RED)
            {
                ++DuelManager.Instance.BlueScore;

                BlueTeamScore[DuelManager.Instance.BlueScore - 1].sprite = BlueTeamScoreFill;

                FirstRed = false;
            }
            else
            {
                ++DuelManager.Instance.RedScore;

                RedTeamScore[DuelManager.Instance.RedScore - 1].sprite = RedTeamScoreFill;

                FirstBlue = false;
            }
        }
    }
}
