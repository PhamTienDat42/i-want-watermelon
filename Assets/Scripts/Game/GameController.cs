using System.Collections.Generic;
using ObjectPools;
using UnityEngine;

namespace GamePlay
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private ObjectPoolManager objectPoolManager;
        [SerializeField] private GameObject parentFruits;
        [SerializeField] private List<Sprite> fruitSprites;
        [SerializeField] private FruitPools fruitPools;
        [SerializeField] private Camera mainCamera;

        private bool isClickable = false;

        private void Start()
        {

        }

        private void Update()
        {
            DragFruits();
        }

        private void DragFruits()
        {
#if UNITY_EDITOR
            if (Input.GetMouseButtonDown(0)/* && isClickable == true*/)
            {
                var mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                var pos = new Vector3(mousePos.x, 5f, 0f);
                fruitPools.GetFruitFromPoolNew(pos, Quaternion.identity);
            }
#elif UNITY_ANDROID
            if(Input.touchCount > 0 && isClickable == true)
            {
                Touch touch = Input.GetTouch(0);

            }
#endif
        }

        public int RandomFruits()
        {
            return Random.Range(1, 10);
        }

        public List<Sprite> FruitSprites => fruitSprites;
        public ObjectPoolManager ObjectPoolManagerFruits => objectPoolManager;
    }
}