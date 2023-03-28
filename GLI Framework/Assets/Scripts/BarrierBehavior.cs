using System;
using System.Collections;
using System.Collections.Generic;
using GLIFramework.Scripts;
using UnityEngine;

public class BarrierBehavior : MonoBehaviour
{
    /// <summary>
    /// Amount of health left on this force field
    /// </summary>
    [field: SerializeField, Tooltip("Amount of health left on this force field"), Header("Object References")]
    public int CurrentForceFieldCharge { get; private set; } = 15; 
    
    /// <summary>
    /// Maximum charge value for the force field
    /// </summary>
    public const int MAX_CHARGE = 15;

    public static Action<GameObject> OnBarrierBroken;
    
    public void DamageForceField(int damageAmount)
    {
        CurrentForceFieldCharge -= damageAmount;
        
        if (CurrentForceFieldCharge <= 0)
        {
            //Call to the gamemanger to start respawning this barrier after it is broken
            OnBarrierBroken?.Invoke(gameObject);
            gameObject.SetActive(false);
        }
    }

    public void RechargeBarrier(int healingAmount)
    {
        CurrentForceFieldCharge += healingAmount;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        CurrentForceFieldCharge = MAX_CHARGE;
    }
}
