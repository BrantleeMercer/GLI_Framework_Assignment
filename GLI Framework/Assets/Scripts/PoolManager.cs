using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace GLIFramework.Scripts
{
    public class PoolManager : MonoBehaviour
    {
        /// <summary>
        /// Static reference to the spawn manager in the scene.
        /// Used to make a singleton object
        /// </summary>
        public static PoolManager Instance;
        
        /// <summary>
        /// Reference to the object that will be pooled
        /// </summary>
        [FormerlySerializedAs("objectToPool")] [field: SerializeField, Tooltip("Reference to the object that will be pooled"),Header("Object References")]
        public GameObject ObjectToPool;
        /// <summary>
        /// Reference to the object that will hold the pooled objects
        /// </summary>
        [field: SerializeField, Tooltip("Reference to the object that will hold the pooled objects")]
        public GameObject ObjectContainer { get; private set; } = null;
        /// <summary>
        /// Amount of the object to add to the list called pooledObjects above
        /// </summary>
        [field: SerializeField, Tooltip("Amount of the object to add to the list called pooledObjects above"), Header("Variables")]
        public int AmountToPool { get; private set; } = 1;
        
        /// <summary>
        /// List of pre-created pooled objects
        /// </summary>
        private List<GameObject> _pooledObjects;
        
        /// <summary>
        /// Helper method to get the poole object
        /// </summary>
        /// <returns>An instance of the pooled object</returns>
        public GameObject GetPooledObject()
        {
            Debug.Log("Get Pooled Object Called");
            for(int i = 0; i < AmountToPool; i++)
            {
                if(!_pooledObjects[i].activeInHierarchy)
                {
                    return _pooledObjects[i];
                }
            }
            Debug.LogError("Object to pool IS null :: PoolManager");
            return null;
        }

        private void GenerateListOfPrefabs()
        {
            //Pre generate list of Objects given a prefab
            _pooledObjects = new List<GameObject>();
            for(int i = 0; i < AmountToPool; i++)
            {
                var temp = Instantiate(ObjectToPool, ObjectContainer.transform, true);
                temp.SetActive(false);
                _pooledObjects.Add(temp);
            }
        }
        
        void Start()
        {
            GenerateListOfPrefabs();
        }
        
        /// <summary>
        /// Set up singleton instance of the PoolManager on awake
        /// </summary>
        private void Awake()
        {
            if (Instance != null && Instance != this) 
            { 
                Destroy(this); 
            } 
            else 
            { 
                Instance = this; 
            } 
        }
    }
}