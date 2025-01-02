using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState_Melee : EnemyState
{
    private Enemy_Melee enemy;

    public MoveState_Melee(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as Enemy_Melee;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        Debug.Log("Move");
    }
}
