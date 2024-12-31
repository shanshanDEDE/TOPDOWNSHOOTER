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
    public int bulletsPerShot;
    public float defaultFireRate;             //子彈預設發射間隔
    public float fireRate = 1;                  //子彈發射間隔
    private float lastShootTime;

    [Header("Burst fire(點放射擊)")]
    public bool burstAvalible;          //是否有點放射擊功能
    public bool burstActive;            //點放射擊是否啟用

    public int burstBulletsPerShot;             //每次射擊的子彈數量
    public float burstFireRate;                 //每次射擊的發射間隔
    public float burstFireDelay = 0.1f;         //每發子彈間的發射時間間隔

    [Header("Magazine 細節")]
    public int bulletsInMagazine;       //彈匣內目前的子彈數量
    public int magazineCapacity;        //彈匣容量
    public int totalReserveAmmo;        //總儲備彈藥

    [Range(1, 3)]
    public float reloadSpeed = 1;               //裝子彈的速度
    [Range(1, 3)]
    public float equipmentSpeed = 1;            //裝備武器的速度
    [Range(2, 12)]
    public float gunDistance = 4;               //該武器的子彈射程
    [Range(3, 8)]
    public float cameraDistance = 6;            //該武器的攝影機距離

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

    #region Burst fire(點放射擊) 方法

    //取得該槍目前的模式(點放射擊的部分)
    public bool BurstActivated()
    {
        //鎖定霰彈槍只有點放射擊(因此設定霰彈槍時,不會通過toogleBurst取改變,所以只要設定預設的部分就好了)
        if (weaponType == WeaponType.Shotgun)
        {
            burstFireDelay = 0;
            return true;
        }

        return burstActive;
    }

    public void ToggleBurst()
    {
        if (burstAvalible == false)
        {
            return;
        }

        burstActive = !burstActive;

        //切換點放射擊
        if (burstActive)
        {
            //切換每次發射子彈數量
            bulletsPerShot = burstBulletsPerShot;
            //切換每次子彈發射間隔
            fireRate = burstFireRate;
        }
        else
        {
            //回覆預設
            bulletsPerShot = 1;
            fireRate = defaultFireRate;
        }
    }


    #endregion

    public bool CanShoot()
    {
        if (HaveEnoughBullets() && ReadyToFire())
        {
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

