using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(CharacterController))]
    public class MovementController : MonoBehaviour, IMixedRealityInputHandler<Vector2>
    {
        [Header("Input")]
        [SerializeField] private MixedRealityInputAction action = MixedRealityInputAction.None;
        [SerializeField] private float sensitivity = 0.1f;
        [SerializeField] private float magnitude = 0.2f;

        [Header("World")]
        [SerializeField] private float maxSpeed = 5.0f;
        [SerializeField] private float gravity = 10.0f;

        [Header("Head")]
        [SerializeField] private Transform head = null;

        private CharacterController controller = null;
        private Vector2 thumbstick = Vector2.zero;
        private float horizontalSpeed = 0.0f;
        private float verticalSpeed = 0.0f;

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
            CoreServices.InputSystem?.RegisterHandler<IMixedRealityInputHandler<Vector2>>(this);

            if (!controller) Debug.LogWarning("No CharacterController Component Provided!");
        }

        private void FixedUpdate()
        {
            HandleHeight();
            CalculateMovement();
        }

        public void OnInputChanged(InputEventData<Vector2> eventData)
        {
            if (eventData.MixedRealityInputAction != action)
            {
                return;
            }

            thumbstick = eventData.InputData;
            eventData.Use();
        }

        private void HandleHeight()
        {
            // Get the head in local space
            controller.height = head.localPosition.y;

            // Cut in half
            Vector3 center = Vector3.zero;
            center.y = controller.height / 2;
            center.y += controller.skinWidth;

            // Move capsule in local space
            center.x = head.localPosition.x;
            center.z = head.localPosition.z;

            // Apply
            controller.center = center;
        }

        private void CalculateMovement()
        {
            // Figure out movement orientation
            Quaternion orientation = CalculateOrientation();
            Vector3 movement = Vector3.zero;

            // If not moving
            if (thumbstick.magnitude <= magnitude) horizontalSpeed = 0;
            else horizontalSpeed += thumbstick.magnitude * sensitivity;

            // Clamp Speed
            horizontalSpeed = Mathf.Clamp(horizontalSpeed, -maxSpeed, maxSpeed);

            // Orientation
            movement += orientation * (horizontalSpeed * Vector3.forward);

            // Gravity
            if (controller.isGrounded) verticalSpeed = -gravity * 2;
            else verticalSpeed += -gravity * 2 * Time.deltaTime;
            movement.y = verticalSpeed * Time.deltaTime;

            // Apply
            controller.Move(movement * Time.deltaTime);
        }

        private Quaternion CalculateOrientation()
        {
            float rotation = Mathf.Atan2(thumbstick.x, thumbstick.y);
            rotation *= Mathf.Rad2Deg;

            Vector3 orientationEuler = new Vector3(0, head.eulerAngles.y + rotation, 0);
            return Quaternion.Euler(orientationEuler);
        }
    }
}
