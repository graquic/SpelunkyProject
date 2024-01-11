using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum PoolType
{
    Bullet,    
    Bomb,
    Enemy,
    ShotParticle,
    HitParticle,


}

[Serializable]
public class ObjectPool
{
    public PoolType poolType;
    public GameObject poolPrefab;
    public int initCount;
}

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Instance { get; private set; }

    [SerializeField] ObjectPool[] objectPools;

    Dictionary<PoolType, List<GameObject>> poolDictionary = new();
    Dictionary<PoolType, GameObject> prefabDictionary = new();
    Dictionary<PoolType, Transform> poolParentDictionary = new();

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        
    }
    private void Start()
    {
        InitializePool();
    }

    void InitializePool()
    {
        foreach (ObjectPool pool in objectPools)
        {
            poolDictionary[pool.poolType] = new List<GameObject>();
            prefabDictionary[pool.poolType] = pool.poolPrefab;

            Transform poolParent = new GameObject(pool.poolType.ToString()).transform;
            poolParent.parent = this.transform;
            poolParentDictionary[pool.poolType] = poolParent;


            for (int i = 0; i < pool.initCount; i++)
            {
                GameObject obj = Instantiate(prefabDictionary[pool.poolType], poolParentDictionary[pool.poolType]);
                obj.SetActive(false);

                poolDictionary[pool.poolType].Add(obj);
                
            }
        }

        
    }

    public GameObject GetObject(PoolType type)
    {
        if (poolDictionary[type].Count <= 0)
        {
            GameObject obj = Instantiate(prefabDictionary[type]);
            obj.SetActive(true);

            return obj;
        }
        else
        {
            GameObject obj = poolDictionary[type][0];
            poolDictionary[type].RemoveAt(0);
            obj.SetActive(true);

            return obj;
        }
    }

    public void ReturnObject(PoolType type, GameObject obj)
    {
        obj.SetActive(false);
        poolDictionary[type].Add(obj);
        obj.transform.parent = poolParentDictionary[type];
        obj.transform.localPosition = Vector3.zero;
    }
}
