﻿using ObjectPools;
using System;
using UnityEngine;

namespace GamePlay
{
    public class Fruits : MonoBehaviour
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
            Fruits otherFruit = collision.gameObject.GetComponent<Fruits>();

            if (otherFruit != null)
            {
                if (CanCombine(otherFruit))
                {
                    CombineObjects(otherFruit);
                }
            }
        }

        private bool CanCombine(Fruits otherFruit)
        {
            return fruitPoint == otherFruit.fruitPoint;
        }

        public void SpawnNewFruit(int newPoints, Vector3 pos, Vector2 velocity)
        {
            Fruits newFruit = fruitPools.GetFruitFromPoolNew(pos);
            newFruit.fruitPoint = newPoints;
            newFruit.GetComponent<SpriteRenderer>().sprite = controller.FruitSprites[(newPoints - 1) % 9];
            float spriteRadius = newFruit.GetComponent<SpriteRenderer>().sprite.bounds.size.x / 2f;
            newFruit.GetComponent<CircleCollider2D>().radius = spriteRadius;
            newFruit.GetComponent<Rigidbody2D>().velocity = velocity;
            Debug.Log(velocity);
            if (Math.Abs(velocity.y) < 0.5f && Math.Abs(velocity.x) < 0.1f)
            {
                newFruit.GetComponent<Rigidbody2D>().AddForce(Vector2.up, ForceMode2D.Impulse);
            }
            newFruit.gameObject.SetActive(true);
        }

        private void CombineObjects(Fruits otherFruit)
        {
            Fruits higherFruit = (transform.position.y > otherFruit.transform.position.y) ? this : otherFruit;
            velocity = (transform.position.y > otherFruit.transform.position.y) ? this.rb.velocity : otherFruit.gameObject.GetComponent<Rigidbody2D>().velocity;

            if (!gameObject.activeSelf && !otherFruit.gameObject.activeSelf)
            {
                return;
            }
            controller.Score += fruitPoint;
            gameView.SetScore();

            var newPoints = fruitPoint + 1;
            if(newPoints == MaxPoint)
            {
                controller.WaterMelonCount++;
                gameView.SetWatermelonCount();
                return;
            }

            gameObject.SetActive(false);
            otherFruit.gameObject.SetActive(false);

            SpawnNewFruit(newPoints, higherFruit.transform.position, velocity/2f);

            transform.localPosition = otherFruit.gameObject.transform.localPosition = Vector3.zero;
            transform.localRotation = otherFruit.gameObject.transform.localRotation = Quaternion.identity;
            rb.gravityScale = otherFruit.GetComponent<Rigidbody2D>().gravityScale = 1f;
        }
    }
}