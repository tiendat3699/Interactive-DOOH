using UnityEngine;

namespace GadGame.Singleton
{
    /// <summary>
    /// This is basic singleton. This will destroy any new
    /// versions created, leaving the original instance intact
    /// </summary>
    /// <typeparam name="T">Type of object you want to use singleton</typeparam>
    public abstract class Singleton<T> : MonoBehaviour where T : Component
    {
        private static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();
                    if (_instance == null)
                    {
                        GameObject obj = new(typeof(T).Name);
                        _instance = obj.AddComponent<T>();
                    }
                }

                return _instance;
            }
        }

        protected virtual void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }
            
            _instance = this as T;
        }
    }
}