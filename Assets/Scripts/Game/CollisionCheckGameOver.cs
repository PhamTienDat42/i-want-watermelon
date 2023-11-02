using ObjectPools;
using UnityEngine;

namespace GamePlay
{
    public class CollisionCheckGameOver : MonoBehaviour
    {
        [SerializeField] private GameController controller;
        [SerializeField] private FruitPools fruitPools;
        private bool isColliding = false;
        private readonly float collisionDuration = 3f;

        private const string Fruit = "Fruit";

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(Fruit))
            {
                isColliding = true;
                StartCoroutine(CheckCollisionDuration());
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag(Fruit))
            {
                isColliding = false;
            }
        }

        private System.Collections.IEnumerator CheckCollisionDuration()
        {
            float startTime = Time.time;

            while (isColliding)
            {
                if (Time.time - startTime > collisionDuration)
                {
                    controller.NextFruit.gameObject.SetActive(false);
                    StartCoroutine(fruitPools.ReturnActiveFruitsAndScoreWithDelay(0.25f));
                    controller.IsClickable = false;
                    yield break;
                }
                yield return null;
            }
        }
    }
}