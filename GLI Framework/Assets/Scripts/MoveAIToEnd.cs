using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace GLIFramework.Scripts
{
    public class MoveAIToEnd : MonoBehaviour
    {
        /// <summary>
        /// Reference to the start point object
        /// </summary>
        [field: SerializeField, Tooltip("Reference to the start point object"), Header("Object References")]
        public GameObject StartPoint { get; private set; } = null;
        /// <summary>
        /// Reference to the end point object
        /// </summary>
        [field: SerializeField, Tooltip("Reference to the end point object")]
        public GameObject EndPoint { get; private set; } = null;
        /// <summary>
        /// Reference to the AI NavMeshAgent
        /// </summary>
        [field: SerializeField, Tooltip("Reference to the AI NavMeshAgent")]
        public NavMeshAgent AiAgent { get; private set; } = null;
        
        // Start is called before the first frame update
        void Start()
        {
            AiAgent.destination = EndPoint.transform.position;
        }
    }
}
