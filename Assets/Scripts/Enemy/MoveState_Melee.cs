using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveState_Melee : EnemyState
{
    private Enemy_Melee enemy;

    private Vector3 destination;

    public MoveState_Melee(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as Enemy_Melee;
    }

    public override void Enter()
    {
        base.Enter();

        destination = enemy.GetPatrolDestination();

        //設置目的地(使用NavMeshAgent)
        enemy.agent.SetDestination(destination);
    }

    public override void Exit()
    {
        base.Exit();

    }

    public override void Update()
    {
        base.Update();

        if (enemy.PlayerInAggresionRange())
        {
            stateMachine.ChangeState(enemy.recoveryState);
            return;
        }

        //面向目標
        enemy.transform.rotation = enemy.FaceTarget(GetNextPathPoint());

        if (enemy.agent.remainingDistance <= enemy.agent.stoppingDistance + 0.05f)
        {
            stateMachine.ChangeState(enemy.idleState);
        }
    }

    //返回下一個目標(NavMeshAgent的pathcorners)
    private Vector3 GetNextPathPoint()
    {
        NavMeshAgent agent = enemy.agent;
        NavMeshPath path = agent.path;

        // 如果只剩下一個轉角則直接面向目標
        if (path.corners.Length < 2)
        {
            return agent.destination;
        }

        for (int i = 0; i < path.corners.Length; i++)
        {
            // 如果到達目標則返回下一個目標
            if (Vector3.Distance(agent.transform.position, path.corners[i]) < 1)
            {
                return path.corners[i + 1];
            }
        }

        return agent.destination;
    }
}
