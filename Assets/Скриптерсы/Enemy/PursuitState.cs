using UnityEngine;

namespace Скриптерсы.Enemy
{
    public class PursuitState: FsmState
    {
        private EnemyBase _enemyBase;
        private Fsm _fsm;
        
        public PursuitState(Fsm fsm, EnemyBase enemyBase) : base(fsm)
        {
            _enemyBase = enemyBase;
            _fsm = fsm;
        }

        public override void Enter()
        {
            base.Enter();

            _enemyBase.navMeshAgent.destination = _enemyBase.PlayerTransform.position;
        }

        public override void Update()
        {
            base.Update();
            
            _enemyBase.navMeshAgent.destination = _enemyBase.PlayerTransform.position;

            if (Vector3.Distance(_enemyBase.transform.position, _enemyBase.anchors[_enemyBase.currentPoint].position) >=
                _enemyBase.EnemyData.MaxRangePursuit)
            {
                _fsm.ChangeState<PatrolState>();
            }
        }

        public override void Exit()
        {
            base.Exit();
            
        }
    }
}