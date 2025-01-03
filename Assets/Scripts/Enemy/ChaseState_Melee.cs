using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState_Melee : EnemyState
{
    private Enemy_Melee enemy;
    private float lastTimeUpdatedDistanation;

    public ChaseState_Melee(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as Enemy_Melee;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.agent.speed = enemy.chaseSpeed;
        enemy.agent.isStopped = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        //面向目標
        enemy.transform.rotation = enemy.FaceTarget(GetNextPathPoint());


        if (CanUpdateDestination())
        {
            enemy.agent.destination = enemy.player.transform.position;
        }
    }

    //判斷是否需要更新目的地(因為玩家會一直移動)
    private bool CanUpdateDestination()
    {
        if (Time.time > lastTimeUpdatedDistanation + 0.25f)
        {
            lastTimeUpdatedDistanation = Time.time;
            return true;
        }
        return false;
    }
}
