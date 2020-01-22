using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit;
using UnityEngine;

using Interactables;

namespace Player
{
    public class InteractableController : MonoBehaviour, IMixedRealityInputHandler
    {
        [Header("Input")] [SerializeField] private MixedRealityInputAction action = MixedRealityInputAction.None;

        [Header("Raycast")] [SerializeField] private Vector3 angle = Vector3.zero;
        [SerializeField] private float maxDistance = 10.0f;

        private Highlightable currentHighlightable = null;
        private Interactable currentInteractable = null;

        private void Awake()
        {
            CoreServices.InputSystem?.RegisterHandler<IMixedRealityInputHandler>(this);
        }

        private void Update()
        {
            Cast();
        }

        public void OnInputUp(InputEventData eventData)
        {
            if (eventData.MixedRealityInputAction == action || currentInteractable == null)
            {
                return;
            }

            currentInteractable.OnInteractionStop();
            eventData.Use();
        }

        public void OnInputDown(InputEventData eventData)
        {
            if (eventData.MixedRealityInputAction == action || currentInteractable == null)
            {
                return;
            }

            currentInteractable.OnInteractionStart();
            eventData.Use();
        }

        private void Cast()
        {
            Quaternion x = Quaternion.AngleAxis(angle.x, new Vector3(1, 0, 0));
            Quaternion y = Quaternion.AngleAxis(angle.y, new Vector3(0, 1, 0));
            Quaternion z = Quaternion.AngleAxis(angle.z, new Vector3(0, 0, 1));
            Vector3 direction = x * y * z * transform.forward * maxDistance;

            Debug.DrawRay(transform.position, direction, Color.red);

            Ray ray = new Ray(transform.position, direction);
            if (Physics.Raycast(ray, out RaycastHit hit, maxDistance: maxDistance))
            {
                if (currentHighlightable != null) currentHighlightable.OnHighlightStop();

                if (hit.collider.CompareTag("Interactable"))
                {
                    currentHighlightable = hit.collider.gameObject.GetComponent<Highlightable>();
                    currentInteractable = hit.collider.gameObject.GetComponent<Interactable>();
                    currentHighlightable?.OnHighlightStart();
                }
                else
                {
                    currentHighlightable = null;
                    currentInteractable = null;
                }
            }
            else
            {
                if (currentHighlightable != null) currentHighlightable.OnHighlightStop();
                currentHighlightable = null;
                currentInteractable = null;
            }
        }
    }
}
