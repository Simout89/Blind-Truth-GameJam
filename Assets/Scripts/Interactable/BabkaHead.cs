using UnityEngine;

namespace Скриптерсы.Interactable
{
    public class BabkaHead : MonoBehaviour
    {
        [SerializeField] private Transform headBone;
        [SerializeField] private Vector3 rotationOffset; // оффсет в градусах
        private Transform playerCamera;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out CameraController cameraController))
                playerCamera = cameraController.CameraTransform;
        }

        private void FixedUpdate()
        {
            if (playerCamera == null)
                return;

            // направление от головы к игроку
            Vector3 direction = playerCamera.position - headBone.position;
            direction.y = 0f; // ограничиваем только горизонтальный поворот (по желанию)

            if (direction.sqrMagnitude < 0.001f)
                return;

            // вычисляем целевой поворот
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);

            // применяем оффсет
            targetRotation *= Quaternion.Euler(rotationOffset);

            // обнуляем вращение по Z
            Vector3 euler = targetRotation.eulerAngles;

            headBone.rotation = Quaternion.Euler(euler);
        }
    }
}