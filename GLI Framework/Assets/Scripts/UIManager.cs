using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace GLIFramework.Scripts
{
    public enum LabelName
    {
        Score,
        Ammo,
        Enemy,
        Time,
    }
    
    public class UIManager : MonoBehaviour
    {
        /// <summary>
        /// Set up for singleton instance of the UI Manager
        /// </summary>
        public UIManager Instance { get; private set; }
        
        /// <summary>
        /// TextMeshPro GUI that holds the score count
        /// </summary>
        [field: SerializeField, Tooltip("TextMeshPro GUI that holds the score count"), Header("TextMeshPro References")]
        public TextMeshProUGUI ScoreCountUGUI { get; private set; } = null;
        /// <summary>
        /// TextMeshPro GUI that holds the Ammo count
        /// </summary>
        [field: SerializeField, Tooltip("TextMeshPro GUI that holds the Ammo count")]
        public TextMeshProUGUI AmmoCountUGUI { get; private set; } = null;
        /// <summary>
        /// TextMeshPro GUI that holds the Enemy count
        /// </summary>
        [field: SerializeField, Tooltip("TextMeshPro GUI that holds the Enemy count")]
        public TextMeshProUGUI EnemiesCountUGUI { get; private set; } = null;
        /// <summary>
        /// TextMeshPro GUI that holds the Time remaining
        /// </summary>
        [field: SerializeField, Tooltip("TextMeshPro GUI that holds the Time remaining")]
        public TextMeshProUGUI TimeCountUGUI { get; private set; } = null;

        /// <summary>
        /// Time remaining for the game in seconds
        /// </summary>
        [field: SerializeField, Tooltip("Time remaining for the game"), Header("Variables")]
        public float TimeValue { get; private set; } = 600f;
        
        public void UpdateCount(LabelName label, float amount = 0)
        {
            switch (label)
            {
                case LabelName.Score:
                    ScoreCountUGUI.text = (int.Parse(ScoreCountUGUI.text) + amount).ToString();
                    break;
                case LabelName.Ammo:
                    AmmoCountUGUI.text = (int.Parse(AmmoCountUGUI.text) + amount).ToString();
                    break;
                case LabelName.Time:
                    break;
                case LabelName.Enemy:
                    EnemiesCountUGUI.text = (int.Parse(EnemiesCountUGUI.text) + amount).ToString();
                    break;
            }
        }

        private void InitializeCounts()
        {
            ScoreCountUGUI.text = "0";
            AmmoCountUGUI.text = "10";
            EnemiesCountUGUI.text = GameManager.Instance.TotalBotCount.ToString();
        }

        private void UpdateGameTimer(float timeToDisplay)
        {
            if (timeToDisplay < 0)
                timeToDisplay = 0;
            
            float minutes = Mathf.FloorToInt(timeToDisplay / 60);
            float seconds = Mathf.FloorToInt(timeToDisplay % 60);

            TimeCountUGUI.text = String.Format("{0:00}:{1:00}", minutes, seconds);
        }

        private void Update()
        {
            if (TimeValue > 0)
                TimeValue -= Time.deltaTime;
            else
                TimeValue = 0;
            
            UpdateGameTimer(TimeValue);
        }

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