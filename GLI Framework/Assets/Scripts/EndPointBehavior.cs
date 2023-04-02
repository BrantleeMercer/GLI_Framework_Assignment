using System;
using System.Collections;
using GLIFramework.Scripts;
using UnityEngine;
using UnityEngine.AI;

public class EndPointBehavior : MonoBehaviour
{
    /// <summary>
    /// Set at the start of the game, this variable holds the total number of bots that will be spawned in the game
    /// </summary>
    private float _totalBotsInRound = 0;
    /// <summary>
    /// After every kill, this number will increase to keep track of the percentage of bots that were taken out
    /// </summary>
    private float _totalBotsKilled = 0;
    /// <summary>
    /// After a bot has entered the end point, this will decrement to keep track of how many bots are left
    /// </summary>
    private float _totalBotsLeft = 0;

    public static Action<float> OnLastBotFinishedRun;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.tag.Equals("AI"))
            return;

        var aiAgent = other.gameObject.GetComponent<NavMeshAgent>();

        if (aiAgent == null)
        {
            Debug.LogError("No NavMeshAgent :: EndPointBehavior");
            return;
        }
        
        StartCoroutine(TurnOffPooledAIObject(aiAgent));
    }

    private IEnumerator TurnOffPooledAIObject(NavMeshAgent aiAgent)
    {
        AudioManager.Instance.PlaySoundEffect(SoundFX.AIBotDone);
        while (!aiAgent.isStopped)
        {
            yield return null;
        }

        aiAgent.gameObject.SetActive(false);
        _totalBotsLeft--;

        if (_totalBotsLeft == 0)  //Broadcast that the last bot has finished it's run
        {
            float percentageKilled = (_totalBotsKilled / _totalBotsInRound) * 100;
            OnLastBotFinishedRun?.Invoke(percentageKilled);
        }

    }
    
    /// <summary>
    /// Listener to the MoveAIToEnd class that will keep track of how many bots have been killed
    /// </summary>
    private void ChangeTotalBotsKilled()
    {
        _totalBotsKilled++;
        _totalBotsLeft--;
    }

    private void Start()
    {
        _totalBotsLeft = _totalBotsInRound = GameManager.Instance.TotalBotCount;
        
        MoveAIToEnd.OnBotHasDied += ChangeTotalBotsKilled;
    }
    
    private void OnDestroy()
    {
        MoveAIToEnd.OnBotHasDied -= ChangeTotalBotsKilled;
    }
}
