using UnityEngine;

namespace Devenant
{
    public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        public static T instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = FindFirstObjectByType<T>();
                }

                return _instance;
            }
        }

        private static T _instance;
    }
}