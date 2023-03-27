using System;
using System.Collections;
using GLIFramework.Scripts.Enums;
using UnityEngine;
using UnityEngine.AI;

namespace GLIFramework.Scripts
{
    
    public class WayPointBehavior : MonoBehaviour
    {
        /// <summary>
        /// Number of seconds to wait until resetting the animation from hide to run
        /// </summary>
        [field: SerializeField, Tooltip("Number of seconds to wait until resetting the animation from hide to run")]
        public float NumberOfSecondsToWaitTillMoving { get; private set; } = 2f;

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
            if(!ShouldHide)
                yield break;
            
            //After getting to the way point, set the animation to cover idle for hiding
            _agentMove.AiAgentAnimationManager.ChangeAnimationState(AIAnims.Cover_idle);
            
            yield return new WaitForSeconds(NumberOfSecondsToWaitTillMoving);
            
            //After the set amount of seconds, move the AI bot again and set the animation to running
            _agentMove.AiAgent.isStopped = false;
            _agentMove.MoveAIToNextDest();
        }
    }    
}

