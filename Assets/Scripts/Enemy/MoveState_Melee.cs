using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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



        if (enemy.agent.remainingDistance <= 1)
        {
            stateMachine.ChangeState(enemy.idleState);
        }
    }
}
