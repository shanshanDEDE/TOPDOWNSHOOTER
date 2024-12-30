using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class PlayerAnimationEvent : MonoBehaviour
{
    private PlayerWeaponVisuals visualController;
    private PlayerWeaponController weaponController;

    private void Start()
    {
        visualController = GetComponentInParent<PlayerWeaponVisuals>();
        weaponController = GetComponentInParent<PlayerWeaponController>();
    }


    public void ReloadIsOver()
    {
        visualController.MaximizeRigWeight();
        weaponController.CurrentWeapon().RefillBullets();

        weaponController.SetWeaponReady(true);

        //啟動讓手的rig weight恢復1
    }

    public void ReturnRig()
    {
        visualController.MaximizeRigWeight();
        //啟動讓手的rig weight恢復1

        visualController.MaximizeLeftHandWeight();
    }


    public void WeaponEquipingIsOver()
    {
        weaponController.SetWeaponReady(true);
    }

    public void SwitchOnWeaponModel() => visualController.SwitchOnCurrentWeaponModel();
}
