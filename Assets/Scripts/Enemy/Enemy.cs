using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//一切的起源,初始化狀態機跟狀態都在這邊
public class Enemy : MonoBehaviour
{
    public EnemyStateMachine stateMachine { get; private set; }

    //宣告所有狀態
    public EnemyState idelState { get; private set; }
    public EnemyState moveState { get; private set; }

    void Start()
    {
        //初始化實例狀態機
        stateMachine = new EnemyStateMachine();

        //初始化所有實例狀態
        idelState = new EnemyState(this, stateMachine, "Idel");
        moveState = new EnemyState(this, stateMachine, "Move");

        //透過狀態機切換初始化狀態為idel
        stateMachine.Initialize(idelState);
    }

    void Update()
    {
        //透過update所有狀態機的父類的update來持續進行不同狀態的行為
        stateMachine.CurrentState.Update();

        if (Input.GetKeyDown(KeyCode.V))
        {
            stateMachine.ChangeState(idelState);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            stateMachine.ChangeState(moveState);
        }
    }
}
