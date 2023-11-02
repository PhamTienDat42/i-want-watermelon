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
        [SerializeField] private GameController controller;
        [SerializeField] private GameView gameView;

        private List<Fruit> fruitPools = new();
        private int lastDeactivatedFruitIndex = -1;
        private readonly float shakeForce = 5f;
        private readonly float shakeThreshold = 3f;

        private void Awake()
        {
            InitializePool();
        }

        private void InitializePool()
        {
            //fruitPools = new List<Fruit>();

            for (int i = 0; i < poolSize; i++)
            {
                Fruit obj = Instantiate(GetRandomFruitPrefab());
                obj.InstantiateFruits(this, controller, gameView);
                obj.transform.parent = fruitsParents.transform;
                obj.gameObject.SetActive(false);
                fruitPools.Add(obj);
            }
        }

        private void Update()
        {
            //ShakePhone
            if (Input.acceleration.sqrMagnitude >= shakeThreshold * shakeThreshold && controller.BoolShake == true)
            {
                StartCoroutine(ShakePhone());
            }
        }

        private IEnumerator ShakePhone()
        {
            controller.BoolShake = false;
            controller.NextFruit.gameObject.SetActive(false);
            ApplyShakeForce();
            yield return new WaitForSeconds(2.0f);
            controller.NextFruit.gameObject.SetActive(true);
            controller.ResetCountDownTime(180f);
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

            InitializePool();
            Fruit newFruit = fruitPools[(lastDeactivatedFruitIndex+1)%fruitPools.Count];
            newFruit.transform.position = position;
            newFruit.gameObject.SetActive(true);
            return newFruit;
        }

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
                    controller.Model.CurrentScore += fruit.FruitPoint;
                    gameView.SetScore();
                    yield return new WaitForSeconds(delay);
                    ReturnFruitToPool(fruit);
                }
            }
            var highScore = PlayerPrefs.GetInt(Constants.HighScore, 0);
            if (controller.Model.CurrentScore > highScore)
            {
                highScore = controller.Model.CurrentScore;
                controller.Model.SetHighScore(highScore);
            }
        }

        public void ReturnFruitToPool(Fruit fruit)
        {
            fruit.gameObject.SetActive(false);
        }

        public void ReturnFruitToPoolRandom(Fruit fruit)
        {
            var randomFruit = GetRandomFruitPrefab();
            fruit.SpriteRenderer.sprite = randomFruit.GetComponent<SpriteRenderer>().sprite;
            fruit.FruitPoint = randomFruit.FruitPoint;
            fruit.CircleCollider2D.radius = randomFruit.GetComponent<CircleCollider2D>().radius;
            fruit.gameObject.SetActive(false);
        }

        private Fruit GetRandomFruitPrefab()
        {
            return fruitPrefabs[UnityEngine.Random.Range(0, 5)];
        }

        //Shake Phone
        private void ApplyShakeForce()
        {
            Vector2 shakeDirection = Random.insideUnitCircle.normalized;
            Vector2 shakeForce = shakeDirection * this.shakeForce;
            var rigidList = new List<Rigidbody2D>();

            foreach (var fruit in fruitPools)
            {
                if (fruit.gameObject.activeSelf)
                {
                    rigidList.Add(fruit.Rb);
                }
            }

            foreach (var rb in rigidList)
            {
                rb.AddForce(shakeForce, ForceMode2D.Impulse);
            }
        }
    }
}