using UnityEngine;

using Interactables;
using Atom.Builder;

namespace Atom
{
    [RequireComponent(typeof(Collider))]
    public class AtomButton : MonoBehaviour, Highlightable, Interactable
    {
        [Header("Build")]
        [SerializeField] private AtomBuilder builder = null;

        [Header("Highlight")]
        [SerializeField] private Renderer button = null;
        [Range(0f, 0.1f)]
        [SerializeField] private float thickness = 0.025f;

        public void OnInteractionStart()
        {
            builder.CreateAtom();
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
