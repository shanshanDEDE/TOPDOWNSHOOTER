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
}
