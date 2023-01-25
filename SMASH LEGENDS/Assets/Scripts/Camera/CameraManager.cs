using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Junpyo
{
    public class CameraManager : MonoBehaviour
    {
        private static CameraManager m_Instance;
        public static CameraManager Instance()
        {

            if (m_Instance == null)
            {
                m_Instance = GameObject.Find("CameraManager").GetComponent<CameraManager>();
            }

            return m_Instance;
        }

        [SerializeField] 
        public Transform target;

        [SerializeField] 
        private float distance = 6f;

        [SerializeField] 
        private float CameraSpeed = 3.0f;

        [SerializeField]
        private GameObject MainCamera_Root;
        private Camera MainCamera;

        [SerializeField]
        private GameObject DeadCamera_Root;
        private Camera DeadCamera;

        [HideInInspector]
        public bool IsDead;

        [SerializeField]
        private float ZoomInSpeed;

        [SerializeField]
        private float ZoomOutSpeed;

        [SerializeField]
        private float BattleZoomInSpeed;

        [SerializeField]
        private float BattleZoomOutSpeed;

        private Vector3 TargetCameraPos;

        public float BattleDelay;

        public bool IsHilight;

        public float Test;

        public float ShakeDuration = 0.2f;
        public float magnitude;
        public bool Right = false;

        public float Out;
        public float ins;

        private Vector3 StartPos;

        private void Awake()
        {
            MainCamera = Camera.main;
            DeadCamera = DeadCamera_Root.GetComponent<Camera>();
        }

        private void LateUpdate()
        {
            Vector3 TargetCameraPos = transform.forward * -distance + target.position;
            transform.position = Vector3.Lerp(transform.position, TargetCameraPos, CameraSpeed * Time.deltaTime);
        }

        public void SetCameraPos()
        {    
            transform.position = transform.forward * -distance + target.position;
        }

        //플레이어가 죽을 경우
        public void CameraChange()
        {
            IsDead = !IsDead;
        
            //IsDead상태가 true일 경우 Main카메라를 끄고 Dead카메라를 킨다.
            MainCamera_Root.SetActive(!IsDead);
            DeadCamera_Root.SetActive(IsDead);
            //메인카메라 뷰도 똑같이 30으로 설정
            if (IsDead)
            {
                //하이라이트인 경우 전에 실행했던 줌효과를 멈춤
                IsHilight = true;
                StopCoroutine("BattleZoomIn");
                StopCoroutine("BattleZoomOut");

                MainCamera.fieldOfView = 35.0f;
                StartCoroutine(ZoomIn());
            }
            else
            {
                DeadCamera.fieldOfView = 50.0f;
                StartCoroutine(ZoomOut());
            }
        }

        IEnumerator ZoomOut()
        {
            target.GetComponent<CameraTarget>().BattleMode = false;

            while (true)
            {
                MainCamera.fieldOfView += ZoomOutSpeed * Time.deltaTime;

                if (MainCamera.fieldOfView >= 50.0f)
                {
                    MainCamera.fieldOfView = 50.0f;
                    IsHilight = false;
                    yield break;
                }
                yield return null;
            }
        }

        IEnumerator ZoomIn()
        {
            while (true)
            {
                DeadCamera.fieldOfView -= ZoomInSpeed * Time.deltaTime;

                if (DeadCamera.fieldOfView <= 35.0f)
                {
                    DeadCamera.fieldOfView = 35.0f;
                    yield break;
                }
                yield return null;
            }
        }

        public void BattleModeOn()
        {
            if (!IsHilight)
            {
                //IEnumerator temp = BattleZoomIn();
                StopCoroutine("BattleZoomIn");
                StopCoroutine("CameraBackZoomIn");
                StartCoroutine("BattleZoomOut");
            }
        }

        public void BattleModeOff()
        {
            if (!IsHilight)
            {
                //IEnumerator temp = BattleZoomOut();
                StopCoroutine("BattleZoomOut");
                StopCoroutine("CameraBackZoomIn");
                StartCoroutine("BattleZoomIn");
            }
        }

        IEnumerator BattleZoomIn()
        {
            yield return new WaitForSeconds(BattleDelay);

            target.GetComponent<CameraTarget>().BattleMode = false;

            while (true)
            {
                MainCamera.fieldOfView -= BattleZoomInSpeed * Time.deltaTime;

                if (MainCamera.fieldOfView <= 50.0f)
                {
                    MainCamera.fieldOfView = 50.0f;
                    yield break;
                }

                yield return null;
            }
        }
        IEnumerator BattleZoomOut()
        {
            while (true)
            {
                MainCamera.fieldOfView += BattleZoomOutSpeed * Time.deltaTime;

                if (MainCamera.fieldOfView >= Out)
                {
                    MainCamera.fieldOfView = Out;
                    yield break;
                }

                yield return null;
            }
        }

        public void CameraShaking()
        {
            StartPos = transform.position;
            StartCoroutine(Shaking());
        }

        IEnumerator Shaking()
        {
            float timer = 0;

            while (timer <= ShakeDuration)
            {
                transform.localPosition = (Vector3)Random.insideUnitSphere * magnitude + StartPos;

                timer += Time.deltaTime;
                yield return null;
            }

            transform.localPosition = StartPos;
            yield break;
        }

        public void CameraBack()
        {
            if (!IsHilight)
            {
                StopCoroutine("BattleZoomOut");
                StopCoroutine("BattleZoomIn");
                StartCoroutine("CameraBackZoomIn");
            }
        }

        IEnumerator CameraBackZoomIn()
        {
            target.GetComponent<CameraTarget>().BattleMode = false;

            while (true)
            {
                MainCamera.fieldOfView -= BattleZoomInSpeed * Time.deltaTime;

                if (MainCamera.fieldOfView <= 50.0f)
                {
                    MainCamera.fieldOfView = 50.0f;
                    yield break;
                }

                yield return null;
            }
        }
    }
}