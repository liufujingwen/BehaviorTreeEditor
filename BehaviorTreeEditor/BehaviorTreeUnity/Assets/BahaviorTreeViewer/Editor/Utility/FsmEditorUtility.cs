using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BT;

namespace BT.Editor
{
    public static class FsmEditorUtility
    {
        public static EventType ReserveEvent(params Rect[] areas)
        {
            EventType eventType = Event.current.type;
            foreach (Rect area in areas)
            {
                if ((area.Contains(Event.current.mousePosition) && (eventType == EventType.MouseDown || eventType == EventType.ScrollWheel)))
                {
                    Event.current.type = EventType.Ignore;
                }
            }
            return eventType;
        }

        public static void ReleaseEvent(EventType type)
        {
            if (Event.current.type != EventType.Used)
            {
                Event.current.type = type;
            }
        }
    }
}