using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CurvyPath
{
    [System.Serializable]
    public class ObjectPoolItem
    {
        public GameObject objectToPoolChar012;
        public GameObject objectToPoolChar345;
        public GameObject objectToPoolChar789;
        public GameObject objectToPoolChar101112;
        public bool shouldExpand;
        public int amountToPool;
    }
    public class ObjectPooling : MonoBehaviour
    {
        public static ObjectPooling SharedInstance;
        public List<ObjectPoolItem> itemsToPool;
        public List<GameObject> pooledObjects;
        public static readonly string CURRENT_CHARACTER_KEY = "SGLIB_CURRENT_CHARACTER";
        public int currentCharacter;
        void Awake()
        {
            SharedInstance = this;
        }

        private void Start()
        {
           currentCharacter = PlayerPrefs.GetInt(CURRENT_CHARACTER_KEY);
            Debug.Log("current Charcter" + currentCharacter);
        }


        public void PoolingObjFunc()
        {
            pooledObjects = new List<GameObject>(); //list for pooled objects
            foreach (ObjectPoolItem item in itemsToPool)
            {
                for (int i = 0; i < item.amountToPool; i++)
                {

                    if (currentCharacter >= 0 && currentCharacter <= 3)
                    {
                        GameObject obj = Instantiate(item.objectToPoolChar012);
                        obj.SetActive(false);
                        pooledObjects.Add(obj);
                    }
                }
            }
        }
        public GameObject GetPooledObjectByTag(string tag)
        {
            for (int i = 0; i < pooledObjects.Count; i++)
            {
                if (pooledObjects[i] != null)
                {
                    if (!pooledObjects[i].activeInHierarchy && pooledObjects[i].tag == tag)
                    {
                        return pooledObjects[i];
                    }
                }
            }

            foreach (ObjectPoolItem item in itemsToPool)
            {
                if (item.objectToPoolChar012.tag == tag)
                {
                    if (item.shouldExpand)
                    {
                        if (currentCharacter >= 0 && currentCharacter <= 3)
                        {
                            GameObject obj = (GameObject)Instantiate(item.objectToPoolChar012);
                            obj.SetActive(true);
                            pooledObjects.Add(obj);
                            return obj;
                        }
                    }
                }
            }
            return null;
        }
    }
}