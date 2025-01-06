using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Range : Enemy
{
    public Transform weaponholder;

    public float fireRate = 1;  //子彈發射間隔
    public GameObject bulletPrefab;
    public Transform gunPoint;
    public float bulletSpeed = 20;
    public int bulletsToShot = 5;           //進冷卻前發射子彈的數量
    public float weaponCooldown = 1.5f;     //子彈射完後武器進冷卻時間

    public IdleState_Range idleState { get; private set; }
    public MoveState_Range moveState { get; private set; }
    public BattleState_Range battleState { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        idleState = new IdleState_Range(this, stateMachine, "Idle");
        moveState = new MoveState_Range(this, stateMachine, "Move");
        battleState = new BattleState_Range(this, stateMachine, "Battle");
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idleState);
    }


    protected override void Update()
    {
        base.Update();

        stateMachine.CurrentState.Update();
    }

    public void FirtSingleBattle()
    {
        anim.SetTrigger("Shoot");

        Vector3 bulletsDirection = ((player.position + Vector3.up) - gunPoint.position).normalized;

        GameObject newBullet = ObjectPool.instance.GetObject(bulletPrefab, gunPoint.position);
        newBullet.transform.position = gunPoint.position;
        newBullet.transform.rotation = Quaternion.LookRotation(gunPoint.forward);

        newBullet.GetComponent<Enemy_Bullet>().BulletSetup();

        Rigidbody rbNewBullet = newBullet.GetComponent<Rigidbody>();
        rbNewBullet.mass = 20 / bulletSpeed;
        rbNewBullet.velocity = bulletsDirection * bulletSpeed;
    }

    public override void EnterBattleMode()
    {
        if (inBattleMode) return;

        base.EnterBattleMode();
        stateMachine.ChangeState(battleState);

    }
}
