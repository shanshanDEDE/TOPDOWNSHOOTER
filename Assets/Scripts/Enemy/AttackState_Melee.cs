using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState_Melee : EnemyState
{
    private Enemy_Melee enemy;
    private Vector3 attackDirection;
    private float attackMoveSpeed;

    private const float MAX_ATTACK_DISTACE = 50f;

    public AttackState_Melee(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as Enemy_Melee;
    }

    public override void Enter()
    {
        base.Enter();

        attackMoveSpeed = enemy.attackData.moveSpeed;
        enemy.anim.SetFloat("AttackAnimationSpeed", enemy.attackData.animationSpeed);
        enemy.anim.SetFloat("AttackIndex", enemy.attackData.attackIndex);

        enemy.PullWeapon(); //拿出武器

        //先讓敵人停止移動
        enemy.agent.isStopped = true;
        enemy.agent.velocity = Vector3.zero;



    }

    public override void Exit()
    {
        base.Exit();

        //預設為0的那個動畫(動畫是使用blentree)
        enemy.anim.SetFloat("RecoveryIndex", 0);

        //如果在攻擊範圍內則使用1的那個
        if (enemy.PlayerInAttackRange())
        {
            enemy.anim.SetFloat("RecoveryIndex", 1);
        }
    }

    public override void Update()
    {
        base.Update();

        //取得是否為手動旋轉狀態
        if (enemy.ManualRotationActive())
        {
            enemy.transform.rotation = enemy.FaceTarget(enemy.player.position);
            attackDirection = enemy.transform.position + (enemy.transform.forward * MAX_ATTACK_DISTACE);
        }

        //取得是否為手動移動狀態
        if (enemy.ManualMovementActive())
        {
            //自己設定攻擊往前移動
            enemy.transform.position =
                Vector3.MoveTowards(enemy.transform.position, attackDirection, attackMoveSpeed * Time.deltaTime);
        }


        if (triggerCalled)
        {

            if (enemy.PlayerInAttackRange())
            {
                stateMachine.ChangeState(enemy.recoveryState);
            }
            else
            {
                stateMachine.ChangeState(enemy.chaseState);
            }
        }
    }
}
