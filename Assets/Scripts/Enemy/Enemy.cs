using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//一切的起源,初始化狀態機跟狀態都在這邊
public class Enemy : MonoBehaviour
{
    //這邊是狀態機的基本資訊(因為敵人幾乎都有這些狀態值所以統一寫在這裡)

    [SerializeField] protected int healthPoints = 20;

    [Header("Idle 資訊")]
    public float idleTime;          //idle狀態持續時間
    public float aggresionRange;    //攻擊範圍

    [Header("Move 資訊")]
    public float moveSpeed;      //移動速度
    public float chaseSpeed;     //追擊速度
    public float turnSpeed;      //轉向速度
    private bool manualMovement; //手動移動(我們在攻擊後是否自製移動中)
    private bool manualRotation;

    [SerializeField] private Transform[] patrolPoints;  //要巡邏目的地陣列
    private Vector3[] patrolPointsPosition;             //巡邏目的地位置
    private int currentPatrolIndex;                     //目前巡邏目的地索引

    public bool inBattleMode { get; private set; }

    public Transform player { get; private set; }

    public Animator anim { get; private set; }

    public NavMeshAgent agent { get; private set; }     //NavMeshAgent

    public EnemyStateMachine stateMachine { get; private set; }

    public Enemy_Visuals visuals { get; private set; }


    //宣告所有狀態
    //-----------(這邊可以針對不同敵人在他們那邊宣告(目前這個類的子類那邊))-----------


    protected virtual void Awake()
    {
        //初始化實例狀態機
        stateMachine = new EnemyStateMachine();

        visuals = GetComponent<Enemy_Visuals>();

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
        //判斷是否進入戰鬥模式
        if (ShouldEnterBattleMode())
        {
            EnterBattleMode();
        }

        //透過update所有狀態機的父類的update來持續進行不同狀態的行為
        //-----------(這邊可以針對不同敵人在他們那邊宣告(目前這個類的子類那邊))-----------
    }

    //判斷是否進入戰鬥模式
    protected bool ShouldEnterBattleMode()
    {
        //判斷玩家是否在發現範圍內
        bool inAggresionRange = Vector3.Distance(transform.position, player.position) < aggresionRange;

        if (inAggresionRange && !inBattleMode)
        {
            EnterBattleMode();
            return true;
        }

        return false;

    }

    public virtual void EnterBattleMode()
    {
        inBattleMode = true;
    }

    public virtual void GetHit()
    {
        EnterBattleMode();
        healthPoints--;
    }

    //受到攻擊時的衝擊
    public virtual void DeathImpact(Vector3 force, Vector3 hitPoint, Rigidbody rb)
    {
        StartCoroutine(DeathImpactCourutine(force, hitPoint, rb));
    }

    //必須讓受到攻擊的物件回傳一個協程(為了讓他延遲一下,不然老師說跟運動學同時會出錯)
    private IEnumerator DeathImpactCourutine(Vector3 force, Vector3 hitPoint, Rigidbody rb)
    {
        yield return new WaitForSeconds(.1f);

        rb.AddForceAtPosition(force, hitPoint, ForceMode.Impulse);
    }

    //面向目標
    public void FaceTarget(Vector3 target)
    {
        Quaternion targetRotation = Quaternion.LookRotation(target - transform.position);

        Vector3 currentEulerAngles = transform.rotation.eulerAngles;

        float yRotation = Mathf.LerpAngle(currentEulerAngles.y, targetRotation.eulerAngles.y, Time.deltaTime * turnSpeed);

        transform.rotation = Quaternion.Euler(currentEulerAngles.x, yRotation, currentEulerAngles.z);
    }


    #region 動畫事件

    //開啟\關閉手動移動
    public void ActivateManualMovement(bool manualMovement) => this.manualMovement = manualMovement;

    //取得是否為手動移動狀態
    public bool ManualMovementActive() => manualMovement;

    public void ActivateManualRotation(bool manualRotation) => this.manualRotation = manualRotation;

    public bool ManualRotationActive() => manualRotation;


    public void AnimationTrigger() => stateMachine.CurrentState.AnimationTrigger();

    public virtual void AbilityTrigger()
    {
        stateMachine.CurrentState.AbilityTrigger();
    }

    #endregion

    #region 巡邏點邏輯
    //取得巡邏目的地的位置
    public Vector3 GetPatrolDestination()
    {
        Vector3 destination = patrolPointsPosition[currentPatrolIndex];

        currentPatrolIndex++;

        if (currentPatrolIndex >= patrolPoints.Length)
        {
            currentPatrolIndex = 0;
        }

        return destination;
    }

    //初始化巡邏點
    private void InitializePatrolPoint()
    {
        patrolPointsPosition = new Vector3[patrolPoints.Length];

        for (int i = 0; i < patrolPoints.Length; i++)
        {
            patrolPointsPosition[i] = patrolPoints[i].position;
            patrolPoints[i].gameObject.SetActive(false);
        }
    }

    #endregion


    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, aggresionRange);
    }
}
