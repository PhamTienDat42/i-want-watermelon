using ObjectPools;
using UnityEngine;

namespace GamePlay
{
    public class Fruits : MonoBehaviour
    {
        [SerializeField] private int fruitPoint;
        private GameController controller;
        private FruitPools fruitPools;
        private SpriteRenderer spriteRenderer;
        private Rigidbody2D rb;
        private const string BGTag = "Background";

        void Start()
        {
            var controllerObj = GameObject.FindGameObjectWithTag("GameController");
            controller = controllerObj.GetComponent<GameController>();
            var fruitPoolobj = GameObject.FindGameObjectWithTag("FruitPools");
            fruitPools = fruitPoolobj.GetComponent<FruitPools>();

            spriteRenderer = GetComponent<SpriteRenderer>();
            rb = GetComponent<Rigidbody2D>();
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

        void SpawnNewFruit(int newPoints, Vector3 pos)
        {
            Fruits newFruit = fruitPools.GetFruitFromPoolNew(pos, transform.rotation);
            newFruit.fruitPoint = newPoints;
            newFruit.GetComponent<SpriteRenderer>().sprite = controller.FruitSprites[(newPoints-1)%9];
            newFruit.gameObject.SetActive(true);
        }

        private void CombineObjects(Fruits otherFruit)
        {
            Fruits higherFruit = (transform.position.y > otherFruit.transform.position.y) ? this : otherFruit;

            if (!gameObject.activeSelf && !otherFruit.gameObject.activeSelf)
            {
                return;
            }
            var newPoitnts = fruitPoint + 1;
            gameObject.SetActive(false);
            otherFruit.gameObject.SetActive(false);
            SpawnNewFruit(newPoitnts, higherFruit.transform.position);
            if (rb != null)
            {
                Vector2 forceDirection = (transform.position - higherFruit.transform.position).normalized;
                rb.AddForce(forceDirection*3f, ForceMode2D.Impulse);
            }
        }
    }
}