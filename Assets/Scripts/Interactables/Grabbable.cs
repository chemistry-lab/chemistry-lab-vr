using UnityEngine;

using Player;

namespace Interactables
{
    [RequireComponent(typeof(Rigidbody))]
    public class Grabbable : MonoBehaviour
    {
        [Header("Equipping")]
        [SerializeField] private bool isEquippable = false;
        [SerializeField] private Vector3 equippableOffset = Vector3.zero;

        [Header("Throwing")]
        [SerializeField] private bool isThrowable = false;
        [SerializeField] private float throwableMultiplier = 2.0f;

        private Rigidbody rigidBody = null;
        private int grabbableLayer = 9;
        private int defaultLayer = 0;

        private void Awake()
        {
            rigidBody = GetComponent<Rigidbody>();
            if (!rigidBody) Debug.LogWarning("No Rigidbody Component Provided!");

            defaultLayer = gameObject.layer;
        }

        public void OnPickup()
        {
            gameObject.layer = grabbableLayer;
        }

        public void OnDrop()
        {
            gameObject.layer = defaultLayer;
        }

        public void OnDrop(Vector3 velocity, Vector3 angularVelocity)
        {
            rigidBody.velocity = velocity * throwableMultiplier;
            rigidBody.angularVelocity = angularVelocity * throwableMultiplier;
        }

        public GrabbableController ActiveController { get; set; }

        public bool IsEquippable { get { return isEquippable; } }
        public Vector3 EquippableOffset { get { return equippableOffset; } }

        public bool IsThrowable { get { return isThrowable; } }
        public float ThrowableMultiplier { get { return throwableMultiplier; } }
    }
}
