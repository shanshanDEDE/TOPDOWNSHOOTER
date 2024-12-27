using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class PlayerAnimationEvent : MonoBehaviour
{
    private WeaponVisualController visualController;

    private void Start()
    {
        visualController = GetComponentInParent<WeaponVisualController>();
    }


    public void ReloadIsOver()
    {
        visualController.ReturnRigWeighthToOne();

        //啟動讓手的rig weight恢復1
    }

    public void ReturnRig()
    {
        visualController.ReturnRigWeighthToOne();
        //啟動讓手的rig weight恢復1

        visualController.ReturnWeightToLeftHandIK();
    }


    public void WeaponGrabIsOver()
    {

        visualController.SetBusyGrabbingWeaponTo(false);
    }
}
