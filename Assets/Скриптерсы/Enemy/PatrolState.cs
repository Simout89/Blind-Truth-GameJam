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
            
            if(enemyBase.PlayerTransform != null)
                enemyBase.PlayerTransform.GetComponent<PursuitHandler>().StopPursuit(enemyBase);

            enemyBase.navMeshAgent.speed = enemyBase.EnemyData.DefaultSpeed;
            enemyBase.navMeshAgent.destination = enemyBase.anchors[enemyBase.currentPoint].position;
        }

        public override void Update()
        {
            base.Update();

            if (Vector3.Distance(enemyBase.anchors[enemyBase.currentPoint].position, enemyBase.transform.position) <= 1f)
            {
                if (enemyBase.LoopPath)
                {
                    // Режим цикличного патрулирования
                    enemyBase.currentPoint++;
                    
                    if (enemyBase.currentPoint >= enemyBase.anchors.Length)
                    {
                        enemyBase.currentPoint = 0;
                    }
                }
                else
                {
                    // Режим патрулирования туда-обратно
                    if (forwardDirection)
                    {
                        enemyBase.currentPoint++;
                        
                        if (enemyBase.currentPoint >= enemyBase.anchors.Length)
                        {
                            enemyBase.currentPoint = enemyBase.anchors.Length - 1;
                            forwardDirection = false;
                        }
                    }
                    else
                    {
                        enemyBase.currentPoint--;
                        
                        if (enemyBase.currentPoint < 0)
                        {
                            enemyBase.currentPoint = 0;
                            forwardDirection = true;
                        }
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