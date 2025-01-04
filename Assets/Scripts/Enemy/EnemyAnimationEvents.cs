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
    public void AbilityEvent() => enemy.GetComponent<Enemy_Melee>().TriggerAbility();
}
