using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState_Melee : EnemyState
{
    private Enemy_Melee enemy;

    private Vector3 attackDirection;

    private const float MAX_ATTACK_DISTACE = 50f;

    public AttackState_Melee(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as Enemy_Melee;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.PullWeapon(); //拿出武器

        //先讓敵人停止移動
        enemy.agent.isStopped = true;
        enemy.agent.velocity = Vector3.zero;


        attackDirection = enemy.transform.position + (enemy.transform.forward * MAX_ATTACK_DISTACE);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        //取得是否為手動移動狀態
        if (enemy.ManualMovementActive())
        {
            //自己設定攻擊往前移動
            enemy.transform.position =
                Vector3.MoveTowards(enemy.transform.position, attackDirection, enemy.attackMoveSpeed * Time.deltaTime);
        }


        if (triggerCalled)
        {
            stateMachine.ChangeState(enemy.chaseState);
        }
    }
}
