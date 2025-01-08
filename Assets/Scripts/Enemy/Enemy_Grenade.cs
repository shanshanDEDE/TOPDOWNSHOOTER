using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Grenade : MonoBehaviour
{
    [SerializeField] private GameObject explosionFx;
    [SerializeField] private float impactRadius;
    [SerializeField] private float upwardsMultiplier = 1;
    private Rigidbody rb;
    private float timer;
    private float impactPower;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer < 0)
        {
            Explode();
        }
    }

    private void Explode()
    {
        GameObject newFx = ObjectPool.instance.GetObject(explosionFx, transform.position);
        //newFx.transform.position = transform.position;

        ObjectPool.instance.ReturnObject(newFx, 1);
        ObjectPool.instance.ReturnObject(gameObject);

        Collider[] colliders = Physics.OverlapSphere(transform.position, impactRadius);

        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.AddExplosionForce(impactPower, transform.position, impactRadius, upwardsMultiplier, ForceMode.Impulse);
            }
        }
    }

    public void SetupGrenade(Vector3 target, float timeToTarget, float countdown, float impactPower)
    {
        rb.velocity = CalculateLaunchVelocity(target, timeToTarget);
        //具體爆炸時間
        timer = countdown + timeToTarget;
        this.impactPower = impactPower;
    }

    //用公式計算發射速度
    private Vector3 CalculateLaunchVelocity(Vector3 target, float timeToTarget)
    {
        Vector3 direction = target - transform.position;
        Vector3 directionXZ = new Vector3(direction.x, 0f, direction.z);

        Vector3 velocityXY = directionXZ / timeToTarget;

        float velocityY =
            (direction.y - (Physics.gravity.y * Mathf.Pow(timeToTarget, 2)) / 2) / timeToTarget;

        Vector3 launchVelocity = velocityXY + Vector3.up * velocityY;
        return launchVelocity;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, impactRadius);
    }
}
