using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct AmmoData
{
    public WeaponType weaponType;
    [Range(1, 100)] public int minAmount;
    [Range(1, 100)] public int maxAmount;
}

public enum AmmoBoxType { smallBox, bigBox };

public class Pickup_Ammo : Interactable
{
    //武器類型
    [SerializeField] private AmmoBoxType boxType;


    //設定大小箱分別有什麼武器會使用
    [SerializeField] private List<AmmoData> smallBoxAmmo;
    [SerializeField] private List<AmmoData> bigBoxAmmo;

    //箱子模型(例如:小箱,大箱)
    [SerializeField] private GameObject[] boxModel;

    private void Start()
    {
        //設定箱子模組
        SetupBoxModel();
    }

    public override void Interaction()
    {
        List<AmmoData> currentAmmoList = smallBoxAmmo;

        if (boxType == AmmoBoxType.bigBox)
        {
            currentAmmoList = bigBoxAmmo;
        }

        foreach (AmmoData ammo in currentAmmoList)
        {
            //依照上面定義的ammo weapontype帶入WeaponInSlots方法內找weapon
            Weapon weapon = weaponController.WeaponInSlots(ammo.weaponType);

            AddBulletsToWeapon(weapon, GetBulletAmount(ammo));
        }

        ObjectPool.instance.ReturnObject(gameObject);
    }

    //取得隨機後得到的子彈數量
    private int GetBulletAmount(AmmoData ammoData)
    {
        float min = Mathf.Min(ammoData.minAmount, ammoData.maxAmount);
        float max = Mathf.Max(ammoData.minAmount, ammoData.maxAmount);

        float randomAmmoAmount = Random.Range(min, max);
        return Mathf.RoundToInt(randomAmmoAmount);
    }

    private void AddBulletsToWeapon(Weapon weapon, int amount)
    {
        if (weapon == null)
        {
            return;
        }

        weapon.totalReserveAmmo += amount;
    }

    //設定箱子模組
    private void SetupBoxModel()
    {
        //找到現在武器類型對應的箱子為哪個
        for (int i = 0; i < boxModel.Length; i++)
        {
            boxModel[i].SetActive(false);
            if (i == ((int)boxType))
            {
                boxModel[i].SetActive(true);
                UpdateMeshAndMaterial(boxModel[i].GetComponent<MeshRenderer>());
            }
        }
    }
}
