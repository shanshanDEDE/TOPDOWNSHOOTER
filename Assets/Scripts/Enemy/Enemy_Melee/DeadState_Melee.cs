using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState_Melee : EnemyState
{
    private Enemy_Melee enemy;
    private Enemy_Ragdoll ragdoll;

    private bool intereactionDisabled;

    public DeadState_Melee(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as Enemy_Melee;
        ragdoll = enemy.GetComponent<Enemy_Ragdoll>();
    }

    public override void Enter()
    {
        base.Enter();

        intereactionDisabled = false;

        enemy.anim.enabled = false;
        enemy.agent.isStopped = true;

        //近來在啟用運動學 才不會導致死亡前的東西影響到死亡後的運動
        ragdoll.RagdollActive(true);  //不啟用isKinematic(啟用運動學的意思)

        stateTimer = 1.5f;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        //DisableInteractionIfShould();
    }

    private void DisableInteractionIfShould()
    {
        //這邊不做使用
        //讓屍體取消這些可以防止效能的損失(但不能鞭屍跟碰撞了)
        if (stateTimer < 0 && intereactionDisabled == false)
        {
            intereactionDisabled = true;
            ragdoll.RagdollActive(false);
            ragdoll.ColliderActive(false);
        }
    }
}
