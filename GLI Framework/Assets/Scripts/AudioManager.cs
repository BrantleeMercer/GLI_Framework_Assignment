using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GLIFramework.Scripts
{
    public enum Sounds
    {
        Gunfire,
        BarrierHit,
        AIBotDeath,
        AIBotDone,
    }
    
    public class AudioManager : MonoBehaviour
    {
        ///<summary>
        /// Set up for the Auido manger singleton instance
        /// </summary>
        public static AudioManager Instance { get; private set; }
        
        /// <summary>
        /// References to the main Audio Source
        /// </summary>
        [field: SerializeField, Tooltip("References to the main Audio Source"), Header("Audio Source")]
        public AudioSource MAudioSource { get; private set; } = null;
        /// <summary>
        /// Audio clip for shooting the gun
        /// </summary>
        [field: SerializeField, Tooltip("Audio clip for shooting the gun"), Header("Audio Clips")]
        public AudioClip WeaponFire { get; private set; } = null;
        /// <summary>
        /// Audio clip for shooting the barrier
        /// </summary>
        [field: SerializeField, Tooltip("Audio clip for shooting the barrier")]
        public AudioClip BarrierShot { get; private set; } = null;
        /// <summary>
        /// Audio Background music
        /// </summary>
        [field: SerializeField, Tooltip("Audio Background music")]
        public AudioClip BackgroundMusic { get; private set; } = null;
        /// <summary>
        /// Audio clip for bot death
        /// </summary>
        [field: SerializeField, Tooltip("Audio clip for bot death")]
        public AudioClip AIBotDeath { get; private set; } = null;
        /// <summary>
        /// Audio clip for bot who has reached the finish line
        /// </summary>
        [field: SerializeField, Tooltip("Audio clip for bot who has reached the finish line")]
        public AudioClip AIBotRunCompleted { get; private set; } = null;
        
        
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
}

