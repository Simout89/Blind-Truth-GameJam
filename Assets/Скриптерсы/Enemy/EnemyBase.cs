using System;
using System.Collections;
using System.Linq;
using FMODUnity;
using UnityEngine;
using UnityEngine.AI;
using Скриптерсы.Datas;

namespace Скриптерсы.Enemy
{
    public class EnemyBase : MonoBehaviour
    {
        [SerializeField] public NavMeshAgent navMeshAgent;
        [SerializeField] private Transform container;
        [SerializeField] public Transform[] anchors;
        [SerializeField] public bool LoopPath = false;
        [SerializeField] public EnemyHealth EnemyHealth;
        [field: SerializeField] public EnemyData EnemyData;
        [HideInInspector] public int currentPoint = 0;
        [SerializeField] private TriggerDetector _triggerDetector;
        [SerializeField] private LayerMask _layerMask;
        [SerializeField] public Transform attackZone;
        [SerializeField] public Animator Animator;
        [SerializeField] private float footstepInterval = 0.3f;
        [SerializeField] private float minStepSpeed = 1f;
        
        [Header("FOV Settings")]
        [SerializeField] private float viewAngle = 90f;
        [SerializeField] private float viewDistance = 15f;
        [SerializeField] private bool showFOVGizmos = true;
        [SerializeField] private Color fovColor = new Color(1f, 0f, 0f, 0.3f);
        [SerializeField] private Color detectionColor = new Color(0f, 1f, 0f, 0.5f);
        [SerializeField] private int fovResolution = 20;
        
        private TimedInvoker stepSoundInvoker;
        public Transform PlayerTransform { get; private set; }

        private Fsm _fsm;

        private Coroutine detectionCoroutine;
        private bool isPlayerInFOV = false;

        private void Awake()
        {
            EnemyHealth.Init(EnemyData);
            navMeshAgent.speed = EnemyData.DefaultSpeed;

            stepSoundInvoker = new TimedInvoker(PlayStepSound, footstepInterval);
            
            _fsm = new Fsm();
            _fsm.AddState(new PatrolState(_fsm, this));
            _fsm.AddState(new PursuitState(_fsm, this));
            _fsm.AddState(new AttackState(_fsm, this));
            _fsm.AddState(new DeathState(_fsm, this));
            _fsm.ChangeState<PatrolState>();

            if (anchors != null && anchors.Length > 0)
                transform.position = anchors[0].position;
        }

        private void PlayStepSound()
        {
            if(EnemyData.FootStepSound != "")
                RuntimeManager.PlayOneShot(EnemyData.FootStepSound, transform.position);
        }

        private void OnEnable()
        {
            EnemyHealth.OnTakeDamage += HandleTakeDamage;
            EnemyHealth.OnDeath += HandleDeath;

            _triggerDetector.onTriggerEntered += HandleEntered;
            _triggerDetector.onTriggerExited += HandleExited;
        }

        private void OnDisable()
        {
            EnemyHealth.OnTakeDamage -= HandleTakeDamage;
            EnemyHealth.OnDeath -= HandleDeath;

            
            _triggerDetector.onTriggerEntered -= HandleEntered;
            _triggerDetector.onTriggerExited -= HandleExited;
        }

        private void HandleDeath()
        {
            _fsm.ChangeState<DeathState>();
        }

        private void HandleEntered(Collider obj)
        {
            if (obj.GetComponent<UnityEngine.CharacterController>() && detectionCoroutine == null)
            {
                PlayerTransform = obj.transform;
                detectionCoroutine = StartCoroutine(PlayerDetection());
            }
        }

        private void HandleExited(Collider obj)
        {
            if (obj.GetComponent<UnityEngine.CharacterController>() && detectionCoroutine != null)
            {
                StopCoroutine(detectionCoroutine);
                detectionCoroutine = null;
                isPlayerInFOV = false;
            }
        }

        private void HandleTakeDamage(DamageInfo damageInfo)
        {
            if(EnemyData.TakeDamageSound != "")
                RuntimeManager.PlayOneShot(EnemyData.TakeDamageSound, transform.position);
            
            PlayerTransform = damageInfo.Transform;
            if (_fsm.CurrentState.GetType() == typeof(PatrolState))
            {
                _fsm.ChangeState<PursuitState>();
            }
        }

