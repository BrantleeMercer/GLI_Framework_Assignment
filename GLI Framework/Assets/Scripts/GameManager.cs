using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace GLIFramework.Scripts
{
    public class GameManager : MonoBehaviour
    {
        /// <summary>
        /// Singleton Setup for the game manager class
        /// </summary>
        public static GameManager Instance { get; private set; }

        /// <summary>
        /// Reference to the input action map used to quit the game  I.E Escape Key
        /// </summary>
        [field: SerializeField, Tooltip("Reference to the input action map used to quit the game  I.E Escape Key"),Header("Input References")]
        private InputActionReference QuitButtonInputReference { get; set; } = null;

        /// <summary>
        /// Total Number of bots that will be in the scene.  Used to track for win/lose condition
        /// </summary>
        [field: SerializeField, Tooltip("Total Number of bots that will be in the scene.  Used to track for win/lose condition"), Header("Variables")]
        public int TotalBotCount { get; private set; } = 5;
        /// <summary>
        /// Game Manager to keep track of the players score
        /// </summary>
        public int PlayerPoints { get; set; } = 0; //Not being visible in the inspector is an intended consequence

        /// <summary>
        /// Method called when the barriers are destroyed to start rebuilding them
        /// </summary>
        private void ShieldsDown(GameObject barrierObject)
        {
            StartCoroutine(RespawnTheShields(barrierObject));
        }
        
        /// <summary>
        /// Using the new input system, this listens for the quit button to be pressed, then opens the quit menu
        /// </summary>
        private void OpenQuitMenu(InputAction.CallbackContext context)
        {
            if (!Cursor.visible)
            {
                Cursor.visible = true;
                UIManager.Instance.GameOverUIContainer.SetActive(true);
            }
            else
            {
                Cursor.visible = false;
                UIManager.Instance.GameOverUIContainer.SetActive(false);
            }
        }

        /// <summary>
        /// Coroutine that is used to replinish the barriers energy
        /// </summary>
        /// <param name="barrierPrefab">Reference to the barrier that was destroyed</param>
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

        /// <summary>
        /// Helper method ot decrement the total bot count by 1
        /// </summary>
        public void DecrementTotalBotCount()
        {
            TotalBotCount--;
        }

        /// <summary>
        /// Used to reset the scene and replay the level
        ///   Attached to the replay button in the Hierarchy
        /// </summary>
        public void ReplayGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        /// <summary>
        /// Used to quit the game
        ///    Attached to the quit button in the Hierarchy
        /// </summary>
        public void QuitGame()
        {
            Application.Quit();
        }

        private void OnEnable()
        {
            QuitButtonInputReference.action.Enable();
            QuitButtonInputReference.action.performed += OpenQuitMenu;
            
            BarrierBehavior.OnBarrierBroken += ShieldsDown;
        }

        

        private void OnDisable()
        {
            QuitButtonInputReference.action.Disable(); 
            QuitButtonInputReference.action.performed -= OpenQuitMenu;
                        
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
