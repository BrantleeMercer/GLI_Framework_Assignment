using GLIFramework.Scripts.Enums;
using TMPro;
using UnityEngine;

namespace GLIFramework.Scripts
{
    public enum LabelName //TODO: Make this a separate Enum inside of the enum folder
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
        public static UIManager Instance { get; private set; }
        
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
        /// Game object that holds the Game Over section of the UI
        /// </summary>
        [field: SerializeField, Tooltip("Game object that holds the Game Over section of the UI"), Header("Object References")]
        public GameObject GameOverUIContainer { get; private set; } = null;
        /// <summary>
        /// Game object that holds the Game Over message UI
        /// </summary>
        [field: SerializeField, Tooltip("Game object that holds the Game Over message UI")]
        public GameObject GameOverUIMessageContainer { get; private set; } = null;
        /// <summary>
        /// Game object that holds the You Win message UI
        /// </summary>
        [field: SerializeField, Tooltip("Game object that holds the You Win message UI")]
        public GameObject YouWinUIMessageContainer { get; private set; } = null;

        /// <summary>
        /// Time remaining for the game in seconds
        /// </summary>
        [field: SerializeField, Tooltip("Time remaining for the game"), Header("Variables")]
        public float TimeValue { get; private set; } = 600f;

        /// <summary>
        /// Method to update the different counts in the HUD
        /// </summary>
        /// <param name="label"><see cref="LabelName"/> of Score, Ammo, or Enemy</param>
        /// <param name="amount">Float amount to change the values (Use negative values to decrease amount)</param>
        public void UpdateCount(LabelName label)
        {
            switch (label)
            {
                case LabelName.Score:
                    GameManager.Instance.AddPointsToPlayer();
                    ScoreCountUGUI.text = GameManager.Instance.PlayerPoints.ToString();
                    break;
                case LabelName.Ammo:
                    GameManager.Instance.DecrementTotalAmmoCount();
                    AmmoCountUGUI.text = GameManager.Instance.TotalAmmoCount.ToString();
                    break;
                case LabelName.Time:
                    break;
                case LabelName.Enemy:
                    //Decrement the total bots remaining
                    GameManager.Instance.DecrementTotalBotCount();
                    EnemiesCountUGUI.text = GameManager.Instance.TotalBotCount.ToString();
                    break;
            }
        }

        /// <summary>
        /// Helper method to handle what to show at the end of the game session
        /// </summary>
        /// <param name="status"><see cref="GameStatus"/> Win or Loss</param>
        public void EndGameConditions(GameStatus status)
        {
            Cursor.visible = true;
            GameOverUIContainer.SetActive(true);
            if (status == GameStatus.Win)
                YouWinUIMessageContainer.SetActive(true);
            else 
                GameOverUIMessageContainer.SetActive(true);
        }

        /// <summary>
        /// Method used to set the default values of the score, ammo, enemies, and set the game over
        ///    container to hidden
        /// </summary>
        private void InitializeCounts()
        {
            Cursor.visible = false;
            ScoreCountUGUI.text = "0";
            AmmoCountUGUI.text = GameManager.Instance.TotalAmmoCount.ToString();
            EnemiesCountUGUI.text = GameManager.Instance.TotalBotCount.ToString();
            GameOverUIContainer.SetActive(false);
        }

        /// <summary>
        /// Method to update the timer 
        /// </summary>
        /// <param name="timeToDisplay"></param>
        private void UpdateGameTimer(float timeToDisplay)
        {
            if (timeToDisplay < 0)
            {
                timeToDisplay = 0;
                GameOverSequence(LabelName.Time);
            }
            
            float minutes = Mathf.FloorToInt(timeToDisplay / 60);
            float seconds = Mathf.FloorToInt(timeToDisplay % 60);

            TimeCountUGUI.text = $"{minutes:00}:{seconds:00}";
        }

        /// <summary>
        /// Method to call when a loss condition has been met
        /// </summary>
        /// <param name="cause">calling Enemy, Ammo, or Time to set the cause of the loss condition</param>
        private void GameOverSequence(LabelName cause)
        {
            Cursor.visible = true;
            
            switch (cause)
            {
                case LabelName.Ammo:
                    //anything special to satisfy the ammo game over goes here
                    break;
                case LabelName.Enemy:
                    //anything special to satisfy the enemy count game over goes here
                    break;
                case LabelName.Time:
                    //anything special to satisfy the time game over goes here
                    break;
                default:
                    return;
            }

            EndGameConditions(GameStatus.Loss);
        }

        private void Update()
        {
            if (TimeValue > 0)
            {
                TimeValue -= Time.deltaTime;
                UpdateGameTimer(TimeValue);
            }
            else
                TimeValue = 0;
            
        }

        private void Start()
        {
            InitializeCounts();
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