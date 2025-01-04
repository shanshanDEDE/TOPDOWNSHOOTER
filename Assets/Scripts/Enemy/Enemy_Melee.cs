using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//透過struck來創建一個AttackData來存放不同的攻擊的資訊
[System.Serializable]
public struct AttackData
{
    public string attackName;
    public float attackRange;   //攻擊範圍
    public float moveSpeed;     //攻擊移動速度
    public float attackIndex;
    [Range(1, 2)]
    public float animationSpeed;
    public AttackType_Melee attackType;
}

public enum AttackType_Melee { Close, Charge }
public enum EnemyMelee_Type { Regular, Shield, Dodge }

public class Enemy_Melee : Enemy
{

    //宣告所有狀態
    public IdleState_Melee idleState { get; private set; }
    public MoveState_Melee moveState { get; private set; }
    public RecoveryState_Melee recoveryState { get; private set; }
    public ChaseState_Melee chaseState { get; private set; }
    public AttackState_Melee attackState { get; private set; }
    public DeadState_Melee deadState { get; private set; }
    public AbilityState_Melee abilityState { get; private set; }

    [Header("Enemy 設定")]
    public EnemyMelee_Type meleeType;
    public Transform shieldTransform;
    public float dodgeCooldown;
    private float lastTimeDodge;

    [Header("Attack Data")]
    public AttackData attackData;
    public List<AttackData> attackList;

    [SerializeField] private Transform hiddenWeapon;
    [SerializeField] private Transform pulledWeapon;

    protected override void Awake()
    {
        base.Awake();

        //初始化實例所有狀態
        idleState = new IdleState_Melee(this, stateMachine, "Idle");
        moveState = new MoveState_Melee(this, stateMachine, "Move");
        recoveryState = new RecoveryState_Melee(this, stateMachine, "Recovery");
        chaseState = new ChaseState_Melee(this, stateMachine, "Chase");
        attackState = new AttackState_Melee(this, stateMachine, "Attack");
        deadState = new DeadState_Melee(this, stateMachine, "Idle");    //idle只是個站位符號,死亡我們用ragdoll
        abilityState = new AbilityState_Melee(this, stateMachine, "AxeThrow");

    }


    protected override void Start()
    {
        base.Start();

        //透過狀態機切換初始化狀態
        stateMachine.Initialize(idleState);

        InitializeSpeciality();
    }

    protected override void Update()
    {
        base.Update();

        //透過update所有狀態機的父類的update來持續進行不同狀態的行為
        stateMachine.CurrentState.Update();
    }

    public void TriggerAbility()
    {
        moveSpeed = moveSpeed * .6f;
        Debug.Log("create axe");

        pulledWeapon.gameObject.SetActive(false);
    }

    private void InitializeSpeciality()
    {
        if (meleeType == EnemyMelee_Type.Shield)
        {
            anim.SetFloat("ChaseIndex", 1);
            shieldTransform.gameObject.SetActive(true);
        }
    }

    public override void GetHit()
    {
        base.GetHit();

        if (healthPoints <= 0)
            stateMachine.ChangeState(deadState);
    }

    //拿出武器
    public void PullWeapon()
    {
        hiddenWeapon.gameObject.SetActive(false);
        pulledWeapon.gameObject.SetActive(true);
    }

    //判斷玩家是否在攻擊範圍
    public bool PlayerInAttackRange() => Vector3.Distance(transform.position, player.position) < attackData.attackRange;

    //啟用閃躲
    public void ActivatedDodgeRoll()
    {
        if (meleeType != EnemyMelee_Type.Dodge)
        {
            return;
        }

        if (stateMachine.CurrentState != chaseState)
        {
            return;
        }

        if (Vector3.Distance(transform.position, player.position) < 2f)
        {
            return;
        }

        if (Time.time > lastTimeDodge + dodgeCooldown)
        {
            lastTimeDodge = Time.time;
            anim.SetTrigger("Dodge");
        }
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(transform.position, attackData.attackRange);
    }
}
