using UnityEngine;

using Interactables;
using Molecule.Builder;

namespace Molecule.Interaction
{
    [RequireComponent(typeof(Collider))]
    public class MoleculeButton : MonoBehaviour, Highlightable, Interactable
    {
        [Header("Build")]
        [SerializeField] private MoleculeBuilder builder = null;

        [Header("Highlight")]
        [SerializeField] private Renderer button = null;
        [Range(0f, 0.1f)]
        [SerializeField] private float thickness = 0.025f;

        public void OnInteractionStart()
        {
            builder.CreateMolecule();
        }

        public void OnInteractionStop() { }

        public void OnHighlightStart()
        {
            button.material.SetFloat("_OutlineWidth", thickness);
        }

        public void OnHighlightStop()
        {
            button.material.SetFloat("_OutlineWidth", 0f);
        }
    }
}
