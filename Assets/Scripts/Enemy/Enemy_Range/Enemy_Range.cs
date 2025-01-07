using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Range : Enemy
{
    [Header("掩護系統")]
    public bool canUseCovers = true;
    public CoverPoint lastCover;
    public List<Cover> allCovers = new List<Cover>();

    [Header("武器細節")]
    public Enemy_RangeWeaponType weaponType;
    public Enemy_RangeWeaponData weaponData;

    [Space]
    public Transform gunPoint;
    public Transform weaponholder;
    public GameObject bulletPrefab;

    [SerializeField] List<Enemy_RangeWeaponData> avalibleWeaponData;

    public IdleState_Range idleState { get; private set; }
    public MoveState_Range moveState { get; private set; }
    public BattleState_Range battleState { get; private set; }
    public RunToCoverState_Range runToCoverState { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        idleState = new IdleState_Range(this, stateMachine, "Idle");
        moveState = new MoveState_Range(this, stateMachine, "Move");
        battleState = new BattleState_Range(this, stateMachine, "Battle");
        runToCoverState = new RunToCoverState_Range(this, stateMachine, "Run");

    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idleState);
        visuals.SetupLook();

        //取得要用哪個weaponData
        SetupWeapon();

        //收集附近的Covers加到allCovers
        allCovers.AddRange(CollectNearByCovers());
    }


    protected override void Update()
    {
        base.Update();

        stateMachine.CurrentState.Update();
    }

    #region  Cover System

    public Transform AttemptToFindCover()
    {
        List<CoverPoint> collectedCoverPoints = new List<CoverPoint>();

        foreach (Cover cover in allCovers)
        {
            collectedCoverPoints.AddRange(cover.GetCoverPoints());
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
            lastCover = closestCoverPoint;
        }

        return lastCover.transform;
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

        Vector3 bulletsDirection = ((player.position + Vector3.up) - gunPoint.position).normalized;

        GameObject newBullet = ObjectPool.instance.GetObject(bulletPrefab, gunPoint.position);
        newBullet.transform.position = gunPoint.position;
        newBullet.transform.rotation = Quaternion.LookRotation(gunPoint.forward);

        newBullet.GetComponent<Enemy_Bullet>().BulletSetup();

        Rigidbody rbNewBullet = newBullet.GetComponent<Rigidbody>();

        Vector3 bulletDirectionWithSpread = weaponData.ApplyWeaponSpread(bulletsDirection);

        rbNewBullet.mass = 20 / weaponData.bulletSpeed;
        rbNewBullet.velocity = bulletDirectionWithSpread * weaponData.bulletSpeed;
    }

    public override void EnterBattleMode()
    {
        if (inBattleMode) return;

        base.EnterBattleMode();

        if (canUseCovers)
            stateMachine.ChangeState(runToCoverState);
        else
            stateMachine.ChangeState(battleState);

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
}
