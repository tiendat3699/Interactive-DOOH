using UnityEngine;

namespace GadGame.State
{
    public abstract class State<T> where T : MonoBehaviour
    {
        protected T Runner;

        public void Init(T parent)
        {
            Runner = parent;
        }

        public abstract void Enter();
        public abstract void Update(float time);
        public abstract void Exit();
    }
}