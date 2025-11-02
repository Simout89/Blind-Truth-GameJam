using UnityEngine;

namespace Скриптерсы.Enemy
{
    public class PatrolState: FsmState
    {
        private EnemyBase enemyBase;
        private bool forwardDirection = true;
        
        public PatrolState(Fsm fsm, EnemyBase enemyBase) : base(fsm)
        {
            this.enemyBase = enemyBase;
        }

        public override void Enter()
        {
            base.Enter();
            enemyBase.navMeshAgent.destination = enemyBase.anchors[enemyBase.currentPoint].position;
        }

        public override void Update()
        {
            base.Update();

            if (Vector3.Distance(enemyBase.anchors[enemyBase.currentPoint].position, enemyBase.transform.position) <= 1f)
            {
                if (forwardDirection)
                {
                    enemyBase.currentPoint++;
                    
                    if (enemyBase.currentPoint == enemyBase.anchors.Length)
                    {
                        enemyBase.currentPoint--;
                        forwardDirection = false;
                    }
                }
                else
                {
                    enemyBase.currentPoint--;
                    
                    if (enemyBase.currentPoint == -1)
                    {
                        enemyBase.currentPoint++;
                        forwardDirection = true;
                    }
                }
                enemyBase.navMeshAgent.destination = enemyBase.anchors[enemyBase.currentPoint].position;
            }
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}