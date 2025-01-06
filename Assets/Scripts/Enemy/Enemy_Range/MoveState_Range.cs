using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState_Range : EnemyState
{
    private Enemy_Range enemy;

    private Vector3 destination;

    public MoveState_Range(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as Enemy_Range;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.agent.speed = enemy.moveSpeed;

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

        //面向目標
        enemy.FaceTarget(GetNextPathPoint());

        if (enemy.agent.remainingDistance <= enemy.agent.stoppingDistance + 0.05f)
        {
            stateMachine.ChangeState(enemy.idleState);
        }
    }
}
