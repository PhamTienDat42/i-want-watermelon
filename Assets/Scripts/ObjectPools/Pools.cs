using GamePlay;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectPools
{
    public class Pools<T> where T : MonoBehaviour
    {
        private List<T> pool = new List<T>();
        private T prefab;
        private Transform parentTransform;

        public Pools(T prefab, int initialSize, Transform parent = null)
        {
            this.prefab = prefab;
            this.parentTransform = parent;

            for (int i = 0; i < initialSize; i++)
            {
                CreateObject();
            }
        }

        private T CreateObject()
        {
            T obj = Object.Instantiate(prefab);
            obj.gameObject.SetActive(false);
            obj.transform.SetParent(parentTransform);
            pool.Add(obj);
            return obj;
        }

        public T GetObject()
        {
            foreach (T obj in pool)
            {
                if (!obj.gameObject.activeSelf)
                {
                    obj.gameObject.SetActive(true);
                    return obj;
                }
            }

            return CreateObject();
        }

        public void ReturnObject(T obj)
        {
            obj.gameObject.SetActive(false);
        }
    }
}