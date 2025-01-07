using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunToCoverState_Range : EnemyState
{
    private Enemy_Range enemy;
    private Vector3 destination;

    public RunToCoverState_Range(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as Enemy_Range;
    }

    public override void Enter()
    {
        base.Enter();
        destination = enemy.currentCover.transform.position;

        enemy.visuals.EnableIK(true, false);
        enemy.agent.isStopped = false;
        enemy.agent.speed = enemy.runSpeed;      //設定速度
        enemy.agent.SetDestination(destination);


    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        enemy.FaceTarget(GetNextPathPoint());

        //老師原本設<0.5f 我改成1f
        if (Vector3.Distance(enemy.transform.position, destination) < 1f)
        {
            Debug.Log("進入戰鬥狀態");
            stateMachine.ChangeState(enemy.battleState);

        }
    }


}
