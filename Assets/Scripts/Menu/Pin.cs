using UnityEngine;

using Interactables;
using Utilities;

namespace Menu
{
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(AudioSource))]
    public class Pin : MonoBehaviour, Highlightable, Interactable
    {
        [Header("Interaction")]
        [SerializeField] private Renderer ball = null;
        [SerializeField] private Material activeMaterial = null;
        [SerializeField] private Material inActiveMaterial = null;

        [Header("General")]
        [SerializeField] private SceneLoader loader = null;
        [SerializeField] private UnityScene scene = null;

        [Header("Audio")]
        [SerializeField] private AudioClip clickAudioClip = null;

        private AudioSource audioSource = null;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            audioSource.clip = clickAudioClip;
        }

        public void OnHighlightStart()
        {
            ball.material = activeMaterial;
        }

        public void OnHighlightStop()
        {
            ball.material = inActiveMaterial;
        }

        public void OnInteractionStart()
        {
            audioSource.Play();
            loader.LoadScene(scene);
        }

        public void OnInteractionStop() { }
    }
}
