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
        
        [Header("Audio")] 
        [SerializeField] private AudioSource audioSource = null;
        [SerializeField] private AudioClip audioClip = null;

        private void OnTriggerEnter(Collider other)
        {
            builder.ResetAtom();
            button.material.SetFloat("_OutlineWidth", thickness);
            audioSource.PlayOneShot(audioClip);
        }

        private void OnTriggerExit(Collider other)
        {
            button.material.SetFloat("_OutlineWidth", 0f);
        }
    }
}
