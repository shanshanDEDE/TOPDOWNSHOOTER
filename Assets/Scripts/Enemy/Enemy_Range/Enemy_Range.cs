using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public enum CoverPerk { Unavalible, CanTakeCover, CanTakeAndChangeCover }

public enum UnstoppablePerk { Unavalible, Unstoppable }

public enum GrenadePerk { Unavalible, CanThrowGrenade }

public class Enemy_Range : Enemy
{
    [Header("敵人 perk")]
    public CoverPerk coverPerk;
    public UnstoppablePerk unstoppablePerk;
    public GrenadePerk grenadePerk;

    [Header("Grenade(手榴彈) perk")]
    public GameObject grenadePrefab;
    public float impactPower;
    public float explosionTimer = 0.75f;
    public float timeToTarget = 1.2f;
    public float grenadeCooldown;
    private float lastTimeGrenadeThrown = -10;
    [SerializeField] private Transform grenadeStartPoint;

    [Header("前進 perk")]
    public float advanceSpeed;
    public float advanceStoppingDistance;
    public float advanceDuration = 2.5f;

    [Header("掩護系統")]
    public float minSCoverTime;
    public float safeDistance;
    public CoverPoint currentCover { get; private set; }
    public CoverPoint lastCover { get; private set; }

    [Header("武器細節")]
    public float attackDelay;
    public Enemy_RangeWeaponType weaponType;
    public Enemy_RangeWeaponData weaponData;

    [Space]
    public Transform gunPoint;
    public Transform weaponholder;
    public GameObject bulletPrefab;

    [Header("Aim details")]
    public float slowAim = 4;
    public float fastAim = 20;
    public Transform aim;
    public Transform playersBody;
    public LayerMask whatToIgnore;


    [SerializeField] List<Enemy_RangeWeaponData> avalibleWeaponData;

    #region 狀態
    public IdleState_Range idleState { get; private set; }
    public MoveState_Range moveState { get; private set; }
    public BattleState_Range battleState { get; private set; }
    public RunToCoverState_Range runToCoverState { get; private set; }
    public AdvancePlayerState_Range advancePlayerState { get; private set; }
    public ThrowGrenadeState_Range throwGrenadeState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();

