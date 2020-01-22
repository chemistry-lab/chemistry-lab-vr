using UnityEngine;

using Interactables;
using Atom.Builder;

namespace Atom
{
    [RequireComponent(typeof(Collider))]
    public class AtomButton : MonoBehaviour
    {
        [Header("Build")]
        [SerializeField] private AtomBuilder builder = null;

        [Header("Highlight")]
        [SerializeField] private Renderer button = null;
        [Range(0f, 0.1f)]
        [SerializeField] private float thickness = 0.025f;

        private void OnTriggerEnter(Collider other)
        {
            //builder.ResetAtom();
            button.material.SetFloat("_OutlineWidth", thickness);
        }

        private void OnTriggerExit(Collider other)
        {
            button.material.SetFloat("_OutlineWidth", 0f);
        }
    }
}
