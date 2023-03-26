using System;
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
            if (AiAgent.remainingDistance < 0.5f)
            {
                AiAgent.isStopped = true;
            }
        }

        private void OnEnable()
        {
            Debug.Log("MoveAIToEnd on Enable is called");
            _spawnManager = SpawnManager.Instance;
            
            if (_spawnManager == null)
                Debug.LogError("Spawn Manager not found :: MoveAIToEnd.cs");
            AiAgent.destination = _spawnManager.EndPoint.position;
        }
    }
}