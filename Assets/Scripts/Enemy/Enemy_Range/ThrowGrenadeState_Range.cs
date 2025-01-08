using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowGrenadeState_Range : EnemyState
{
    private Enemy_Range enemy;

    public ThrowGrenadeState_Range(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as Enemy_Range;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.visuals.EnableWeaponModel(false);
        enemy.visuals.EnableIK(false, false);
        enemy.visuals.EnableSceonderyWeaponModel(true);     //啟用右手武器
    }

    public override void Exit()
    {
        base.Exit();

        // 開始延遲啟用和關閉武器模型
        //enemy.visuals.EnableWeaponModel(true);                //啟用左手武器
        //enemy.visuals.EnableSceonderyWeaponModel(false);      //關閉右手武器
        //enemy.StartCoroutine(DelayEnableWeapons());
    }

    /*  IEnumerator DelayEnableWeapons()
     {
         // 等待 0.5 秒
         yield return new WaitForSeconds(0.5f);

         // 啟用左手武器
         enemy.visuals.EnableWeaponModel(true);

         // 關閉右手武器
         enemy.visuals.EnableSceonderyWeaponModel(false);
     } */

    public override void Update()
    {
        base.Update();

        Vector3 playerPos = enemy.player.position + Vector3.up;

        enemy.FaceTarget(enemy.player.position);
        enemy.aim.position = playerPos;

        //透過動畫事件切回battle狀態
        if (triggerCalled)
        {

            stateMachine.ChangeState(enemy.battleState);
        }
    }

    public override void AbilityTrigger()
    {
        base.AbilityTrigger();

        enemy.ThrowGrenade();   // 丟手榴彈
    }

}
