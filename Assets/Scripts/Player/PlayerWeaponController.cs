using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    private Player player;
    //我們計算速度與質量推導公式的預設值(這邊希望造成的效果是我們的子彈速度為20質量為1的情況)
    private const float REFERNCE_BULLET_SPEED = 20;

    [SerializeField] private Weapon currentWeapon;
    private bool weaponReady;
    private bool isShooting;

    [Header("子彈細節")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed;

    [SerializeField] private Transform weaponHolder;

    [Header("Inventory")]

    [SerializeField] private int maxSlots = 2;
    [SerializeField] private List<Weapon> weaponSlots;

    private void Start()
    {
        player = GetComponent<Player>();
        AssignInputEvents();

        Invoke("EquipStartingWeapon", 0.1f);
    }

    private void Update()
    {
        if (isShooting)
        {
            Shoot();
        }
    }


    #region 武器欄位控制 - 撿起\裝備\丟棄\準備 武器

    private void EquipStartingWeapon()
    {
        EquipWeapon(0);
    }


    //裝備武器
    private void EquipWeapon(int i)
    {
        if (weaponSlots.Count <= 1 && i == 1)
        {
            return;
        }

        SetWeaponReady(false);

        currentWeapon = weaponSlots[i];
        //顯示武器
        player.weaponVisuals.PlayerWeaponEquipAnimation();
    }

    public void PickupWeapon(Weapon newWeapon)
    {
        if (weaponSlots.Count >= maxSlots)
        {
            return;
        }

        weaponSlots.Add(newWeapon);
        player.weaponVisuals.SwitchOnBackupWeaponModel();
    }

    private void DropWeapon()
    {
        if (HasOnlyOneWeapon())
        {
            Debug.Log("沒有武器欄位ㄌ");
            return;
        }

        weaponSlots.Remove(currentWeapon);

        currentWeapon = weaponSlots[0];

        EquipWeapon(0);
    }

    public void SetWeaponReady(bool ready) => weaponReady = ready;

    public bool WeaponReady() => weaponReady;

    #endregion


    private void Shoot()
    {
        if (WeaponReady() == false)
            return;

        if (currentWeapon.CanShoot() == false)
            return;

        //如果是single則不會持續射擊
        if (currentWeapon.shootType == ShootType.Single)
        {
            isShooting = false;
        }

        //取得物件池的子彈
        GameObject newBullet = ObjectPool.instance.GetBullet();
        //Instantiate(bulletPrefab, gunPoint.position, Quaternion.LookRotation(gunPoint.forward));

        newBullet.transform.position = GunPoint().position;
        newBullet.transform.rotation = Quaternion.LookRotation(BulletDirection());  //老師BulletDirection沒用而是用GunPoint().forward

        //計算不同速度時子彈應該要有的質量 來讓造成碰撞時的效果一樣(這邊不知道為什麼套用下面的公式揖讓會造成差異)
        Rigidbody rbNewBullet = newBullet.GetComponent<Rigidbody>();

        rbNewBullet.mass = REFERNCE_BULLET_SPEED / bulletSpeed;

        newBullet.GetComponent<Rigidbody>().velocity = BulletDirection() * bulletSpeed;

        // 開始計時，如果 10 秒內沒碰撞，將子彈返回物件池
        StartCoroutine(ReturnBulletAfterTime(newBullet, 10f));

        player.weaponVisuals.PlayFireAnimation();
    }

    // 協程：子彈射出後等待指定時間返回物件池
    private IEnumerator ReturnBulletAfterTime(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);

        // 如果子彈還存在，並且未被其他事件處理，則返回物件池
        if (bullet != null && bullet.activeInHierarchy)
        {
            ObjectPool.instance.ReturnBullet(bullet);
        }
    }

    private void Reload()
    {
        SetWeaponReady(false);
        player.weaponVisuals.PlayerReloadAnimation();
    }

    public Vector3 BulletDirection()
    {
        Transform aim = player.aim.Aim();

        Vector3 direction = (aim.position - GunPoint().position).normalized;

        if (player.aim.CanAimPrecisly() == false && player.aim.Target() == null)
        {
            direction.y = 0;
        }

        return direction;
    }

    public bool HasOnlyOneWeapon() => weaponSlots.Count <= 1;


    public Weapon CurrentWeapon() => currentWeapon;

    //取得放在背後的武器
    public Weapon BackupWeapon()
    {
        foreach (Weapon weapon in weaponSlots)
        {
            if (weapon != currentWeapon)
            {
                return weapon;
            }
        }

        return null;
    }


    public Transform GunPoint() => player.weaponVisuals.CurrentWeaponModel().gunPoint;

    #region 輸入事件
    //定義新版控制器事件
    private void AssignInputEvents()
    {
        PlayerControls controls = player.controls;

        controls.Charcater.Fire.performed += context => isShooting = true;
        controls.Charcater.Fire.canceled += context => isShooting = false;

        controls.Charcater.EquipSlot1.performed += context => EquipWeapon(0);
        controls.Charcater.EquipSlot2.performed += context => EquipWeapon(1);

        controls.Charcater.DropCurrentWeapon.performed += context => DropWeapon();

        controls.Charcater.Reload.performed += context =>
        {
            if (currentWeapon.CanReload() && WeaponReady())
            {
                Reload();
            }
        };
    }



    #endregion
}
