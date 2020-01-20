using UnityEngine;

using Interaction;

namespace Environment.Props.Sample
{
    public class SampleContainer : MonoBehaviour
    {
        [SerializeField] private Renderer liquid = null;

        private Sampleable sample = null;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.GetComponent<Sampleable>()) return;
            sample = other.GetComponent<Sampleable>();
            liquid.material.color = sample.Material.color;
        }

        public Sampleable Sample { get { return sample; } }
    }
}
