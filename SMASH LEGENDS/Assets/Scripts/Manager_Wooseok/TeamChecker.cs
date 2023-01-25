using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Wooseok
{
    public class TeamChecker : MonoBehaviour
    {
        // 싱글톤 패턴을 사용하기 위한 인스턴스 변수
        private static TeamChecker _instance;
        // 인스턴스에 접근하기 위한 프로퍼티
        public static TeamChecker Instance
        {
            get
            {
                // 인스턴스가 없는 경우에 접근하려 하면 인스턴스를 할당해준다.
                if (!_instance)
                {
                    _instance = FindObjectOfType(typeof(TeamChecker)) as TeamChecker;

                    if (_instance == null)
                        Debug.Log("no Singleton teamchecker");
                }
                return _instance;
            }
        }
        public bool TeamCheck(string team1, string team2)
        {
            if ((team1 == "Blue" && team2 == "Red") || (team1 == "Red" && team2 == "Blue"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }

}