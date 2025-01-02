using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//敵人狀態機,像是一台機器從放方法目的是用來改變狀態
public class EnemyStateMachine
{
    public EnemyState CurrentState { get; set; }

    //初始狀態
    public void Initialize(EnemyState startState)
    {
        //切換敵人初始狀態
        CurrentState = startState;
        //進入該狀態時該處發的方法
        CurrentState.Enter();
    }

    //切換狀態
    public void ChangeState(EnemyState newState)
    {
        CurrentState.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }
}
