using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    private Player player;
    //我們計算速度與質量推導公式的預設值(這邊希望造成的效果是我們的子彈速度為20質量為1的情況)
    private const float REFERNCE_BULLET_SPEED = 20;

    [SerializeField] private Weapon currentWeapon;

    [Header("子彈細節")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private Transform gunPoint;

    [SerializeField] private Transform weaponHolder;


    private void Start()
    {
        player = GetComponent<Player>();
        player.controls.Charcater.Fire.performed += context => Shoot();

        currentWeapon.ammo = currentWeapon.maxAmmo;                     //初始武器的子彈設定
    }

    private void Shoot()
    {
        if (currentWeapon.ammo <= 0)
        {
            Debug.Log("No Ammo");
            return;
        }

        currentWeapon.ammo--;

        GameObject newBullet = Instantiate(bulletPrefab, gunPoint.position, Quaternion.LookRotation(gunPoint.forward));


        //計算不同速度時子彈應該要有的質量 來讓造成碰撞時的效果一樣(這邊不知道為什麼套用下面的公式揖讓會造成差異)
        Rigidbody rbNewBullet = newBullet.GetComponent<Rigidbody>();

        rbNewBullet.mass = REFERNCE_BULLET_SPEED / bulletSpeed;

        newBullet.GetComponent<Rigidbody>().velocity = BulletDirection() * bulletSpeed;

        Destroy(newBullet, 10f);

        GetComponentInChildren<Animator>().SetTrigger("Fire");
    }

    public Vector3 BulletDirection()
    {
        //將槍口及子彈生成時的瞄準方向瞄準到aim上
        // weaponHolder.LookAt(aim);
        // gunPoint.LookAt(aim);                    //移動到更好的地方ㄌ

        Transform aim = player.aim.Aim();

        Vector3 direction = (aim.position - gunPoint.position).normalized;

        if (player.aim.CanAimPrecisly() == false && player.aim.Target() == null)
        {
            direction.y = 0;
        }

        return direction;
    }

    public Transform GunPoint() => gunPoint;
}
