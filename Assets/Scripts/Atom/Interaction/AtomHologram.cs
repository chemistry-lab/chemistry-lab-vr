using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit;
using UnityEngine;

using Atom.Builder;
using Player;

namespace Atom.Interaction
{
    public class AtomHologram : MonoBehaviour, IMixedRealityInputHandler
    {
        [Header("Input")]
        [SerializeField] private MixedRealityInputAction action = MixedRealityInputAction.None;
        [SerializeField] private AtomBuilder builder = null;

        private bool inTrigger = false;

        private void Awake()
        {
            CoreServices.InputSystem?.RegisterHandler<IMixedRealityInputHandler>(this);
        }

        public void OnInputUp(InputEventData eventData) { }

        public void OnInputDown(InputEventData eventData)
        {
            if (eventData.MixedRealityInputAction == action && inTrigger) builder.CreateAtom();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<GrabbableController>()) inTrigger = true;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<GrabbableController>()) inTrigger = false;
        }
    }
}
