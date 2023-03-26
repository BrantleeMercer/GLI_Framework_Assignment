using System;
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
    private string InitialState { get; set; } = string.Empty;
    
    /// <summary>
    /// String representation of the current animation state
    /// </summary>
    private string _currentAnimState;

    ///<summary>
    /// This is at the core of changing the Animation States programatically 
    ///</summary>
    public void ChangeAnimationState(string newState)
    {
        //stop the same animation from interrupting itself
        if (_currentAnimState.Equals(newState))
            return;
        //Play the animation
        Animator.Play(newState);
        //Reassign the current state
        _currentAnimState = newState;
    }

    private void Start()
    {
        _currentAnimState = InitialState;
    }
}
