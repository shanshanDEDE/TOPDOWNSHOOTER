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

    [Header("Spread(子彈偏移)")]
    public float baseSpread = 1;
    private float currentSpread = 2;
    public float maximumSpread = 3;

    public float spreadIncreaseRate = 0.15f;

    private float lastSpreadUpdateTime;
    //停止射擊後到重製偏移所需的時間
    private float spreadCooldown = 1;


    #region 子彈偏移方法
    //套用子彈偏移
    public Vector3 ApplySpread(Vector3 originalDirection)
    {
        UpdateSpread();

        float randomozedValue = Random.Range(-currentSpread, currentSpread);

        Quaternion spreadRotation = Quaternion.Euler(randomozedValue, randomozedValue, randomozedValue);

        return spreadRotation * originalDirection;
    }

    //如果射擊停止後超過一秒,子彈偏移重置否則增加偏移量到最大值為止
    private void UpdateSpread()
    {
        if (Time.time > lastSpreadUpdateTime + spreadCooldown)
        {
            currentSpread = baseSpread;
        }
        else
        {
            //隨時間增加子彈偏移
            IncreaseSpread();
        }

        lastSpreadUpdateTime = Time.time;

    }


    //隨時間增加子彈偏移
    private void IncreaseSpread()
    {
        currentSpread = Mathf.Clamp(currentSpread + spreadIncreaseRate, baseSpread, maximumSpread);
    }

    #endregion

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
