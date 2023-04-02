using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GLIFramework.Scripts
{
    public class ExplosionBehavior : MonoBehaviour
    {
        /// <summary>
        /// Reference to the capsule collider for the explosion
        /// </summary>
        [field: SerializeField, Tooltip("Reference to the capsule collider for the explosion"), Header("Object References")]
        public CapsuleCollider ExplosionCapsuleCollider { get; private set; } = null;
        /// <summary>
        /// Integer amount of damage the barrel will do to the AI bots
        /// </summary>
        [field: SerializeField, Tooltip("Integer amount of damage the barrel will do to the AI bots"), Header("Variables")]
        public int ExplosionDamageAmount { get; private set; } = 500;
        /// <summary>
        /// Float amount of seconds to wait to turn off the explosion collider
        /// </summary>
        [field: SerializeField, Tooltip("Float amount of seconds to wait to turn off the explosion collider")]
        public float SecondsUntilTriggerIsOff { get; private set; } = 0.2f;

        private void OnTriggerEnter(Collider other)
        {
            if(!other.tag.Equals("AI"))
                return;
            
            other.GetComponent<MoveAIToEnd>().DamageAIBot(ExplosionDamageAmount);
        }

        private IEnumerator DamageAndDestroyExplosion()
        {
            //After a short time, turn the collider off as to not have wondering bots destroyed for no reason
            yield return new WaitForSeconds(SecondsUntilTriggerIsOff);
            ExplosionCapsuleCollider.enabled = false;
        }
        
        private void Start()
        {
            StartCoroutine(DamageAndDestroyExplosion());
        }
    }
}

