using UnityEngine;

namespace Utilities
{
    [RequireComponent(typeof(ParticleSystem))]
    public class ParticleManager : MonoBehaviour
    {
        [SerializeField] private float lifeTime = 0.0f;

        private ParticleSystem system = null;
        private bool isActive = false;
        private float counter = 0;

        void Update()
        {
            if (isActive)
            {
                counter = Mathf.Max(counter - Time.deltaTime, 0);
                if (counter == 0) Deactivate();
            }
        }
        private void Awake()
        {
            system = GetComponent<ParticleSystem>();
            system.Stop();
        }

        public void Activate()
        {
            counter = lifeTime;
            isActive = true;
            system.Play();
        }

        public void Deactivate()
        {
            counter = 0;
            isActive = false;
            system.Stop();
        }
    }
}
