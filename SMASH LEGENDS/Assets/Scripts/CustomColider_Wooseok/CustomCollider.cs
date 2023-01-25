using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Wooseok
{


    public abstract class CustomCollider : MonoBehaviour
    {
        // Start is called before the first frame update
        protected enum AXIS {X = 0, Y = 1, Z = 2 }

        [SerializeField]
        public Skill ParentSkill;

        [SerializeField]
        public GameObject BaseObject;
        protected GameObject ex_BaseObject;

        [SerializeField]
        protected Vector3 Position;
        [SerializeField]
        protected Vector3 Rotation;
        [SerializeField]
        protected Vector3 Scale;
        [SerializeField]
        protected Vector3 Pivot;
        [SerializeField]
        protected AXIS Axis;

        protected Vector3 ex_position;
        protected Vector3 ex_rotation;
        protected Vector3 ex_scale;
        protected Vector3 ex_pivot;
        protected AXIS ex_axis;


        public abstract void MakeCollider();

        public abstract void UpdateEx();

        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}