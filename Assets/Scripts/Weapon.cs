using UnityEngine;

public enum WeaponType
{
    Pistol,
    Revolver,
    AutoRifle,
    Shotgun,
    Rifle
}

[System.Serializable] //可序列化(讓腳本可以在編輯器中可視)(其他腳本中宣告的話可以變成可視的)
public class Weapon
{
    public WeaponType weaponType;
    public int bulletsInMagazine;       //彈匣內目前的子彈數量
    public int magazineCapacity;        //彈匣容量
    public int totalReserveAmmo;        //總儲備彈藥

    [Range(1, 3)]
    public float reloadSpeed = 1;               //裝子彈的速度
    [Range(1, 3)]
    public float equipmentSpeed = 1;            //裝備武器的速度

    public bool CanShoot()
    {
        return HaveEnoughBullets();
    }

    private bool HaveEnoughBullets()
    {
        if (bulletsInMagazine > 0)
        {
            bulletsInMagazine--;
            return true;
        }

        return false;
    }

    public bool CanReload()
    {
        if (bulletsInMagazine == magazineCapacity)
        {
            return false;
        }

        if (totalReserveAmmo > 0)
        {
            return true;
        }

        return false;
    }

    //補充子彈
    public void RefillBullets()
    {
        //如果希望剩餘的子彈可以留下回到總持有彈藥量,則加這行
        //totalReserveAmmo += bulletsInMagazine;

        int bulletsToReload = magazineCapacity;

        //如果剩餘彈藥量小於彈匣容量,則補充道剩餘彈藥量
        if (bulletsToReload > totalReserveAmmo)
        {
            bulletsToReload = totalReserveAmmo;
        }

        totalReserveAmmo -= bulletsToReload;
        bulletsInMagazine = bulletsToReload;

        if (totalReserveAmmo < 0)
        {
            totalReserveAmmo = 0;
        }
    }
}
