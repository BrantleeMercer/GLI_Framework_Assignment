using System;
using System.Collections;
using GLIFramework.Scripts.Enums;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace GLIFramework.Scripts
{
    
    public class WayPointBehavior : MonoBehaviour
    {
        /// <summary>
        /// Bool if the waypoint is a cover position or not
        /// </summary>
        [field: SerializeField, Tooltip("Bool if the waypoint is a cover position or not")]
        public bool ShouldHide { get; private set; } = true;
        
        private MoveAIToEnd _agentMove;
        
        private void OnTriggerEnter(Collider other)
        {
            Debug.Log($"Collier of object tag is: {other.tag}");
            if (!other.tag.Equals("AI"))
                return;
            
            _agentMove = other.gameObject.GetComponent<MoveAIToEnd>();
            if (!_agentMove)
            {
                Debug.LogError("Didn't find the MoveAIToEnd component :: WayPointBehavior");
                return;
            }
                
            StartCoroutine(HideAndMove());
        }
        
        private IEnumerator HideAndMove()
        {
            //Used for non cover waypoints
            if (!ShouldHide)
            {
                _agentMove.MoveAIToNextDest();
                yield break;
            }
            
            //After getting to the way point, set the animation to cover idle for hiding
            _agentMove.AiAgentAnimationManager.ChangeAnimationState(AIAnims.Cover_idle);
            _agentMove.CurrentState = AIStates.Hide;

            var randomAmountOfTimeToHide = Random.Range(1f, 4f);
            
            yield return new WaitForSeconds(randomAmountOfTimeToHide);
            
            //After the set amount of seconds, move the AI bot again and set the animation to running
            _agentMove.AiAgent.isStopped = false;
            _agentMove.MoveAIToNextDest();
        }
    }    
}

