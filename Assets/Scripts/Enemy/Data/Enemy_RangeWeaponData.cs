using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Data", menuName = "Enemy data/Range weapon Data")]
public class Enemy_RangeWeaponData : ScriptableObject
{
    [Header("武器細節")]
    public Enemy_RangeWeaponType weaponType;
    public float fireRate = 1f; //每秒發射速率

    public int minBulletsPerAttack = 1; //每次攻擊最小子彈數量
    public int maxBulletsPerAttack = 1; //每次攻擊最大子彈數量

    public float minWeaponCooldown = 2;//最小攻擊間隔
    public float maxWeaponCooldown = 3;//最大攻擊間隔

    [Header("子彈細節")]
    public float bulletSpeed = 20f;//子彈速度
    public float weaponSpread = 0.1f;//子彈偏移量

    //取的隨機每次攻擊子彈數量
    public int GetBulletsPerAttack() => Random.Range(minBulletsPerAttack, maxBulletsPerAttack + 1);

    //取得隨機攻擊間隔
    public float GetWeaponCooldown() => Random.Range(minWeaponCooldown, maxWeaponCooldown);


    //套用子彈偏移
    public Vector3 ApplyWeaponSpread(Vector3 originalDirection)
    {
        float randomozedValue = Random.Range(-weaponSpread, weaponSpread);
        float yvalue = randomozedValue * 2 / 3;
        Quaternion spreadRotation = Quaternion.Euler(randomozedValue, (int)yvalue, randomozedValue);

        return spreadRotation * originalDirection;
    }
}
