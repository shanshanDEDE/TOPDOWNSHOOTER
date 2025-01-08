using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationEvents : MonoBehaviour
{
    private Enemy enemy;

    private void Awake()
    {
        enemy = GetComponentInParent<Enemy>();
    }

    //觸發動畫事件
    public void AnimationTrigger() => enemy.AnimationTrigger();


    //開啟手動移動
    public void StartManualMovement() => enemy.ActivateManualMovement(true);
    //關閉手動移動
    public void StopManualMovement() => enemy.ActivateManualMovement(false);

    //開啟手動旋轉
    public void StartManualRotation() => enemy.ActivateManualRotation(true);
    //關閉手動旋轉
    public void StopManualRotation() => enemy.ActivateManualRotation(false);

    //觸發能力
    public void AbilityEvent() => enemy.AbilityTrigger();

    //EnableIK
    public void EnableIK() => enemy.visuals.EnableIK(true, true, 3f);

    public void EnableWaeponModel()
    {
        // 開始延遲啟用和關閉武器模型
        enemy.visuals.EnableWeaponModel(true);                //啟用左手武器
        enemy.visuals.EnableSceonderyWeaponModel(false);      //關閉右手武器
    }
}
