using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancePlayerState_Range : EnemyState
{
    private Enemy_Range enemy;
    private Vector3 playerPos;

    public AdvancePlayerState_Range(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as Enemy_Range;
    }

    public override void Enter()
    {
        base.Enter();

        //啟用左手IK但瞄準敵人的ik開啟
        enemy.visuals.EnableIK(true, true);

        enemy.agent.isStopped = false;          //啟用移動
        enemy.agent.speed = enemy.advanceSpeed; //設定速度
    }


    public override void Update()
    {
        base.Update();

        playerPos = enemy.player.transform.position;

        //往玩家移動
        enemy.agent.SetDestination(playerPos);
        //面相目標
        enemy.FaceTarget(GetNextPathPoint());

        if (Vector3.Distance(enemy.transform.position, playerPos) < enemy.advanceStoppingDistance) //如果距離小於追擊停止距離
        {
            stateMachine.ChangeState(enemy.battleState);
        }
    }
}
