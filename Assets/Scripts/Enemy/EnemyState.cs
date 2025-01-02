using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//狀態,這邊是所有狀態的最上層的父類
public class EnemyState
{
    protected Enemy enemyBase;
    protected EnemyStateMachine stateMachine;

    protected string animBoolName;

    //狀態計時器
    protected float stateTimer;

    //在enemy那邊被初始化建構後要取得enemy本身和stateMachine和animBoolName,方便之後做使用
    public EnemyState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName)
    {
        this.enemyBase = enemyBase;
        this.stateMachine = stateMachine;
        this.animBoolName = animBoolName;
    }


    public virtual void Enter()
    {
        //設定所有狀態進去時會設置動畫參數
        enemyBase.anim.SetBool(animBoolName, true);
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
    }

    public virtual void Exit()
    {
        //設定所有狀態離開時會設置動畫參數
        enemyBase.anim.SetBool(animBoolName, false);
    }
}
