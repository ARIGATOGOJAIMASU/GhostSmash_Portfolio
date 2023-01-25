using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Junpyo
{
    public class ReadyStart : MonoBehaviour
    {
        [SerializeField] GamLogicManager gamLogicManager;
        public void GameStart()
        {
            gamLogicManager.GameStart = true;
            GameManager.Instance.StartPlayer();
        }
    }
}
