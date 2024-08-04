using UnityEngine;

namespace Devenant
{
    [RequireComponent(typeof(IInitializable))]
    public class InitializableObject : MonoBehaviour
    {
        public IInitializable initializable { get { if (_initializable == null) { _initializable = GetComponent<IInitializable>(); } return _initializable; } }
        private IInitializable _initializable;
    }
}
