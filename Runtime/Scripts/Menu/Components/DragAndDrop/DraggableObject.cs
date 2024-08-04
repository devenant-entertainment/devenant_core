using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Devenant
{
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(CanvasGroup))]
    public class DraggableObject : EventableObject
    {
        public Action<DraggableObject> onUpdated;

        public string type { get { return _type; } }
        [SerializeField] private string _type;

        protected RectTransform canvasRectTransform { get { return _canvasRectTransform; } private set { _canvasRectTransform = value; } }
        private RectTransform _canvasRectTransform;

        protected RectTransform rectTransform { get { return _rectTransform; } private set { _rectTransform = value; } }
        private RectTransform _rectTransform;

        public DroppableObject droppableObject { get { return _droppableObject; } private set { _droppableObject = value; } }
        private DroppableObject _droppableObject;

        public bool isDragging { get { return _isDragging; } private set { _isDragging = value; } }
        private bool _isDragging;

        private void OnEnable()
        {
            canvasRectTransform = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
            rectTransform = GetComponent<RectTransform>();

            AddTrigger(EventTriggerType.BeginDrag, (BaseEventData baseEventData) => { OnBeginDrag((PointerEventData)baseEventData); });
            AddTrigger(EventTriggerType.Drag, (BaseEventData baseEventData) => { OnDrag((PointerEventData)baseEventData); });
            AddTrigger(EventTriggerType.EndDrag, (BaseEventData baseEventData) => { OnEndDrag((PointerEventData)baseEventData); });

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

        protected virtual bool OnEndDrag(PointerEventData pointerEventData)
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

        public virtual bool SetDroppableObject(DroppableObject newDroppableObject)
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

        protected virtual void ResetPosition()
        {
            rectTransform.SetParent(droppableObject.transform);
            rectTransform.position = droppableObject.transform.position;
        }
    }
}