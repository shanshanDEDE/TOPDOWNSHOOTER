using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_WeaponModel : MonoBehaviour
{
    public Enemy_MeleeWeaponType weaponType;
    public AnimatorOverrideController overrideController;   //AnimatorOverrideController是可以自己設定那些要用原本的而我們只覆蓋想要改的
    public Enemy_MeleeWeaponData weaponData;

    [SerializeField] private GameObject[] trailEffects;

    private void Awake()
    {

    }

    //開啟或關閉Trail
    public void EnableTrailEffect(bool enable)
    {
        foreach (var effect in trailEffects)
        {
            effect.SetActive(enable);
        }
    }
}
