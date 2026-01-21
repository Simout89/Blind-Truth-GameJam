using System;
using UnityEngine;
using Unity.Cinemachine;
using Zenject;
using Скриптерсы.Datas;
using Скриптерсы.Services;

namespace Скриптерсы
{
    public class CharacterController : MonoBehaviour
    {
        [SerializeField] private UnityEngine.CharacterController _characterController;
        [field: SerializeField] public CharacterControllerData CharacterControllerData { get; private set; }
        [field: SerializeField] public PlayerStats PlayerStats { get; private set; }
        [SerializeField] private CinemachineCamera _virtualCamera;
        [Inject] public IInputService _inputService { get; private set; }

        private float _verticalVelocity;
        private Transform _cameraTransform;

        private bool enable = true;

        private void Awake()
        {
            if (_characterController == null)
                _characterController = GetComponent<UnityEngine.CharacterController>();

            // Получаем Transform главной камеры
            if (_virtualCamera != null)
                _cameraTransform = _virtualCamera.transform;
            else
                _cameraTransform = Camera.main.transform;
        }

        private void Update()
        {
            if(!enable)
                return;
            
            Move();
        }

        private void Move()
        {
            var Move = _inputService.InputSystemActions.Player.Move.ReadValue<Vector2>();
            
            float h = Move.x;
            float v = Move.y;

            // Получаем направления относительно камеры
            Vector3 cameraForward = _cameraTransform.forward;
            Vector3 cameraRight = _cameraTransform.right;

            // Убираем вертикальную составляющую (Y), чтобы персонаж двигался только по горизонтали
            cameraForward.y = 0;
            cameraRight.y = 0;
            cameraForward.Normalize();
            cameraRight.Normalize();

            // Вычисляем направление движения относительно камеры
            Vector3 move = cameraRight * h + cameraForward * v;
            _characterController.Move(move * PlayerStats.MoveSpeed.Multiplier * CharacterControllerData.MoveSpeed * Time.deltaTime + Gravity());

            // Поворачиваем персонажа в направлении движения
            if (move.magnitude > 0.1f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(move);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
            }
        }

        private Vector3 Gravity()
        {
            if (_characterController.isGrounded && _verticalVelocity < 0)
                _verticalVelocity = -2f;

            _verticalVelocity += CharacterControllerData.Gravity * Time.deltaTime;
            return Vector3.up * _verticalVelocity * Time.deltaTime;
        }

        public void Teleport(Vector3 position)
        {
            _characterController.enabled = false;
            transform.position = position;
            _characterController.enabled = true;
            _verticalVelocity = 0f;
        }

        public void Enable()
        {
            enable = true;
        }

        public void Disable()
        {
            enable = false;
            _characterController.Move(Vector3.zero);
        }
    }
}