using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_WeaponModel : MonoBehaviour
{
    public Enemy_MeleeWeaponType weaponType;
    public AnimatorOverrideController overrideController;   //AnimatorOverrideController是可以自己設定那些要用原本的而我們只覆蓋想要改的
}
