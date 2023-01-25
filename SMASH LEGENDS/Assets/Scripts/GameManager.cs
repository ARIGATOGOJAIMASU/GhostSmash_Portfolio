using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class GameManager : MonoBehaviourPunCallbacks
{
    // 싱글톤 패턴을 사용하기 위한 인스턴스 변수
    private static GameManager _instance;
    private Junpyo.CHARACTERNAME CharacterName;
    public List<GameObject> Players = new List<GameObject>();
    public List<Junpyo.PlayerController_FSM> PlayersScript = new List<Junpyo.PlayerController_FSM>();

    //public GameObject TestEffect;
    // 인스턴스에 접근하기 위한 프로퍼티

    public static GameManager Instance
    {
        get
        {
            // 인스턴스가 없는 경우에 접근하려 하면 인스턴스를 할당해준다.
            if (!_instance)
            {
                //_instance = new GameObject($"[{nameof(GameManager)}]").AddComponent<GameManager>();
                _instance = FindObjectOfType(typeof(GameManager)) as GameManager;

                if (_instance == null)
                    Debug.Log("no Singleton obj");
            }
            return _instance;
        }
    }

    public Junpyo.CHARACTERNAME _CharacterName
    {
        get { return CharacterName; }
        set { CharacterName = value; }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        // 인스턴스가 존재하는 경우 새로생기는 인스턴스를 삭제한다.
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
        // 아래의 함수를 사용하여 씬이 전환되더라도 선언되었던 인스턴스가 파괴되지 않는다.
        DontDestroyOnLoad(gameObject);
    }
    public int i_GameMode = 0;
    public int i_PlayerID = 0;
    public int MainLobby_BGM = 0;
    public Junpyo.CHARACTERNAME e_SetChar;
    public Junpyo.CHARACTERNAME e_Temp;

    public void AddPlayer(GameObject obj)
    {
        Players.Add(obj);
        PlayersScript.Add(obj.GetComponent<Junpyo.PlayerController_FSM>());

        string TagString = "";

        if(Players.Count == SetMaxPlayer())
        {
            for (int i =0; i < Players.Count; ++i)
            {
                if(Players[i].GetComponent<PhotonView>().IsMine)
                {
                    TagString =  Players[i].tag;
                    break;
                }
            }

            for (int i = 0; i < Players.Count; ++i)
            {
                PlayersScript[i].TemmDivision(TagString);
            }
        }
    }

    public int SetMaxPlayer()
    {
        switch(i_GameMode)
        {
            case 0:
                return 2;

            case 1:
                return 6;

            case 2:
                return 6;

            default:
                return 2;
        }
    }


//----------------------------------------------------------------------------------------RPC-------------------------------------------------------------------------------------------------

    //----------------------------------------------------------------------------------------Animation-------------------------------------------------------------------------------------------
    public void AnimationTrigger(string name, int ID)
    {
        photonView.RPC(nameof(AnimationTriggerRPC), RpcTarget.All, name, ID);
    }

    public void PlayClip(string name, int ID)
    {
        photonView.RPC(nameof(PlayClipRPC), RpcTarget.All, name, ID);
    }

    public void AnimationBool(string name, bool b, int ID)
    {
        photonView.RPC(nameof(AnimationTBoolRPC), RpcTarget.All, name, b, ID);
    }

    public void AnimationFloat(string name, float f, int ID)
    {
        photonView.RPC(nameof(AnimationFloatRPC), RpcTarget.All, name, f, ID);
    }

    public void AnimationResetTrigger(string name, int ID)
    {
        photonView.RPC(nameof(AnimationResetTriggerRPC), RpcTarget.All, name, ID);
    }

    public void AnimationSetLayerWeight(int index, int weight, int ID)
    {
        photonView.RPC(nameof(AnimationSetLayerWeightRPC), RpcTarget.All, index, weight, ID);
    }

    public void AnimationPlay(int ID, bool on)
    {
        photonView.RPC(nameof(AnimationPlayRPC), RpcTarget.All, ID, on);
    }

    [PunRPC]
    private void AnimationTriggerRPC(string name, int ID)
    {
        foreach (Junpyo.PlayerController_FSM player in PlayersScript)
        {
            if (player.ID == ID)
            {
                player.playerAnimator.SetTrigger(name);
                return;
            }
        }
    }

    [PunRPC]
    private void AnimationTBoolRPC(string name, bool b, int ID)
    {
        foreach (Junpyo.PlayerController_FSM player in PlayersScript)
        {
            if (player.ID == ID)
            {
                player.playerAnimator.SetBool(name, b);
                return;
            }
        }
    }

    [PunRPC]
    private void PlayClipRPC(string clip, int ID)
    {
        foreach (Junpyo.PlayerController_FSM player in PlayersScript)
        {
            if (player.ID == ID)
            {
                player.playerAnimator.Play(clip , 0);
                return;
            }
        }
    }

    [PunRPC]
    private void AnimationFloatRPC(string name, float f, int ID)
    {
        foreach (Junpyo.PlayerController_FSM player in PlayersScript)
        {
            if (player.ID == ID)
            {
                player.playerAnimator.SetFloat(name, f);
                return;
            }
        }
    }

    [PunRPC]
    private void AnimationSetLayerWeightRPC(int index, int weight, int ID)
    {
        foreach (Junpyo.PlayerController_FSM player in PlayersScript)
        {
            if (player.ID == ID)
            {
                player.playerAnimator.SetLayerWeight(index, weight);
                return;
            }
        }
    }

    [PunRPC]
    private void AnimationResetTriggerRPC(string name, int ID)
    {
        foreach (Junpyo.PlayerController_FSM player in PlayersScript)
        {
            if (player.ID == ID)
            {
                player.playerAnimator.ResetTrigger(name);
                return;
            }
        }
    }

    [PunRPC]
    private void AnimationPlayRPC(int ID, bool on)
    {
        foreach (Junpyo.PlayerController_FSM player in PlayersScript)
        {
            if (player.ID == ID)
            {
                if (on)
                {
                    player.playerAnimator.StopPlayback();
                }
                else
                {
                    player.playerAnimator.StartPlayback();
                }

                return;
            }
        }
    }

//----------------------------------------------------------------------------------------Transform-------------------------------------------------------------------------------------------

//-----------------------------------------------------------------------------------Attack-----------------------------------------------------------------------------------
    public void Attack(int type, bool On, int ID)
    {
        photonView.RPC(nameof(AttackRPC), RpcTarget.All, type, On, ID);
    }

    
    [PunRPC]
    public void AttackRPC(int type, bool On, int ID)
    {
        foreach (GameObject player in Players)
        {
            if (player.name == ID.ToString())
            {
                player.GetComponent<Junpyo.PlayerSkill>().AttackInst(type, On);
                return;
            }
        }
    }

//----------------------------------------------------------------------------------Hurt---------------------------------------------------------------------------------------
    public void Hurt(Vector3 dir, float debuff,Wooseok.ATTACKTYPE type, float dag, int hsn, int playerID,int enemyID)
    {
        foreach (Junpyo.PlayerController_FSM enemy in PlayersScript)
        {
            if (enemy.ID == enemyID)
            {
                if (enemy.gameObject.layer == LayerMask.NameToLayer("Dead"))
                {
                    return;
                }

                if (enemy.playerInformation.Curce)
                {
                    dag *= 2.0f;
                }

                if((type == Wooseok.ATTACKTYPE.HEAL))
                {
                    if(enemy.playerInformation.Cur_HP - dag > enemy.playerInformation.HP_Max)
                    {
                        dag = enemy.playerInformation.Cur_HP - enemy.playerInformation.HP_Max;
                    }
                }

                break;
            }
        }

        photonView.RPC(nameof(HurtRPC), RpcTarget.AllViaServer, dir, debuff, type, dag, hsn, playerID, enemyID);
    }

    [PunRPC]
    public void HurtRPC(Vector3 dir, float debuff, Wooseok.ATTACKTYPE type, float dag,int hsn, int playerID, int enemyID)
    {
        foreach (Junpyo.PlayerController_FSM enemy in PlayersScript)
        {
            if (enemy.ID == enemyID)
            {
                if (dag != 0)
                {
                    if ((enemy.playerInformation.Cur_HP - dag) >= 0)
                    {
                        enemy.playerInformation.Cur_HP -= dag;
                    }
                    else
                    {
                        enemy.playerInformation.Cur_HP = 0;
                    }

                    // >> Wooseok: HitSound Play in Enemy
                    enemy.PlayHitSound(hsn);
                    // << 

                    enemy.HP_Bar.fillAmount = enemy.playerInformation.Cur_HP / enemy.playerInformation.HP_Max;
                    enemy.hp_Value.SetHP_Value((int)enemy.playerInformation.Cur_HP);

                    if (enemy.playerInformation.Cur_HP <= 0)
                    {
                        foreach (Junpyo.PlayerController_FSM player in PlayersScript)
                        {
                            if (player.ID == playerID)
                            {
                                player.SmashTrigger();
                                Junpyo.KillLogManager.Instance.KillLogAdd(playerID, enemyID);

                                //자신이 죽였다는 것을 적 플레이어에게 알림
                                enemy.gameObject.layer = LayerMask.NameToLayer("Dead");
                                enemy.EnemyCharacter = player.CharacterName;
                                break;
                            }
                        }
                    }
                }

                //y값을 0으로 초기화해야지 피격 당할 시 위로 간다.
                dir -= new Vector3(0, dir.y, 0);

                enemy.Hurt(dir, debuff, type, dag);
                return;
            }
        }
    }

//--------------------------------------------------------------------------------State---------------------------------------------------------------------------------------------
    public void CameraShaking(int id)
    {
        photonView.RPC(nameof(CameraShakingRPC), RpcTarget.All, id);
    }

    [PunRPC]
    public void CameraShakingRPC(int ID)
    {
        foreach (Junpyo.PlayerController_FSM player in PlayersScript)
        {
            if ((player.ID == ID) && player.IsMine)
            {
                player.CamManager.CameraShaking();
                return;
            }
        }
    }

    public void StateChange(Junpyo.PLAYERSTATE type, int ID)
    {
        photonView.RPC(nameof(StateChangeRPC), RpcTarget.All,type, ID);
    }

    [PunRPC]
    public void StateChangeRPC(Junpyo.PLAYERSTATE type, int ID)
    {
        foreach (Junpyo.PlayerController_FSM player in PlayersScript)
        {
            if (player.ID == ID)
            {
                player.ChangeState(type);
                return;
            }
        }
    }

    public void Dead(bool b, int ID)
    {
        photonView.RPC(nameof(DeadRPC), RpcTarget.All, b, ID);
    }

    [PunRPC]
    public void DeadRPC(bool b,int ID)
    {
        foreach (Junpyo.PlayerController_FSM player in PlayersScript)
        {
            if (player.ID == ID)
            {
                player.Dead(b);
                return;
            }
        }
    }

    public void EnemyListAdd(int OwnerID, int EnemyID)
    {
        photonView.RPC(nameof(EnemyListAddRPC), RpcTarget.All, OwnerID, EnemyID);
    }

    [PunRPC]
    public void EnemyListAddRPC(int OwnerID, int EnemyID)
    {
        foreach (GameObject player in Players)
        {
            if (player.name == OwnerID.ToString())
            {
                Junpyo.PlayerSkill Skill = player.GetComponent<Junpyo.PlayerSkill>();

                foreach (GameObject Enemy in Players)
                {
                    if (Enemy.name == EnemyID.ToString())
                    {
                        Skill.myUltimate.GetComponent<Wooseok.Skill>().slappedtarget.Add(new Wooseok.Pair<GameObject, int>(Enemy, 0));
                        return;
                    }
                }
            }
        }
    }

    public void IKEvent(Junpyo.PLAYERSTATE envent, bool b, int ID)
    {
        photonView.RPC(nameof(IKEventRPC), RpcTarget.All, envent, b, ID);
    }

    [PunRPC]
    public void IKEventRPC(Junpyo.PLAYERSTATE envent, bool b, int ID)
    {
        foreach (Junpyo.PlayerController_FSM player in PlayersScript)
        {
            if (player.ID == ID)
            {
                player.IK_Controller.IKEvent(envent, b);
                return;
            }
        }
    }

    public void GagePus(int gage, int ID)
    {
        photonView.RPC(nameof(GagePusRPC), RpcTarget.All, gage, ID);
    }

    [PunRPC]
    public void GagePusRPC(int gage, int ID)
    {
        foreach (Junpyo.PlayerController_FSM player in PlayersScript)
        {
            if (player.ID == ID)
            {
                player.UtimateGageUp(gage);
                return;
            }
        }
    }

    public void GameOver()
    {
        photonView.RPC(nameof(GameOverRPC), RpcTarget.All);
    }

    [PunRPC]
    public void GameOverRPC()
    {
        foreach (Junpyo.PlayerController_FSM player in PlayersScript)
        {
            player.IsUpdate = false;
            break;
        }
    }

    public void ConditionEffectOn(Wooseok.ATTACKTYPE condition, bool on, int id)
    {
        photonView.RPC(nameof(ConditionEffectOnRPC), RpcTarget.All, condition, on, id);
    }

    [PunRPC]
    public void ConditionEffectOnRPC(Wooseok.ATTACKTYPE condition, bool on, int id)
    {
        foreach (Junpyo.PlayerController_FSM player in PlayersScript)
        {
            if (id == player.ID)
            {
                player.ConditionEffectOn(condition, on);
                return;
            }
        }
    }

    public void HP(float hp, int id)
    {
        photonView.RPC(nameof(HP_RPC), RpcTarget.All, hp, id);
    }

    [PunRPC]
    public void HP_RPC(float hp, int id)
    {
        foreach (Junpyo.PlayerController_FSM player in PlayersScript)
        {
            if (id == player.ID)
            {
                player.playerInformation.Cur_HP = hp;
                player.HP_Bar.fillAmount = player.playerInformation.Cur_HP / player.playerInformation.HP_Max;
                player.hp_Value.SetHP_Value((int)player.playerInformation.Cur_HP);
                return;
            }
        }
    }

    public void FollowUpSkill(int type, int id)
    {
        photonView.RPC(nameof(FollowUpSkillRPC), RpcTarget.All, type, id);
    }

    [PunRPC]
    public void FollowUpSkillRPC(int type, int id)
    {
        foreach (Junpyo.PlayerController_FSM player in PlayersScript)
        {
            if (id == player.ID)
            {
                player.playerSkill.FollowUpSkill(type);
                return;
            }
        }
    }

    public void BoomItem(bool on, int id)
    {
        photonView.RPC(nameof(BoomItemRPC), RpcTarget.All, on, id);
    }

    [PunRPC]
    public void BoomItemRPC(bool on, int id)
    {
        foreach (Junpyo.PlayerController_FSM player in PlayersScript)
        {
            if (id == player.ID)
            {
                player.Boom.SetActive(on);
                player.GetBoom = on;
                return;
            }
        }
    }

    public void StartPlayer()
    {
        foreach (Junpyo.PlayerController_FSM player in PlayersScript)
        {
            if (player.IsMine)
            {
                player.IsUpdate = true;
                return;
            }
        }
    }

    public void MovePlayer(Vector3 targetpos, int id)
    {
        photonView.RPC(nameof(MovePlayerRPC), RpcTarget.All, targetpos, id);
    }

    [PunRPC]
    public void MovePlayerRPC(Vector3 targetpos, int id)
    {
        foreach(Junpyo.PlayerController_FSM player in PlayersScript)
        {
            if(id == player.ID)
            {
                player.playerRigidbody.position = targetpos;
                return;
            }
        }
    }
}
