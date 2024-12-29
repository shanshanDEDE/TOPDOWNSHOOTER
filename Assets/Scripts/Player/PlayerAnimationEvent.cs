using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class PlayerAnimationEvent : MonoBehaviour
{
    private PlayerWeaponVisuals visualController;

    private void Start()
    {
        visualController = GetComponentInParent<PlayerWeaponVisuals>();
    }


    public void ReloadIsOver()
    {
        visualController.MaximizeRigWeight();

        //啟動讓手的rig weight恢復1
    }

    public void ReturnRig()
    {
        visualController.MaximizeRigWeight();
        //啟動讓手的rig weight恢復1

        visualController.MaximizeLeftHandWeight();
    }


    public void WeaponGrabIsOver()
    {

        visualController.SetBusyGrabbingWeaponTo(false);
    }
}
