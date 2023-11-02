using GamePlay;
using ObjectPools;
using System;
using UnityEngine;

namespace Fruits
{
    public class Fruit : MonoBehaviour
    {
        [SerializeField] private int fruitPoint;
        private GameController controller;
        private GameView gameView;
        private SpriteRenderer spriteRenderer;
        private new CircleCollider2D collider;
        private Rigidbody2D rb;
        private Vector2 velocity;

        private FruitPools fruitPools;

        private const int MaxPoint = 10;

        private void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            rb = GetComponent<Rigidbody2D>();
            collider = GetComponent<CircleCollider2D>();
        }

        public void InstantiateFruits(FruitPools fruitPools,GameController gameController, GameView gameView)
        {
            this.fruitPools = fruitPools;
            this.controller = gameController;
            this.gameView = gameView;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            Fruit otherFruit = collision.gameObject.GetComponent<Fruit>();

            if (otherFruit != null)
            {
                if (CanCombine(otherFruit))
                {
                    CombineObjects(otherFruit);
                }
            }
        }

        private bool CanCombine(Fruit otherFruit)
        {
            return fruitPoint == otherFruit.fruitPoint;
        }

        public void SpawnNewFruit(int newPoints, Vector3 pos, Vector2 velocity)
        {
            Fruit newFruit = fruitPools.GetFruitFromPoolNew(pos);
            newFruit.fruitPoint = newPoints;
            newFruit.GetComponent<SpriteRenderer>().sprite = controller.FruitSprites[(newPoints - 1) % 9];
            float spriteRadius = newFruit.GetComponent<SpriteRenderer>().sprite.bounds.size.x / 2f;
            newFruit.GetComponent<CircleCollider2D>().radius = spriteRadius;
            newFruit.GetComponent<Rigidbody2D>().velocity = velocity;
            newFruit.GetComponent<Rigidbody2D>().gravityScale = 1.0f;
            if (Math.Abs(velocity.y) < 0.5f && Math.Abs(velocity.x) < 0.1f)
            {
                newFruit.GetComponent<Rigidbody2D>().AddForce(Vector2.up, ForceMode2D.Impulse);
            }
            newFruit.gameObject.SetActive(true);
        }

        private void CombineObjects(Fruit otherFruit)
        {
            Fruit higherFruit = (transform.position.y > otherFruit.transform.position.y) ? this : otherFruit;
            velocity = (transform.position.y > otherFruit.transform.position.y) ? this.rb.velocity : otherFruit.gameObject.GetComponent<Rigidbody2D>().velocity;

            if (!gameObject.activeSelf && !otherFruit.gameObject.activeSelf)
            {
                return;
            }
            controller.Model.CurrentScore += fruitPoint;
            var highScore = PlayerPrefs.GetInt(Constants.HighScore, 0);
            if(controller.Model.CurrentScore > highScore)
            {
                highScore = controller.Model.CurrentScore;
                controller.Model.SetHighScore(highScore);
            }
            gameView.SetScore();

            var newPoints = fruitPoint + 1;

            gameObject.SetActive(false);
            otherFruit.gameObject.SetActive(false);

            if (newPoints == MaxPoint)
            {
                //controller.Model.WatermelonCount++;
                //PlayerPrefs.SetInt(Constants.WatermelonCount, controller.Model.WatermelonCount);
                controller.Model.SetWatermelonCount();
                gameView.SetWatermelonCount();
            }
            else
            {
                SpawnNewFruit(newPoints, higherFruit.transform.position, velocity / 2f);
            }

            transform.localPosition = otherFruit.gameObject.transform.localPosition = Vector3.zero;
            transform.localRotation = otherFruit.gameObject.transform.localRotation = Quaternion.identity;
            rb.gravityScale = otherFruit.GetComponent<Rigidbody2D>().gravityScale = 0.0f;
        }

        public int FruitPoint { get => fruitPoint; set => fruitPoint = value; }
        public Rigidbody2D Rb => rb;
    }
}