using System.Collections;
using System.Collections.Generic;

using ObjectPools;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace GamePlay
{
    public class GameController : MonoBehaviour
    {
        [Header("Game")]
        [SerializeField] private GameView gameView;
        [SerializeField] private FruitPools fruitPools;
        [SerializeField] private Camera mainCamera;

        [Header("Data")]
        [SerializeField] private List<Sprite> fruitSprites;
        [SerializeField] private GameObject parentFruits;

        [Space(8.0f)]
        [Header("Bound-Collider2D")]
        [SerializeField] private BoxCollider2D topCollider;
        [SerializeField] private BoxCollider2D leftCollider;
        [SerializeField] private BoxCollider2D rightCollider;

        private GameModel model;
        private bool isClickable;

        //fruit
        private Fruits.Fruit nextFruit;
        private float startTime;
        private Vector3 startPos;

        //countdownshake
        private float countdownTime = 10f;
        private bool boolShake = false;

        private void Awake()
        {
            Application.targetFrameRate = 60;

            var modelObj = GameObject.FindGameObjectWithTag(Constants.DataTag);
            if (modelObj != null)
            {
                model = modelObj.GetComponent<GameModel>();
            }
            else
            {
                SceneManager.LoadScene(Constants.MenuScene);
            }
        }

        private void Start()
        {
            isClickable = true;
            startTime = Time.time;

            var startY = mainCamera.orthographicSize - fruitSprites[^1].bounds.size.x / 2f;
            startPos = new Vector3(0f, startY, 0f);
            nextFruit = fruitPools.GetFruitFromPoolNew(startPos);
            SetBoxBound(mainCamera);
        }

        private void Update()
        {
            //Timer
            float elapsedTime = Time.time - startTime;
            UpdateTimerText(elapsedTime);
            CountDownToShake(countdownTime);

            //Game
            DragFruits();
        }

        private void DragFruits()
        {
#if UNITY_EDITOR
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject() && isClickable == true)
            {
                var halfScreenWidth = mainCamera.orthographicSize * mainCamera.aspect;
                var maxStartX = halfScreenWidth - nextFruit.GetComponent<CircleCollider2D>().bounds.size.x / 2f;
                var minStartX = -halfScreenWidth + nextFruit.GetComponent<CircleCollider2D>().bounds.size.x / 2f;
                var mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                var startY = mainCamera.orthographicSize - fruitSprites[^1].bounds.size.x / 2f;
                var posX = mousePos.x;
                if (mousePos.x > maxStartX)
                {
                    posX = maxStartX;
                }
                else if (mousePos.x < minStartX)
                {
                    posX = minStartX;
                }
                var pos = new Vector3(posX, startY, 0f);
                StartCoroutine(Drag(pos));
            }
#elif UNITY_ANDROID
            if (Input.touchCount > 0 && isClickable == true)
            {
                Touch touch = Input.GetTouch(0);
                if (!EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                {
                    if (touch.phase == TouchPhase.Began)
                    {
                        var halfScreenWidth = mainCamera.orthographicSize * mainCamera.aspect;
                        var maxStartX = halfScreenWidth - nextFruit.GetComponent<CircleCollider2D>().bounds.size.x / 2f;
                        var minStartX = -halfScreenWidth + nextFruit.GetComponent<CircleCollider2D>().bounds.size.x / 2f;
                        Vector3 touchPos = Camera.main.ScreenToWorldPoint(touch.position);
                        var startY = mainCamera.orthographicSize - fruitSprites[^1].bounds.size.x / 2f;
                        var posX = touchPos.x;
                        if (touchPos.x > maxStartX)
                        {
                            posX = maxStartX;
                        }
                        else if (touchPos.x < minStartX)
                        {
                            posX = minStartX;
                        }
                        var pos = new Vector3(touchPos.x, startY, 0f);
                        StartCoroutine(Drag(pos));
                    }
                }
            }
#endif
        }

        private IEnumerator Drag(Vector3 pos)
        {
            isClickable = false;
            nextFruit.gameObject.SetActive(false);
            nextFruit.transform.localPosition = pos;
            nextFruit.gameObject.SetActive(true);
            nextFruit.GetComponent<Rigidbody2D>().gravityScale = 1f;
            yield return new WaitForSeconds(1.5f);
            nextFruit = fruitPools.GetFruitFromPoolNew(startPos);
            isClickable = true;
        }

        private void SetBoxBound(Camera mainCamera)
        {
            float screenHeight = mainCamera.orthographicSize * 2f;
            float screenWidth = screenHeight * mainCamera.aspect;

            SetBoundPosition(topCollider, new Vector2(screenWidth, 0.01f), new Vector3(0f, screenHeight / 2f - fruitSprites[^1].bounds.size.x, 0f));
            SetBoundPosition(leftCollider, new Vector2(0.01f, screenHeight), new Vector3(-screenWidth / 2f, 0f, 0f));
            SetBoundPosition(rightCollider, new Vector2(0.01f, screenHeight), new Vector3(screenWidth / 2f, 0f, 0f));
        }

        private void SetBoundPosition(BoxCollider2D collider2D, Vector2 size, Vector3 localPos)
        {
            collider2D.size = size;
            collider2D.transform.localPosition = localPos;
        }

        //Timer
        private void UpdateTimerText(float elapsedTime)
        {
            int hours = Mathf.FloorToInt(elapsedTime / 3600f);
            int minutes = Mathf.FloorToInt((elapsedTime % 3600f) / 60f);
            int seconds = Mathf.FloorToInt(elapsedTime % 60f);

            gameView.SetTimer(hours, minutes, seconds);
        }

        private void CountDownToShake(float elapsedTime)
        {
            if (countdownTime > 0)
            {
                countdownTime -= Time.deltaTime;
                int minutes = Mathf.FloorToInt((elapsedTime % 3600f) / 60f);
                int seconds = Mathf.FloorToInt(elapsedTime % 60f);
                gameView.SetCountDownTimer(minutes, seconds);

                if (countdownTime <= 0 && !boolShake)
                {
                    boolShake = true;
                }
            }
        }

        public void ResetCountDownTime(float time)
        {
            countdownTime = time;
        }

        public List<Sprite> FruitSprites => fruitSprites;
        public bool IsClickable { get => isClickable; set => isClickable = value; }
        public Fruits.Fruit NextFruit => nextFruit;
        public GameModel Model => model;
        public bool BoolShake { get => boolShake; set => boolShake = value; }
    }
}