using System;
using System.Collections;
using GLIFramework.Scripts;
using UnityEngine;
using UnityEngine.AI;

public class EndPointBehavior : MonoBehaviour
{

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

    private static IEnumerator TurnOffPooledAIObject(NavMeshAgent aiAgent)
    {
        AudioManager.Instance.PlaySoundEffect(SoundFX.AIBotDone);
        while (!aiAgent.isStopped)
        {
            yield return null;
        }

        aiAgent.gameObject.SetActive(false);
    }
}
