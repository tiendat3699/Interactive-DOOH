using Pools.Runtime;
using UnityEngine;

namespace GadGame.MiniGame
{
    public class Basket : MonoBehaviour
    {
        public bool Active;

        public Vector3 Position
        {
            get => transform.position;
            set => transform.position = value;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if(!Active) return;
            if (other.TryGetComponent(out ICollect item))
            {
                item.Collect();
            }
        }
    }
}