using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//一切的起源,初始化狀態機跟狀態都在這邊
public class Enemy : MonoBehaviour
{
    //這邊是狀態機的基本資訊(因為敵人幾乎都有這些狀態值所以統一寫在這裡)

    public float turnSpeed;
    public float aggresionRange;    //攻擊範圍

    [Header("Idle 資訊")]
    public float idleTime;      //idle狀態持續時間

    [Header("Move 資訊")]
    public float moveSpeed;     //移動速度

    [SerializeField] private Transform[] patrolPoints;  //要巡邏目的地陣列
    private int currentPatrolIndex;                     //目前巡邏目的地索引


    public Transform player { get; private set; }

    public Animator anim { get; private set; }

    public NavMeshAgent agent { get; private set; }     //NavMeshAgent

    public EnemyStateMachine stateMachine { get; private set; }

    //宣告所有狀態
    //-----------(這邊可以針對不同敵人在他們那邊宣告(目前這個類的子類那邊))-----------


    protected virtual void Awake()
    {
        //初始化實例狀態機
        stateMachine = new EnemyStateMachine();

        //取的agent組件
        agent = GetComponent<NavMeshAgent>();

        //取的animator組件
        anim = GetComponentInChildren<Animator>();

        //取的player組件
        player = GameObject.Find("Player").GetComponent<Transform>();


        //初始化實例所有狀態
        //-----------(這邊可以針對不同敵人在他們那邊宣告(目前這個類的子類那邊))-----------

        //透過狀態機切換初始化狀態
        //-----------(這邊可以針對不同敵人在他們那邊宣告(目前這個類的子類那邊))-----------

    }

    protected virtual void Start()
    {
        //一開始先將所有的巡邏點離開父物件
        InitializePatrolPoint();
    }



    protected virtual void Update()
    {
        //透過update所有狀態機的父類的update來持續進行不同狀態的行為
        //-----------(這邊可以針對不同敵人在他們那邊宣告(目前這個類的子類那邊))-----------
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, aggresionRange);
    }

    public void AnimationTrigger() => stateMachine.CurrentState.AnimationTrigger();

    //判斷玩家是否在攻擊範圍
    public bool PlayerInAggresionRange() => Vector3.Distance(transform.position, player.position) < aggresionRange;

    //取得巡邏目的地的位置
    public Vector3 GetPatrolDestination()
    {
        Vector3 destination = patrolPoints[currentPatrolIndex].transform.position;

        currentPatrolIndex++;

        if (currentPatrolIndex >= patrolPoints.Length)
        {
            currentPatrolIndex = 0;
        }

        return destination;
    }

    //一開始先將所有的巡邏點離開父物件
    private void InitializePatrolPoint()
    {
        foreach (Transform t in patrolPoints)
        {
            t.parent = null;
        }
    }

    //面向目標
    public Quaternion FaceTarget(Vector3 target)
    {
        Quaternion targetRotation = Quaternion.LookRotation(target - transform.position);

        Vector3 currentEulerAngles = transform.rotation.eulerAngles;

        float yRotation = Mathf.LerpAngle(currentEulerAngles.y, targetRotation.eulerAngles.y, Time.deltaTime * turnSpeed);

        return Quaternion.Euler(currentEulerAngles.x, yRotation, currentEulerAngles.z);
    }

}
