using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool instance;

    [SerializeField] private int poolSize = 10;

    //Queue先進先出的概念
    //透過字典來存放對應的物件池   //透過字典可以在方法中透過參數取得對應的物件池
    private Dictionary<GameObject, Queue<GameObject>> poolDictionary = new Dictionary<GameObject, Queue<GameObject>>();

    //由於某些物件會一開始就在關卡上,而我們的物件持機制是,生成物件後才生成物件池
    //因此撿起物品腳本那邊會導致時間序先進行xx才進行start的問題(可以參考85課)
    //因此這邊手動在start時先生出一個物件池
    [Header("To Initialize")]
    [SerializeField] private GameObject weaponPickup;
    [SerializeField] private GameObject ammoPickup;

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
        InitializeNewPool(weaponPickup);
        InitializeNewPool(ammoPickup);
    }

    //取得物件(從Queue裡取)         //透過字典可以在方法中透過參數取得對應的物件池
    public GameObject GetObject(GameObject prefab)
    {
        //如果沒有該類型prefab的物件池,則生成一個
        if (poolDictionary.ContainsKey(prefab) == false)
        {
            InitializeNewPool(prefab);
        }

        //防止物件池不夠用的情況(再生成一個物件)
        if (poolDictionary[prefab].Count == 0)
        {
            CreateNewObject(prefab);
        }

        GameObject ObjectToGet = poolDictionary[prefab].Dequeue();
        ObjectToGet.SetActive(true);
        ObjectToGet.transform.parent = null;
        return ObjectToGet;
    }

    //將物件塞回物件池(Queue)
    public void ReturnObject(GameObject objectToReturn, float delay = 0.001f)
    {
        StartCoroutine(DelayReturn(delay, objectToReturn));
    }

    private IEnumerator DelayReturn(float delay, GameObject objectToReturn)
    {
        yield return new WaitForSeconds(delay);
        ReturnToPool(objectToReturn);
    }

    //將物件塞回物件池(Queue)
    private void ReturnToPool(GameObject objectToReturn)
    {
        //透過這樣的方式取得該物件池的原始prefab名稱(沒有(clone的))
        GameObject originalPrefab = objectToReturn.GetComponent<PooledObject>().originalPrefab;

        //將物件設為不啟用
        objectToReturn.SetActive(false);
        objectToReturn.transform.parent = transform;

        poolDictionary[originalPrefab].Enqueue(objectToReturn);
    }

    //初始化生成物件池們
    private void InitializeNewPool(GameObject prefab)
    {
        poolDictionary[prefab] = new Queue<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            CreateNewObject(prefab);
        }
    }

    //生成物件池的物件
    private void CreateNewObject(GameObject prefab)
    {
        GameObject newObject = Instantiate(prefab, transform);
        //透過這行這樣的方式然物件池使用時不會因為prefab名稱而導致不一樣的錯誤,而是依照這邊設定好的originalPrefab
        newObject.AddComponent<PooledObject>().originalPrefab = prefab;
        newObject.SetActive(false);

        poolDictionary[prefab].Enqueue(newObject);
    }
}
