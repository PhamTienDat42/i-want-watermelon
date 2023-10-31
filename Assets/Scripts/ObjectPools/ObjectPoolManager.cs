using GamePlay;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectPools
{
    public class ObjectPoolManager : MonoBehaviour
    {
        private readonly Dictionary<string, Pools<Fruits>> objectPools = new();

        [SerializeField] private List<Fruits> fruitPrefabs; // Nếu bạn có nhiều loại đối tượng, sử dụng mảng hoặc List để chứa các prefab

        void Start()
        {
            InitializeObjectPools();
        }

        void InitializeObjectPools()
        {
            foreach (Fruits fruitPrefab in fruitPrefabs)
            {
                Pools<Fruits> objectPool = new Pools<Fruits>(fruitPrefab, 5, transform);
                objectPools.Add(fruitPrefab.name, objectPool);
            }
        }

        public Fruits GetObject(string objectTypeName)
        {
            if (objectPools.TryGetValue(objectTypeName, out Pools<Fruits> objectPool))
            {
                return objectPool.GetObject();
            }

            Debug.LogError($"Object pool for type {objectTypeName} not found!");
            return null;
        }

        public void ReturnObject(Fruits obj)
        {
            string objectTypeName = obj.name;
            if (objectPools.TryGetValue(objectTypeName, out Pools<Fruits> objectPool))
            {
                objectPool.ReturnObject(obj);
            }
            else
            {
                Debug.LogError($"Object pool for type {objectTypeName} not found!");
            }
        }
    }
}