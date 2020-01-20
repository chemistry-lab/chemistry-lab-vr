using UnityEngine;

namespace Interaction
{
    public class Sampleable : MonoBehaviour
    {
        [SerializeField] private string description = null;
        [SerializeField] private Material material = null;

        public string Description { get { return description; } }
        public Material Material { get { return material; } }
    }
}
