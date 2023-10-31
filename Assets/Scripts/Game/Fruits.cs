using UnityEngine;

namespace GamePlay
{
    public class Fruits : MonoBehaviour
    {
        [SerializeField] private int fruitPoint;

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
            // T?o ra ??i t??ng m?i v?i giá tr? m?i (Ch?a implement logic t?o m?i ??i t??ng)
        }
    }
}