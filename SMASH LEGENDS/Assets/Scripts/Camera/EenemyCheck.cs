using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Junpyo
{
    public class EenemyCheck : MonoBehaviour
    {
        [HideInInspector] public Transform CameraTarget;
        [HideInInspector] public CameraTarget TargetScript;
        private Vector3 EnemyPosition;
        private bool BattleMode;
        private List<GameObject> EnemyList = new List<GameObject>();
        [HideInInspector] public bool IsSmash;
        [SerializeField] public PlayerController_FSM Controller;

        private void Start()
        {
            if (Controller.IsMine)
            {
                CameraTarget = GameObject.Find("CameraTarget").transform;
                TargetScript = CameraTarget.GetComponent<CameraTarget>();
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if (Controller.IsMine)
            {
                //상대방에 범위안에 들어올시 활성화
                if (!Controller.CompareTag(other.tag))
                {
                    BattleMode = true;
                    EnemyList.Add(other.gameObject);
                    CameraManager.Instance().BattleModeOn();

                    //하이라이트 연출시간이 아니면 진행
                    if (!CameraManager.Instance().IsHilight)
                    {
                        TargetScript.BattleMode = true;
                    }
                }
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (Controller.IsMine)
            {
                GameObject enemy = other.gameObject;

                if (EnemyList.Count >= 2)
                {
                    for (int i = 0; i < EnemyList.Count; ++i)
                    {
                        if (Vector3.Distance(EnemyList[i].transform.position, transform.position)
                            < Vector3.Distance(enemy.transform.position, transform.position))
                        {
                            enemy = EnemyList[i];
                        }
                    }
                }

                EnemyPosition = enemy.transform.position;
                BattleMode = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (Controller.IsMine)
            {
                if (!Controller.CompareTag(other.tag))
                {
                    EnemyList.Remove(other.gameObject);

                    if (EnemyList.Count == 0)
                    {
                        BattleMode = false;

                        if (!IsSmash)
                        {
                            CameraManager.Instance().BattleModeOff();
                        }
                    }
                }
            }
        }

        private void Update()
        {
            if (Controller.IsMine)
            {
                if (BattleMode)
                {
                    Vector3 CameraTargetPos = transform.position +
                        (EnemyPosition - transform.position).normalized *
                        (Vector3.Distance(EnemyPosition, transform.position) / 2);
                    CameraTarget.position = Vector3.Lerp(CameraTarget.position, CameraTargetPos, 20 * Time.deltaTime);
                }
            }
        }
    }
}
