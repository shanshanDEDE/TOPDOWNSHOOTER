using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup_Weapon : Interactable
{
    private PlayerWeaponController weaponController;
    [SerializeField] private Weapon_Data weaponData;

    public override void Interaction()
    {
        weaponController.PickupWeapon(weaponData);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if (weaponController == null)
        {
            weaponController = other.GetComponent<PlayerWeaponController>();
        }
    }
}
