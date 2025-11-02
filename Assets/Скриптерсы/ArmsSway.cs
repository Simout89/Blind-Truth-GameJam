using UnityEngine;
using Zenject;
using CharacterController = Скриптерсы.CharacterController;

namespace _Script.Player
{
    public class ArmsSway: MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform arms;
        [SerializeField] private CharacterController playerController;

        [Header("Mouse Look Sway")]
        [SerializeField] private bool enableMouseSway = true;
        [SerializeField] private float swayAmount = 0.02f;
        [SerializeField] private float maxSwayAmount = 0.05f;
        [SerializeField] private float smoothAmount = 6f;

        [Header("Movement Sway")]
        [SerializeField] private bool enableMovementSway = true;
        [SerializeField] private float movementSwayAmount = 0.015f;
        [SerializeField] private float movementSwaySpeed = 5f;
        [SerializeField] private float movementBobAmount = 0.02f;

        [Header("Passive Sway")]
        [SerializeField] private bool enablePassiveSway = true;
        [SerializeField] private float passiveSwayAmount = 0.005f;
        [SerializeField] private float passiveSwaySpeed = 1f;

        private Vector3 initialPosition;
        private Vector3 currentSway;
        private float movementSwayTimer;
        private Vector3 landingOffset;
        private Vector3 jumpOffset;
        private float passiveSwayTimer;

        void Awake()
        {
            initialPosition = arms.localPosition;
            currentSway = Vector3.zero;
            landingOffset = Vector3.zero;
            jumpOffset = Vector3.zero;
        }

        void LateUpdate()
        {
            Vector3 targetSway = Vector3.zero;

            // Mouse look sway
            if (enableMouseSway)
            {
                Vector2 look = playerController._inputService.InputSystemActions.Player.Look.ReadValue<Vector2>();
                float moveX = -look.x * swayAmount;
                float moveY = -look.y * swayAmount;

                moveX = Mathf.Clamp(moveX, -maxSwayAmount, maxSwayAmount);
                moveY = Mathf.Clamp(moveY, -maxSwayAmount, maxSwayAmount);

                targetSway = new Vector3(moveX, moveY, 0);
            }

            // Movement sway
            if (enableMovementSway)
            {
                Vector2 moveInput = playerController._inputService.InputSystemActions.Player.Move.ReadValue<Vector2>();

                if (moveInput.magnitude > 0.1f)
                {
                    movementSwayTimer += Time.deltaTime * movementSwaySpeed;

                    float horizontalSway = Mathf.Sin(movementSwayTimer) * movementSwayAmount * moveInput.magnitude;
                    float verticalBob = Mathf.Sin(movementSwayTimer * 2f) * movementBobAmount * moveInput.magnitude;
                    float directionTilt = moveInput.x * movementSwayAmount * 0.5f;

                    targetSway += new Vector3(horizontalSway + directionTilt, verticalBob, 0);
                }
                else
                {
                    movementSwayTimer = 0f;
                }
            }

            // Passive sway
            if (enablePassiveSway)
            {
                passiveSwayTimer += Time.deltaTime * passiveSwaySpeed;
                float swayX = Mathf.Sin(passiveSwayTimer * 0.7f) * passiveSwayAmount;
                float swayY = Mathf.Sin(passiveSwayTimer) * passiveSwayAmount;
                targetSway += new Vector3(swayX, swayY, 0);
            }

            currentSway = Vector3.Lerp(currentSway, targetSway, Time.deltaTime * smoothAmount);
            arms.localPosition = initialPosition + currentSway + landingOffset + jumpOffset;
        }
    }
}
