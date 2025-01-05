using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Shield : MonoBehaviour
{
    private Enemy_Melee enemy;
    [SerializeField] private int durability; //耐久值

    private void Awake()
    {
        enemy = GetComponentInParent<Enemy_Melee>();
    }

    public void ReduceDurability()
    {
        durability--;
        if (durability <= 0)
        {
            enemy.anim.SetFloat("ChaseIndex", 0);   //切換回沒拿盾牌的追擊動畫
            Destroy(gameObject);
        }
    }
}
