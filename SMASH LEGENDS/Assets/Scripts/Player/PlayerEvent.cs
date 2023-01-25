using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Junpyo
{
    public enum EVENTTYPE { SETPOS}

    public struct EventStruct
    {
        public EVENTTYPE Type;
        public Vector3 Pos;

        public EventStruct(EVENTTYPE type, Vector3 pos)
        {
            Type = type;
            Pos = pos;
        }
    }

    public class PlayerEvent : MonoBehaviour
    {
        public List<EventStruct> EventList = new List<EventStruct>();

        public void EventStart()
        {
            for (int i = 0; i < EventList.Count; ++i)
            {
                switch (EventList[i].Type)
                {
                    case EVENTTYPE.SETPOS:
                        SetPos(EventList[i].Pos);
                        break;
                }
            }

            EventList.Clear();
        }

        public void SetPos(Vector3 pos)
        {
            transform.position = pos; 
        }

        public void AddEvent(EventStruct temp)
        {
            EventList.Add(temp);
        }

        public bool IsEvent()
        {
            return EventList.Count > 0 ? true : false;
        }
    }
}
