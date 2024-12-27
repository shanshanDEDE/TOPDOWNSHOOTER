using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{

    private Player player;

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private Transform gunPoint;

    [SerializeField] private Transform weaponHolder;
    [SerializeField] private Transform aim;


    private void Start()
    {
        player = GetComponent<Player>();
        player.controls.Charcater.Fire.performed += context => Shoot();
    }

    private void Shoot()
    {
        GameObject newBullet = Instantiate(bulletPrefab, gunPoint.position, Quaternion.LookRotation(gunPoint.forward));

        newBullet.GetComponent<Rigidbody>().velocity = BulletDirection() * bulletSpeed;

        Destroy(newBullet, 10f);

        GetComponentInChildren<Animator>().SetTrigger("Fire");
    }

    //固定住y軸
    private Vector3 BulletDirection()
    {
        //將槍口及子彈生成時的瞄準方向瞄準到aim上
        weaponHolder.LookAt(aim);
        gunPoint.LookAt(aim);

        Vector3 direction = (aim.position - gunPoint.position).normalized;

        if (player.aim.CanAimPrecisly() == false)
        {
            direction.y = 0;
        }

        return direction;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(weaponHolder.position, weaponHolder.position + weaponHolder.forward * 25f);

        Gizmos.color = Color.yellow;

        //Gizmos.DrawLine(gunPoint.position, gunPoint.position + BulletDirection() * 25f);
    }
}
