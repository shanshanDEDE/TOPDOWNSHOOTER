using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup_Weapon : Interactable
{
    [SerializeField] private Weapon_Data weaponData;
    [SerializeField] private Weapon weapon;

    [SerializeField] private BackupWeaponModel[] models;

    private bool oldWeapon;

    private void Start()
    {
        if (oldWeapon == false)
        {
            weapon = new Weapon(weaponData);
        }

        SetupGameObject();
    }

    //設定要撿起的武器資訊
    public void SetupPickupWeapon(Weapon weapon, Transform transform)
    {
        oldWeapon = true;

        this.weapon = weapon;
        weaponData = weapon.weaponData;

        this.transform.position = transform.position + new Vector3(0, 0.75f, 0);
    }

    //更新顯示的武器的對應(名稱,樣式)
    [ContextMenu("Update Item Model")]
    public void SetupGameObject()
    {
        gameObject.name = "Pickup_Weapon - " + weaponData.weaponType.ToString();
        SetupWeaponModel();
    }


    //更顯該顯示的武器模型
    private void SetupWeaponModel()
    {
        foreach (BackupWeaponModel model in models)
        {
            model.gameObject.SetActive(false);

            if (model.weaponType == weaponData.weaponType)
            {
                model.gameObject.SetActive(true);
                UpdateMeshAndMaterial(model.GetComponent<MeshRenderer>());
            }
        }
    }

    public override void Interaction()
    {
        weaponController.PickupWeapon(weapon);

        ObjectPool.instance.ReturnObject(gameObject);
    }
}
