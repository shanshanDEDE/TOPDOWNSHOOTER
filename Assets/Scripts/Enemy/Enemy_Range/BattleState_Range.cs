using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BattleState_Range : EnemyState
{
    private Enemy_Range enemy;

    private float lastTimeShot = -10;
    private int bulletsShot = 0;

    private int bulletsPerAttack;
    private float weaponCooldown;

    private float coverCheckTimer;
    private bool firstTimeAttack = true;

    public BattleState_Range(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as Enemy_Range;
    }

    public override void Enter()
    {
        base.Enter();

        SetupValueFirstAttack();    //初始設定值

        //針對Enemy_Range這個敵人防止他因前一個狀態的移動而飄移
        enemy.agent.isStopped = true;
        enemy.agent.velocity = Vector3.zero;



        enemy.visuals.EnableIK(true, true);

        stateTimer = enemy.attackDelay; //延遲射擊時間
    }

    public override void Update()
    {
        base.Update();

        //如果看見玩家(這邊因為是在玩家進入攻擊範圍時進入battlestate才會執行這邊,因此可以確保這邊做使用沒關西)
        if (enemy.IsSeeingPlayer())
        {
            enemy.FaceTarget(enemy.aim.position);
        }

        //是否應該追擊玩家
        if (MustAdvancePlayer())
        {
            stateMachine.ChangeState(enemy.advancePlayerState);
        }

        ChangeCoverIfShould();  //是否應該切換去找新的掩護點

        if (stateTimer > 0)
        {
            return;
        }

        //如果進冷卻前該發射的子彈射完了,就結束
        if (WeaponOutOfBullets())
        {
            //如果為Unstoppable並且準備好繼續走
            if (enemy.IsUnstoppable() && UnStoppableWalkRead())
            {
                //設定追擊時間
                enemy.advanceDuration = weaponCooldown;
                //進入追擊
                stateMachine.ChangeState(enemy.advancePlayerState);
            }

            //如果武器冷卻結束
            if (WeaponOnCoolDown())
            {
                //重製武器
                AttempToResetWeapon();
            }
            return;
        }

        //如果可以射擊 && 準心是否在敵人身上
        if (CanShoot() && enemy.IsAimOnPlayer())
        {
            Shoot();    //射擊
        }
    }

    //是否應該追擊敵人
    private bool MustAdvancePlayer()
    {
        if (enemy.IsUnstoppable())
            return false;

        //是否玩家是在攻擊範圍內
        return enemy.IsPlayerInAgrresionRange() == false && ReadyToLeaveCover();
    }

    //是否可以繼續走
    private bool UnStoppableWalkRead()
    {
        //距離玩家距離
        float distanceToPlayer = Vector3.Distance(enemy.transform.position, enemy.player.position);
        bool outOfStoppingDistance = distanceToPlayer > enemy.advanceStoppingDistance;               //距離玩家大於追擊距離
        bool unstoppableWalkOnCooldown =
            Time.time < enemy.weaponData.minWeaponCooldown + enemy.advancePlayerState.lastTimeAdvanced;

        return outOfStoppingDistance && unstoppableWalkOnCooldown == false;
    }

    #region 掩護系統region

    //是否準備好離開掩體
    private bool ReadyToLeaveCover()
    {
        return Time.time > enemy.minSCoverTime + enemy.runToCoverState.lastTimeTookCover;
    }

    private void ChangeCoverIfShould()
    {
        if (enemy.coverPerk != CoverPerk.CanTakeAndChangeCover) return;

        coverCheckTimer -= Time.deltaTime;

        if (coverCheckTimer < 0)
        {
            coverCheckTimer = 0.5f;     //每0.5秒檢查一次

            //如果玩家在敵人視線內沒有遮掩物或者玩家離太近
            //則尋找下一個目標
            if (ReadyToChangeCover())
            {
                if (enemy.CanGetCover())
                {
                    stateMachine.ChangeState(enemy.runToCoverState);
                }
            }

        }
    }

    //是否準備好可以切換到下一個掩護點
    private bool ReadyToChangeCover()
    {
        //如果玩家在敵人視線內沒有遮掩物或者玩家離太近
        bool inDanger = IsPlayerInClearSight() || IsPlayerClose();
        //如果時間大於上次在advancePlayerState+敵人的advanceDuration的話
        bool advanceDurationIsOver = Time.time > enemy.advancePlayerState.lastTimeAdvanced + enemy.advanceDuration;

        return inDanger && advanceDurationIsOver;
    }
    //是否玩家離太近
    private bool IsPlayerClose()
    {
        return Vector3.Distance(enemy.transform.position, enemy.player.transform.position) < enemy.safeDistance;
    }

    //如果玩家在敵人視線內沒有遮掩物
    private bool IsPlayerInClearSight()
    {
        Vector3 directionToPlayer = enemy.player.transform.position - enemy.transform.position;

        if (Physics.Raycast(enemy.transform.position, directionToPlayer, out RaycastHit hit))
        {
            return hit.collider.gameObject.GetComponentInParent<Player>();
        }

        return false;
    }

    #endregion

    #region 武器region

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

    //初始設定值
    private void SetupValueFirstAttack()
    {
        if (firstTimeAttack)
        {
            firstTimeAttack = false;

            //取得資料(使用的取得方法內未在一個範圍內隨機抽數)
            bulletsPerAttack = enemy.weaponData.GetBulletsPerAttack();
            weaponCooldown = enemy.weaponData.GetWeaponCooldown();
        }
    }

    #endregion
}
