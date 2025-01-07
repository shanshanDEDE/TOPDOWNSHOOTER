using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancePlayerState_Range : EnemyState
{
    private Enemy_Range enemy;
    private Vector3 playerPos;

    public float lastTimeAdvanced { get; private set; }

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

        if (enemy.IsUnstoppable())               //如果為Unstoppable
        {
            //啟用左手IK但不啟用槍瞄準IK
            enemy.visuals.EnableIK(true, false);
            stateTimer = enemy.advanceDuration;
        }
    }

    public override void Exit()
    {
        base.Exit();
        lastTimeAdvanced = Time.time;
    }


    public override void Update()
    {
        base.Update();

        playerPos = enemy.player.transform.position;
        enemy.UpdateAimPosition();//更新準新位置

        //往玩家移動
        enemy.agent.SetDestination(playerPos);
        //面相目標
        enemy.FaceTarget(GetNextPathPoint());

        if (CanEnterBattleMode() && enemy.IsSeeingPlayer())     //判斷是否可以進入戰鬥模式並且有看到玩家
        {
            stateMachine.ChangeState(enemy.battleState);
        }
    }

    //判斷是否可以進入戰鬥模式
    private bool CanEnterBattleMode()
    {
        //距離是否小於追擊停止距離
        bool closeEnoughToPlayer = Vector3.Distance(enemy.transform.position, playerPos) < enemy.advanceStoppingDistance;

        //如果為Unstoppable
        if (enemy.IsUnstoppable())
        {
            return closeEnoughToPlayer || stateTimer < 0;      //如果走太久或者離玩家太近
        }
        else
        {
            return closeEnoughToPlayer;
        }
    }
}
