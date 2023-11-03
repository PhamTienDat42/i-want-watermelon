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

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(Constants.FruitTag))
            {
                isColliding = true;
                StartCoroutine(CheckCollisionDuration());
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag(Constants.FruitTag))
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
                    controller.IsClickable = false;
                    StartCoroutine(fruitPools.ReturnActiveFruitsAndScoreWithDelay(0.25f));
                    yield return new WaitForSeconds(1.0f);
                    yield break;
                }
                yield return null;
            }
        }
    }
}