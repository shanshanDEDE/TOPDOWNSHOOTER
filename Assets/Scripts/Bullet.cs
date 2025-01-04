using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float impactForce; //受到攻擊時的衝擊力

    private BoxCollider cd;
    private Rigidbody rb;
    private MeshRenderer meshRenderer;
    private TrailRenderer trailRenderer;


    [SerializeField] private GameObject bulletImpactFX;


    private Vector3 startPosition;
    private float flyDistance;
    private bool bulletDisable;

    private void Awake()
    {
        cd = GetComponent<BoxCollider>();
        rb = GetComponent<Rigidbody>();
        meshRenderer = GetComponent<MeshRenderer>();
        trailRenderer = GetComponent<TrailRenderer>();
    }

    //設定子彈距離
    public void BulletSetup(float flyDistance, float impactForce)
    {
        this.impactForce = impactForce;

        bulletDisable = false;
        cd.enabled = true;
        meshRenderer.enabled = true;

        trailRenderer.time = 0.25f;
        startPosition = transform.position;
        this.flyDistance = flyDistance + 0.5f; //0.5是因為雷射我們設計上最後有一小多多出來讓美術看起來有漸漸消失的感覺(PlayerAim script中的updateAimVisuals方法的Tip)
    }

    private void Update()
    {
        //設定子彈飛行距離超過一定距離時trail會見退
        FadeTrailIfNeeded();

        //關閉子彈顯示及碰撞
        DisableBulletIfNeeded();

        //送回物件池
        ReturnToPoolIfNeeded();
    }

    //送回物件池
    private void ReturnToPoolIfNeeded()
    {
        //送回物件池
        if (trailRenderer.time < 0)
        {
            ReturnBulletToPool();
        }
    }

    //關閉子彈顯示及碰撞
    private void DisableBulletIfNeeded()
    {
        //設定子彈距離上限
        if (Vector3.Distance(startPosition, transform.position) > flyDistance && !bulletDisable)
        {
            //關閉
            cd.enabled = false;
            meshRenderer.enabled = false;
        }
    }

    //設定子彈飛行距離超過一定距離時trail會見退
    private void FadeTrailIfNeeded()
    {
        //設定子彈飛行距離超過時trail不會繼續顯示
        if (Vector3.Distance(startPosition, transform.position) > flyDistance - 1.5)
        {
            trailRenderer.time -= 2 * Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {


        CreateImpactFX(collision);
        ReturnBulletToPool();

        Enemy enemy = collision.gameObject.GetComponentInParent<Enemy>();
        EnemyShield shield = collision.gameObject.GetComponentInParent<EnemyShield>();

        //如果碰撞的是盾牌
        if (shield != null)
        {
            //減少耐久值
            shield.ReduceDurability();
            return;
        }

        if (enemy != null)
        {
            //計算衝擊力
            Vector3 force = rb.velocity.normalized * impactForce;
            //取得衝擊的物件的rigidbody
            Rigidbody hitRigidbody = collision.collider.attachedRigidbody;

            enemy.GetHit();
            //敵人受到攻擊的衝擊
            enemy.HitImpact(force, collision.contacts[0].point, hitRigidbody);
        }


    }

    private void ReturnBulletToPool()
    {
        ObjectPool.instance.ReturnObject(gameObject);
    }

    private void CreateImpactFX(Collision collision)
    {
        //判斷第一個接觸到的點產生特效
        if (collision.contacts.Length > 0)
        {
            ContactPoint contact = collision.contacts[0];

            GameObject newImpactFX = ObjectPool.instance.GetObject(bulletImpactFX);
            newImpactFX.transform.position = contact.point;

            ObjectPool.instance.ReturnObject(newImpactFX, 1);
        }
    }
}
