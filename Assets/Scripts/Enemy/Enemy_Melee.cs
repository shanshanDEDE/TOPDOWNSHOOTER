using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Melee : Enemy
{
    //宣告所有狀態
    public IdleState_Melee idleState { get; private set; }
    public MoveState_Melee moveState { get; private set; }
    public RecoveryState_Melee recoveryState { get; private set; }
    public ChaseState_Melee chaseState { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        //初始化實例所有狀態
        idleState = new IdleState_Melee(this, stateMachine, "Idle");
        moveState = new MoveState_Melee(this, stateMachine, "Move");
        recoveryState = new RecoveryState_Melee(this, stateMachine, "Recovery");
        chaseState = new ChaseState_Melee(this, stateMachine, "Chase");
    }


    protected override void Start()
    {
        base.Start();

        //透過狀態機切換初始化狀態
        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();

        //透過update所有狀態機的父類的update來持續進行不同狀態的行為
        stateMachine.CurrentState.Update();
    }
}