        private void Update()
        {
            if (navMeshAgent.velocity.magnitude > EnemyData.PursuitSpeed / 2)
            {
                stepSoundInvoker.SetInterval(footstepInterval / 3);
                stepSoundInvoker.Tick();
            }
            else if (navMeshAgent.velocity.magnitude > 1f)
            {
                stepSoundInvoker.SetInterval(footstepInterval);
                stepSoundInvoker.Tick();
            }
            
            _fsm.Update();
        }

        private bool IsPlayerInFOV()
        {
            if (PlayerTransform == null) return false;

            Vector3 directionToPlayer = (PlayerTransform.position - _triggerDetector.transform.position).normalized;
            float distanceToPlayer = Vector3.Distance(_triggerDetector.transform.position, PlayerTransform.position);

            // Проверка дистанции
            if (distanceToPlayer > viewDistance) return false;

            // Проверка угла обзора
            float angleToPlayer = Vector3.Angle(_triggerDetector.transform.forward, directionToPlayer);
            if (angleToPlayer > viewAngle / 2f) return false;

            return true;
        }

        private IEnumerator PlayerDetection()
        {
            while (true)
            {
                if (!IsPlayerInFOV())
                {
                    isPlayerInFOV = false;
                    yield return null;
                    continue;
                }

                Vector3 direction = (PlayerTransform.position - _triggerDetector.transform.position).normalized;
        
                if (Physics.Raycast(_triggerDetector.transform.position, direction, out RaycastHit hit, viewDistance, _layerMask, QueryTriggerInteraction.Ignore) &&
                    hit.collider.GetComponent<CharacterController>())
                {
                    isPlayerInFOV = true;
                    float distanceFromAnchor = Vector3.Distance(PlayerTransform.position, anchors[currentPoint].position);
            
                    if (_fsm.CurrentState.GetType() == typeof(PatrolState) && 
                        distanceFromAnchor < EnemyData.MaxRangePursuit)
                    {
                        // Проверяем, может ли враг достичь игрока через NavMesh
                        NavMeshPath path = new NavMeshPath();
                        if (navMeshAgent.CalculatePath(PlayerTransform.position, path) && 
                            path.status == NavMeshPathStatus.PathComplete)
                        {
                            _fsm.ChangeState<PursuitState>();
                        }
                    }
                }
                else
                {
                    isPlayerInFOV = false;
                }
        
                yield return null;
            }
        }

        public void UpdateAnchorsFromContainer()
        {
            if (container == null)
            {
                anchors = Array.Empty<Transform>();
                return;
            }

            var newAnchors = container.GetComponentsInChildren<Transform>()
                .Where(t => t != container)
                .ToArray();

            if (!anchors.SequenceEqual(newAnchors))
            {
                anchors = newAnchors;
            }
        }

        private void OnDrawGizmos()
        {
            // Рисуем зону атаки
            if (attackZone != null && EnemyData != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(attackZone.position, EnemyData.AttackZoneRadius);
            }

            // Рисуем FOV
            if (showFOVGizmos && _triggerDetector != null)
            {
                DrawFOV();
            }
        }

        private void DrawFOV()
        {
            Vector3 origin = _triggerDetector.transform.position;
            Vector3 forward = _triggerDetector.transform.forward;

            // Цвет зависит от того, видит ли враг игрока
            Gizmos.color = isPlayerInFOV ? detectionColor : fovColor;

            // Рисуем линии границ FOV
            Vector3 leftBoundary = Quaternion.Euler(0, -viewAngle / 2f, 0) * forward * viewDistance;
            Vector3 rightBoundary = Quaternion.Euler(0, viewAngle / 2f, 0) * forward * viewDistance;

            Gizmos.DrawLine(origin, origin + leftBoundary);
            Gizmos.DrawLine(origin, origin + rightBoundary);

            // Рисуем дугу FOV
            Vector3 previousPoint = origin + leftBoundary;
            for (int i = 1; i <= fovResolution; i++)
            {
                float angle = -viewAngle / 2f + (viewAngle / fovResolution) * i;
                Vector3 direction = Quaternion.Euler(0, angle, 0) * forward;
                Vector3 point = origin + direction * viewDistance;
                
                Gizmos.DrawLine(previousPoint, point);
                previousPoint = point;
            }

            // Линия к центру FOV
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(origin, origin + forward * viewDistance);

            // Если игрок в зоне видимости, рисуем линию к нему
            if (isPlayerInFOV && PlayerTransform != null)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(origin, PlayerTransform.position);
            }
        }

        public Transform GetContainer() => container;
    }
}