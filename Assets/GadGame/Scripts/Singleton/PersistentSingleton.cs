using UnityEngine;

namespace GadGame.Singleton
{
    /// <summary>
    /// Persistent version of the singleton. This will survive through scene
    /// loads. Perfect for system classes which require stateful, persistent data. Or audio sources
    /// where music plays through loading screens, etc
    /// </summary>
    /// <typeparam name="T">Type of object you want to use singleton</typeparam>
    public abstract class PersistentSingleton<T> : Singleton<T> where T: Component
    {
        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
        }
    }
}