using UnityEngine;

using System.Collections.Generic;
using GamePlay;

namespace ObjectPools
{
    public class FruitPools : MonoBehaviour
    {
        [SerializeField] private Fruits fruits;
        [SerializeField] private int poolSize = 10;
        [SerializeField] private GameObject fruitsParents;

        private List<Fruits> fruitPools;
        private int lastDeactivatedFruitIndex = -1;

        void Start()
        {
            InitializePool();
        }

        void InitializePool()
        {
            fruitPools = new List<Fruits>();

            for (int i = 0; i < poolSize; i++)
            {
                Fruits obj = Instantiate(fruits);
                obj.transform.parent = fruitsParents.transform;
                obj.gameObject.SetActive(false);
                fruitPools.Add(obj);
            }
        }

        public Fruits GetObjectFromPool(Vector3 position, Quaternion rotation)
        {
            foreach (Fruits obj in fruitPools)
            {
                if (!obj.gameObject.activeInHierarchy)
                {
                    obj.transform.position = position;
                    obj.transform.rotation = rotation;
                    obj.gameObject.SetActive(true);
                    return obj;
                }
            }

            // If all objects are currently in use, instantiate a new one
            Fruits newObj = Instantiate(fruits, position, rotation);
            fruitPools.Add(newObj);
            return newObj;
        }

        public Fruits GetFruitFromPoolNew(Vector3 position, Quaternion rotation)
        {
            for (int i = 0; i < fruitPools.Count; i++)
            {
                int indexToCheck = (lastDeactivatedFruitIndex + i + 1) % fruitPools.Count;
                Debug.Log(indexToCheck);
                Fruits fruit = fruitPools[indexToCheck];

                if (!fruit.gameObject.activeInHierarchy)
                {
                    fruit.transform.position = position;
                    fruit.transform.rotation = rotation;
                    fruit.gameObject.SetActive(true);
                    lastDeactivatedFruitIndex = indexToCheck;
                    return fruit;
                }
            }

            Fruits newObj = Instantiate(fruits, position, rotation);
            fruitPools.Add(newObj);
            lastDeactivatedFruitIndex = fruitPools.Count - 1;
            return newObj;
        }
    }
}