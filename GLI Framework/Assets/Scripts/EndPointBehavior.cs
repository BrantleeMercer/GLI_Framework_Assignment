using System;
using System.Collections;
using GLIFramework.Scripts;
using UnityEngine;
using UnityEngine.AI;

public class EndPointBehavior : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("End Point trigger entered");
        if (!other.tag.Equals("AI"))
            return;

        Debug.Log("It was an AI");
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
        Debug.Log($"Coroutine started from end point");
        while (!aiAgent.isStopped)
        {
            yield return null;
            Debug.Log($"AI agent isStopped: {aiAgent.isStopped}");
        }

        aiAgent.gameObject.SetActive(false);
    }
}
