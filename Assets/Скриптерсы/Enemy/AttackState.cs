using System.Collections;
using UnityEngine;

namespace Скриптерсы.Enemy
{
    public class AttackState: FsmState
    {
        private EnemyBase _enemyBase;
        private Fsm _fsm;
        private Coroutine _coroutine;
        
        public AttackState(Fsm fsm, EnemyBase enemyBase) : base(fsm)
        {
            _enemyBase = enemyBase;
            _fsm = fsm;
        }

        public override void Enter()
        {
            base.Enter();

            _enemyBase.navMeshAgent.destination = _enemyBase.transform.position;
            _enemyBase.navMeshAgent.isStopped = true;
            
            if(_coroutine == null)
                _coroutine = _enemyBase.StartCoroutine(Attack());
        }

        public override void Update()
        {
            base.Update();
        }

        public override void Exit()
        {
            base.Exit();
        }

        public IEnumerator Attack()
        {
            yield return new WaitForSeconds(_enemyBase.EnemyData.DelayBeforeAttack);
            
            Collider[] hitColliders = Physics.OverlapSphere(_enemyBase.attackZone.position, _enemyBase.EnemyData.AttackZoneRadius);
            foreach (var VARIABLE in hitColliders)
            {
                if (VARIABLE.GetComponent<CharacterController>() &&
                    VARIABLE.TryGetComponent(out IDamageable damageable))
                {
                    damageable.TakeDamage(new DamageInfo(_enemyBase.EnemyData.Damage, "enemy", _enemyBase.transform));
                }
            }
            
            _enemyBase.navMeshAgent.isStopped = false;
            _fsm.ChangeState<PursuitState>();
            _coroutine = null;
        }
    }
}