using GLIFramework.Scripts;
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
    /// Reference to the end point object
    /// </summary>
    [field: SerializeField, Tooltip("Reference to the end point object")]
    public Transform EndPoint { get; private set; } = null;
    
    /// <summary>
    /// Reference to the Input Action to spawn an AI key
    /// </summary>
    [field: SerializeField, Tooltip("Reference to the Input Action to spawn an AI key"),
            Header("Input Action References")]
    public InputActionReference SpawnAIKeyReference { get; private set; } = null;

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

    private void OnEnable()
    {
        SpawnAIKeyReference.action.Enable();
        SpawnAIKeyReference.action.performed += OnSpawnAIKeyPressed;
    }
    private void OnDisable()
    {
        SpawnAIKeyReference.action.Disable();
        SpawnAIKeyReference.action.performed -= OnSpawnAIKeyPressed;
    }

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