        idleState = new IdleState_Range(this, stateMachine, "Idle");
        moveState = new MoveState_Range(this, stateMachine, "Move");
        battleState = new BattleState_Range(this, stateMachine, "Battle");
        runToCoverState = new RunToCoverState_Range(this, stateMachine, "Run");
        advancePlayerState = new AdvancePlayerState_Range(this, stateMachine, "Advance");
        throwGrenadeState = new ThrowGrenadeState_Range(this, stateMachine, "ThrowGrenade");
    }

    protected override void Start()
    {
        base.Start();

        playersBody = player.GetComponent<Player>().playerBody;
        aim.parent = null;

        InitializePerk();

        stateMachine.Initialize(idleState);
        visuals.SetupLook();

        //取得要用哪個weaponData
        SetupWeapon();
    }


    protected override void Update()
    {
        base.Update();

        stateMachine.CurrentState.Update();
    }

    //初始化 perk
    protected override void InitializePerk()
    {
        //如果是Unstoppable
        if (IsUnstoppable())
        {
            advanceSpeed = 1;
            anim.SetFloat("AdvanceAnimIndex", 1); //1是慢速行走的動畫
        }
    }

    //是否可以丟手榴彈
    public bool CanThrowGrenade()
    {
        if (grenadePerk == GrenadePerk.Unavalible)
        {
            return false;
        }

        if (Vector3.Distance(player.transform.position, transform.position) < safeDistance)
        {
            return false;
        }

        if (Time.time > grenadeCooldown + lastTimeGrenadeThrown)
        {
            return true;
        }

        return false;
    }

    //丟手榴彈
    public void ThrowGrenade()
    {
        lastTimeGrenadeThrown = Time.time;

        GameObject newGrenade = ObjectPool.instance.GetObject(grenadePrefab, grenadeStartPoint.position);
        // newGrenade.transform.position = grenadeStartPoint.position;

        Enemy_Grenade newGrenadeScript = newGrenade.GetComponent<Enemy_Grenade>();
        newGrenadeScript.SetupGrenade(player.transform.position, timeToTarget, explosionTimer, impactPower);
    }

    public override void EnterBattleMode()
    {
        if (inBattleMode) return;

        base.EnterBattleMode();

        if (CanGetCover())
            stateMachine.ChangeState(runToCoverState);
        else
            stateMachine.ChangeState(battleState);

    }

    #region  Cover System

    public bool CanGetCover()
    {
        if (coverPerk == CoverPerk.Unavalible)
        {
            return false;
        }

        currentCover = AttemptToFindCover()?.GetComponent<CoverPoint>();

        if (lastCover != currentCover && currentCover != null)
        {
            return true;
        }

        Debug.Log("找不到Cover");
        return false;

    }

    private Transform AttemptToFindCover()
    {
        List<CoverPoint> collectedCoverPoints = new List<CoverPoint>();

        foreach (Cover cover in CollectNearByCovers())
        {
            collectedCoverPoints.AddRange(cover.GetValidCoverPoints(transform));
        }

        CoverPoint closestCoverPoint = null;
        float shortestDistance = float.MaxValue;

        //找到距離最近的Cover
        foreach (CoverPoint coverPoint in collectedCoverPoints)
        {
            float currentDistance = Vector3.Distance(transform.position, coverPoint.transform.position);

            if (currentDistance < shortestDistance)
            {
                closestCoverPoint = coverPoint;
                shortestDistance = currentDistance;
            }
        }

        if (closestCoverPoint != null)
        {
            //將上一個Cover的occupied設為false
            lastCover?.SetOccupied(false);
            lastCover = currentCover;

            currentCover = closestCoverPoint;
            //將這個Cover的occupied設為true
            currentCover.SetOccupied(true);

            return currentCover.transform;
        }

        return null;
    }

    //收集附近的Covers
    private List<Cover> CollectNearByCovers()
    {
        float coverRadiusCheck = 30;
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, coverRadiusCheck);
        List<Cover> collecterCovers = new List<Cover>();

        foreach (Collider collider in hitColliders)
        {
            Cover cover = collider.GetComponent<Cover>();

            //因為有些物件我們設置collider時有兩個以上的collider所以這邊要多一個檢查是否已經包含這個cover物件了
            if (cover != null && collecterCovers.Contains(cover) == false)
            {
                collecterCovers.Add(cover);
            }
        }

        return collecterCovers;
    }

    #endregion

    public void FirtSingleBattle()
    {
        anim.SetTrigger("Shoot");

        Vector3 bulletsDirection = (aim.position - gunPoint.position).normalized;

        GameObject newBullet = ObjectPool.instance.GetObject(bulletPrefab, gunPoint.position);
        newBullet.transform.position = gunPoint.position;
        newBullet.transform.rotation = Quaternion.LookRotation(gunPoint.forward);

        newBullet.GetComponent<Enemy_Bullet>().BulletSetup();

        Rigidbody rbNewBullet = newBullet.GetComponent<Rigidbody>();

        Vector3 bulletDirectionWithSpread = weaponData.ApplyWeaponSpread(bulletsDirection);

        rbNewBullet.mass = 20 / weaponData.bulletSpeed;
        rbNewBullet.velocity = bulletDirectionWithSpread * weaponData.bulletSpeed;
    }

    //取得要用哪個weaponData
    private void SetupWeapon()
    {
        List<Enemy_RangeWeaponData> filteredDate = new List<Enemy_RangeWeaponData>();

        //從avalibleWeaponData中去找到指定武器的Data來用
        foreach (var weaponData in avalibleWeaponData)
        {
            if (weaponData.weaponType == weaponType)
            {
                filteredDate.Add(weaponData);
            }
        }

        if (filteredDate.Count > 0)
        {
            //隨機取一個
            int radom = Random.Range(0, filteredDate.Count);
            weaponData = filteredDate[radom];
        }
        else
        {
            Debug.LogWarning("找不到武器資料");
        }

        gunPoint = visuals.currentWeaponModel.GetComponent<Enemy_RangeWeaponModel>().gunPoint;
    }

    #region 敵人 Aim region

    //依據是否准心在依定範圍內決定更新瞄準速度
    public void UpdateAimPosition()
    {
        float aimSpeed = IsAimOnPlayer() ? fastAim : slowAim;
        aim.position = Vector3.MoveTowards(aim.position, playersBody.position, aimSpeed * Time.deltaTime);
    }

    //準心是否在敵人身上
    public bool IsAimOnPlayer()
    {
        float distanceAimToPlayer = Vector3.Distance(aim.position, player.position);

        return distanceAimToPlayer < 2;
    }

    //是否看到玩家
    public bool IsSeeingPlayer()
    {
        //自己本身的位置
        Vector3 myPosition = transform.position + Vector3.up;

        Vector3 directionToPlayer = playersBody.position - myPosition;

        //~whatToIgnore加上~表示有那些層是不要的意思
        if (Physics.Raycast(myPosition, directionToPlayer, out RaycastHit hit, Mathf.Infinity, ~whatToIgnore))
        {
            if (hit.transform == player)
            {
                UpdateAimPosition();
                return true;
            }
        }
        return false;
    }

    #endregion

    //取得是否為Unstoppable
    public bool IsUnstoppable() => unstoppablePerk == UnstoppablePerk.Unstoppable;

}
