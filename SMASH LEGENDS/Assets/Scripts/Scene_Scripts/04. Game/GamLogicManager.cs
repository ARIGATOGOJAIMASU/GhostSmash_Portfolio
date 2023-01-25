using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Junpyo
{
    public class GamLogicManager : MonoBehaviourPunCallbacks
    {
        [SerializeField] protected List<Sprite> i_Num;                                    // 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 sprite
        [SerializeField] public List<GameObject> PlayerPref;                            // PlayerPrefabs
        [SerializeField] public List<GameObject> spownPoint = new List<GameObject>();   // Player SpawnPoint

        [SerializeField] protected float StartDelayTime = 0.0f;                           // Occupation Trigger On before DelayTime

        //[SerializeField] private GameObject OccupationTrigger;                          // Occupation Range

        protected GameObject go;                                                          // Player
        protected int MyIndex;                                                            // My SpawnPos Idx

        [HideInInspector] public int RedTeamPlayer = 0;                                 // In Occupation Trigger RedTeam Player Num
        [HideInInspector] public int BlueTeamPlayer = 0;                                // In Occupation Trigger BlueTeam Player Num

        [SerializeField] protected Image Min;
        [SerializeField] protected Image TenSec;
        [SerializeField] protected Image OneSec;
        [SerializeField] protected Junpyo.UIManagement playerManagement;

        public int ReadyPlayer;

        protected float GameTime = 180.0f;

        [HideInInspector] public bool GameStart;

        [SerializeField] protected GameObject LodingCanvas;
        [SerializeField] protected GameObject ReadyStart_Ani;

        protected void CreatePlayer()
        {
            if (PhotonNetwork.InRoom)
            {
                int actorNum = PhotonNetwork.LocalPlayer.ActorNumber;
                Player[] sortedPlayers = PhotonNetwork.PlayerList;

                for (int i = 0; i < sortedPlayers.Length; ++i)
                {
                    if (sortedPlayers[i].ActorNumber == actorNum)
                    {
                        GameManager.Instance.i_PlayerID = i;
                        go = PhotonNetwork.Instantiate
                            (
                                PlayerPref[(int)GameManager.Instance.e_SetChar].name,
                                spownPoint[GameManager.Instance.i_PlayerID].transform.position,
                                spownPoint[GameManager.Instance.i_PlayerID].transform.rotation
                            );

                        break;
                    }
                }

                if (GameManager.Instance.i_PlayerID % 2 == 0)
                {
                    Debug.Log("블루Tag변경");
                    Debug.Log(go);
                    Debug.Log(go.GetComponent<PhotonView>().ViewID);
                    photonView.RPC(nameof(SetTagRPC), RpcTarget.All, go.GetComponent<PhotonView>().ViewID, "Blue");
                }
                else
                {
                    Debug.Log("레트Tag변경");
                    photonView.RPC(nameof(SetTagRPC), RpcTarget.All, go.GetComponent<PhotonView>().ViewID, "Red");
                }

                Debug.Log("나감");
            }
        }

        [PunRPC]
        protected void SetTagRPC(int playerIndex, string tag)
        {
            Debug.Log("ㄹㅇ시작");
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

            foreach (GameObject obj in players)
            {
                if (obj.GetComponent<PhotonView>().ViewID == playerIndex)
                {
                    Debug.Log("시작");
                    Junpyo.PlayerController_FSM PlayerScript = obj.GetComponent<Junpyo.PlayerController_FSM>();

                    PlayerScript.gameObject.tag = tag;
                    PlayerScript.SpawnPos = spownPoint[MyIndex].transform.position;
                    PlayerScript.SpawnPos = spownPoint[GameManager.Instance.i_PlayerID].transform.position;

                    Debug.Log("중간");
                    GameManager.Instance.AddPlayer(obj);
                    Junpyo.KillLogManager.Instance.AddPlayer(obj);
                    playerManagement.AddPlayer(PlayerScript);
                    Debug.Log("끝");
                }
            }
        }


        public void ReayPlayer()
        {
            photonView.RPC(nameof(ReayPlayerRPC), RpcTarget.MasterClient);
        }

        [PunRPC]
        public void ReayPlayerRPC()
        {
            ++ReadyPlayer;

            if (ReadyPlayer == GameManager.Instance.SetMaxPlayer())
            {
                photonView.RPC(nameof(GameStartRPC), RpcTarget.AllViaServer);
            }
        }

        [PunRPC]
        public void GameStartRPC()
        {
            LodingCanvas.SetActive(false);
            ReadyStart_Ani.SetActive(true);
        }

        protected virtual void GameTimer() { }

        public virtual void GameSet() { }
    }
}
