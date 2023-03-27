using System;
using GLIFramework.Scripts.Enums;
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
        /// Reference to the Animation Manager for this AI Unit
        /// </summary>
        [field: SerializeField, Tooltip("Reference to the AI NavMeshAgent")]
        public AnimationManager AiAgentAnimationManager { get; private set; } = null;

        /// <summary>
        /// Reference to the SpawnManager singleton
        /// </summary>
        private SpawnManager _spawnManager;
        /// <summary>
        /// Counter to determine what waypoint the AI is at
        /// </summary>
        private int _currentWayPointIndex = 0;
        /// <summary>
        /// Total number of waypoints, excluding the starting waypoint
        /// </summary>
        private static int _totalWayPointCount = 0;
        /// <summary>
        /// Reference to the next destination transform
        /// </summary>
        private Transform _nextDestination = null;

        public void MoveAIToNextDest()
        {
            //Keep track of which waypoint the AI bot is at locally
            _currentWayPointIndex++;
            AiAgent.isStopped = false;

            // //If the current waypoint is out of the index bounds, I.E it is past the last point, we'll set it to the end point, else it goes to the next waypoint in the list
            _nextDestination = _currentWayPointIndex >= _totalWayPointCount ? _spawnManager.EndPoint 
                : _spawnManager.WayPointTransforms[_currentWayPointIndex];
            
            AiAgent.destination = _nextDestination.position;
            AiAgentAnimationManager.ChangeAnimationState(AIAnims.Running);
            Debug.Log($"Next Destination is: {_nextDestination.name}");
        }
        
        private void Update()
        {
            //When agent is close to the stopping point, set the isStopped variable to true
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

            if (!_spawnManager)
            {
                Debug.LogError("Spawn Manager not found :: MoveAIToEnd.cs");
                return;
            }
            
            //At the start, lets get the total number of way points
            _totalWayPointCount = _spawnManager.WayPointTransforms.Length;
            //Set starting waypoint
            _nextDestination = _spawnManager.WayPointTransforms[_currentWayPointIndex];

            Debug.Log($"Next Destination is: {_nextDestination.name}");
            AiAgent.destination = _nextDestination.position;
        }
    }
}
