using System;
using System.Collections;
using GLIFramework.Scripts;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviour
{
    /// <summary>
    /// Static reference to the spawn manager in the scene.
    /// Used to make a singleton object
    /// </summary>
    public static SpawnManager Instance;
    
    /// <summary>
    /// Start Point for the spawn manager to spawn the pooled AI Agent
    /// </summary>
    [field: SerializeField, Tooltip("Start Point for the spawn manager to spawn the pooled AI Agent"),
            Header("Object References")]
    public Transform StartPointTransform { get; private set; } = null; 
    /// <summary>
    /// Reference to the end point object
    /// </summary>
    [field: SerializeField, Tooltip("Reference to the end point object")]
    public Transform EndPoint { get; private set; } = null;
    /// <summary>
    /// List of waypoints for the AI bots to run through
    /// </summary>
    [field: SerializeField, Tooltip("List of waypoints for the AI bots to run through")]
    public Transform[] WayPointTransforms { get; private set; } = null;
    /// <summary>
    /// Reference to the AIBot object pool manager
    /// </summary>
    [field: SerializeField, Tooltip("Reference to the AIBot object pool manager")]
    public PoolManager BotPoolManager { get; private set; } = null;
    
    /// <summary>
    /// Reference to the Input Action to spawn an AI key
    /// </summary>
    [field: SerializeField, Tooltip("Reference to the Input Action to spawn an AI key"),
            CanBeNull ,Header("Input Action References")]
    public InputActionReference SpawnAIKeyReference { get; private set; } = null;
    
    /// <summary>
    /// Seconds as a float till the next AI Bot is spawned at the start point
    /// </summary>
    [field: SerializeField, Tooltip("Seconds as a float till the next AI Bot is spawned at the start point")
            ,Header("Variables")]
    public float TimeTillNextBotSpawns { get; private set; } = 5f;

    private int _maxNumOfBots = 0;

    /// <summary>
    /// Coroutine to play in scene. After set amount of seconds, pulled from the TimeTillNextBotSpawns variable,
    /// this spawns a new bot till the scene is destroyed
    /// </summary>
    private IEnumerator AutoGenAIBots()
    {
        yield return new WaitForSeconds(2f);
        bool keepSpawning = true;
        int count = 0;
        while (keepSpawning)
        {
            var spawnedAgent = BotPoolManager.GetPooledObject();
            if (spawnedAgent)
            {
                spawnedAgent.transform.position = StartPointTransform.position;
                spawnedAgent.SetActive(true);

                count++;
                yield return new WaitForSeconds(Random.Range(5f, 9f));
            }
            
            //Stop spawning the AI Bots if there are more than the max number for this level
            if (count >= _maxNumOfBots)
            {
                keepSpawning = false;
            }
            
        }
    }

    private void OnEnable()
    {
        StartCoroutine(AutoGenAIBots());
    }

    private void OnDisable()
    {
        StopCoroutine(AutoGenAIBots());
    }

    private void Start()
    {
        _maxNumOfBots = GameManager.Instance.TotalBotCount;
    }

    /// <summary>
    /// Set up singleton instance of the spawn manager on awake
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
