using UnityEngine;
using TMPro;

namespace Environment.Props.Sample
{
    [RequireComponent(typeof(Collider))]
    public class SampleMachine : MonoBehaviour
    {
        [Header("Screen")]
        [SerializeField] private TextMeshPro screen = null;
        [SerializeField] private string text = "";

        public void OnTriggerEnter(Collider other)
        {
            SampleContainer container = other.GetComponent<SampleContainer>();
            if (container && container.Sample) screen.text = container.Sample.Description;
        }

        public void OnTriggerExit(Collider other)
        {
            SampleContainer container = other.GetComponent<SampleContainer>();
            if (container && container.Sample) screen.text = text;
        }
    }
}
