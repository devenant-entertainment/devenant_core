using System.Linq;
using UnityEngine;

namespace Devenant
{
    [System.Serializable]
    public class PlatformValue<T>
    {
        [System.Serializable]
        public class Value
        {
            public RuntimePlatform platform { get { return _platform; } private set { _platform = value; } }
            [SerializeField] private RuntimePlatform _platform;

            public T value { get { return _value; } private set { _value = value; } }
            [SerializeField] private T _value;

            public Value(RuntimePlatform platform, T value)
            {
                this.platform = platform;
                this.value = value;
            }
        }

        public Value[] values { get { return _values; } }
        [SerializeField] private Value[] _values;

        public T value { get { if(_value == null) _value = values.ToList().Find((x) => x.platform == Application.platform).value; return _value; } }
        private T _value;
    }
}
