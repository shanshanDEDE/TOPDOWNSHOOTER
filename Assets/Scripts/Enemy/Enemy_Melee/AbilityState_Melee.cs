using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityState_Melee : EnemyState
{
    private Enemy_Melee enemy;
    private Vector3 movementDirection;

    private const float MAX_MOVEMENT_DISTANCE = 20f;

    private float moveSpeed;

    public AbilityState_Melee(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as Enemy_Melee;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.EnableWeaponModel(true);

        moveSpeed = enemy.moveSpeed;
        movementDirection = enemy.transform.position + (enemy.transform.forward * MAX_MOVEMENT_DISTANCE);
    }

    public override void Exit()
    {
        base.Exit();

        enemy.moveSpeed = moveSpeed;
        enemy.anim.SetFloat("RecoveryIndex", 1);
    }

    public override void Update()
    {
        base.Update();

        //取得是否為手動旋轉狀態
        if (enemy.ManualRotationActive())
        {
            enemy.FaceTarget(enemy.player.position);
            movementDirection = enemy.transform.position + (enemy.transform.forward * MAX_MOVEMENT_DISTANCE);
        }


        //取得是否為手動移動狀態
        if (enemy.ManualMovementActive())
        {
            //自己設定攻擊往前移動
            enemy.transform.position =
                Vector3.MoveTowards(enemy.transform.position, movementDirection, enemy.moveSpeed * Time.deltaTime);
        }


        if (triggerCalled)
        {
            stateMachine.ChangeState(enemy.recoveryState);
        }
    }

    public override void AbilityTrigger()
    {
        base.AbilityTrigger();

        GameObject newAxe = ObjectPool.instance.GetObject(enemy.axePrefab, enemy.axeStartPoint.position);

        newAxe.transform.position = enemy.axeStartPoint.position;
        newAxe.GetComponent<Enemy_Axe>().AxeSetup(enemy.axeFlySpeed, enemy.player, enemy.axeAimTimer);
    }
}
