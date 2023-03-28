using System;
using System.Collections;
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
        /// Reference to the AIs Current State
        /// </summary>
        [field: SerializeField, Tooltip("Reference to the AIs Current State")]
        public AIStates CurrentState { get; set; } = AIStates.Run;
        /// <summary>
        /// Reference to the AIs Current Health
        /// </summary>
        [field: SerializeField, Tooltip("Reference to the AIs Current Health")]
        public int Health { get; private set; } = 10;
        /// <summary>
        /// Seconds until the death animation is done and we want to return the object to the object pool
        /// </summary>
        [field: SerializeField, Tooltip("Seconds until the death animation is done and we want to return the object to the object pool")]
        public float SecondsTillDestroyingBot { get; private set; } = 3f;

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
        /// <summary>
        /// Reference to the AIs Max Health
        /// </summary>
        private const int MAX_HEALTH = 10;

        /// <summary>
        /// Method that sets the the next waypoint location after the bot has reached its
        /// current destination
        /// </summary>
        public void MoveAIToNextDest()
        {
            //Keep track of which waypoint the AI bot is at locally
            _currentWayPointIndex++;
            AiAgent.isStopped = false;
            CurrentState = AIStates.Run;

            // //If the current waypoint is out of the index bounds, I.E it is past the last point, we'll set it to the end point, else it goes to the next waypoint in the list
            _nextDestination = _currentWayPointIndex >= _totalWayPointCount ? _spawnManager.EndPoint 
                : _spawnManager.WayPointTransforms[_currentWayPointIndex];
            
            AiAgent.destination = _nextDestination.position;
            AiAgentAnimationManager.ChangeAnimationState(AIAnims.Running);
        }

        /// <summary>
        /// Coroutine to call when bot health has fallen to or below 0
        /// </summary>
        private IEnumerator AIBotHasDied()
        {
            CurrentState = AIStates.Death;
            AiAgent.isStopped = true;
            AiAgentAnimationManager.ChangeAnimationState(AIAnims.Death);

            yield return new WaitForSeconds(SecondsTillDestroyingBot);
            //Add the 50 points to the player total
            GameManager.Instance.PlayerPoints += 50;
            //Decrement the total bots remaining
            GameManager.Instance.DecrementTotalBotCount();
            gameObject.SetActive(false);
            
            
        }

        /// <summary>
        /// Helper function to set the amount of damage a bot has taken
        /// </summary>
        /// <param name="damageAmount">integer amount to decrease the bots health by</param>
        public void DamageAIBot(int damageAmount)
        {
            Health -= damageAmount;
        }
        
        private void Update()
        {
            //When agent is close to the stopping point, set the isStopped variable to true
            if (AiAgent.remainingDistance < 0.2f)
            {
                AiAgent.isStopped = true;
            }

            //Call death routine when bot has no health left
            if (Health <= 0)
                StartCoroutine(AIBotHasDied());
        }

        /// <summary>
        /// Start moving the AI from the starting position to the ending position.  This is done in the
        /// OnEnable function due to object pooling.  When the bot is set to active in the hierarchy, that is
        /// when the ending position will be set.
        /// </summary>
        private void OnEnable()
        {
            Health = MAX_HEALTH;
            CurrentState = AIStates.Run;
            _currentWayPointIndex = 0;
            
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

            AiAgent.destination = _nextDestination.position;
        }
    }
}
