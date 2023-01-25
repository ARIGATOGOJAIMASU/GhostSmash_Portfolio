using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Junpyo;

namespace Wooseok
{
    public abstract class FieldItem : MonoBehaviour
    {
        
        public float amount;
        public abstract void Effect(Junpyo.PlayerController_FSM player);
    }
}


