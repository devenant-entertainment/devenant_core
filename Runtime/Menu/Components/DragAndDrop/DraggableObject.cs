using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Devenant
{
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(CanvasGroup))]
    [RequireComponent(typeof(EventTrigger))]
    public class DraggableObject : MonoBehaviour
    {
        public Action<DroppableObject> onDroppableObjectUpdated;

        protected RectTransform canvasRectTransform { get { return _canvasRectTransform; } private set { _canvasRectTransform = value; } }
        private RectTransform _canvasRectTransform;

        protected RectTransform rectTransform { get { return _rectTransform; } private set { _rectTransform = value; } }
        private RectTransform _rectTransform;

        protected EventTrigger eventTrigger { get { return _eventTrigger; } private set { _eventTrigger = value; } }
        private EventTrigger _eventTrigger;

        public DroppableObject droppableObject { get { return _droppableObject; } private set { _droppableObject = value; } }
        private DroppableObject _droppableObject;

        public bool isDragging { get { return _isDragging; } private set { _isDragging = value; } }
        private bool _isDragging;

        private void OnEnable()
        {
            canvasRectTransform = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
            rectTransform = GetComponent<RectTransform>();
            eventTrigger = GetComponent<EventTrigger>();

            eventTrigger.triggers.Add(AddTrigger(EventTriggerType.BeginDrag, (BaseEventData baseEventData) =>
            {
                OnBeginDrag((PointerEventData)baseEventData);
            }));

            eventTrigger.triggers.Add(AddTrigger(EventTriggerType.Drag, (BaseEventData baseEventData)=> 
            {
                OnDrag((PointerEventData)baseEventData);
            }));

            eventTrigger.triggers.Add(AddTrigger(EventTriggerType.EndDrag, (BaseEventData baseEventData) =>
            {
                OneEndDrag((PointerEventData)baseEventData);
            }));

            droppableObject = GetComponentInParent<DroppableObject>();
        }

        protected virtual void OnBeginDrag(PointerEventData pointerEventData)
        {
            isDragging = true;

            rectTransform.SetParent(canvasRectTransform);
        }

        protected virtual void OnDrag(PointerEventData pointerEventData)
        {
            if(isDragging)
            {
                rectTransform.position = pointerEventData.position;
            }
        }

        protected virtual void OneEndDrag(PointerEventData pointerEventData)
        {
            isDragging = false;

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerEventData, results);

            foreach(RaycastResult result in results)
            {
                DroppableObject oldDroppableObject = droppableObject;
                DroppableObject newDroppableObject = result.gameObject.GetComponent<DroppableObject>();

                if (newDroppableObject != null)
                {
                    DraggableObject otherDraggableObject = newDroppableObject.draggableObject;

                    if(newDroppableObject.SetDraggableObject(this))
                    {
                        oldDroppableObject.SetDraggableObject(otherDraggableObject);

                        return;
                    }
                }
            }

            rectTransform.SetParent(droppableObject.transform);
            rectTransform.position = droppableObject.transform.position;
        }

        public void SetDroppableObject(DroppableObject droppableObject)
        {
            if (this.droppableObject != droppableObject)
            {
                this.droppableObject = droppableObject;
                rectTransform.SetParent(droppableObject.transform);
                rectTransform.position = droppableObject.transform.position;

                onDroppableObjectUpdated?.Invoke(droppableObject);
            }
        }

        private EventTrigger.Entry AddTrigger(EventTriggerType eventTriggerType, UnityAction<BaseEventData> action)
        {
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = eventTriggerType;
            entry.callback.AddListener(action);
            return entry;
        }
    }
}