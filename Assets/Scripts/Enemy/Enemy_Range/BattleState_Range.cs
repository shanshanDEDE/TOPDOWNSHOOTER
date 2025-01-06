using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleState_Range : EnemyState
{
    private Enemy_Range enemy;

    private float lastTimeShot = -10;
    private int bulletsShot = 0;

    private int bulletsPerAttack;
    private float weaponCooldown;

    public BattleState_Range(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as Enemy_Range;
    }

    public override void Enter()
    {
        base.Enter();

        //取得資料(使用的取得方法內未在ˋ一個範圍內隨機抽數)
        bulletsPerAttack = enemy.weaponData.GetBulletsPerAttack();
        weaponCooldown = enemy.weaponData.GetWeaponCooldown();

        enemy.visuals.EnableIK(true);
    }


    public override void Exit()
    {
        base.Exit();

        enemy.visuals.EnableIK(false);
    }

    public override void Update()
    {
        base.Update();

        enemy.FaceTarget(enemy.player.position);

        //如果進冷卻前該發射的子彈射完了,就結束
        if (WeaponOutOfBullets())
        {
            //如果武器冷卻結束
            if (WeaponOnCoolDown())
            {
                //重製武器
                AttempToResetWeapon();
            }
            return;
        }

        //如果可以射擊
        if (CanShoot())
        {
            Shoot();    //射擊
        }
    }

    private void AttempToResetWeapon()
    {
        bulletsShot = 0;

        //取得資料(使用的取得方法內未在ˋ一個範圍內隨機抽數)
        bulletsPerAttack = enemy.weaponData.GetBulletsPerAttack();
        weaponCooldown = enemy.weaponData.GetWeaponCooldown();

    }

    private bool WeaponOnCoolDown()
    {
        return Time.time > lastTimeShot + weaponCooldown;
    }

    private bool WeaponOutOfBullets()
    {
        return bulletsShot >= bulletsPerAttack;

    }


    private bool CanShoot()
    {
        return Time.time > lastTimeShot + 1 / enemy.weaponData.fireRate;
    }

    private void Shoot()
    {
        enemy.FirtSingleBattle();
        lastTimeShot = Time.time;

        bulletsShot++;
    }
}
