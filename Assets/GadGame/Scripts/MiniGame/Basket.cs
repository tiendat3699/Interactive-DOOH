using UnityEngine;

namespace GadGame.MiniGame
{
    public class Basket : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out ICollect item))
            {
                item.Collect();
            }
        }
    }
}