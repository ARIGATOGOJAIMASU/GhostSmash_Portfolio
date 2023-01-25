using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Wooseok
{
    public class SelfDestroy : MonoBehaviour
    {
        [SerializeField] float LifeTime;
        [SerializeField] Queue queueEffect;

        float timer;
        public enum DYINGTYPE { DESTROY, DISABLE }

        [SerializeField]
        DYINGTYPE MyDyingType;

        private void FixedUpdate()
        {
            if (timer <= LifeTime)
            {
                timer += Time.fixedDeltaTime;
            }
            else
            {
                Dying();
            }
        }

        void Dying()
        {
            switch (MyDyingType)
            {
                case DYINGTYPE.DESTROY:
                    Destroy(this.gameObject);
                    break;
                case DYINGTYPE.DISABLE:
                    this.gameObject.SetActive(false);
                    break;
            }

        }

    }
}