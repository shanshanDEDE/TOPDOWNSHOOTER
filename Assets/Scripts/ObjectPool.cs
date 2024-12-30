using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool instance;

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int poolSize = 10;

    //Queue先進先出的概念
    private Queue<GameObject> bulletPool;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

    }

    private void Start()
    {
        bulletPool = new Queue<GameObject>();
        CreateInitialPool();
    }

    //取得子彈(從Queue裡取)
    public GameObject GetBullet()
    {
        //防止物ˋ建池不夠用的情況(再生成一個物件池)
        if (bulletPool.Count == 0)
        {
            CreateNewObject();
        }

        GameObject bulletToGet = bulletPool.Dequeue();
        bulletToGet.SetActive(true);
        bulletToGet.transform.parent = null;
        return bulletToGet;
    }

    //將子彈塞回Queue
    public void ReturnBullet(GameObject bullet)
    {
        bullet.SetActive(false);
        bulletPool.Enqueue(bullet);
        bullet.transform.parent = transform;
    }

    private void CreateInitialPool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            CreateNewObject();
        }
    }

    private void CreateNewObject()
    {
        GameObject newBullet = Instantiate(bulletPrefab, transform);
        newBullet.SetActive(false);
        bulletPool.Enqueue(newBullet);
    }
}
