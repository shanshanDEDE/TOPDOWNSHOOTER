using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//狀態,這邊是所有狀態的最上層的父類
public class EnemyState
{
    protected Enemy enemyBase;
    protected EnemyStateMachine stateMachine;

    protected string animBoolName;

    protected bool triggerCalled;

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

        triggerCalled = false;
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

    public void AnimationTrigger() => triggerCalled = true;

    //返回下一個目標(NavMeshAgent的pathcorners)
    protected Vector3 GetNextPathPoint()
    {
        NavMeshAgent agent = enemyBase.agent;
        NavMeshPath path = agent.path;

        // 如果只剩下一個轉角則直接面向目標
        if (path.corners.Length < 2)
        {
            return agent.destination;
        }

        for (int i = 0; i < path.corners.Length; i++)
        {
            // 如果到達目標則返回下一個目標
            if (Vector3.Distance(agent.transform.position, path.corners[i]) < 1)
            {
                return path.corners[i + 1];
            }
        }

        return agent.destination;
    }
}
