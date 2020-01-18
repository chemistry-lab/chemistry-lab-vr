using UnityEngine;

namespace Utilities
{
    [RequireComponent(typeof(ParticleSystem))]
    public class ParticleManager : MonoBehaviour
    {

        private ParticleSystem ParticleSystem;

        public float LifeTime;
        private float lifeCounter;
        private bool active = false;

        void Update()
        {
            if (active)
            {
                lifeCounter = Mathf.Max(lifeCounter - Time.deltaTime, 0);
                if (lifeCounter == 0)
                {
                    Deactivate();
                }
            }
        }
        private void Awake()
        {
            ParticleSystem = GetComponent<ParticleSystem>();
            ParticleSystem.Stop();
        }

        public void Activate()
        {
            lifeCounter = LifeTime;
            active = true;
            ParticleSystem.Play();
        }

        public void Deactivate()
        {
            lifeCounter = 0;
            active = false;
            ParticleSystem.Stop();
        }
    }
}
