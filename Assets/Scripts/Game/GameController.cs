using System.Collections;
using System.Collections.Generic;
using ObjectPools;
using UnityEngine;

namespace GamePlay
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private GameView gameView;
        [SerializeField] private GameObject parentFruits;
        [SerializeField] private List<Sprite> fruitSprites;
        [SerializeField] private FruitPools fruitPools;
        [SerializeField] private Camera mainCamera;
        [SerializeField] private GameObject boxBoundParent;

        private Fruits.Fruit nextFruit;
        private bool isClickable = true;
        private int score = 0;
        public int WaterMelonCount { get; set; }

        private float startTime;


        private readonly Vector3 startPos = new(0f, 4f,0f);

        private void Awake()
        {
            Application.targetFrameRate = 60;
        }

        private void Start()
        {
            isClickable = true;
            startTime = Time.time;
            nextFruit = fruitPools.GetFruitFromPoolNew(startPos);
            CreateScreenBoundsColliders(mainCamera, boxBoundParent);
        }

        private void Update()
        {
            //Timer
            float elapsedTime = Time.time - startTime;
            UpdateTimerText(elapsedTime);

            //Game
            DragFruits();
        }

        private void DragFruits()
        {
#if UNITY_EDITOR
            if (Input.GetMouseButtonDown(0) && isClickable == true)
            {
                var mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                var pos = new Vector3(mousePos.x, 4f, 0f);
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
            yield return new WaitForSeconds(1.5f);
            nextFruit = fruitPools.GetFruitFromPoolNew(startPos);
            isClickable = true;
        }

        public int RandomFruits()
        {
            return Random.Range(1, 10);
        }

        private void CreateScreenBoundsColliders(Camera mainCamera, GameObject parent)
        {
            float screenHeight = mainCamera.orthographicSize * 2f;
            float screenWidth = screenHeight * mainCamera.aspect;
            CreateSingleBoundCollider("Top", new Vector2(screenWidth, 0.01f), new Vector3(0f, screenHeight / 2f, 0f), parent);
            CreateSingleBoundCollider("Left", new Vector2(0.01f, screenHeight), new Vector3(-screenWidth / 2f, 0f, 0f), parent);
            CreateSingleBoundCollider("Right", new Vector2(0.01f, screenHeight), new Vector3(screenWidth / 2f, 0f, 0f), parent);
        }

        private void CreateSingleBoundCollider(string objName, Vector2 size, Vector3 localPos, GameObject parent)
        {
            GameObject newBound = new(objName);
            var collider = newBound.AddComponent<BoxCollider2D>();
            collider.size = size;
            newBound.transform.localPosition = localPos;
            newBound.transform.parent = parent.transform;
        }

        //Timer
        private void UpdateTimerText(float elapsedTime)
        {
            int hours = Mathf.FloorToInt(elapsedTime / 3600f);
            int minutes = Mathf.FloorToInt((elapsedTime % 3600f) / 60f);
            int seconds = Mathf.FloorToInt(elapsedTime % 60f);

            gameView.SetTimer(hours, minutes, seconds);
        }

        public List<Sprite> FruitSprites => fruitSprites;
        public int Score { get => score; set => score = value; }
    }
}