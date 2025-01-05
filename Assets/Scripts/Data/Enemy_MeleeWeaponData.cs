using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Data", menuName = "Enemy data/Melee weapon Data")]
public class Enemy_MeleeWeaponData : ScriptableObject
{
    public List<Enemy_MeleeAttackData> attackData;
    public float turnSpeed = 10;
}
