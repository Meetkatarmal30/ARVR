
using UnityEngine;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace VirtualLibrary.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class FirstPersonController : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float walkSpeed = 3.5f;
        [SerializeField] private float sprintSpeed = 6f;
        [SerializeField] private float gravity = 18f;

        [Header("Mouse Look")]
        [SerializeField] private float mouseSensitivityX = 2f;
        [SerializeField] private float mouseSensitivityY = 2f;
        [SerializeField] private float minPitchAngle = -75f;
        [SerializeField] private float maxPitchAngle = 75f;

        [Header("Camera")]
        [SerializeField] private Transform playerCameraTransform;

        private CharacterController characterController;
        private float verticalVelocity;
        private float cameraPitch;
        private bool cursorLocked;

        public bool IsMoving { get; private set; }
        public float CurrentSpeed { get; private set; }

        private void Awake()
        {
            characterController = GetComponent<CharacterController>();

            if (playerCameraTransform == null)
            {
                Camera cam = GetComponentInChildren<Camera>();
                if (cam != null)
                    playerCameraTransform = cam.transform;
            }

            if (playerCameraTransform != null)
            {
                Vector3 angles = playerCameraTransform.localEulerAngles;
                cameraPitch = angles.x > 180f ? angles.x - 360f : angles.x;
            }
        }

        private void OnEnable()
        {
            UnlockCursor();
        }

        private void OnDisable()
        {
            UnlockCursor();
        }

        private void Update()
        {
            if (characterController == null ||
                !characterController.enabled)
                return;

            if (EscapePressed())
            {
                UnlockCursor();
                return;
            }

            if (!cursorLocked && ClickPressed())
                LockCursor();

            if (!cursorLocked)
                return;

            HandleLook();
            HandleMovement();
        }

        private void HandleLook()
        {
            Vector2 mouseDelta = ReadMouseDelta();

            float mouseX = mouseDelta.x * mouseSensitivityX * 0.1f;
            float mouseY = mouseDelta.y * mouseSensitivityY * 0.1f;

            transform.Rotate(Vector3.up * mouseX);

            cameraPitch -= mouseY;
            cameraPitch = Mathf.Clamp(
                cameraPitch, minPitchAngle, maxPitchAngle);

            if (playerCameraTransform != null)
            {
                playerCameraTransform.localRotation =
                    Quaternion.Euler(cameraPitch, 0f, 0f);
            }
        }

        private void HandleMovement()
        {
            Vector2 input = ReadMovementInput();

            Vector3 direction =
                transform.forward * input.y +
                transform.right * input.x;

            direction = Vector3.ClampMagnitude(direction, 1f);

            bool sprinting = SprintPressed();
            float speed = sprinting ? sprintSpeed : walkSpeed;

            IsMoving = input.sqrMagnitude > 0.01f;
            CurrentSpeed = IsMoving ? speed : 0f;

            if (characterController.isGrounded && verticalVelocity < 0f)
                verticalVelocity = -0.5f;
            else
                verticalVelocity -= gravity * Time.deltaTime;

            Vector3 velocity = direction * speed;
            velocity.y = verticalVelocity;

            characterController.Move(velocity * Time.deltaTime);
        }

        private Vector2 ReadMovementInput()
        {
#if ENABLE_INPUT_SYSTEM
            if (Keyboard.current != null)
            {
                float x = 0f;
                float y = 0f;

                if (Keyboard.current.aKey.isPressed) x -= 1f;
                if (Keyboard.current.dKey.isPressed) x += 1f;
                if (Keyboard.current.sKey.isPressed) y -= 1f;
                if (Keyboard.current.wKey.isPressed) y += 1f;

                return Vector2.ClampMagnitude(new Vector2(x, y), 1f);
            }
#endif

#if ENABLE_LEGACY_INPUT_MANAGER
            return new Vector2(
                Input.GetAxisRaw("Horizontal"),
                Input.GetAxisRaw("Vertical"));
#else
            return Vector2.zero;
#endif
        }

        private Vector2 ReadMouseDelta()
        {
#if ENABLE_INPUT_SYSTEM
            if (Mouse.current != null)
                return Mouse.current.delta.ReadValue();
#endif

#if ENABLE_LEGACY_INPUT_MANAGER
            return new Vector2(
                Input.GetAxis("Mouse X"),
                Input.GetAxis("Mouse Y"));
#else
            return Vector2.zero;
#endif
        }

        private bool ClickPressed()
        {
#if ENABLE_INPUT_SYSTEM
            if (Mouse.current != null &&
                Mouse.current.leftButton.wasPressedThisFrame)
                return true;
#endif

#if ENABLE_LEGACY_INPUT_MANAGER
            if (Input.GetMouseButtonDown(0))
                return true;
#endif

            return false;
        }

        private bool EscapePressed()
        {
#if ENABLE_INPUT_SYSTEM
            if (Keyboard.current != null &&
                Keyboard.current.escapeKey.wasPressedThisFrame)
                return true;
#endif

#if ENABLE_LEGACY_INPUT_MANAGER
            if (Input.GetKeyDown(KeyCode.Escape))
                return true;
#endif

            return false;
        }

        private bool SprintPressed()
        {
#if ENABLE_INPUT_SYSTEM
            if (Keyboard.current != null &&
                (Keyboard.current.leftShiftKey.isPressed ||
                 Keyboard.current.rightShiftKey.isPressed))
                return true;
#endif

#if ENABLE_LEGACY_INPUT_MANAGER
            if (Input.GetKey(KeyCode.LeftShift) ||
                Input.GetKey(KeyCode.RightShift))
                return true;
#endif

            return false;
        }

        private void LockCursor()
        {
            cursorLocked = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void UnlockCursor()
        {
            cursorLocked = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        public void SetLocomotionEnabled(bool enabled)
        {
            this.enabled = enabled;

            if (!enabled)
            {
                IsMoving = false;
                CurrentSpeed = 0f;
            }
        }

        public void TeleportTo(Vector3 position, Quaternion rotation)
        {
            if (characterController == null)
                return;

            characterController.enabled = false;
            transform.SetPositionAndRotation(position, rotation);
            characterController.enabled = true;
        }
    }
}
