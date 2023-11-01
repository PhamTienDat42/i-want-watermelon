using UnityEngine;

using System.Collections.Generic;
using GamePlay;
using System;

namespace ObjectPools
{
    public class FruitPools : MonoBehaviour
    {
        [SerializeField] private List<Fruits> fruitPrefabs;
        [SerializeField] private Fruits fruits;
        [SerializeField] private int poolSize = 10;
        [SerializeField] private GameObject fruitsParents;

        private List<Fruits> fruitPools;
        private int lastDeactivatedFruitIndex = -1;

        [SerializeField] private GameController controller;
        [SerializeField] private GameView gameView;

        private void Awake()
        {
            InitializePool();
        }

        void InitializePool()
        {
            fruitPools = new List<Fruits>();

            for (int i = 0; i < poolSize; i++)
            {
                Fruits obj = Instantiate(GetRandomFruitPrefab());
                obj.InstantiateFruits(this, controller, gameView);
                obj.transform.parent = fruitsParents.transform;
                obj.gameObject.SetActive(false);
                fruitPools.Add(obj);
            }
        }

        public Fruits GetFruitFromPoolNew(Vector3 position)
        {
            for (int i = 0; i < fruitPools.Count; i++)
            {
                int indexToCheck = (lastDeactivatedFruitIndex + i + 1) % fruitPools.Count;
                Fruits fruit = fruitPools[indexToCheck];

                if (!fruit.gameObject.activeInHierarchy)
                {
                    fruit.transform.position = position;
                    fruit.gameObject.SetActive(true);
                    lastDeactivatedFruitIndex = indexToCheck;
                    return fruit;
                }
            }

            Fruits newObj = Instantiate(fruits);
            fruitPools.Add(newObj);
            lastDeactivatedFruitIndex = fruitPools.Count - 1;
            return newObj;
        }

        private Fruits GetRandomFruitPrefab()
        {
            return fruitPrefabs[UnityEngine.Random.Range(0, 6)];
        }
    }
}