using UnityEngine;

namespace GamePlay
{
    public class Fruits : MonoBehaviour
    {
        [SerializeField] private int fruitPoint;

        private Rigidbody2D rb;
        private const string BGTag = "Background";

        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag(BGTag))
            {
                rb.velocity = Vector2.zero;
                rb.gravityScale = 0f;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Fruits otherFruit = other.gameObject.GetComponent<Fruits>();

            if (otherFruit != null)
            {
                if (CanCombine(otherFruit))
                {
                    CombineObjects(otherFruit);
                }
            }

            if (!other.isTrigger)
            {
                // D?ng l?i ho?c th?c hi?n các hành ??ng khác tùy thu?c vào yêu c?u c?a b?n
                Debug.Log("Stopped on the background");
                // ??t v?n t?c c?a Rigidbody v? 0 ?? ??i t??ng d?ng l?i
                if (rb != null)
                {
                    rb.velocity = Vector2.zero;
                    rb.gravityScale = 0f; // T?t tr?ng l?c n?u c?n
                }
            }
        }

        private bool CanCombine(Fruits otherFruit)
        {
            return fruitPoint == otherFruit.fruitPoint;
        }

        private void CombineObjects(Fruits otherFruit)
        {
            int newPointValue = fruitPoint + otherFruit.fruitPoint;
            Debug.Log($"Combined {fruitPoint} + {otherFruit.fruitPoint} = {newPointValue}");

            gameObject.SetActive(false);
            otherFruit.gameObject.SetActive(false);
        }
    }
}