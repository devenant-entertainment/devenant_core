using UnityEngine;

namespace Devenant
{
    public class DroppableObject : MonoBehaviour
    {
        public static Action<DroppableObject> onUpdated;

        public string type { get { return _type; } }
        [SerializeField] private string _type;

        public DraggableObject draggableObject { get { return _draggableObject; } set { _draggableObject = value; onUpdated?.Invoke(this); } }
        private DraggableObject _draggableObject;
    }
}
