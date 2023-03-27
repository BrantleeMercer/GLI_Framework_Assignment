using System.Collections;
using GLIFramework.Scripts;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

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
    /// List of waypoints for the AI bots to run through
    /// </summary>
    [field: SerializeField, Tooltip("List of waypoints for the AI bots to run through")]
    public Transform[] WayPointTransforms { get; private set; } = null;
    /// <summary>
    /// Reference to the end point object
    /// </summary>
    [field: SerializeField, Tooltip("Reference to the end point object")]
    public Transform EndPoint { get; private set; } = null;
    
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

    /// <summary>
    /// Method to use a button press to spawn a new AI bot
    /// </summary>
    private void OnSpawnAIKeyPressed(InputAction.CallbackContext context)
    {
        Debug.Log("Spawn AI Key Pressed!");
        var spawnedAgent = PoolManager.Instance.GetPooledObject();
        if (!spawnedAgent)
            return;
        
        Debug.Log("Setting Position");
        spawnedAgent.transform.position = StartPointTransform.position;
        spawnedAgent.SetActive(true);
        Debug.Log($"Turning on the object: {spawnedAgent.activeInHierarchy}");
    }
    
    /// <summary>
    /// Coroutine to play indefinably in scene if there is no button set up to spawn AI bots.
    ///  After set amount of seconds, pulled from the TimeTillNextBotSpawns variable, this spawns a
    /// new bot till the scene is destroyed. 
    /// </summary>
    /// <returns></returns>
    private IEnumerator AutoGenAIBots() //enable on death?
    {
        yield return new WaitForSeconds(2f);
        while (true)
        {
            var spawnedAgent = PoolManager.Instance.GetPooledObject();
            if (spawnedAgent)
            {
                Debug.Log("Setting Position");
                spawnedAgent.transform.position = StartPointTransform.position;
                spawnedAgent.SetActive(true);
                Debug.Log($"Turning on the object: {spawnedAgent.activeInHierarchy}");
            }
            
            
            yield return new WaitForSeconds(Random.Range(5f, 9f));
        }
    }

    private void OnEnable()
    {
        SpawnAIKeyReference.action.Enable();
        SpawnAIKeyReference.action.performed += OnSpawnAIKeyPressed;

        StartCoroutine(AutoGenAIBots());
    }

    private void OnDisable()
    {
        
        SpawnAIKeyReference.action.Disable();
        SpawnAIKeyReference.action.performed -= OnSpawnAIKeyPressed;
        
        StopCoroutine(AutoGenAIBots());
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
