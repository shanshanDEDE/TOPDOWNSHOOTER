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
        enemy.PullWeapon(); //拿出武器

        attackMoveSpeed = enemy.attackData.moveSpeed;
        enemy.anim.SetFloat("AttackAnimationSpeed", enemy.attackData.animationSpeed);
        enemy.anim.SetFloat("AttackIndex", enemy.attackData.attackIndex);
        enemy.anim.SetFloat("SlashAttackIndex", Random.Range(0, 6));


        //先讓敵人停止移動
        enemy.agent.isStopped = true;
        enemy.agent.velocity = Vector3.zero;

        attackDirection = enemy.transform.position + (enemy.transform.forward * MAX_ATTACK_DISTACE);
    }

    public override void Exit()
    {
        base.Exit();

        SetupNextAttack();
    }

    private void SetupNextAttack()
    {
        //判斷玩家如果夠近則啟用1動畫否則啟用2動畫
        int recoveryIndex = PlayerClose() ? 1 : 0;
        enemy.anim.SetFloat("RecoveryIndex", recoveryIndex);


        //更新用哪個攻擊資訊
        enemy.attackData = UpdatedAttackData();
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

    //判斷玩家是否夠近
    private bool PlayerClose() => Vector3.Distance(enemy.transform.position, enemy.player.position) <= 1;

    //更新用哪個攻擊資訊
    private AttackData UpdatedAttackData()
    {
        List<AttackData> validAttacks = new List<AttackData>(enemy.attackList);

        //如果玩家夠近
        if (PlayerClose())
        {
            //移除Charge的那個AttackDat
            validAttacks.RemoveAll(parameter => parameter.attackType == AttackType_Melee.Charge);
        }

        int random = Random.Range(0, validAttacks.Count);

        return validAttacks[random];
    }
}
