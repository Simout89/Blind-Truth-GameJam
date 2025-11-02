using UnityEngine;

namespace Скриптерсы.Enemy
{
    public class DeathState: FsmState
    {
        private EnemyBase _enemyBase;
        
        public DeathState(Fsm fsm, EnemyBase enemyBase) : base(fsm)
        {
            _enemyBase = enemyBase;
        }

        public override void Enter()
        {
            _enemyBase.Animator.SetTrigger("Death");
            _enemyBase.navMeshAgent.destination = _enemyBase.transform.position;
            _enemyBase.navMeshAgent.isStopped = true;
            _enemyBase.StopAllCoroutines();

            _enemyBase.GetComponent<Collider>().enabled = false;
            _enemyBase.navMeshAgent.enabled = false;
            base.Enter();
        }

        public override void Update()
        {
            base.Update();
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}