using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Junpyo
{
    public class GroundCheack : MonoBehaviour
    {
        [SerializeField] private Transform playerTransform;
        private LineRenderer lineRenderer;

        //그라운드 체크용
        public Ray GroundCheackRay = new Ray();
        public RaycastHit GroundHit = new RaycastHit();
        public LayerMask layerMask;
        private float LastY;
        [HideInInspector]public bool Hang;
        public Vector3 NetworkPos;
        [SerializeField] bool IsLine;

        private void Awake()
        {
            lineRenderer = GetComponent<LineRenderer>();
            GroundCheackRay.direction = -playerTransform.up;
            Hang = false;
        }

        // Update is called once per frame
        private void LateUpdate()
        {
            if(Hang)
            {
                return;
            }

            //Ray위치 초기화
            Vector3 GroundPos = playerTransform.position;
            GroundCheackRay.origin = new Vector3(GroundPos.x, GroundPos.y + 0.01f, GroundPos.z);

            //그라운드 체크
            if (Physics.Raycast(GroundCheackRay, out GroundHit, 100, layerMask))
            {
                //판넬위치 설정
                GroundPos = GroundHit.point;
                GroundPos.y += 0.05f;
                LastY = GroundPos.y;
                transform.position = GroundPos;

                if (IsLine)
                {
                    //땅이랑 가까우면 굳이 점선 표현을 안 한다.
                    if (Vector3.Distance(transform.position, playerTransform.position) > 0.1f)
                    {
                        lineRenderer.enabled = true;
                        //공중점선 표현
                        lineRenderer.SetPosition(0, playerTransform.position);
                        lineRenderer.SetPosition(1, GroundPos);
                    }
                    else
                    {
                        lineRenderer.enabled = false;
                    }
                }
            }
            //밑에 아무것도 닿지 않는 경우
            else
            {
                transform.position = new Vector3(transform.position.x, LastY, transform.position.z);

                if (IsLine)
                {
                    lineRenderer.enabled = false;
                }
            }         
        }

        public void LineSet()
        {
            IsLine = !IsLine;

            lineRenderer.enabled = IsLine;
        }

        public void ColorChange()
        {
            GetComponent<SpriteRenderer>().color = Color.red;
            lineRenderer.startColor = Color.red;
            lineRenderer.endColor = Color.red;
        }
    }
}
