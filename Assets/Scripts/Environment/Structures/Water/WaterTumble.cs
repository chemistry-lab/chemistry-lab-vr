using UnityEngine;

namespace Environment.Structures.Water
{
    public class WaterTumble : MonoBehaviour
    {
        [SerializeField] private float maxZRotation = 3.0f;
        [SerializeField] private float speed = 0.8f;

        private float currentZRotation = 0f;
        private float modifier = 1.0f;

        void Update()
        {
            if (currentZRotation >= maxZRotation) modifier = -1;
            if (currentZRotation <= 0f) modifier = 1;

            float delta = 1 * modifier * Time.deltaTime * speed;
            if (modifier == 1.0f)
            {
                if (delta + currentZRotation > maxZRotation)
                {
                    delta = maxZRotation - currentZRotation;
                }
            }
            else
            {
                if (currentZRotation - delta < 0f)
                {
                    delta = -currentZRotation;
                }
            }

            transform.Rotate(0, 0, delta);
            currentZRotation += delta;
        }
    }
}
