using System;
using GLIFramework.Scripts.Enums;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    /// <summary>
    /// Reference to the Animator on the object
    /// </summary>
    [field: SerializeField, Tooltip("Reference to the Animator on the object"), Header("Component References")]
    private Animator Animator { get; set; } = null;
    /// <summary>
    /// The initial animation state to be set
    /// </summary>
    [field: SerializeField, Tooltip("The initial animation state to be set"), Header("Variables")]
    private AIAnims InitialState { get; set; } = AIAnims.Idle;
    
    /// <summary>
    /// String representation of the current animation state
    /// </summary>
    private string _currentAnimState;

    ///<summary>
    /// This is at the core of changing the Animation States programatically 
    ///</summary>
    public void ChangeAnimationState(AIAnims newState)
    {
        if(_currentAnimState.Equals(String.Empty))
            _currentAnimState = InitialState.ToString();
        
        //stop the same animation from interrupting itself
        if (_currentAnimState.Equals(newState.ToString()))
            return;
        //Play the animation
        Animator.Play(newState.ToString());
        //Reassign the current state
        _currentAnimState = newState.ToString();
    }

    public string GetCurrentAnimationState()
    {
        return _currentAnimState;
    }

    private void OnEnable()
    {
        _currentAnimState = InitialState.ToString();
        ChangeAnimationState(InitialState);
    }
}
