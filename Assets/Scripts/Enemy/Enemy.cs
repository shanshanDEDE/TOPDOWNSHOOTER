using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//一切的起源,初始化狀態機跟狀態都在這邊
public class Enemy : MonoBehaviour
{
    [Header("Idle 資訊")]
    public float idleTime;      //idle狀態持續時間

    public EnemyStateMachine stateMachine { get; private set; }

    //宣告所有狀態
    //-----------(這邊可以針對不同敵人在他們那邊宣告(目前這個類的子類那邊))-----------


    protected virtual void Awake()
    {
        //初始化實例狀態機
        stateMachine = new EnemyStateMachine();

        //初始化實例所有狀態
        //-----------(這邊可以針對不同敵人在他們那邊宣告(目前這個類的子類那邊))-----------

        //透過狀態機切換初始化狀態
        //-----------(這邊可以針對不同敵人在他們那邊宣告(目前這個類的子類那邊))-----------

    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {
        //透過update所有狀態機的父類的update來持續進行不同狀態的行為
        //-----------(這邊可以針對不同敵人在他們那邊宣告(目前這個類的子類那邊))-----------
    }
}
