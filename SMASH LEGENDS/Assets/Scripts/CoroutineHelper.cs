using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Junpyo
{
    public class CoroutineHelper : MonoBehaviour
    {
        private static MonoBehaviour monoInstance;

        //Awake메서드가 호출 된 후에 한번만 실행
        [RuntimeInitializeOnLoadMethod]
        private static void Initializer()
        {
            monoInstance = new GameObject($"[{nameof(CoroutineHelper)}]").AddComponent<CoroutineHelper>();
            DontDestroyOnLoad(monoInstance.gameObject);
        }

        //여러 코루틴을 돌리기 위해 public new Static을 사용
        public new static Coroutine StartCoroutine(IEnumerator coroutine)
        {
            return monoInstance.StartCoroutine(coroutine);
        }

        public new static void StopCoroutine(Coroutine coroutine)
        {
            monoInstance.StopCoroutine(coroutine);
        }
    }
}