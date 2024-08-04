using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Devenant
{
    [RequireComponent(typeof(EventTrigger))]
    public abstract class EventableObject : MonoBehaviour
    {
        public EventTrigger eventTrigger { get { if(_eventTrigger == null) { _eventTrigger = GetComponent<EventTrigger>(); } return _eventTrigger; } }
        private EventTrigger _eventTrigger;

        protected void AddTrigger(EventTriggerType eventTriggerType, Action<BaseEventData> action)
        {
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = eventTriggerType;
            entry.callback.AddListener((BaseEventData baseEventData) => { action?.Invoke(baseEventData); });

            eventTrigger.triggers.Add(entry);
        }
    }
}
