using UnityEngine;
using UnityEngine.AI;

namespace GLIFramework.Scripts
{
    public class MoveAIToEnd : MonoBehaviour
    {
        /// <summary>
        /// Reference to the AI NavMeshAgent
        /// </summary>
        [field: SerializeField, Tooltip("Reference to the AI NavMeshAgent")]
        public NavMeshAgent AiAgent { get; private set; } = null;

        /// <summary>
        /// Reference to the SpawnManager singleton
        /// </summary>
        private SpawnManager _spawnManager;

        private void Update()
        {
            if (AiAgent.remainingDistance < 0.2f)
            {
                AiAgent.isStopped = true;
            }
        }

        /// <summary>
        /// Start moving the AI from the starting position to the ending position.  This is done in the
        /// OnEnable function due to object pooling.  When the bot is set to active in the hierarchy, that is
        /// when the ending position will be set.
        /// </summary>
        private void OnEnable()
        {
            Debug.Log("MoveAIToEnd on Enable is called");
            _spawnManager = SpawnManager.Instance;
            
            if (_spawnManager == null)
                Debug.LogError("Spawn Manager not found :: MoveAIToEnd.cs");
            
            Debug.Log($"MoveAIToEnd End point is at location: {_spawnManager.EndPoint.position}\nAI Location is : {this.transform.position}");
            AiAgent.destination = _spawnManager.EndPoint.position;
            AiAgent.isStopped = false;
            Debug.Log($"MoveAIToEnd remainging distance is: {AiAgent.remainingDistance}");
        }
    }
}
