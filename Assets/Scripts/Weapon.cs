using UnityEngine;

public enum WeaponType
{
    Pistol,
    Revolver,
    AutoRifle,
    Shotgun,
    Rifle
}

public enum ShootType
{
    Single,
    Auto
}

[System.Serializable] //可序列化(讓腳本可以在編輯器中可視)(其他腳本中宣告的話可以變成可視的)
public class Weapon
{
    public WeaponType weaponType;

    [Header("射擊細節")]

    public ShootType shootType;
    public float fireRate = 1;                  //子彈發射間隔
    private float lastShootTime;

    [Header("Magazine 細節")]
    public int bulletsInMagazine;       //彈匣內目前的子彈數量
    public int magazineCapacity;        //彈匣容量
    public int totalReserveAmmo;        //總儲備彈藥

    [Range(1, 3)]
    public float reloadSpeed = 1;               //裝子彈的速度
    [Range(1, 3)]
    public float equipmentSpeed = 1;            //裝備武器的速度


    public bool CanShoot()
    {
        if (HaveEnoughBullets() && ReadyToFire())
        {
            bulletsInMagazine--;
            return true;
        }

        return false;
    }

    private bool ReadyToFire()
    {
        if (Time.time > lastShootTime + 1 / fireRate)
        {
            lastShootTime = Time.time;
            return true;
        }

        return false;
    }

    #region Reload methods

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

    private bool HaveEnoughBullets()
    {
        if (bulletsInMagazine > 0)
        {
            return true;
        }

        return false;
    }

    #endregion
}
