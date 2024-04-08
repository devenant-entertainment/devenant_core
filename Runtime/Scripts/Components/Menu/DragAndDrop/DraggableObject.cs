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
        public static Action<DraggableObject> onUpdated;

        public string type { get { return _type; } }
        [SerializeField] private string _type;

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

        protected virtual bool OnBeginDrag(PointerEventData pointerEventData)
        {
            if (droppableObject == null)
            {
                return false;
            }

            isDragging = true;

            rectTransform.SetParent(canvasRectTransform);

            return true;
        }

        protected virtual bool OnDrag(PointerEventData pointerEventData)
        {
            if(droppableObject == null)
            {
                return false;
            }

            if(isDragging)
            {
                rectTransform.position = pointerEventData.position;
            }

            return true;
        }

        protected virtual bool OneEndDrag(PointerEventData pointerEventData)
        {
            if(droppableObject == null)
            {
                return false;
            }

            isDragging = false;

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerEventData, results);

            foreach(RaycastResult result in results)
            {
                DroppableObject oldDroppableObject = droppableObject;
                DroppableObject newDroppableObject = result.gameObject.GetComponent<DroppableObject>();

                if(newDroppableObject != null)
                {
                    DraggableObject otherDraggableObject = newDroppableObject.draggableObject;

                    if(SetDroppableObject(newDroppableObject))
                    {
                        if(otherDraggableObject != null)
                        {
                            otherDraggableObject.SetDroppableObject(oldDroppableObject);
                        }
                        else
                        {
                            oldDroppableObject.draggableObject = null;
                        }

                        return true;
                    }
                }
            }

            ResetPosition();

            return false;
        }

        public bool SetDroppableObject(DroppableObject newDroppableObject)
        {
            if(newDroppableObject == null)
            {
                return false;
            }

            if(newDroppableObject == droppableObject)
            {
                return false;
            }

            if(newDroppableObject.type != type)
            {
                return false;
            }

            droppableObject = newDroppableObject;
            droppableObject.draggableObject = this;

            ResetPosition();

            onUpdated?.Invoke(this);

            return true;
        }

        private void ResetPosition()
        {
            rectTransform.SetParent(droppableObject.transform);
            rectTransform.position = droppableObject.transform.position;
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