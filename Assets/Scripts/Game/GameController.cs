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

        private bool isClickable = true;
        private int score = 0;
        public int WaterMelonCount { get; set; }


        private void Start()
        {
            isClickable = true;
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
                var pos = new Vector3(mousePos.x, 5f, 0f);
                //fruitPools.GetFruitFromPoolNew(pos);
                StartCoroutine(Drag(pos));
            }
#elif UNITY_ANDROID
            if(Input.touchCount > 0 && isClickable == true)
            {
                Touch touch = Input.GetTouch(0);

            }
#endif
        }

        private IEnumerator Drag(Vector3 pos)
        {
            isClickable = false;
            fruitPools.GetFruitFromPoolNew(pos);
            yield return new WaitForSeconds(1.5f);
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