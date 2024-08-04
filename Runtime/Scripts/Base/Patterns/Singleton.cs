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

                    if (_instance == null) 
                    { 
                        _instance = new GameObject(typeof(T).ToString()).AddComponent<T>();

                        DontDestroyOnLoad(_instance.gameObject);
                    }
                }

                return _instance;
            }
        }

        private static T _instance;
    }
}