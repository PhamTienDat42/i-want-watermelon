using UnityEngine;

using System.Collections.Generic;
using GamePlay;
using Fruits;
using System.Collections;

namespace ObjectPools
{
    public class FruitPools : MonoBehaviour
    {
        [SerializeField] private List<Fruit> fruitPrefabs;
        [SerializeField] private int poolSize = 10;
        [SerializeField] private GameObject fruitsParents;

        private List<Fruit> fruitPools;
        private int lastDeactivatedFruitIndex = -1;

        [SerializeField] private GameController controller;
        [SerializeField] private GameView gameView;

        private void Awake()
        {
            InitializePool();
        }

        void InitializePool()
        {
            fruitPools = new List<Fruit>();

            for (int i = 0; i < poolSize; i++)
            {
                Fruit obj = Instantiate(GetRandomFruitPrefab());
                obj.InstantiateFruits(this, controller, gameView);
                obj.transform.parent = fruitsParents.transform;
                obj.gameObject.SetActive(false);
                fruitPools.Add(obj);
            }
        }

        public Fruit GetFruitFromPoolNew(Vector3 position)
        {
            for (int i = 0; i < fruitPools.Count; i++)
            {
                int indexToCheck = (lastDeactivatedFruitIndex + i + 1) % fruitPools.Count;
                Fruit fruit = fruitPools[indexToCheck];

                if (!fruit.gameObject.activeInHierarchy)
                {
                    fruit.transform.position = position;
                    fruit.gameObject.SetActive(true);
                    lastDeactivatedFruitIndex = indexToCheck;
                    return fruit;
                }
            }

            Fruit newObj = Instantiate(GetRandomFruitPrefab());
            fruitPools.Add(newObj);
            lastDeactivatedFruitIndex = fruitPools.Count - 1;
            return newObj;
        }

        //public void ReturnActiveFruitsAndScore()
        //{
        //    foreach (Fruit fruit in fruitPools)
        //    {
        //        if (fruit.gameObject.activeSelf)
        //        {
        //            controller.Score += fruit.FruitPoint;

        //            ReturnFruitToPool(fruit);
        //        }
        //    }

        //    gameView.SetScore();
        //}

        public IEnumerator ReturnActiveFruitsAndScoreWithDelay(float delay)
        {
            foreach (Fruit fruit in fruitPools)
            {
                if (fruit.gameObject.activeSelf)
                {
                    fruit.Rb.bodyType = RigidbodyType2D.Static;
                }
            }

            foreach (Fruit fruit in fruitPools)
            {
                if (fruit.gameObject.activeSelf)
                {
                    controller.Score += fruit.FruitPoint;
                    gameView.SetScore();
                    yield return new WaitForSeconds(delay);
                    ReturnFruitToPool(fruit);
                }
            }
        }

        public void ReturnFruitToPool(Fruit fruit)
        {
            fruit.gameObject.SetActive(false);
        }

        private Fruit GetRandomFruitPrefab()
        {
            return fruitPrefabs[UnityEngine.Random.Range(0, 5)];
        }
    }
}