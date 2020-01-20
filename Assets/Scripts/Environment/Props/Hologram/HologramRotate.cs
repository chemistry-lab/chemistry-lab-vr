using UnityEngine;

namespace Environment.Props.Hologram
{
    public class HologramRotate : MonoBehaviour
    {
        [SerializeField] private float speed = 30f;

        void Update()
        {
            transform.Rotate(speed * Time.deltaTime, speed * Time.deltaTime, 0f);
        }
    }
}
