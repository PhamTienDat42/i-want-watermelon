using System.Collections;
using System.Collections.Generic;
using ObjectPools;
using UnityEngine;

namespace GamePlay
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private GameObject parentFruits;
        [SerializeField] private List<Sprite> fruitSprites;
        [SerializeField] private FruitPools fruitPools;
        [SerializeField] private Camera mainCamera;
        //[SerializeField] private SpriteRenderer nextFruitSprite;

        private Fruits nextFruit;
        private bool isClickable = true;
        private int score = 0;
        public int WaterMelonCount { get; set; }

        private readonly Vector3 startPos = new Vector3(0f, 4f,0f);

        private void Awake()
        {
            Application.targetFrameRate = 60;
        }

        private void Start()
        {
            isClickable = true;
            nextFruit = fruitPools.GetFruitFromPoolNew(startPos);
            //nextFruitSprite.sprite = nextFruit.GetComponent<SpriteRenderer>().sprite;
        }

        private void Update()
        {
            DragFruits();
        }

        private void DragFruits()
        {
#if UNITY_EDITOR
            if (Input.GetMouseButtonDown(0) && isClickable == true)
            {
                var mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                var pos = new Vector3(mousePos.x, 4f, 0f);
                //fruitPools.GetFruitFromPoolNew(pos);
                StartCoroutine(Drag(pos));
            }
#elif UNITY_ANDROID
            if (Input.touchCount > 0 && isClickable == true)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    Vector3 touchPos = Camera.main.ScreenToWorldPoint(touch.position);

                    var pos = new Vector3(touchPos.x, 4f, 0f);
                    StartCoroutine(Drag(pos));
                }
            }
#endif
        }

        private IEnumerator Drag(Vector3 pos)
        {
            isClickable = false;
            nextFruit.transform.localPosition = pos;
            nextFruit.GetComponent<Rigidbody2D>().gravityScale = 1f;
            //nextFruitSprite.sprite = nextFruit.GetComponent<SpriteRenderer>().sprite;
            yield return new WaitForSeconds(1.5f);
            nextFruit = fruitPools.GetFruitFromPoolNew(startPos);
            isClickable = true;
        }

        public int RandomFruits()
        {
            return Random.Range(1, 10);
        }

        public List<Sprite> FruitSprites => fruitSprites;
        public int Score { get => score; set => score = value; }
    }
}