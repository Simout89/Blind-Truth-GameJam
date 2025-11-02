using System;
using System.Collections;
using System.Linq;
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
        public Transform PlayerTransform { get; private set; }

        private Fsm _fsm;

        private Coroutine detectionCoroutine;

        private void Awake()
        {
            EnemyHealth.Init(EnemyData);
            
            _fsm = new Fsm();
            _fsm.AddState(new PatrolState(_fsm, this));
            _fsm.AddState(new PursuitState(_fsm, this));
            _fsm.AddState(new AttackState(_fsm, this));
            _fsm.ChangeState<PatrolState>();

            if (anchors != null && anchors.Length > 0)
                transform.position = anchors[0].position;
        }

        private void OnEnable()
        {
            EnemyHealth.OnTakeDamage += HandleTakeDamage;

            _triggerDetector.onTriggerEntered += HandleEntered;
            _triggerDetector.onTriggerExited += HandleExited;
        }

        private void OnDisable()
        {
            EnemyHealth.OnTakeDamage -= HandleTakeDamage;
            
            _triggerDetector.onTriggerEntered -= HandleEntered;
            _triggerDetector.onTriggerExited -= HandleExited;
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
            }
        }

        private void HandleTakeDamage(DamageInfo damageInfo)
        {
            PlayerTransform = damageInfo.Transform;
            if (_fsm.CurrentState.GetType() == typeof(PatrolState))
            {
                _fsm.ChangeState<PursuitState>();
            }
        }

        private void Update()
        {
            _fsm.Update();
        }

        private IEnumerator PlayerDetection()
        {
            while (true)
            {
                Vector3 direction = (PlayerTransform.position - _triggerDetector.transform.position).normalized;
        
                if (Physics.Raycast(_triggerDetector.transform.position, direction, out RaycastHit hit, 100, _layerMask, QueryTriggerInteraction.Ignore) &&
                    hit.collider.GetComponent<CharacterController>())
                {
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
            Gizmos.DrawSphere(attackZone.position, EnemyData.AttackZoneRadius);
        }

        public Transform GetContainer() => container;
    }
}