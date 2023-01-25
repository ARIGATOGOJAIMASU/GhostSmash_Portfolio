using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


#if UNITY_EDITOR

using UnityEditor.SceneManagement;

#endif


namespace Wooseok
{
    //Edit에서 사용가능
    [ExecuteInEditMode]
    public class SectorColumnCollider : CustomCollider
    {
        // Start is called before the first frame update

        public Stick PivotSectorCol;
        public Skill_Colider StickCol;
        [SerializeField]
        private int StickNumber;
        int ex_StickNumber;
        [SerializeField]
        private float Angle;
        float ex_angle;

        [SerializeField]
        float mininterval;

        [SerializeField]
        public List<GameObject> ColliderList;
        [SerializeField]
        GameObject BaseObj;

        bool somethingchanged = false;
        
        // Update is called once per frame
        void Update()
        {
            if (BaseObj == null)
            {
                BaseObj = Instantiate(BaseObject, this.transform); // BaseObj를 생성하여, 복사 할 게임 오브젝트의 기반으로 삼음.
                BaseObj.SetActive(false);
            }

            if (somethingchanged)
            {
                MakeCollider();
                UpdateEx();
                somethingchanged = false;
            }
        }

        public override void MakeCollider()
        {

#if UNITY_EDITOR

            
            var stage = UnityEditor.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage(); // 현재 스테이지가 어디인가 확인
            Stick pivotPoint = BaseObj.GetComponent<Stick>(); // BaseObj에 있는 Stick의 주소를 지정
            pivotPoint.PivotSectorCol = this;
            BaseObj.transform.localPosition = Position; // 막대기 로컬 트랜스폼 지정해주기1
            BaseObj.transform.localEulerAngles = Rotation; // 2
            BaseObj.transform.localScale = Scale; // 3

            if(stage != null) // PrefabScene인 경우
            {
                foreach (GameObject stick in ColliderList) // 콜라이더 리스트에 있는 게임 오브젝트들을 삭제함
                {
                    GameObject.DestroyImmediate(stick);
                }
            }
            else // 프리팹인 경우 프리팹을 해제하고 삭제함.
            {
                if(
                    PrefabUtility.GetCorrespondingObjectFromSource(this.gameObject) != null &&
                    PrefabUtility.GetPrefabInstanceHandle(this.gameObject) != null
                    )
                {
                    PrefabUtility.UnpackPrefabInstance(this.gameObject, PrefabUnpackMode.OutermostRoot, InteractionMode.AutomatedAction);
                }

                foreach (GameObject stick in ColliderList) // 콜라이더 리스트에 있는 게임 오브젝트들을 삭제함
                {
                    GameObject.DestroyImmediate(stick);
                }
            }

            for(int i = 0; i < ColliderList.Count; i++)
            {
                ColliderList[i] = null;
            }

            ColliderList.Clear();
            GameObject tempgo;
            
            //ㅇㅇㅇㄱㄷ
            if(pivotPoint.gameObject != null)
            {
                pivotPoint.StickCol.gameObject.transform.localPosition = new Vector3(this.Scale.x * this.Pivot.x / 2, this.Scale.y * this.Pivot.y / 2, this.Pivot.z * this.Scale.z / 2);
            }
            
            if (StickNumber % 2 == 0) // 짝수 경우 위치 지정
            {
                for (int i = 0; i < StickNumber; i++)
                {
                    tempgo = Instantiate(BaseObj, this.transform.position + BaseObj.transform.localPosition, Quaternion.Euler(this.transform.localEulerAngles + BaseObj.transform.localEulerAngles), transform);
                    switch (Axis)
                    {
                        case AXIS.X:
                            tempgo.transform.Rotate(Vector3.right, (((float)i - ((float)StickNumber / 2.0f)) * Angle / (StickNumber - 1)));
                            break;
                        case AXIS.Y:
                            tempgo.transform.Rotate(Vector3.up, (((float)i - ((float)StickNumber / 2.0f)) * Angle / (StickNumber - 1)));
                            break;
                        case AXIS.Z:
                            tempgo.transform.Rotate(Vector3.forward, (((float)i - ((float)StickNumber / 2.0f)) * Angle / (StickNumber - 1)));
                            break;
                        default:
                            break;
                    }
                    ColliderList.Add(tempgo);
                    tempgo.name += (" " + i);
                    tempgo.SetActive(true);

                }
            }
            else // 홀수경우 위치 지정
            {
                for (int i = 0; i < StickNumber; i++)
                {
                    tempgo = Instantiate(BaseObj, this.transform.position + BaseObj.transform.localPosition, Quaternion.Euler(this.transform.localEulerAngles + BaseObj.transform.localEulerAngles), transform);
                    switch (Axis)
                    {
                        case AXIS.X:
                            tempgo.transform.Rotate(Vector3.right, (((float)i - ((float)StickNumber / 2.0f)) * Angle / (StickNumber - 1)));
                            break;
                        case AXIS.Y:
                            tempgo.transform.Rotate(Vector3.up, (((float)i - ((float)StickNumber / 2.0f)) * Angle / (StickNumber - 1)));
                            break;
                        case AXIS.Z:
                            tempgo.transform.Rotate(Vector3.back, (((float)i - ((float)StickNumber / 2.0f)) * Angle / (StickNumber - 1)));
                            break;
                        default:
                            break;
                    }
                    ColliderList.Add(tempgo);
                    tempgo.SetActive(true);
                }
            }
#endif

        }

        //Value가 변경시 활성화
        private void OnValidate()
        {
            somethingchanged = true;
        }

        public override void UpdateEx()
        {
            if (Vector3.Distance(ex_position, Position) < mininterval)
            {
                ex_position = Position;
            }
            if(Vector3.Distance(ex_rotation, Rotation) < mininterval)
            {
                ex_rotation = Rotation;
            }
            if(Vector3.Distance(ex_scale, Scale) < mininterval)
            {
                ex_scale = Scale;
            }
            if(Vector3.Distance(ex_pivot, Pivot) < mininterval)
            {
                ex_pivot = Pivot;
            }
            if(Axis != ex_axis)
            {
                ex_axis = Axis;
            }
            if(ex_BaseObject != BaseObject)
            {
                ex_BaseObject = BaseObject;
            }
            if(ex_StickNumber != StickNumber)
            {
                ex_StickNumber = StickNumber;
            }
            if(Mathf.Abs(360 - (ex_angle - Angle % 360)) < mininterval)
            {
                ex_angle = Angle;
            }
        }
    }
}
