using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace GLIFramework.Scripts
{
    public class GameManager : MonoBehaviour
    {
        /// <summary>
        /// Singleton Setup for the game manager class
        /// </summary>
        public static GameManager Instance { get; private set; }
        /// <summary>
        /// Game Manager to keep track of the players score
        /// </summary>
        public int PlayerPoints { get; set; } = 0;
        
        /// <summary>
        /// Total Number of bots that will be in the scene.  Used to track for win/lose condition
        /// </summary>
        [field: SerializeField, Tooltip("Total Number of bots that will be in the scene.  Used to track for win/lose condition"), Header("Variables")]
        public int TotalBotCount { get; private set; } = 5;

        private void ShieldsDown(GameObject barrierObject)
        {
            StartCoroutine(RespawnTheShields(barrierObject));
        }

        public IEnumerator RespawnTheShields(GameObject barrierPrefab)
        {
            var barrier = barrierPrefab.GetComponent<BarrierBehavior>();
            if(!barrier)
                Debug.LogError("Didn't get the barrier behavior object");
            
            yield return new WaitForSeconds(3f);
            barrierPrefab.SetActive(true);
                
            while (barrier.CurrentForceFieldCharge < BarrierBehavior.MAX_CHARGE)
            {
                barrier.RechargeBarrier(1);
                yield return new WaitForSeconds(0.3f);
            }
        }

        public void DecrementTotalBotCount()
        {
            TotalBotCount--;
        }

        private void OnEnable()
        {
            BarrierBehavior.OnBarrierBroken += ShieldsDown;
        }
        
        private void OnDisable()
        {
            BarrierBehavior.OnBarrierBroken -= ShieldsDown;
        }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
                Instance = this;
        }
    }
}
