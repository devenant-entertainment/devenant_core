using UnityEngine;

namespace Devenant
{
    public class DroppableObject : MonoBehaviour
    {
        public Action<DraggableObject> onDraggableObjectUpdated;

        public DraggableObject draggableObject { get { return _draggableObject; } private set { _draggableObject = value; } }
        private DraggableObject _draggableObject;

        private void OnEnable()
        {
            draggableObject = GetComponentInChildren<DraggableObject>();
        }

        public virtual bool SetDraggableObject(DraggableObject draggableObject)
        {
            if (this.draggableObject != draggableObject)
            {
                this.draggableObject = draggableObject;

                if(draggableObject != null)
                {
                    draggableObject.SetDroppableObject(this);
                }

                onDraggableObjectUpdated?.Invoke(draggableObject);
            }

            return true;
        }
    }
}
